using SVI_NFT_R.CellData;
using SVI_NFT_R.EqToEq.SdvGammaFoldableLine;
using SVI_NFT_R.Properties;
using SVI_NFT_R.UI.Dialog;
using SVI_NFT_R.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormMainFlow : CFormCommon, CFormInterface
    {
        private CDialogNotification mDialogNotification;
        private bool mbThreadShouldStop = false;
        private Thread mThreadUpdatePosition;
        private readonly CDocument m_objDocument;
        private CSafetyIO m_objSafetyIO;
        private List<SpeedButton> mVacuumButtons = new List<SpeedButton>();
        private List<SpeedButton> mCellDataButtons = new List<SpeedButton>();
        private bool m_bIsBusy = false;
        private bool mbBlinkSignal = false;
        private readonly Stopwatch swBlinkSignal = Stopwatch.StartNew();
        private TimeSpan mBlinkTimeSpan = TimeSpan.FromMilliseconds(1000);
        private readonly Color mLoadColor = Color.LightGreen;
        private readonly Color mProcessingColor = Color.MediumSpringGreen;
        private readonly Color mUnloadColor = Color.LightSeaGreen;
        private readonly List<DrawComponentGroupPosition> mDrawPositionList = new List<DrawComponentGroupPosition>();
        private readonly UI.OffsetData IN_ROBOT_INITIALIZE = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.6296296f), Y = UI.OffsetItem.Ratio(0f) };
        private readonly UI.OffsetData IN_ROBOT_LOAD_DOWN = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.2263374f), Y = UI.OffsetItem.Ratio(0.2457002f) };
        private readonly UI.OffsetData IN_ROBOT_LOAD_APPROACH = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.2510288f), Y = UI.OffsetItem.Ratio(0.2309582f) };
        private readonly UI.OffsetData IN_ROBOT_MCR_DOWN = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.3950617f), Y = UI.OffsetItem.Ratio(0.6437346f) };
        private readonly UI.OffsetData IN_ROBOT_MCR_APPROACH = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.4197531f), Y = UI.OffsetItem.Ratio(0.6289926f) };
        private readonly UI.OffsetData IN_ROBOT_UNLOAD_DOWN = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.8230453f), Y = UI.OffsetItem.Ratio(0.4078624f) };
        private readonly UI.OffsetData IN_ROBOT_UNLOAD_APPROACH = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.7983539f), Y = UI.OffsetItem.Ratio(0.3931204f) };
        private readonly UI.OffsetData OUT_ROBOT_INITIALIZE = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.1777778f), Y = UI.OffsetItem.Ratio(0f) };
        private readonly UI.OffsetData OUT_ROBOT_LOAD_DOWN = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.3185185f), Y = UI.OffsetItem.Ratio(0.4068627f) };
        private readonly UI.OffsetData OUT_ROBOT_LOAD_APPROACH = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.362963f), Y = UI.OffsetItem.Ratio(0.3921569f) };
        private readonly UI.OffsetData OUT_ROBOT_UNLOAD_DOWN = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.9481481f), Y = UI.OffsetItem.Ratio(0.02696078f) };
        private readonly UI.OffsetData OUT_ROBOT_UNLOAD_APPROACH = new UI.OffsetData() { X = UI.OffsetItem.Ratio(0.9037037f), Y = UI.OffsetItem.Ratio(0.0122549f) };
        private readonly bool mbIsDesignMode = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormMainFlow(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            // Display 영역에 맞게 Form 크기 피팅
            Size = PnlDisplayArea.Size;
            BackColor = PnlDisplayArea.BackColor;
        }

        public CDialogNotification ShowNotificationDialog(CAlarmDefine.EMessageList messageIndex, params object[] args)
        {
            mDialogNotification.SetMessage(messageIndex, args);
            mDialogNotification.ShouldBeHiden = false;
            if (mDialogNotification.Visible == false)
            {
                mDialogNotification.SetTimer(true);
                mDialogNotification.CenterToOwner();
                mDialogNotification.Show();
            }
            return mDialogNotification;
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
        private void CFormMainFlow_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormMainFlow_FormClosed(object sender, FormClosedEventArgs e)
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

                mbThreadShouldStop = false;
                mThreadUpdatePosition = new Thread(ThreadUpdatePosition);
                mThreadUpdatePosition.Start();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_objSafetyIO.Deinitialize();
            mbThreadShouldStop = true;
            mThreadUpdatePosition.Join(150);
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
                m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 세이프티 IO 초기화
                m_objSafetyIO = new CSafetyIO(m_objDocument);
                if (false == m_objSafetyIO.Initialize())
                {
                    break;
                }
                // 설비 이미지 화면 초기화
                if (false == InitializeEquipmentImage())
                {
                    break;
                }

                InitializeButtonTag();
                InitializePositionInformation();
                InitializeNotificationDialog();
                // 버튼 색상 정의
                SetButtonColor();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ///////////////////////////
            // !OnShown 이벤트가 발생하는 시점에는 AutoScale이 반영되어 컴포넌트 크기 조절이 완료된 시점임
            mDrawPositionList.ForEach(i =>
            {
                i.LockOneCycle();
                i.UpdateLayout();
                i.UnlockOneCycle();
            });
            ///////////////////////////
        }

        private void InitializeButtonTag()
        {
            UcCellInShuttleP1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InShuttleP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.IN_SHUTTLE_VACUUM_P1] });
            UcCellInShuttleP2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InShuttleP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.IN_SHUTTLE_VACUUM_P2] });
            UcCellInRobotP1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InRobotP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P1] });
            UcCellInRobotP2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InRobotP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P2] });
            UcCellInRobot90P1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InRobotP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P1] });
            UcCellInRobot90P2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InRobotP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P2] });
            UcCellInspStageP1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InspStageP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.INSP_STAGE_VACUUM_P1] });
            UcCellInspStageP2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.InspStageP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.INSP_STAGE_VACUUM_P2] });
            UcCellOutRobotP1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.OutRobotP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P1] });
            UcCellOutRobotP2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.OutRobotP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P2] });
            UcCellOutRobot90P1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.OutRobotP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P1] });
            UcCellOutRobot90P2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.OutRobotP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P2] });
            UcCellOutFlipP1.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.OutFlipP1, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P1_1], m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P1_2] });
            UcCellOutFlipP2.BtnCellData.Tag = new CellDataDisplayInformation(m_objDocument, ECellData.OutFlipP2, new CVacuum[] { m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P2_1], m_objDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P2_2] });

            UcCellInShuttleP1.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.IN_SHUTTLE_VACUUM_P1, CDeviceIODefine.EDigitalInput.X_IN_SHUTTLE_P1_CELL_DETECT_SENSOR);
            UcCellInShuttleP2.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.IN_SHUTTLE_VACUUM_P2, CDeviceIODefine.EDigitalInput.X_IN_SHUTTLE_P2_CELL_DETECT_SENSOR);
            UcCellInRobotP1.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P1, CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P1_CELL_DETECT_SENSOR);
            UcCellInRobotP2.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P2, CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P2_CELL_DETECT_SENSOR);
            UcCellInRobot90P1.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P1, CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P1_CELL_DETECT_SENSOR);
            UcCellInRobot90P2.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.IN_ROBOT_VACUUM_P2, CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P2_CELL_DETECT_SENSOR);
            UcCellInspStageP1.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.INSP_STAGE_VACUUM_P1, CDeviceIODefine.EDigitalInput.X_INSP_STAGE_P1_CELL_DETECT_SENSOR);
            UcCellInspStageP2.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.INSP_STAGE_VACUUM_P2, CDeviceIODefine.EDigitalInput.X_INSP_STAGE_P2_CELL_DETECT_SENSOR);
            UcCellOutRobotP1.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P1, CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P1_CELL_DETECT_SENSOR);
            UcCellOutRobotP2.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P2, CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P2_CELL_DETECT_SENSOR);
            UcCellOutRobot90P1.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P1, CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P1_CELL_DETECT_SENSOR);
            UcCellOutRobot90P2.BtnIndicator.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_ROBOT_VACUUM_P2, CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P2_CELL_DETECT_SENSOR);
            UcCellOutFlipP1.BtnIndicatorTop.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P1_1);
            UcCellOutFlipP1.BtnIndicatorLeft.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P1_2);
            UcCellOutFlipP2.BtnIndicatorTop.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P2_1);
            UcCellOutFlipP2.BtnIndicatorLeft.Tag = new VacuumDisplayInformation(m_objDocument, CProcessMotion.EVacuum.OUT_FLIP_VACUUM_P2_2);

            List<Control> allButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            mCellDataButtons = allButtons
                .Where(button => ((SpeedButton)button).Tag is CellDataDisplayInformation)
                .Select(button => (SpeedButton)button)
                .ToList();
            mVacuumButtons = allButtons
                .Where(button => ((SpeedButton)button).Tag is VacuumDisplayInformation)
                .Select(button => (SpeedButton)button)
                .ToList();

            // 근접 센서 UI 초기화
            foreach (var button in mVacuumButtons)
            {
                if (button.Tag is VacuumDisplayInformation info
                    && info.HasDetectSensor == false
                    )
                {
                    continue;
                }
                if (button.Parent.Parent is UcCell ucCell)
                {
                    ucCell.SensorImageSize = new Size(16, 16);
                    ucCell.SensorImageAlign = ContentAlignment.TopRight;
                    ucCell.SensorImageMargin = new Padding(1);
                    ucCell.SensorOnImage = Resources.sensor_on;
                    ucCell.SensorOffImage = Resources.sensor_off;
                }
                else if (button.Parent.Parent is UcCellIndicator ucCellIndicator)
                {
                    ucCellIndicator.SensorImageSize = new Size(16, 16);
                    ucCellIndicator.SensorImageAlign = ContentAlignment.TopRight;
                    ucCellIndicator.SensorImageMargin = new Padding(1);
                    ucCellIndicator.SensorOnImage = Resources.sensor_on;
                    ucCellIndicator.SensorOffImage = Resources.sensor_off;
                }
            }
        }

        private Control[] GetAlignedLocationControlArray(params Control[] controls)
        {
            if (controls.Length < 2)
            {
                return controls;
            }

            Control firstControl = controls[0];
            Point leftTopPoint = firstControl.Location;
            foreach (var control in controls)
            {
                control.Location = leftTopPoint;
            }
            return controls;
        }

        private void InitializePositionInformation()
        {
            var addItem = new DrawComponentGroupPosition(
                PnlInShuttleBoundary,
                new Control[] { PnlBaseInShuttle }
                );
            addItem.RequestGetRatioPositionX += new Func<double>(() => DrawComponentGroupPosition.GetRatioPositionFromMotorObject(m_objDocument.m_objProcessMain.m_objProcessMotion.InShuttle.MotorStageX.Axis));
            mDrawPositionList.Add(addItem);

            addItem = new DrawComponentGroupPosition(
                PnlInRobotBoundary,
                GetAlignedLocationControlArray(PnlBaseInRobot, PnlBaseInRobot90)
                );
            addItem.Tag = CProcessMotion.ERobot.IN_ROBOT;
            addItem.RequestGetOffsetData += new Func<UI.OffsetData>(() =>
            {
                UI.OffsetData result = null;
                var nachi = m_objDocument.m_objProcessMain.m_objProcessMotion.InRobot.Nachi;
                var command = nachi.GetCommand();
                if (InRobotNachi.ECommand.Idle == command)
                {
                    var status = nachi.GetStatus();
                    switch (status)
                    {
                        case InRobotNachi.EStatus.Unknown:
                        case InRobotNachi.EStatus.Initialize:
                            result = IN_ROBOT_INITIALIZE;
                            break;
                        // P1
                        case InRobotNachi.EStatus.LoadProcessBegin:
                        case InRobotNachi.EStatus.LoadProcessExit:
                            result = IN_ROBOT_LOAD_APPROACH;
                            break;
                        case InRobotNachi.EStatus.LoadPermit:
                        case InRobotNachi.EStatus.LoadCycleEnd:
                            result = IN_ROBOT_LOAD_DOWN;
                            break;
                        // P3
                        case InRobotNachi.EStatus.McrProcessBegin:
                        case InRobotNachi.EStatus.McrProcessExit:
                            result = IN_ROBOT_MCR_APPROACH;
                            break;
                        case InRobotNachi.EStatus.McrPermit:
                        case InRobotNachi.EStatus.McrCycleEnd:
                            result = IN_ROBOT_MCR_DOWN;
                            break;
                        // P4
                        case InRobotNachi.EStatus.UnloadProcessBegin:
                        case InRobotNachi.EStatus.UnloadProcessExit:
                            result = IN_ROBOT_UNLOAD_APPROACH;
                            break;
                        case InRobotNachi.EStatus.UnloadPermit:
                        case InRobotNachi.EStatus.UnloadCycleEnd:
                            result = IN_ROBOT_UNLOAD_DOWN;
                            break;
                    }
                }
                else
                {
                    switch (command)
                    {
                        case InRobotNachi.ECommand.Initialize:
                            result = IN_ROBOT_INITIALIZE;
                            break;
                        // P1
                        case InRobotNachi.ECommand.LoadProcessBegin:
                        case InRobotNachi.ECommand.LoadProcessExit:
                            result = IN_ROBOT_LOAD_APPROACH;
                            break;
                        case InRobotNachi.ECommand.LoadPermit:
                        case InRobotNachi.ECommand.LoadCycleEnd:
                            result = IN_ROBOT_LOAD_DOWN;
                            break;
                        // P3
                        case InRobotNachi.ECommand.McrProcessBegin:
                        case InRobotNachi.ECommand.McrProcessExit:
                            result = IN_ROBOT_MCR_APPROACH;
                            break;
                        case InRobotNachi.ECommand.McrPermit:
                        case InRobotNachi.ECommand.McrCycleEnd:
                            result = IN_ROBOT_MCR_DOWN;
                            break;
                        // P4
                        case InRobotNachi.ECommand.UnloadProcessBegin:
                        case InRobotNachi.ECommand.UnloadProcessExit:
                            result = IN_ROBOT_UNLOAD_APPROACH;
                            break;
                        case InRobotNachi.ECommand.UnloadPermit:
                        case InRobotNachi.ECommand.UnloadCycleEnd:
                            result = IN_ROBOT_UNLOAD_DOWN;
                            break;
                    }
                }
                return result;
            });
            mDrawPositionList.Add(addItem);

            addItem = new DrawComponentGroupPosition(
                PnlInspStageBoundary,
                new Control[] { PnlBaseInspStage }
                );
            addItem.RequestGetRatioPositionY += new Func<double>(() => DrawComponentGroupPosition.GetRatioPositionFromMotorObject(m_objDocument.m_objProcessMain.m_objProcessMotion.InspStage.MotorStageY.Axis));
            addItem.VerticalFlip = true;
            mDrawPositionList.Add(addItem);

            addItem = new DrawComponentGroupPosition(
                PnlOutRobotBoundary,
                GetAlignedLocationControlArray(PnlBaseOutRobot, PnlBaseOutRobot90)
                );
            addItem.Tag = CProcessMotion.ERobot.OUT_ROBOT;
            addItem.RequestGetOffsetData += new Func<UI.OffsetData>(() =>
            {
                UI.OffsetData result = null;
                var nachi = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot.Nachi;
                var command = nachi.GetCommand();
                if (OutRobotNachi.ECommand.Idle == command)
                {
                    var status = nachi.GetStatus();
                    switch (status)
                    {
                        case OutRobotNachi.EStatus.Unknown:
                        case OutRobotNachi.EStatus.Initialize:
                            result = OUT_ROBOT_INITIALIZE;
                            break;
                        // P1
                        case OutRobotNachi.EStatus.LoadProcessBegin:
                        case OutRobotNachi.EStatus.LoadProcessExit:
                            result = OUT_ROBOT_LOAD_APPROACH;
                            break;
                        case OutRobotNachi.EStatus.LoadPermit:
                        case OutRobotNachi.EStatus.LoadCycleEnd:
                            result = OUT_ROBOT_LOAD_DOWN;
                            break;
                        // P4
                        case OutRobotNachi.EStatus.UnloadProcessBegin:
                        case OutRobotNachi.EStatus.UnloadProcessExit:
                            result = OUT_ROBOT_UNLOAD_APPROACH;
                            break;
                        case OutRobotNachi.EStatus.UnloadPermit:
                        case OutRobotNachi.EStatus.UnloadCycleEnd:
                            result = OUT_ROBOT_UNLOAD_DOWN;
                            break;
                    }
                }
                else
                {
                    switch (command)
                    {
                        case OutRobotNachi.ECommand.Initialize:
                            result = OUT_ROBOT_INITIALIZE;
                            break;
                        // P1
                        case OutRobotNachi.ECommand.LoadProcessBegin:
                        case OutRobotNachi.ECommand.LoadProcessExit:
                            result = OUT_ROBOT_LOAD_APPROACH;
                            break;
                        case OutRobotNachi.ECommand.LoadPermit:
                        case OutRobotNachi.ECommand.LoadCycleEnd:
                            result = OUT_ROBOT_LOAD_DOWN;
                            break;
                        // P4
                        case OutRobotNachi.ECommand.UnloadProcessBegin:
                        case OutRobotNachi.ECommand.UnloadProcessExit:
                            result = OUT_ROBOT_UNLOAD_APPROACH;
                            break;
                        case OutRobotNachi.ECommand.UnloadPermit:
                        case OutRobotNachi.ECommand.UnloadCycleEnd:
                            result = OUT_ROBOT_UNLOAD_DOWN;
                            break;
                    }
                }
                return result;
            });
            mDrawPositionList.Add(addItem);

            addItem = new DrawComponentGroupPosition(
                PnlOutConveyorBoundary,
                 new Control[] { BtnCellOutConveyorP1, BtnCellOutConveyorP2 }
                );
            addItem.RequestGetRatioPositionX += new Func<double>(() =>
            {
                bool isArrivalUnloadPosition = m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.MotorConveyorX.IsArrivalUnloadPosition();
                SetControlVisible(BtnCellOutConveyorP1, isArrivalUnloadPosition == false);
                SetControlVisible(BtnCellOutConveyorP2, isArrivalUnloadPosition == false);
                return DrawComponentGroupPosition.GetRatioPositionFromMotorObject(m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.MotorConveyorX.Axis);
            });
            mDrawPositionList.Add(addItem);
        }

        private void InitializeNotificationDialog()
        {
            mDialogNotification = new CDialogNotification(m_objDocument);
            mDialogNotification.Owner = this;
            mDialogNotification.TopLevel = false;
            PnlDisplayArea.Controls.Add(mDialogNotification);
            mDialogNotification.BringToFront();
        }

        /// <summary>
        /// 설비 이미지 화면 초기화
        /// </summary>
        /// <returns></returns>
        private bool InitializeEquipmentImage()
        {
            bool bReturn = false;

            do
            {
                // 이미지 투명도
                PnlEquipmentImage.BackgroundImage = SetImageOpacity(Properties.Resources.TOP_VIEW_FLOW, 0.3f);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설비 이미지 투명하게
        /// </summary>
        /// <param name="imgPic"></param>
        /// <param name="imagOpac"></param>
        /// <returns></returns>
        private static Image SetImageOpacity(Image imgPic, float imagOpac)
        {
            Bitmap bmp = new Bitmap(imgPic.Width, imgPic.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                ColorMatrix colormatrix = new ColorMatrix();
                colormatrix.Matrix33 = imagOpac;
                ImageAttributes imgAttribute = new ImageAttributes();
                imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(imgPic, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imgAttribute);
            }
            return bmp;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnStart, m_colorNormal);
            SetButtonBackColor(BtnStop, m_colorNormal);
            SetButtonBackColor(BtnLotEnd, m_colorNormal);

            SetButtonBackColor(BtnTitleSafetyKey, m_colorLabelSub);
            SetButtonBackColor(BtnSafetyKeyAutoMode, m_colorNormal);

            SetButtonBackColor(BtnCellOut, m_colorNormal);

            SetButtonBackColor(BtnTitleRunMode, m_colorLabelSub);
            SetButtonBackColor(BtnTitleRunModeType, m_colorLabelSub);
            SetButtonBackColor(BtnTitleEquipmentID, m_colorLabelSub);
            SetButtonBackColor(BtnTitlePpid, m_colorLabelSub);

            SetButtonBackColor(BtnEquipmentBase, m_colorLabelCategory);
            pnlDescriptionBase.BackColor = m_colorLabelCategory;
            SetButtonBackColor(BtnTitleIn, m_colorLabelCategory);
            SetButtonBackColor(BtnTitleOut, m_colorLabelCategory);
            SetButtonBackColor(BtnTitleWork, m_colorLabelCategory);

            SetButtonBackColor(BtnIn, mLoadColor);
            SetButtonBackColor(BtnWork, mProcessingColor);
            SetButtonBackColor(BtnOut, mUnloadColor);

            SetButtonBackColor(BtnTitleLoadSafety, m_colorLabelSub);
            SetButtonBackColor(BtnTitleLoadHandshake, m_colorLabelSub);
            SetButtonBackColor(BtnTitleUnloadHandshake, m_colorLabelSub);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(UcCell.BtnCellData))
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
                    SetControlButtonEnable(Controls, true);
                    break;
                case CDefine.EUserAuthorityLevel.ENGINEER:
                    SetControlButtonEnable(Controls, true);
                    break;
                case CDefine.EUserAuthorityLevel.MASTER:
                    SetControlButtonEnable(Controls, true);
                    break;
                default:
                    break;
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
                SetButtonChangeLanguage(BtnStart);
                SetButtonChangeLanguage(BtnStop);
                SetButtonChangeLanguage(BtnLotEnd);
                SetButtonChangeLanguage(BtnTitleSafetyKey);

                SetButtonChangeLanguage(BtnEquipmentBase);
                SetButtonChangeLanguage(BtnTitleRunMode);
                SetButtonChangeLanguage(BtnTitleRunModeType);
                SetButtonChangeLanguage(BtnTitleOut);
                SetButtonChangeLanguage(BtnTitleWork);
                SetButtonChangeLanguage(BtnTitleIn);
                SetButtonChangeLanguage(BtnCellOut);

                SetButtonChangeLanguage(BtnTitleLoadSafety);
                SetButtonChangeLanguage(BtnTitleLoadHandshake);
                SetButtonChangeLanguage(BtnLoadPending);
                SetButtonChangeLanguage(BtnLoadHandshaking);
                SetButtonChangeLanguage(BtnTitleUnloadHandshake);

                mDialogNotification?.SetChangeLanguage();

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
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objLabel"></param>
        private void SetLabelChangeLanguage(Label objLabel)
        {
            string uiText = m_objDocument.GetDatabaseUIText(objLabel.Name, Name);
            if (objLabel.Text != uiText)
            {
                objLabel.Text = uiText;
            }
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
            mDialogNotification?.SetTimer(bTimer);
            if (bTimer == true)
            {
                Timer_Tick(timer, EventArgs.Empty);
            }
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
            }
            mDialogNotification?.SetVisible(bVisible);
        }

        /// <summary>
        /// Device 상태 정보 갱신
        /// </summary>
        private void SetUpdateDeviceUI()
        {
        }

        /// <summary>
        /// 설비 상태에 따른 UI 변경
        /// </summary>
        private void SetEquipmentStatusUI()
        {
            if (
                CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.Setup == m_objDocument.GetRunStatus()
                || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                )
            {
                SetButtonBackColor(BtnStart, m_colorClick);
                SetButtonBackColor(BtnStop, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnStart, m_colorNormal);
                SetButtonBackColor(BtnStop, m_colorClick);
            }

            if (CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus())
            {
                SetButtonBackColor(BtnLotEnd, m_colorClick);
            }
            else
            {
                SetButtonBackColor(BtnLotEnd, m_colorNormal);
            }
        }

        /// <summary>
        /// 설비 운용 모드 UI 변경
        /// </summary>
        private void SetEquipmentModeStatus()
        {
            // 설비 런 모드 표시
            SetButtonText(BtnRunMode, m_objDocument.GetRunStatus().ToString().Replace("RUN_MODE_", ""));
            SetButtonBackColor(BtnRunMode, m_colorLabelData);
            // 설비 런 모드 타입 표시
            SetButtonText(BtnRunModeType, m_objDocument.GetRunMode().ToString().Replace("RUN_MODE_TYPE_", ""));
            SetButtonBackColor(BtnRunModeType, m_colorLabelData);
            // EQP ID 표시
            SetButtonText(BtnEquipmentID, m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID);
            // PPID 표시
            string PPID = m_objDocument.m_objConfig.GetSystemParameter().strPPID;
            SetButtonText(BtnPpid, $"{PPID}");
        }

        /// <summary>
        /// 설비 세이프티 메뉴얼, 오토에 따른 UI 변경
        /// </summary>
        private void SetEquipmentSafetyUI()
        {
            // AUTO 모드인 경우
            if (CDefine.ESafetyKey.SAFETY_KEY_AUTO == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT)
                && CDefine.ESafetyKey.SAFETY_KEY_AUTO == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR)
                )
            {
                SetButtonBackColor(BtnSafetyKeyAutoMode, m_colorClick);
                SetButtonText(BtnSafetyKeyAutoMode, "AUTO");
            }
            else
            {
                SetButtonBackColor(BtnSafetyKeyAutoMode, m_colorNormal);
                SetButtonText(BtnSafetyKeyAutoMode, "TEACH");
            }

            if (
                CDefine.ESafetyKey.SAFETY_KEY_TEACH == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT)
                || CDefine.ESafetyKey.SAFETY_KEY_TEACH == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR)
                )
            {
                // Teach Mode 일경우 Cell out 버튼 보이도록하자.
                if (BtnCellOut.Visible != true)
                {
                    BtnCellOut.Visible = true;
                }
            }
            else
            {
                // Auto Mode 일경우 Cell out 버튼 숨김
                if (BtnCellOut.Visible != false)
                {
                    BtnCellOut.Visible = false;
                }
            }
        }

        /// <summary>
        /// 상류 장비와 인터페이스 상태 표시
        /// </summary>
        private void SetEquipmentInterfaceUpper()
        {
            var signalControllers = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.SignalControllers[0] as LoadInterfaceSignalController;
            SetButtonBackColor(UcHandshakeSignalLoadSafetyItem1.BtnIndicator1, signalControllers.UpperBits.EmsSafe.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadSafetyItem2.BtnIndicator1, signalControllers.UpperBits.InterlockDoorClosed.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadSafetyItem3.BtnIndicator1, signalControllers.UpperBits.InterlockSafe.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem1.BtnIndicator1, signalControllers.UpperBits.Heartbeat.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem2.BtnIndicator1, signalControllers.UpperBits.SendAble.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem3.BtnIndicator1, signalControllers.UpperBits.SendStart.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem4.BtnIndicator1, signalControllers.UpperBits.SendComplete.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem5.BtnIndicator1, signalControllers.UpperBits.SendVacuumOnP1.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem6.BtnIndicator1, signalControllers.UpperBits.SendVacuumOnP2.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem7.BtnIndicator1, signalControllers.UpperBits.SendCellP1.Value ? m_colorInputOn : m_colorInputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem8.BtnIndicator1, signalControllers.UpperBits.SendCellP2.Value ? m_colorInputOn : m_colorInputOff);

            SetButtonBackColor(UcHandshakeSignalLoadSafetyItem1.BtnIndicator2, signalControllers.SelfBits.EmsSafe.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadSafetyItem2.BtnIndicator2, signalControllers.SelfBits.InterlockDoorClosed.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadSafetyItem3.BtnIndicator2, signalControllers.SelfBits.InterlockSafe.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem1.BtnIndicator2, signalControllers.SelfBits.Heartbeat.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem2.BtnIndicator2, signalControllers.SelfBits.ReceiveAble.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem3.BtnIndicator2, signalControllers.SelfBits.ReceiveStart.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem4.BtnIndicator2, signalControllers.SelfBits.ReceiveComplete.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem5.BtnIndicator2, signalControllers.SelfBits.ReceiveVacuumOnP1.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem6.BtnIndicator2, signalControllers.SelfBits.ReceiveVacuumOnP2.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem7.BtnIndicator2, signalControllers.SelfBits.ReceiveCellP1.Value ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalLoadItem8.BtnIndicator2, signalControllers.SelfBits.ReceiveCellP2.Value ? m_colorOutputOn : m_colorOutputOff);

            SetButtonBackColor(BtnLoadHandshaking, m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.IsHandShaking ? m_colorOn : m_colorNormal);
            SetButtonBackColor(BtnLoadPending, m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.IsPending ? m_colorOff : m_colorNormal);
        }

        /// <summary>
        /// 하류 장비와 인터페이스 상태 표시
        /// </summary>
        private void SetEquipmentInterfaceLower()
        {
            SetButtonBackColor(UcHandshakeSignalUnloadItem1.BtnIndicator1, m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.IsExternalConveyorRun ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(UcHandshakeSignalUnloadItem1.BtnIndicator2, m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_LOWER_CONVEYOR_RUN_ABLE) ? m_colorInputOn : m_colorInputOff);
        }

        /// <summary>
        /// 목적 : 셀 상태 표시
        /// </summary>
        private void SetCellStatusDisplay()
        {
            bool bIsManualTrackOut = false;
            // Cell Data
            foreach (SpeedButton button in mCellDataButtons)
            {
                var cellInformation = button.Tag as CellDataDisplayInformation;
                if (null != cellInformation)
                {
                    Color backgroundColor = Color.WhiteSmoke;
                    Color foreColor = Color.Black;
                    if (
                        cellInformation.CellDataHandler.IsCellExist() == true
                        && cellInformation.IsManualTrackOut == false
                        )
                    {
                        switch (cellInformation.CellDataHandler.Data.Cell.FrontInspResult)
                        {
                            case EInspectionResult.BIN1:
                                foreColor = Color.Blue;
                                break;

                            case EInspectionResult.REJECT:
                            case EInspectionResult.BIN2:
                            case EInspectionResult.BIN3:
                                foreColor = Color.FromArgb(211, 57, 50);
                                break;

                            default:
                                break;
                        }
                        switch (cellInformation.CellDataHandler.ProcessIndex)
                        {
                            case EProcess.InShuttle:
                                {
                                    backgroundColor = mUnloadColor;
                                    if (cellInformation.CellDataHandler.Data.Cell.PreAlignStatus != EStatus.Done)
                                    {
                                        backgroundColor = mProcessingColor;
                                    }
                                }
                                break;
                            case EProcess.InRobot:
                                {
                                    backgroundColor = mUnloadColor;
                                    if (cellInformation.CellDataHandler.Data.Cell.McrStatus != EStatus.Done)
                                    {
                                        backgroundColor = mProcessingColor;
                                    }
                                }
                                break;
                            case EProcess.InspStage:
                                {
                                    backgroundColor = mUnloadColor;
                                    if (cellInformation.CellDataHandler.Data.Cell.InspStatus != EStatus.Done)
                                    {
                                        backgroundColor = mProcessingColor;
                                    }
                                }
                                break;
                            case EProcess.OutRobot:
                                {
                                    backgroundColor = mUnloadColor;
                                    if (cellInformation.CellDataHandler.Data.Cell.RearInspResult == EInspectionResult.None)
                                    {
                                        backgroundColor = mProcessingColor;
                                    }
                                }
                                break;
                            case EProcess.OutFlip:
                                {
                                    backgroundColor = mUnloadColor;
                                }
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                    else if (
                        cellInformation.CellDataHandler.IsCellExist() == true
                        && cellInformation.IsManualTrackOut == true
                        )
                    {
                        backgroundColor = Color.Plum;
                        bIsManualTrackOut = true;
                    }

                    SetButtonBackColor(button, backgroundColor);
                    SetButtonForeColor(button, foreColor);
                }
            }
            // Cell Out
            SetButtonBackColor(BtnCellOut, bIsManualTrackOut == true ? m_colorOn : m_colorNormal, bIsManualTrackOut);

            // Vacuum
            foreach (SpeedButton button in mVacuumButtons)
            {
                var vacuumInformation = button.Tag as VacuumDisplayInformation;
                if (null != vacuumInformation)
                {
                    SetButtonBackColor(button, vacuumInformation.GetBackgroundColor());
                }
                if (button.Parent.Parent is UcCell ucCell)
                {
                    if (null != vacuumInformation)
                    {
                        ucCell.IsSensorOn = vacuumInformation.IsDetectSensorOn();
                    }
                }
                else if (button.Parent.Parent is UcCellIndicator ucCellIndicator)
                {
                    if (null != vacuumInformation)
                    {
                        ucCellIndicator.IsSensorOn = vacuumInformation.IsDetectSensorOn();
                    }
                }
            }

            // Sensor
            SetButtonBackColor(BtnSensorDetectP1, m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.Sensor.IsCellDetectSensor1 ? m_colorOn : m_colorNormal);
            SetButtonBackColor(BtnSensorDetectP2, m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.Sensor.IsCellDetectSensor2 ? m_colorOn : m_colorNormal);
            SetButtonBackColor(BtnSensorBlockedDetect, m_objDocument.m_objProcessMain.m_objProcessMotion.OutFlip.Sensor.IsConveyorBlockedSensor ? m_colorOn : m_colorNormal);
        }

        private void ClearDataDeleteFlags()
        {
            foreach (var button in mCellDataButtons)
            {
                var cellDataDisplayInformation = (CellDataDisplayInformation)button.Tag;
                if (cellDataDisplayInformation != null)
                {
                    cellDataDisplayInformation.ResetManualTrackOut();
                }
            }
        }

        /// <summary>
        /// 목적 : 설비 런
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (
                m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsBatchMoving() == true
                || m_objDocument.m_objProcessMain.m_objProcessMotion.CheckAllSequenceManagerStateIsIdle() == false
                )
            {
                return;
            }
            // 시작버튼 중복 실행 방지
            if (true == m_bIsBusy)
            {
                return;
            }

            do
            {
                m_bIsBusy = true;

                /************************************************************************ 
                 * 실제 런시 오퍼레이터가 마스터 모드로 조치 후 그대로 런을 돌려서 예기치 못한 문제가 
                 * 발생하는 경우가 있어서 자동 로그아웃 플래그를 셋업 관련 플래그로 보고 강제로 마스터 
                 * 권한을 풀어줌
                 ************************************************************************/
                if (m_objDocument.IsAutoLogoutNotUsed == false)
                {
                    m_objDocument.IsMasterLogin = false;
                }

                // 설비 런 모드 상태
                if (
                    CDefine.ERunStatus.Start == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.Setup == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.LoadingStop == m_objDocument.GetRunStatus()
                    || CDefine.ERunStatus.Initialize == m_objDocument.GetRunStatus()
                    )
                {
                    // Msg: 설비 자동 운전 상태 입니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.MACHINE_IS_AUTO_RUN_STATUS);
                    break;
                }

                // 공유 메모리 내에 클리어 되지 않은 알람이 있는 경우 튕김
                if (true == m_objDocument.GetIsAlarmMessage())
                {
                    // Msg: 설비 알람 상태를 확인하여 주십시오.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.CHECK_THE_EQUIPMENT_ALARM_STATUS);
                    m_objDocument.SetChangeAlarmForm();
                    break;
                }

                // 마스터 권한인 경우 조건 안 봄
                if (false == m_objDocument.IsMasterLogin)
                {
                    // 설비 세이프티 오토 상태 체크
                    // 정면 or 후면 세이프티 키가 오토 상태가 아닌 경우
                    if (
                        CDefine.ESafetyKey.SAFETY_KEY_AUTO != m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT)
                        || CDefine.ESafetyKey.SAFETY_KEY_AUTO != m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR)
                        )
                    {
                        // Msg: 설비 도어를 잠그고 세이프티 키 상태를 오토로 변경해 주십시오.
                        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOCK_THE_EQUIPMENT_DOOR_AND_CHANGE_THE_SAFETY_KEY_STATUS_TO_AUTO);
                        break;
                    }
                }

                // 설비 환경 상태 확인
                if (false == m_objDocument.m_objProcessMain.CheckEqpStatus())
                {
                    m_objDocument.SetChangeInitializeForm();
                    break;
                }

                // 설비 모터 상태 확인
                if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    if (false == m_objDocument.m_objProcessMain.m_objProcessMotion.CheckServoStatus())
                    {
                        m_objDocument.SetChangeInitializeForm();
                        break;
                    }
                }

                // 설비 런 전에 셀 데이터 및 실물 일치 상태 확인
                if (false == m_objDocument.m_objProcessMain.m_objProcessMotion.CheckPreConditionBeforeRun())
                {
                    break;
                }

                // 초기화 상태 확인
                if (false == m_objDocument.m_objProcessMain.m_objProcessMotion.CheckInitialize())
                {
                    m_objDocument.SetChangeInitializeForm();
                    break;
                }

                // 드라이런 모드 경고
                if (m_objDocument.GetRunMode() != CDefine.ERunMode.RealRun)
                {
                    MessageBox.Show(m_objDocument.GetUserMessage(CAlarmDefine.EMessageList.WARNING_DEVICE_OPERATING_MODE_SET_PARAM2, "EQUIPMENT", "OP_MODE_DRY_RUN"), "[ WARNING ]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // 외관검사기 스킵 모드 경고
                else if (Enum.GetValues(typeof(CDefine.EInspectType)).Cast<CDefine.EInspectType>().Any(x => m_objDocument.m_objConfig.GetInspOptionParameter(x).eInspectionMode != CConfig.CInspOptionParameter.EInspectionMode.INSPECT_USE))
                {
                    MessageBox.Show(m_objDocument.GetUserMessage(CAlarmDefine.EMessageList.WARNING_DEVICE_OPERATING_MODE_SET_PARAM2, "INSPECT", "INSP_MODE_SKIP"), "[ WARNING ]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 가상 구성 사용중 경고
                if (m_objDocument.m_objConfig.UsingVirtualConfig == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_WARNING, CAlarmDefine.EMessageList.USING_VIRTUAL_CONFIG);
                }

                // 설비 동작 유무
                // Msg: 시작하시겠습니까?
                if (DialogResult.No == m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_START))
                {
                    break;
                }

                if (
                    true == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                    && CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                    && CDefine.ERunMode.RealRun == m_objDocument.GetRunMode()
                    )
                {
                    // 심 연결 안되어 있으면 연결하자.
                    if (
                        false == m_objDocument.m_bCIMConnected
                        || CCIMDefine.EControlState.CRST_ONLINE_REMOTE != m_objDocument.m_eControlState
                        )
                    {
                        m_objDocument.GetMainFrame().ShowCIMConnect(true);
                        while (true)
                        {
                            if (true == m_objDocument.GetMainFrame().m_objDialogCIMConnectWait.IsCancel())
                            {
                                m_bIsBusy = false;
                                return;
                            }
                            if (
                                true == m_objDocument.m_bCIMConnected
                                && CCIMDefine.EControlState.CRST_ONLINE_REMOTE == m_objDocument.m_eControlState
                                )
                            {
                                break;
                            }
                            Application.DoEvents();
                            Thread.Sleep(10);
                        }
                        m_objDocument.GetMainFrame().ShowCIMConnect(false);
                    }
                }

                // 오토 속도 전환
                m_objDocument.SetMoveModeType(CDefine.EMoveModeType.MOVE_MODE_TYPE_AUTO);
                // 설비 시작 상태로 변경
                m_objDocument.SetRunStatus(CDefine.ERunStatus.Setup);

                if (
                    true == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                    && CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode
                    && CDefine.ERunMode.RealRun == m_objDocument.GetRunMode()
                    )
                {
                    // System 담당자 요청사항
                    // 현재 Recipe가 상위에 등록이 안되있을경우 OCAP 발생 (V3 미대전용 시나리오)
                    m_objDocument.m_objRecipe.SetCurrentRecipeReportCim();
                    Thread.Sleep(500);
                }

                m_objDocument.m_objProcessMain.m_objProcessMotion.SetRestartCommand(CProcessAbstract.EProcessManagerRestart.PROCESS_MANAGER_RESTART_START);
                // 설비 상태가 시작되면 셀아웃 초기화.
                ClearDataDeleteFlags();
                m_objDocument.MfqInputCount = 0;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [RunMode : {1}]", "BtnStart_Click", CDefine.ERunStatus.Start.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);

            } while (false);

            m_bIsBusy = false;
        }

        /// <summary>
        /// 설비 정지 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (
                CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.Setup != m_objDocument.GetRunStatus()
                )
            {
                // 설비 정지 상태로 변경
                m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                m_objDocument.m_bForceStopButton = true;
                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [RunMode : {1}]", "BtnStop_Click", CDefine.ERunStatus.Stopping.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        /// <summary>
        /// 사이클 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCycleStop_Click(object sender, EventArgs e)
        {
            if (
                CDefine.ERunStatus.Stop != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.LoadingStop != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                && CDefine.ERunStatus.Setup != m_objDocument.GetRunStatus()
                && CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF == m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE]
                )
            {
                if (DialogResult.Yes != m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_LOT_END))
                    return;

                if (CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF == m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
                {
                    // 설비 정지중 상태로 변경
                    m_objDocument.SetRunStatus(CDefine.ERunStatus.LoadingStop);
                    m_objDocument.m_bForceStopButton = true;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0}] [RunMode : {1}]", "BtnCycleStop_Click", CDefine.ERunStatus.LoadingStop.ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        /// <summary>
        /// 목적 : 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 설비 상태에 따른 UI 변경
            SetEquipmentStatusUI();
            Thread.Sleep(0);
            // 설비 세이프티 메뉴얼, 오토에 따른 UI 변경
            SetEquipmentSafetyUI();
            Thread.Sleep(0);
            // 앞 장비와 인터페이스 상태 표시
            SetEquipmentInterfaceUpper();
            Thread.Sleep(0);
            // 뒷 장비와 인터페이스 상태 표시
            SetEquipmentInterfaceLower();
            Thread.Sleep(0);
            // 셀 정보 표시
            SetCellStatusDisplay();
            Thread.Sleep(0);
            // 설비 운용 모드 표시
            SetEquipmentModeStatus();
            Thread.Sleep(0);
            SetUpdateDeviceUI();
            Thread.Sleep(0);
            if (swBlinkSignal.Elapsed > mBlinkTimeSpan)
            {
                swBlinkSignal.Restart();
                mbBlinkSignal = !mbBlinkSignal;
            }

            if (mDialogNotification != null
                && mDialogNotification.ShouldBeHiden == true
                && mDialogNotification.Visible == true
                )
            {
                mDialogNotification.CloseDialog();
            }
        }

        private void SetPositionInformationUpdate()
        {
            mDrawPositionList.ForEach(item =>
            {
                item.Reflesh();
                item.IsRuntimeDesignMode = mbIsDesignMode;
            });

            BtnMfqInputBase.Visible = m_objDocument.IsManualInputMode;

            if (true == m_objDocument.IsManualInputMode)
            {
                SetButtonText(BtnMfqInputBase, string.Format("MANUAL INPUT ( COUNT : {0} )", m_objDocument.MfqInputCount));
            }

            // In/Out Robot 90도 회전 여부에 따른 UI 변경
            {
                bool bIsInRobotTurned = false;
                var inRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.InRobot.Nachi;
                if (inRobot.GetCommand() == InRobotNachi.ECommand.Idle)
                {
                    switch (inRobot.GetStatus())
                    {
                        case InRobotNachi.EStatus.UnloadProcessBegin:
                        case InRobotNachi.EStatus.UnloadPermit:
                        case InRobotNachi.EStatus.UnloadCycleEnd:
                        case InRobotNachi.EStatus.UnloadProcessExit:
                            bIsInRobotTurned = true;
                            break;
                    }
                }
                else
                {
                    switch (inRobot.GetCommand())
                    {
                        case InRobotNachi.ECommand.UnloadProcessBegin:
                        case InRobotNachi.ECommand.UnloadPermit:
                        case InRobotNachi.ECommand.UnloadCycleEnd:
                        case InRobotNachi.ECommand.UnloadProcessExit:
                            bIsInRobotTurned = true;
                            break;
                    }
                }
                bool bIsOutRobotTurned = false;
                var outRobot = m_objDocument.m_objProcessMain.m_objProcessMotion.OutRobot.Nachi;
                if (outRobot.GetCommand() == OutRobotNachi.ECommand.Idle)
                {
                    switch (outRobot.GetStatus())
                    {
                        case OutRobotNachi.EStatus.LoadProcessBegin:
                        case OutRobotNachi.EStatus.LoadPermit:
                        case OutRobotNachi.EStatus.LoadCycleEnd:
                        case OutRobotNachi.EStatus.LoadProcessExit:
                            bIsOutRobotTurned = true;
                            break;
                    }
                }
                else
                {
                    switch (outRobot.GetCommand())
                    {
                        case OutRobotNachi.ECommand.LoadProcessBegin:
                        case OutRobotNachi.ECommand.LoadPermit:
                        case OutRobotNachi.ECommand.LoadCycleEnd:
                        case OutRobotNachi.ECommand.LoadProcessExit:
                            bIsOutRobotTurned = true;
                            break;
                    }
                }

                SetControlVisible(PnlBaseInRobot, mbIsDesignMode || !bIsInRobotTurned);
                SetControlVisible(PnlBaseInRobot90, mbIsDesignMode || bIsInRobotTurned);
                SetControlVisible(PnlBaseOutRobot, mbIsDesignMode || !bIsOutRobotTurned);
                SetControlVisible(PnlBaseOutRobot90, mbIsDesignMode || bIsOutRobotTurned);
            }
        }

        /// <summary>
        /// 목적 : CELL OUT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCellOut_Click(object sender, EventArgs e)
        {
            string strLog = string.Empty;
            strLog = string.Format("[{0}] [{1}]", "BtnCellOut_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            var cellDataDisplayInformations = mCellDataButtons
                .Where(item => item.Tag is CellDataDisplayInformation)
                .Select(item => item.Tag as CellDataDisplayInformation)
                .Where(item => item.IsManualTrackOut == true)
                .ToArray();
            if (cellDataDisplayInformations.Length == 0)
            {
                return;
            }

            if (m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_RUN_CELL_OUT) == DialogResult.No)
            {
                return;
            }

            foreach (var item in cellDataDisplayInformations)
            {
                if (item.CellDataHandler.Data.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK)
                {
                    item.CellDataHandler.Data.Judgement.Judge = item.ManualTrackOutJudge;
                    item.CellDataHandler.Data.Judgement.Description = item.ManualTrackOutDescription;
                    item.CellDataHandler.Data.Judgement.ReasonCode = item.ManualTrackOutReasonCode;
                    m_objDocument.m_objProcessMain.m_objProcessMotion.TrackOut.ManualTrackOutRequestPushInQueue(item.CellDataHandler.Data.DeepClone());
                }

                // 데이터 초기화
                item.VacuumOrNull?.Where(v => v != null).ToList().ForEach(v => v.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE));
                CellDataManager.DataReset(item.CellDataHandler);

                strLog = string.Format("[{0}] [Process : {1}] [Position : {2}] [{3}]", "BtnCellOut_Click", item.CellDataHandler.ProcessIndex, item.CellDataHandler.PositionIndex, "Data Reset");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }

            // CELL이 다른 프로세스로 넘어가는 도중에 알람이 발생 했을 경우 Cell ID와 Inner ID를 클리어 해준다.
            foreach (var cellDataHandler in CellDataManager.Cells.Values)
            {
                if (cellDataHandler.IsCellExist() == false)
                {
                    cellDataHandler.Data.Cell.InnerID = string.Empty;
                    cellDataHandler.Data.Cell.CellID = string.Empty;
                }
            }

            // Clear All
            ClearDataDeleteFlags();

            strLog = string.Format("[{0}] [{1}]", "BtnCellOut_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        /// <summary>
        /// 목적 : 시뮬레이션 모드에서 세이프티 키 AUTO 상태로 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSafetyKeyAutoMode_Click(object sender, EventArgs e)
        {
            do
            {
                // 시뮬레이션 모드가 아니면 튕김
                if (CDefine.ESimulationMode.SIMULATION_MODE_ON != m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
                {
                    break;
                }
                // 정면 세이프티 키 AUTO 상태로 전환
                var frontCurrentStatus = m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT);
                var rearCurrentStatus = m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR);
                if (
                    frontCurrentStatus == CDefine.ESafetyKey.SAFETY_KEY_AUTO
                    && rearCurrentStatus == CDefine.ESafetyKey.SAFETY_KEY_AUTO
                    )
                {
                    m_objSafetyIO.SetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT, CDefine.ESafetyKey.SAFETY_KEY_TEACH);
                    m_objSafetyIO.SetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR, CDefine.ESafetyKey.SAFETY_KEY_TEACH);
                }
                else
                {
                    m_objSafetyIO.SetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT, CDefine.ESafetyKey.SAFETY_KEY_AUTO);
                    m_objSafetyIO.SetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR, CDefine.ESafetyKey.SAFETY_KEY_AUTO);
                }

                if (
                    CDefine.ESafetyKey.SAFETY_KEY_AUTO == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT)
                    && CDefine.ESafetyKey.SAFETY_KEY_AUTO == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR)
                    )
                {
                    m_objDocument.SetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH, false);
                }
                else
                {
                    m_objDocument.SetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH, true);
                }
            } while (false);
        }

        private void BtnTowerLampBuzzer_Click(object sender, EventArgs e)
        {
            do
            {
                // 알람 없는 경우
                if (false == m_objDocument.GetIsAlarmMessage())
                {
                    m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest = !m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest;
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                    break;
                }

                if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                    m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest = true;
                }
                else
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                    m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.IsForceDoorBuzzerOffRequest = false;
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0} : {1}]", "BtnBuzzerOff_Click", m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus().ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void BtnCellDataCellOut_Click(object sender, EventArgs e)
        {
            string strLog = string.Format("[{0}] [{1}]", "BtnCellDataCellOut_Click", true);
            m_objDocument.SetUpdateButtonLog(this, strLog);

            var button = sender as SpeedButton;
            if (button == null)
            {
                return;
            }
            var cellDataDisplayInformation = button.Tag as CellDataDisplayInformation;
            if (cellDataDisplayInformation == null)
            {
                return;
            }
            var cellDataHandler = cellDataDisplayInformation.CellDataHandler;

            if (m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] == CCIMDefine.EMoveState.MOVE_STATE_RUNNING)
            {
                return;
            }
            if (cellDataHandler.IsCellExist() == false)
            {
                return;
            }
            if (
                m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT) == CDefine.ESafetyKey.SAFETY_KEY_AUTO
                && m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR) == CDefine.ESafetyKey.SAFETY_KEY_AUTO
                )
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LOCK_THE_EQUIPMENT_DOOR_AND_CHANGE_THE_SAFETY_KEY_STATUS_TO_TEACH);
                return;
            }

            // 만약 Cell Out 예정 되어있으면 해제한다.
            if (cellDataDisplayInformation.IsManualTrackOut == true)
            {
                if (m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_DELETE_IT_FROM_THE_CELL_OUT_LIST) == DialogResult.No)
                {
                    return;
                }
                cellDataDisplayInformation.ResetManualTrackOut();
                strLog = string.Format("[{0}] [Process : {1}] [Position : {2}] [CellID : {3}] [{4}]", "SetCellOutData", cellDataHandler.ProcessIndex, cellDataHandler.PositionIndex, cellDataHandler.GetInnerID(), "Remove");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
            //  만약 키가 없다면 Cell Out 예정 등록 시킨다.
            else
            {
                if (
                    cellDataDisplayInformation.VacuumOrNull != null
                    && cellDataDisplayInformation.VacuumOrNull.Any(v => v != null && v.Status == CVacuumAbstract.EVacuumStatus.STS_ON)
                    )
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.REMOVE_THE_CELL_FROM_THE_STAGE);
                    return;
                }

                if (DialogResult.No == m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_REGISTER_IN_THE_CELL_OUT_LIST))
                {
                    return;
                }

                // 해당 셀아웃 코드 받아서 넣자
                using (CDialogCellOutCode objDialogCellOutCode = new CDialogCellOutCode(m_objDocument))
                {
                    if (objDialogCellOutCode.ShowDialog() == DialogResult.OK)
                    {
                        cellDataDisplayInformation.ManualTrackOutJudge = objDialogCellOutCode.m_CellOutCode;
                        cellDataDisplayInformation.ManualTrackOutDescription = $"{cellDataHandler.ProcessIndex.ToString().ToUpper()}_{cellDataHandler.PositionIndex + 1}_MANUAL_TRACKOUT";

#if !CIM_V3_GAMMA
                        // V1, V3-5F
                        cellDataDisplayInformation.ManualTrackOutReasonCode = "USE39";
#else
                        // V3 (G Project)
                        cellDataDisplayInformation.ManualTrackOutReasonCode = "GMCEQDR";
#endif
                        cellDataDisplayInformation.IsManualTrackOut = true;
                        strLog = string.Format("[{0}] [Process : {1}] [Position : {2}] [CellID : {3}] [{4}]", "SetCellOutData", cellDataHandler.ProcessIndex, cellDataHandler.PositionIndex, cellDataHandler.GetInnerID(), "Add");
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                    }
                }
            }

            strLog = string.Format("[{0}] [{1}]", "BtnCellDataCellOut_Click", false);
            m_objDocument.SetUpdateButtonLog(this, strLog);
        }

        private void ThreadUpdatePosition()
        {
            // 이렇게 안하면 GC가 자주 호출됨!
            var updatePositionMethod = new Action(SetPositionInformationUpdate);

            while (mbThreadShouldStop == false)
            {
                if (timer.Enabled == true)
                {
                    // 포지션 위치 표시
                    Invoke(updatePositionMethod);
                    Thread.Sleep(5);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private class CellDataDisplayInformation
        {
            public CellDataHandler CellDataHandler { get; private set; }
            public CVacuum[] VacuumOrNull { get; private set; }
            public bool IsManualTrackOut { get; set; } = false;
            public string ManualTrackOutJudge { get; set; } = string.Empty;
            public string ManualTrackOutDescription { get; set; } = string.Empty;
            public string ManualTrackOutReasonCode { get; set; } = string.Empty;
            private readonly CDocument mDocument;

            public CellDataDisplayInformation(CDocument document, ECellData cellDataIndex, CVacuum[] vacuumOrNull)
            {
                mDocument = document;
                VacuumOrNull = vacuumOrNull;
                CellDataHandler = CellDataManager.Cells[cellDataIndex];
            }

            public void ResetManualTrackOut()
            {
                IsManualTrackOut = false;
                ManualTrackOutJudge = string.Empty;
                ManualTrackOutDescription = string.Empty;
                ManualTrackOutReasonCode = string.Empty;
            }
        }

        private class VacuumDisplayInformation
        {
            public bool HasDetectSensor { get; private set; } = false;
            public CProcessMotion.EVacuum VacuumIndex { get; private set; }
            public CDeviceIODefine.EDigitalInput DetectSensorIndex { get; private set; }
            public CVacuum Vacuum { get; private set; }
            private readonly CDocument mDocument;

            public VacuumDisplayInformation(CDocument document, CProcessMotion.EVacuum vacuumIndex, CDeviceIODefine.EDigitalInput detectSensor)
            {
                mDocument = document;
                VacuumIndex = vacuumIndex;
                Vacuum = mDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[VacuumIndex];
                DetectSensorIndex = detectSensor;
                HasDetectSensor = true;
            }

            public VacuumDisplayInformation(CDocument document, CProcessMotion.EVacuum vacuumIndex)
            {
                mDocument = document;
                VacuumIndex = vacuumIndex;
                Vacuum = mDocument.m_objProcessMain.m_objProcessMotion.m_objVacuum[VacuumIndex];
                HasDetectSensor = false;
            }

            public Color GetBackgroundColor()
            {
                if (Vacuum.Command == CVacuumAbstract.EVacuumCommand.CMD_ON)
                {
                    switch (Vacuum.Status)
                    {
                        case CVacuumAbstract.EVacuumStatus.STS_ON:
                            return m_colorOn;
                    }
                }
                else if (Vacuum.Command == CVacuumAbstract.EVacuumCommand.CMD_OFF)
                {
                    switch (Vacuum.Status)
                    {
                        case CVacuumAbstract.EVacuumStatus.STS_OFF:
                            return Color.WhiteSmoke;
                    }
                }
                return m_colorOff;
            }

            public bool IsDetectSensorOn()
            {
                return mDocument.GetIOBit(DetectSensorIndex);
            }
        }


        private class DataDisplay<TData>
        {
            public object Tag { get; set; } = null;
            public bool IsChecked { get; set; } = false;
            public bool IsDataDeleteAble
            {
                get
                {
                    if (mDataDeleteAble == null)
                    {
                        return true;
                    }
                    return mDataDeleteAble.Invoke(this);
                }
            }
            public readonly DataDisplay<TData> Parent = null;
            public readonly Control Control;
            public readonly TData DataHandler;
            public IReadOnlyList<DataDisplay<TData>> StripData => mStripData;
            private readonly List<DataDisplay<TData>> mStripData = new List<DataDisplay<TData>>();
            private readonly Func<DataDisplay<TData>, bool> mDataDeleteAble;
            private readonly Action<DataDisplay<TData>> mAfterDeleteEvent;
            private readonly Func<DataDisplay<TData>, Color> mForeColorStatus;
            private readonly Func<DataDisplay<TData>, Color> mBackColorStatus;
            private readonly Func<DataDisplay<TData>, string> mTextStatus;
            private static CDocument mDocument;

            public DataDisplay(Control control, TData dataHandler, Func<DataDisplay<TData>, Color> getForeColorOrNull, Func<DataDisplay<TData>, Color> getBackColorOrNull, Func<DataDisplay<TData>, string> getTextOrNull, Func<DataDisplay<TData>, bool> getDataDeleteAbleOrNull, Action<DataDisplay<TData>> afterDeleteEvent)
            {
                Control = control;
                DataHandler = dataHandler;
                mForeColorStatus = getForeColorOrNull;
                mBackColorStatus = getBackColorOrNull;
                mTextStatus = getTextOrNull;
                mDataDeleteAble = getDataDeleteAbleOrNull;
                mAfterDeleteEvent = afterDeleteEvent;
            }

            private DataDisplay(DataDisplay<TData> parent, Control control, TData dataHandler, Func<DataDisplay<TData>, Color> getForeColorOrNull, Func<DataDisplay<TData>, Color> getBackColorOrNull, Func<DataDisplay<TData>, string> getTextOrNull, Func<DataDisplay<TData>, bool> getDataDeleteAbleOrNull)
            {
                Parent = parent;
                Control = control;
                DataHandler = dataHandler;
                mForeColorStatus = getForeColorOrNull;
                mBackColorStatus = getBackColorOrNull;
                mTextStatus = getTextOrNull;
                mDataDeleteAble = getDataDeleteAbleOrNull;
            }

            public void AddStripData(Control control, TData dataHandler, Func<DataDisplay<TData>, Color> getForeColorOrNull, Func<DataDisplay<TData>, Color> getBackColorOrNull, Func<DataDisplay<TData>, string> getTextOrNull, Func<DataDisplay<TData>, bool> getDataDeleteAbleOrNull)
            {
                DataDisplay<TData> addData = new DataDisplay<TData>(this, control, dataHandler, getForeColorOrNull, getBackColorOrNull, getTextOrNull, getDataDeleteAbleOrNull);
                mStripData.Add(addData);
            }

            public void Update()
            {
                if (mForeColorStatus != null)
                {
                    Color color = mForeColorStatus.Invoke(this);
                    if (Control.ForeColor != color)
                    {
                        Control.ForeColor = color;
                    }
                }
                if (mBackColorStatus != null)
                {
                    Color color = mBackColorStatus.Invoke(this);
                    if (Control.BackColor != color)
                    {
                        Control.BackColor = color;
                    }
                }
                if (mTextStatus != null)
                {
                    string text = mTextStatus.Invoke(this);
                    if (Control.Text != text)
                    {
                        Control.Text = text;
                    }
                }

                foreach (var item in mStripData)
                {
                    item.Update();
                }
            }

            public void RaiseAfterDeleteEvent()
            {
                mAfterDeleteEvent?.Invoke(this);
            }

            public static void SetDocument(CDocument document)
            {
                mDocument = document;
            }
        }
    }
}