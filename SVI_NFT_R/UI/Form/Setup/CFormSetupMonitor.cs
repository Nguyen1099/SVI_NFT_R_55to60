using Microsoft.WindowsAPICodePack.Dialogs;
using SVI_NFT_R.CellData;
using SVI_NFT_R.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupMonitor : CFormCommon, CFormInterface
    {
        private class CCellInformation
        {
            public EProcess ProcessIndex;
            public int PositionIndex;
            public CCellInformation(EProcess processIndex, int positionIndex)
            {
                ProcessIndex = processIndex;
                PositionIndex = positionIndex;
            }
        }
        private readonly CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument">CDocument의 인스턴스</param>
        public CFormSetupMonitor(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();
            CellInformationUiInitialize();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
        }

        private void CellInformationUiInitialize()
        {
            foreach (var item in CellDataManager.ProcessCells)
            {
                var cellInfoUI = new UcMonitorCellInfo(item.Value.Count);
                cellInfoUI.Tag = item.Key;
                for (int i = 0; i < cellInfoUI.CellButtons.Length; i++)
                {
                    cellInfoUI.CellButtons[i].Visible = i < item.Value.Count;
                    if (cellInfoUI.CellButtons[i].Visible == false)
                    {
                        continue;
                    }
                    cellInfoUI.CellButtons[i].Tag = new CCellInformation(item.Key, i);
                    cellInfoUI.CellButtons[i].Click += BtnCellData_Click;
                }
                PnlCellInfoBase.Controls.Add(cellInfoUI);
            }
        }

        /// <summary>
        /// 폼 생성 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupMonitor_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormSetupMonitor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns>초기화 성공 여부</returns>
        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                // 폼 초기화
                if (false == InitializeForm())
                    break;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
        }

        /// <summary>
        /// 폼 초기화
        /// </summary>
        /// <returns>초기화 성공 여부</returns>
        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 버튼 색상 정의
                SetButtonColor();
                // 다이얼 로그 인스턴스 등록
                SetButtonTag();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 초기 색상 설정
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitle, m_colorLabel);
            SetButtonBackColor(BtnTitleGlobalFlags, m_colorLabelSub);

            // 모든 Title 버튼 색 변경 (모든 ImageButton을 m_colorNormal색으로 바꾼다)
            List<Control> imageButtons = Controls.GetChildControlListByType(typeof(ImageButton));
            foreach (ImageButton imageButton in imageButtons)
            {
                if (null != imageButton)
                {
                    SetButtonBackColor(imageButton, m_colorNormal);
                }
            }
        }

        /// <summary>
        /// 버튼 이벤트에서 사용될 Tag 인스턴스 초기화
        /// </summary>
        private void SetButtonTag()
        {
            // Title 버튼 Tag 등록
            BtnFlagDeveloperMode.Tag = new Action(() => SetButtonBackColor(BtnFlagDeveloperMode, m_objDocument.IsMasterLogin ? m_colorGreen : m_colorNormal));
            BtnFlagDeveloperModeLoginReady.Tag = new Action(() => SetButtonBackColor(BtnFlagDeveloperModeLoginReady, m_objDocument.IsMasterLoginEnabled ? m_colorGreen : m_colorNormal));
            BtnFlagUnuseAutoLogout.Tag = new Action(() => SetButtonBackColor(BtnFlagUnuseAutoLogout, m_objDocument.IsAutoLogoutNotUsed ? m_colorGreen : m_colorNormal));

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals("BtnP1")
                    && false == btn.Name.Equals("BtnP2")
                    && false == btn.Name.Equals("BtnP3")
                    && false == btn.Name.Equals("BtnP4")
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 설비 상태 or 권한 레벨에 따라 자원 상태 변경
        /// </summary>
        private void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            // 권한 레벨에 따라 원하는 방향으로 사용
            switch (objUserInformation.m_eAuthorityLevel)
            {
                case CDefine.EUserAuthorityLevel.OPERATOR:
                    base.SetControlButtonEnable(Controls, false);
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    base.SetControlButtonEnable(Controls, false);
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    base.SetControlButtonEnable(Controls, true);
                    break;
                default:
                    break;
            }

            BtnTest.Visible = m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON;
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns>언어 변경 성공 여부</returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                foreach (var item in PnlCellInfoBase.Controls.GetChildControlListByType(typeof(UcMonitorCellInfo)).Cast<UcMonitorCellInfo>())
                {
                    item.BtnManagerName.Text = Resource.Get(item.Tag).ToString();
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 타이머 설정
        /// </summary>
        /// <param name="bTimer">타이머 활성화 여부</param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        /// <summary>
        /// Visible 설정
        /// </summary>
        /// <param name="bVisible">Visible 활성화 여부</param>
        public void SetVisible(bool bVisible)
        {
            Visible = bVisible;

            if (true == bVisible)
            {
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
            }
        }

        /// <summary>
        /// 타이머 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            List<Control> allButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton button in allButtons)
            {
                if (null != button)
                {
                    var cellInformation = button.Tag as CCellInformation;
                    if (null != cellInformation)
                    {
                        bool bCellExist = CellDataManager.ProcessCells[cellInformation.ProcessIndex][cellInformation.PositionIndex].IsCellExist();
                        bool bInspectionFinished = false;
                        Color backColor = Color.Empty;
                        if (bCellExist == true)
                        {
                            if (bInspectionFinished == true)
                            {
                                backColor = m_colorOrange;
                            }
                            else
                            {
                                backColor = m_colorOn;
                            }
                        }
                        SetButtonBackColor(button, backColor);
                    }
                    var updateAction = button.Tag as Action;
                    if (null != updateAction)
                    {
                        updateAction.Invoke();
                    }
                }
            }

            SetLabelText(lblOverrideSpeedRate, $"{m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[0].OverrideSpeedRate} ( % )");
            SetLabelText(lblSolenoidDelay, $"{m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[0].GetVacuumParameter().SolenoidDelayTime} ( ms )");
            SetLabelText(lblTodayInput, $"금일 투입량 : {ProductCounter.TodayTrackIn.Count}");
            SetLabelText(lblTodayOutput, $"금일 배출량 : {ProductCounter.TodayTrackOut.Count}");
            SetLabelText(lblTotalOutput, $"총 배출량 : {ProductCounter.TotalTrackOut.Count}");
            SetLabelText(LblMtbiTime, MtbiDataCollector.GetElapsedTime());
        }

        private void SetLabelText(Label label, string text)
        {
            if (label.Text != text)
            {
                label.Text = text;
            }
        }

        /// <summary>
        /// Cell Data 버튼 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellData_Click(object sender, EventArgs e)
        {
            var btnCellData = sender as Control;
            var cellData = (CCellInformation)btnCellData.Tag;
            if (null != cellData)
            {
                var cellDataHandler = CellDataManager.ProcessCells[cellData.ProcessIndex][cellData.PositionIndex];
                // Cell Data가 있다면 삭제
                if (cellDataHandler.IsCellExist() == true)
                {
                    if (cellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK)
                    {
                        cellDataHandler.Data.Judgement.Judge = CDefine.CIM_JUDGE_OUT;
                        cellDataHandler.Data.Judgement.Description = string.Format("{0}_{1}_MANUAL_TRACKOUT", cellData.ProcessIndex.ToString().ToUpper().Replace("PROCESS_", ""), cellData.PositionIndex + 1);
#if !CIM_V3_GAMMA
                        // V1, V3-5F
                        cellDataHandler.Data.Judgement.ReasonCode = "USE39";
#else
                        // V3 (G Project)
                        cellDataHandler.Data.Judgement.ReasonCode = "GMCEQDR";
#endif
                        m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.ManualTrackOutRequestPushInQueue(cellDataHandler.Data.DeepClone());
                    }
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [CellDataRemove({cellDataHandler.CellDataIndex})] [{cellDataHandler.Data.GetInnerID()}]");
                    CellDataManager.DataReset(cellDataHandler);

                    //m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.DryRunSignalClear();
                }
                // 없으면 Cell Data 생성
                else if (cellDataHandler.IsCellExist() == false)
                {
                    CellDataManager.DataReset(cellDataHandler);
                    cellDataHandler.Data.IsUse = true;
                    cellDataHandler.CreateInnerID();
                    cellDataHandler.Data.Cell.CellID = $"TEST_{cellData.ProcessIndex}_{cellData.PositionIndex}_{DateTime.Now:yyyyMMddHHmmssfff}";
                    cellDataHandler.Data.EqInterface.CellID = cellDataHandler.GetCellID();
                    cellDataHandler.Data.Cell.CellID = cellDataHandler.GetCellID();
                    cellDataHandler.Data.Reader.ReaderType = EReaderType.None;
                    cellDataHandler.Data.Reader.ReaderID = "9";
                    //cellDataHandler.Data.Reader.ReaderResultCode = CCIMDefine.ReaderResultCode.OK;
                    cellDataHandler.Data.Reader.RetryCount = 0;
                    cellDataHandler.Data.Cell.InputTime = DateTime.Now;
                    switch (cellDataHandler.ProcessIndex)
                    {
                        case EProcess.InShuttle:
                        case EProcess.InRobot:
                        case EProcess.InspStage:
                        case EProcess.OutRobot:
                        case EProcess.OutFlip:
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                    cellDataHandler.IsChanged = true;
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [CellDataCreate({cellDataHandler.CellDataIndex})] [{cellDataHandler.Data.GetInnerID()}]");
                }
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            //DEVICE.SviShortSideEquipment.SviCellInformationData cellInformationData = new DEVICE.SviShortSideEquipment.SviCellInformationData();
            //cellInformationData.PositionA.CellID = "CellIDA";
            //cellInformationData.PositionB.CellID = "CellIDB";
            //cellInformationData.PositionA.InnerID = "InnerIDA";
            //cellInformationData.PositionB.InnerID = "InnerIDB";
            //m_objDocument.m_objProcessMain.m_objSviShortSideEquipmentInterface.HLSendCellInformation(cellInformationData);

            //DEVICE.SviShortSideEquipment.SviTotalResultData totalResultData = new DEVICE.SviShortSideEquipment.SviTotalResultData();
            //totalResultData.CellID = "CellIDA";
            //totalResultData.SviResult = "BIN1";
            //totalResultData.SviReasonCode = new string[] { "TEST", "DEFECT", "1", "", "", "" };
            //m_objDocument.m_objProcessMain.m_objSviShortSideEquipmentInterface.HLSendTotalResultData(totalResultData);

            //int processIndex = (int)CStructureInfoData.EProcess.PROCESS_NACHI_OUT_ROBOT;
            //int positionIndex = (int)CDefine.EPosition.POSITION_A;
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].eCellUsed = CStructureInfoData.ECellUsed.CELL_USE;
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objCell.strCellID = "CellIDA";
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objCell.strInnerID = "InnerIDA";
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objReader.strReaderResultCode = ((int)CCIMDefine.ReaderResultCode.OK).ToString();
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objCell.strTrackingResult = CStructureInfoData.ETrackingResult.TKIN_OK.ToString();
            //processIndex = (int)CStructureInfoData.EProcess.PROCESS_NACHI_OUT_ROBOT;
            //positionIndex = (int)CDefine.EPosition.POSITION_B;
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].eCellUsed = CStructureInfoData.ECellUsed.CELL_USE;
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objCell.strCellID = "CellIDB";
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objCell.strInnerID = "InnerIDB";
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objReader.strReaderResultCode = ((int)CCIMDefine.ReaderResultCode.OK).ToString();
            //m_objDocument.m_objStructure.m_objProcessData[processIndex].objPosition[positionIndex].objCell.strTrackingResult = CStructureInfoData.ETrackingResult.TKIN_OK.ToString();
            //m_objDocument.m_objProcessMain.m_objProcessMotion.m_objProcessManagerNachiOutRobot.m_objProcessMotionSviShortSideEquipmentInterface.SetCommand(CProcessMotionSviShortSideEquipmentInterface.ECommand.CMD_CELL_DATA_BACKUP_TO_FILE);
            //m_objDocument.m_objProcessMain.m_objProcessMotion.m_objProcessManagerNachiOutRobot.m_objProcessMotionSviShortSideEquipmentInterface.WaitForEndProcess();
            //m_objDocument.m_objStructure.SetDataReset(CStructureInfoData.EProcess.PROCESS_NACHI_OUT_ROBOT);

            //CellLotInformationRequest data = new CellLotInformationRequest();
            //data.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            //data.BODY.OPTIONCODE = "DEFECT";
            //data.BODY.CELLIDS.Add("A4TJ1SI8FCFBE105");
            //m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCellLotInformationRequest(data);
            //m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");

            //DoorInterlock.SetSimulationLoaderPauseRequest(true);
            //DoorInterlock.SetSimulationUnloaderPauseRequest(true);
        }

        private void btnVacuumOffAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 설비 내부에 모든 진공을 해제 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (CellDataManager.Cells.Values.IsCellExistFromList() == true)
            {
                if (MessageBox.Show($" 설비 내부에 셀 정보가 있습니다. 설비 내부에 셀이 있으면 떨어 질 수 있습니다.{Environment.NewLine} 계속 진행 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            Parallel.ForEach(m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum.Values, vacuum =>
            {
                vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
            });
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
        }

        private void btnCellOutAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 설비 내부에 모든 셀 정보를 제거 하시겠습니까?{Environment.NewLine}(설비 내부에 실물을 제거하지 않으면 겹침 현상이 발생 할 수 있습니다.)", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var cellDataHandler in CellDataManager.Cells.Values)
            {
                if (cellDataHandler.IsCellExist() == true)
                {
                    // Cell Data가 있다면 삭제
                    if (cellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK)
                    {
                        cellDataHandler.Data.Judgement.Judge = CDefine.CIM_JUDGE_OUT;
                        cellDataHandler.Data.Judgement.Description = string.Format("{0}_{1}_MANUAL_TRACKOUT", cellDataHandler.ProcessIndex.ToString().Replace("PROCESS_", ""), cellDataHandler.PositionIndex + 1);
#if !CIM_V3_GAMMA
                        // V1, V3-5F
                        cellDataHandler.Data.Judgement.ReasonCode = "USE39";
#else
                        // V3 (G Project)
                        cellDataHandler.Data.Judgement.ReasonCode = "GMCEQDR";
#endif
                        m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.ManualTrackOutRequestPushInQueue(cellDataHandler.Data.DeepClone());
                    }
                    sb.Append($"{cellDataHandler.CellDataIndex} ");
                    CellDataManager.DataReset(cellDataHandler);
                }
            }

            Parallel.ForEach(m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum.Values, vacuum =>
            {
                vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
            });
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [{sb.ToString()}]");
        }

        private void btnUpperInterfaceSignalAllClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 상류 설비와 인터페이스 신호를 초기화 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [LoadInterfaces.DryRunSignalClear]");
            m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.DryRunSignalClear();
        }

        private void btnLowerInterfaceSignalAllClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 하류 설비와 인터페이스 신호를 초기화 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [UnloadInterface.DryRunSignalClear]");
            //m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.DryRunSignalClear();
        }

        private void btnOverrideSpeedRateEdit_Click(object sender, EventArgs e)
        {
            using (FormKeyPad keypad = new FormKeyPad(m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[0].OverrideSpeedRate, 10, 120))
            {
                if (keypad.ShowDialog() == DialogResult.OK)
                {
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [OverrideSpeedRate : {keypad.m_dResultValue}]");
                    foreach (var motor in m_objDocument.m_objProcessMain.m_objProcessMotion.m_objMotor.Values)
                    {
                        motor.OverrideSpeedRate = Convert.ToInt32(keypad.m_dResultValue);
                    }
                }
            }
        }

        private void btnSolenoidDelayEdit_Click(object sender, EventArgs e)
        {
            int initValue = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[0].GetVacuumParameter().SolenoidDelayTime;
            using (FormKeyPad keypad = new FormKeyPad(initValue, CVacuumAbstract.CVacuumDataParameter.SOLENOID_DELAY_MIN, CVacuumAbstract.CVacuumDataParameter.SOLENOID_DELAY_MAX))
            {
                if (keypad.ShowDialog() == DialogResult.OK)
                {
                    int changeValue = Convert.ToInt32(keypad.m_dResultValue);
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [SolenoidDelayTime: {initValue}ms -> {changeValue}ms]");
                    foreach (var vacuum in m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum.Values)
                    {
                        CVacuumAbstract.CVacuumDataParameter param = vacuum.GetVacuumParameter().DeepClone();
                        param.SolenoidDelayTime = changeValue;
                        vacuum.SaveVacuumData("", "", param);
                    }
                }
            }
        }

        private void lblTodayInput_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 카운터를 리셋 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            ProductCounter.TodayTrackIn.Clear();
        }

        private void lblTodayOutput_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 카운터를 리셋 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            ProductCounter.TodayTrackOut.Clear();
        }

        private void lblTotalOutput_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" 카운터를 리셋 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            ProductCounter.TotalTrackOut.Clear();
        }

        private void BtnMtbiStart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($" History DB에 내용이 모두 삭제됩니다. 계속 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }
                BtnMtbiStart.ForeColor = MtbiDataCollector.IsStarted == false ? Color.Blue : Color.DarkBlue;
                MtbiDataCollector.Start(m_objDocument, dialog.FileName);
            }
        }

        private void BtnMtbiFinished_Click(object sender, EventArgs e)
        {
            if (MtbiDataCollector.IsStarted == false)
            {
                return;
            }

            Task.Run(() => MtbiDataCollector.Finished());
            BtnMtbiStart.ForeColor = Color.Black;
        }
    }
}