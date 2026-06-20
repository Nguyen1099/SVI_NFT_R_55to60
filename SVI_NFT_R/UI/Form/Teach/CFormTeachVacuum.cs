using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormTeachVacuum : CFormCommon, CFormInterface
    {
        // {91052E97-CEFB-41EB-A6CF-8A16869691AA}
        private readonly Guid GUID = new Guid(0x91052e97, 0xcefb, 0x41eb, 0xa6, 0xcf, 0x8a, 0x16, 0x86, 0x96, 0x91, 0xaa);
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 진공 리스트
        /// </summary>
        private List<CVacuum> m_listVacuum;
        /// <summary>
        /// 진공 객체
        /// </summary>
        private CVacuum m_objVacuum;
        /// <summary>
        /// 진공 데이터
        /// </summary>
        private CVacuumAbstract.CVacuumDataParameter m_objVacuumDataParameter;
        private bool mbOutputLock = true;
        private bool mbAnalogDataView = false;
        /// <summary>
        /// 선택된 실린더 리스트
        /// </summary>
        private readonly List<CVacuum> mSelectionVacuums = new List<CVacuum>();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormTeachVacuum(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTeachVacuum_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormTeachVacuum_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                // 폼 초기화
                if (false == InitializeForm())
                {
                    break;
                }

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
        /// <returns></returns>
        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 버튼 색상 정의
                SetButtonColor();
                // 진공 객체 정보 추가
                m_listVacuum = m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum.Values.ToList();

                // 그리드 뷰 초기화
                InitializeGridView(GridViewVacuumList);
                // 항목 표시
                SetMotorInformationGridView(m_listVacuum);

                // 실린더 리스트 다중 선택 설정
                GridViewVacuumList.MultiSelect = true;
                // 실린더 처음 항목 데이터 로드
                GridViewVacuumList.Rows[0].Selected = true;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            SetButtonBackColor(BtnTitleVacuumList, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumDelayTime, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumTimeout, m_colorLabelSub);
            SetButtonBackColor(BtnVacuumTimeout, m_colorLabelData);
            SetButtonBackColor(BtnTitleVacuumOffDelayTime, m_colorLabelSub);
            SetButtonBackColor(BtnVacuumOffDelayTime, m_colorLabelData);
            SetButtonBackColor(BtnTitleVacuumInput, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumInput01, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumInput02, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumInput03, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumInput04, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumAnalogInput, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumOutput, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumOutput01, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumOutput02, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumOutput03, m_colorLabelSub);
            SetButtonBackColor(BtnTitleVacuumOutput04, m_colorLabelSub);
            SetButtonBackColor(BtnTitleBlowOutput, m_colorLabelSub);
            SetButtonBackColor(BtnTitleBlowOutput01, m_colorLabelSub);
            SetButtonBackColor(BtnTitleBlowOutput02, m_colorLabelSub);
            SetButtonBackColor(BtnTitleBlowOutput03, m_colorLabelSub);
            SetButtonBackColor(BtnTitleBlowOutput04, m_colorLabelSub);
            SetButtonBackColor(BtnVacuumAnalogInput01, Color.Empty);
            SetButtonBackColor(BtnVacuumAnalogInput02, Color.Empty);
            SetButtonBackColor(BtnVacuumAnalogInput03, Color.Empty);
            SetButtonBackColor(BtnVacuumAnalogInput04, Color.Empty);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnVacuumTimeout.Name)
                    && false == btn.Name.Equals(BtnVacuumOffDelayTime.Name)
                    && false == btn.Name.Equals(BtnVacuumOutput01.Name)
                    && false == btn.Name.Equals(BtnVacuumOutput02.Name)
                    && false == btn.Name.Equals(BtnVacuumOutput03.Name)
                    && false == btn.Name.Equals(BtnVacuumOutput04.Name)
                    && false == btn.Name.Equals(BtnBlowOutput01.Name)
                    && false == btn.Name.Equals(BtnBlowOutput02.Name)
                    && false == btn.Name.Equals(BtnBlowOutput03.Name)
                    && false == btn.Name.Equals(BtnBlowOutput04.Name)
                    && false == btn.Name.Equals(BtnVacuumAnalogInput01.Name)
                    && false == btn.Name.Equals(BtnVacuumAnalogInput02.Name)
                    && false == btn.Name.Equals(BtnVacuumAnalogInput03.Name)
                    && false == btn.Name.Equals(BtnVacuumAnalogInput04.Name)
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
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                base.SetControlButtonEnable(Controls, false);
                if (CDefine.EUserAuthorityLevel.MASTER == objUserInformation.m_eAuthorityLevel)
                {
                    GridViewVacuumList.Enabled = true;
                    // 마스터 권한이면 런타임중 딜레이를 조절 할 수 있도록 한다.
                    BtnVacuumTimeout.Enabled = true;
                    BtnVacuumOffDelayTime.Enabled = true;
                }
                else
                {
                    GridViewVacuumList.Enabled = true;
                }
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        base.SetControlButtonEnable(Controls, false);
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        base.SetControlButtonEnable(Controls, true);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        base.SetControlButtonEnable(Controls, true);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <returns></returns>
        private new bool InitializeGridView(DataGridView objGridView)
        {
            bool bReturn = false;

            do
            {

                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                objGridView.RowHeadersVisible = false;
                // 행 조정 x
                objGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 스크롤바 x
                //objGridView.ScrollBars = ScrollBars.None;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                // 칼럼 정렬 기능 x
                objGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, "신명조", 10.0, FontStyle.Regular);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 진공 정보 그리드 뷰에 고정 틀로 뿌려줌
        /// </summary>
        /// <param name="objMotorInformationList"></param>
        private void SetMotorInformationGridView(List<CVacuum> objMotorInformationList)
        {
            DataGridView objGridView = GridViewVacuumList;
            // 행 초기화
            objGridView.Rows.Clear();

            for (int iLoopCount = 0; iLoopCount < objMotorInformationList.Count; iLoopCount++)
            {
                string strRowData = objMotorInformationList[iLoopCount].GetVacuumName();
                objGridView.Rows.Add(strRowData);
            }
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 데이터 테이블에서 일치하는 ID에 해당하는 TEXT를 불러옴
                SetButtonChangeLanguage(BtnTitleVacuumList);
                SetButtonChangeLanguage(BtnTitleVacuumDelayTime);
                SetButtonChangeLanguage(BtnTitleVacuumTimeout);
                SetButtonChangeLanguage(BtnTitleVacuumOffDelayTime);
                SetButtonChangeLanguage(BtnTitleVacuumInput);
                SetButtonChangeLanguage(BtnTitleVacuumAnalogInput);
                SetButtonChangeLanguage(BtnTitleVacuumOutput);
                SetButtonChangeLanguage(BtnTitleBlowOutput);
                SetButtonChangeLanguage(BtnVacuum);
                SetButtonChangeLanguage(BtnBlow);
                SetButtonChangeLanguage(BtnOutputLock);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            Visible = bVisible;

            if (true == bVisible)
            {
                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                m_objDocument.GetMainFrame().SetCurrentForm(this);
                // 화면 전환시 LOCK 상태로 변경
                mbOutputLock = true;
                mbAnalogDataView = false;
                Focus();
            }
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            string strMilisecond = " ( ㎳ )";
            if (null != m_objVacuum)
            {
                SetControlText(LblVaccumSettingTime, $@"VacuumSetting Time: {m_objVacuum.VacuumSettingTime,7:0.000}{strMilisecond}");
                SetControlText(LblBlowSettingTime, $@"BlowSetting Time: {m_objVacuum.BlowSettingTime,7:0.000}{strMilisecond}");
                SetControlText(LblSensorOnTime, $@"SensorOn Time: {m_objVacuum.VacuumSensorOnTime,7:0.000}{strMilisecond}");
                SetControlText(LblSensorOffTime, $@"SensorOff Time: {m_objVacuum.VacuumSensorOffTime,7:0.000}{strMilisecond}");

                SetControlText(BtnVacuumTimeout, $"{m_objVacuumDataParameter.iVacuumTimeOut}{strMilisecond}");
                SetControlText(BtnVacuumOffDelayTime, $"{m_objVacuumDataParameter.iVacuumOffDelayTime}{strMilisecond}");
                // Vacuum / Blow Button Update
                SetButtonBackColor(BtnVacuum, CVacuumAbstract.EVacuumCommand.CMD_ON == m_objVacuum.GetVacuumCommand() ? m_colorOn : m_colorNormal, CVacuumAbstract.EVacuumCommand.CMD_ON == m_objVacuum.GetVacuumCommand());
                SetButtonBackColor(BtnBlow, CVacuumAbstract.EVacuumCommand.CMD_BLOW == m_objVacuum.GetVacuumCommand() ? m_colorOn : m_colorNormal, CVacuumAbstract.EVacuumCommand.CMD_BLOW == m_objVacuum.GetVacuumCommand());
                // Input
                {
                    var inputList = new Control[]
                    {
                        BtnVacuumInput01,
                        BtnVacuumInput02,
                        BtnVacuumInput03,
                        BtnVacuumInput04,
                    };
                    bool getValue = false;
                    foreach (Control item in inputList)
                    {
                        if (
                            false == string.IsNullOrEmpty(item.Text)
                            && true == item.Visible
                            && true == m_objDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(item.Text, ref getValue)
                            )
                        {
                            SetButtonBackColor(item, true == getValue ? m_colorInputOn : m_colorInputOff);
                        }
                    }
                }
                // Analog Input
                {
                    var analogInputList = new Button[]
                    {
                        BtnVacuumAnalogInput01,
                        BtnVacuumAnalogInput02,
                        BtnVacuumAnalogInput03,
                        BtnVacuumAnalogInput04,
                    };
                    string[] analogInputNames = m_objVacuum.GetInitializeParameter().strVacuumAnalogInputIO;
                    double getValue = 0d;
                    for (int i = 0; i < analogInputList.Length; i++)
                    {
                        if (
                            false == string.IsNullOrEmpty(analogInputNames[i])
                            && true == analogInputList[i].Visible
                            && true == m_objDocument.m_objProcessMain.m_objIO.HLGetAnalog(analogInputNames[i], ref getValue)
                            )
                        {
                            if (false == mbAnalogDataView)
                            {
                                CDeviceIODefine.EAnalogInput moduleIndex = (CDeviceIODefine.EAnalogInput)Enum.Parse(typeof(CDeviceIODefine.EAnalogInput), analogInputNames[i]);
                                SetButtonText(analogInputList[i], string.Format("{0:0.0} ( kPa )", m_objDocument.GetVoltageTokPaVacuum(moduleIndex)));
                            }
                            else
                            {
                                SetButtonText(analogInputList[i], string.Format("{0:0.000} ( V )", getValue));
                            }
                        }
                    }
                }
                // Output
                {
                    var outputList = new Control[]
                    {
                        BtnVacuumOutput01,
                        BtnVacuumOutput02,
                        BtnVacuumOutput03,
                        BtnVacuumOutput04,
                        BtnBlowOutput01,
                        BtnBlowOutput02,
                        BtnBlowOutput03,
                        BtnBlowOutput04,
                    };
                    bool getValue = false;
                    foreach (Control item in outputList)
                    {
                        if (
                            false == string.IsNullOrEmpty(item.Text)
                            && true == item.Visible
                            && true == m_objDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(item.Text, ref getValue)
                            )
                        {
                            SetButtonBackColor(item, true == getValue ? m_colorOutputOn : m_colorOutputOff);
                        }
                    }
                }
            }

            // IO 잠금 버튼 업데이트
            {
                SetButtonBackColor(BtnOutputLock, (true == mbOutputLock) ? m_colorOn : m_colorNormal, (true == mbOutputLock));
            }
        }

        /// <summary>
        /// 목적 :업데이트 진공 데이터
        /// </summary>
        private void SetDataReLoad()
        {
            do
            {
                // 진공 데이터 객체
                m_objVacuumDataParameter = m_objVacuum.GetVacuumParameter().DeepClone();
                // IO 이름 업데이트
                var ioParameter = m_objDocument.m_objConfig.GetIOParameter();
                var initializeParameter = m_objVacuum.GetInitializeParameter();
                var inputList = new Button[]
                {
                    BtnVacuumInput01,
                    BtnVacuumInput02,
                    BtnVacuumInput03,
                    BtnVacuumInput04,
                };
                var inputTitleList = new Button[]
                {
                    BtnTitleVacuumInput01,
                    BtnTitleVacuumInput02,
                    BtnTitleVacuumInput03,
                    BtnTitleVacuumInput04,
                };
                var vacuumOutputList = new Button[]
                {
                    BtnVacuumOutput01,
                    BtnVacuumOutput02,
                    BtnVacuumOutput03,
                    BtnVacuumOutput04,
                };
                var vacuumOutputTitleList = new Button[]
                {
                    BtnTitleVacuumOutput01,
                    BtnTitleVacuumOutput02,
                    BtnTitleVacuumOutput03,
                    BtnTitleVacuumOutput04,
                };
                var blowOutputList = new Button[]
                {
                    BtnBlowOutput01,
                    BtnBlowOutput02,
                    BtnBlowOutput03,
                    BtnBlowOutput04,
                };
                var blowOutputTitleList = new Button[]
                {
                    BtnTitleBlowOutput01,
                    BtnTitleBlowOutput02,
                    BtnTitleBlowOutput03,
                    BtnTitleBlowOutput04,
                };
                var analogInput = new Button[]
                {
                    BtnVacuumAnalogInput01,
                    BtnVacuumAnalogInput02,
                    BtnVacuumAnalogInput03,
                    BtnVacuumAnalogInput04,
                };
                for (int i = 0; i < (int)CVacuumAbstract.EVacuumSolenoidType.QUARD_SOLENOID; i++)
                {
                    inputTitleList[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumInputIO[i]));
                    inputList[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumInputIO[i]));
                    vacuumOutputTitleList[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumOnOutputIO[i]));
                    vacuumOutputList[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumOnOutputIO[i]));
                    blowOutputTitleList[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumBlowOutputIO[i]));
                    blowOutputList[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumBlowOutputIO[i]));
                    analogInput[i].Visible = (false == string.IsNullOrEmpty(initializeParameter.strVacuumAnalogInputIO[i]));
                    if (true == inputList[i].Visible)
                    {
                        foreach (var item in ioParameter.objIOParameter)
                        {
                            if (item.Value.strIOName == initializeParameter.strVacuumInputIO[i])
                            {
                                SetButtonText(inputTitleList[i], item.Key);
                                break;
                            }
                        }
                        SetButtonText(inputList[i], initializeParameter.strVacuumInputIO[i]);
                    }
                    if (true == vacuumOutputList[i].Visible)
                    {
                        foreach (var item in ioParameter.objIOParameter)
                        {
                            if (item.Value.strIOName == initializeParameter.strVacuumOnOutputIO[i])
                            {
                                SetButtonText(vacuumOutputTitleList[i], item.Key);
                                break;
                            }
                        }
                        SetButtonText(vacuumOutputList[i], initializeParameter.strVacuumOnOutputIO[i]);
                    }
                    if (true == blowOutputList[i].Visible)
                    {
                        foreach (var item in ioParameter.objIOParameter)
                        {
                            if (item.Value.strIOName == initializeParameter.strVacuumBlowOutputIO[i])
                            {
                                SetButtonText(blowOutputTitleList[i], item.Key);
                                break;
                            }
                        }
                        SetButtonText(blowOutputList[i], initializeParameter.strVacuumBlowOutputIO[i]);
                    }
                    if (true == analogInput[i].Visible)
                    {
                        if (false == mbAnalogDataView)
                        {
                            SetButtonText(analogInput[i], "0.0 kPa");
                        }
                        else
                        {
                            SetButtonText(analogInput[i], "0.000 V");
                        }
                    }
                }

            } while (false);
        }

        /// <summary>
        /// 타임아웃 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVacuumTimeout_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnVacuumTimeout_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(m_objVacuumDataParameter.iVacuumOffDelayTime, 500, 5000))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objVacuumDataParameter.iVacuumTimeOut = ((int)objKeyPad.m_dResultValue > 0) ? (int)objKeyPad.m_dResultValue : 0;
                    m_objVacuum.SaveVacuumData("", "", m_objVacuumDataParameter);
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [VacuumTimeout : {1:D}] [{2}]", "BtnVacuumTimeout_Click", m_objVacuumDataParameter.iVacuumTimeOut, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 진공 OFF 딜레이 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnVacuumOffDelayTime_Click(object sender, EventArgs e)
        {
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [{1}]", "BtnVacuumOffDelayTime_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            using (FormKeyPad objKeyPad = new FormKeyPad(m_objVacuumDataParameter.iVacuumOffDelayTime, 0, 1000))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    m_objVacuumDataParameter.iVacuumOffDelayTime = ((int)objKeyPad.m_dResultValue > 0) ? (int)objKeyPad.m_dResultValue : 0;
                    m_objVacuum.SaveVacuumData("", "", m_objVacuumDataParameter);
                }
            }
            // 버튼 로그 추가
            strLog = string.Format("[{0}] [VacuumOffDelayTime : {1:D}] [{2}]", "BtnVacuumOffDelayTime_Click", m_objVacuumDataParameter.iVacuumOffDelayTime, false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void GridViewVacuumList_SelectionChanged(object sender, EventArgs e)
        {

            DataGridView objGridView = sender as DataGridView;
            // 셀을 클릭하면 데이터 갱신하는 역활
            do
            {
                try
                {
                    mSelectionVacuums.Clear();

                    foreach (DataGridViewCell row in objGridView.SelectedCells)
                    {
                        mSelectionVacuums.Add(m_listVacuum[row.RowIndex]);
                    }

                    if (0 == mSelectionVacuums.Count)
                    {
                        // 모든 셀의 선택을 해제 했을 때 첫번째 셀을 다시 선택하도록함
                        GridViewVacuumList.Rows[0].Selected = true;
                    }
                    else
                    {
                        m_objVacuum = mSelectionVacuums[0];
                    }

                    // 데이터 리로드
                    SetDataReLoad();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        private async void BtnVacuum_Click(object sender, EventArgs e)
        {
            if (null != m_objVacuum)
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnVacuum_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                var vacuumCommand = m_objVacuum.GetVacuumCommand();
                if (vacuumCommand != CVacuumAbstract.EVacuumCommand.CMD_ON)
                {
                    await Task.Factory.StartNew(() => Parallel.ForEach(mSelectionVacuums, item => item.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.IGNORE)));

                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [Command : {1:D}] [Check : {2}] [{3}]", "BtnVacuum_Click", CVacuumAbstract.EVacuumCommand.CMD_ON, CVacuumAbstract.ESensorCheck.IGNORE, false);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    await Task.Factory.StartNew(() => Parallel.ForEach(mSelectionVacuums, item => item.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE)));

                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [Command : {1:D}] [Check : {2}] [{3}]", "BtnVacuum_Click", CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE, false);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            }
        }

        private async void BtnBlow_Click(object sender, EventArgs e)
        {
            if (null != m_objVacuum)
            {
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [{1}]", "BtnBlow_Click", true);
                m_objDocument.SetUpdateButtonLog(this, strLog);

                var vacuumCommand = m_objVacuum.GetVacuumCommand();
                if (vacuumCommand != CVacuumAbstract.EVacuumCommand.CMD_BLOW)
                {
                    await Task.Factory.StartNew(() => Parallel.ForEach(mSelectionVacuums, item => item.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_BLOW, CVacuumAbstract.ESensorCheck.IGNORE)));

                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [Command : {1:D}] [Check : {2}] [{3}]", "BtnBlow_Click", CVacuumAbstract.EVacuumCommand.CMD_BLOW, CVacuumAbstract.ESensorCheck.IGNORE, false);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                else
                {
                    await Task.Factory.StartNew(() => Parallel.ForEach(mSelectionVacuums, item => item.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE)));

                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [Command : {1:D}] [Check : {2}] [{3}]", "BtnBlow_Click", CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE, false);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
            }
        }

        private void BtnOutputLock_Click(object sender, EventArgs e)
        {
            mbOutputLock = !mbOutputLock;
            // 버튼 로그 추가
            string strLog = string.Format("[{0}] [Lock Mode : {1}]", "BtnOutputLock_Click", mbOutputLock);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void BtnVacuumOutput_Click(object sender, EventArgs e)
        {
            if (null == m_objVacuum)
            {
                return;
            }

            if (false == mbOutputLock)
            {
                Button BtnSelected = (Button)sender;
                bool bResult = false;

                if (true == m_objDocument.m_objProcessMain.m_objIO.HLGetDigitalBit(BtnSelected.Text, ref bResult))
                {
                    if (true == bResult)
                    {
                        m_objDocument.m_objProcessMain.m_objIO.HLSetDigitalBit(BtnSelected.Text, false);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Name : {1}] [Bit : {2}]", "BtnVacuumOutput_Click", BtnSelected.Text, false);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                    else if (false == bResult)
                    {
                        m_objDocument.m_objProcessMain.m_objIO.HLSetDigitalBit(BtnSelected.Text, true);
                        // 버튼 로그 추가
                        string strLog = string.Format("[{0}] [Output Name : {1}] [Bit : {2}]", "BtnVacuumOutput_Click", BtnSelected.Text, true);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }
            else
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.OUTPUT_SIGNAL_CHANGE_LOCK_STATUS);
            }
        }

        private void BtnVacuumAnalogInput_Click(object sender, EventArgs e)
        {
            CUserInformation objUserInformation = m_objDocument.GetUserInformation();
            if (CDefine.EUserAuthorityLevel.MASTER == objUserInformation.m_eAuthorityLevel)
            {
                mbAnalogDataView = !mbAnalogDataView;
            }
        }

        private async void BtnTitleVacuumAnalogInput_Click(object sender, EventArgs e)
        {
            string setText = "";
            var analogInputList = new Button[]
            {
                BtnVacuumAnalogInput01,
                BtnVacuumAnalogInput02,
                BtnVacuumAnalogInput03,
                BtnVacuumAnalogInput04,
            };
            string[] analogInputNames = m_objVacuum.GetInitializeParameter().strVacuumAnalogInputIO;
            double getValue = 0d;
            await Task.Factory.StartNew(() =>
            {
                const int sampleCount = 10;
                for (int j = 0; j < sampleCount; j++)
                {
                    for (int i = 0; i < analogInputList.Length; i++)
                    {
                        if (
                            false == string.IsNullOrEmpty(analogInputNames[i])
                            && true == analogInputList[i].Visible
                            && true == m_objDocument.m_objProcessMain.m_objIO.HLGetAnalog(analogInputNames[i], ref getValue)
                            )
                        {
                            setText += string.Format("{0}\t", getValue);
                        }
                    }
                    if (j != sampleCount - 1)
                    {
                        setText += string.Format("\n");
                    }
                    Thread.Sleep(100);
                }
            });
            Clipboard.SetText(setText);
        }

        private void CFormTeachVacuum_KeyDown(object sender, KeyEventArgs e)
        {
            // 변경 불가
            if (BtnVacuumTimeout.Enabled == false)
            {
                return;
            }

            // 파라메터 복사
            if (
                e.Control == true
                && e.KeyCode == Keys.C
                )
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write(GUID.ToByteArray());
                    string jsonString = JsonConvert.SerializeObject(m_objVacuumDataParameter, typeof(CVacuumAbstract.CVacuumDataParameter), new JsonSerializerSettings() { });
                    writer.Write(jsonString);
                    Clipboard.SetDataObject(memoryStream.ToArray());
                    m_objDocument.SetUpdateButtonLog(this, $"Copy to clipboard.\t{jsonString}");
                }
            }

            // 파라메터 붙여 넣기
            if (
                e.Control == true
                && e.KeyCode == Keys.V
                )
            {
                byte[] clipboardValue = Clipboard.GetDataObject().GetData(typeof(byte[])) as byte[];
                if (clipboardValue != null)
                {
                    using (var memoryStream = new MemoryStream(clipboardValue))
                    using (var reader = new BinaryReader(memoryStream))
                    {
                        Guid readGuid = new Guid(reader.ReadBytes(16));
                        if (GUID == readGuid)
                        {
                            string jsonString = reader.ReadString();
                            m_objVacuumDataParameter = (CVacuumAbstract.CVacuumDataParameter)JsonConvert.DeserializeObject(jsonString, typeof(CVacuumAbstract.CVacuumDataParameter));
                            m_objVacuum.SaveVacuumData("", "", m_objVacuumDataParameter);
                            m_objDocument.SetUpdateButtonLog(this, $"Paste from clipboard.\t{jsonString}");
                        }
                    }
                }
            }
        }
    }
}