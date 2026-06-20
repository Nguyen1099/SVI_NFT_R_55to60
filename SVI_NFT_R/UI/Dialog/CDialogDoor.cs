using SVI_NFT_R.EHS;
using SVI_NFT_R.EqToEq.SdvGammaFoldableLine;
using SVI_NFT_R.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogDoor : CFormCommon
    {
        private class CheckGourpSet
        {
            public string Uiid { get; set; }
            public string Name { get; set; }
            public CDefine.ECheckGroup GroupIndex { get; set; }
            public readonly List<IoIndicatorSet> CheckItems = new List<IoIndicatorSet>();
        }

        private class InterlockMapSet
        {
            public UcCell Control { get; set; }
            public CDefine.ECheckGroup GroupIndex { get; set; }
            public IoIndicatorSet IoIndicator { get; set; }
            public LineBetweenControls LineControl { get; set; }
        }

        private readonly Color mColorInterlockMapLocationSelected = Color.FromArgb(30, Color.Red);
        private readonly Color mColorInterlockMapLocationNormal = Color.FromArgb(30, Color.Black);
        private readonly Color mColorInterlockMapLineSelected = Color.FromArgb(230, Color.Red);
        private readonly Color mColorInterlockMapLineNormal = Color.FromArgb(230, Color.Black);
        private readonly List<CheckGourpSet> mCheckGroupItems = new List<CheckGourpSet>(16);
        private readonly List<InterlockMapSet> mInterlockMapItems = new List<InterlockMapSet>(32);
        private int mCheckGroupIndex = 0;
        private int mCheckPageIndex = 0;
        private int mCheckPageCount = 1;
        private UcCell[] mControlUcCheckGroupItems;
        private UcCell[] mControlUcCheckItems;
        private CDocument m_objDocument;
        private CSafetyIO m_objSafetyIO;
        private string m_strMessageKeyNoOwn;
        private string m_strMessageInnerOperatorCheck;
        private string m_strMessageDoorLock;
        private string m_strMessageDoorOpen;
        private int mModeChangeDelayTick = 0;
        private readonly UcDoorStatus[] mDoorStatus;
        private readonly UcDoorControl[] mDoorControl;
        private bool mbBlinkSignal = false;
        private bool mbTriggerUpdateIndoorLamp = false;
        private bool mbPasswordShow = false;
        private CDefine.ELanguage mLastLanguage;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogDoor(CDocument objDocument)
        {
            // 도큐먼트 포인트 받음
            m_objDocument = objDocument;
            InitializeComponent();

            mDoorStatus = new UcDoorStatus[]
            {
                UcDoorStatusMainFront1,
                UcDoorStatusMainFront2,
                UcDoorStatusMainFront3,
                UcDoorStatusMainFront4,
                UcDoorStatusMainRear1,
                UcDoorStatusMainRear2,
                UcDoorStatusMainRear3,
            };

            mDoorControl = new UcDoorControl[]
            {
                UcDoorControlSlideFront,
                UcDoorControlSlideRear,
            };

            UcDoorControlSlideFront.Tag = Door.EGroup.KeyFront;
            UcDoorControlSlideRear.Tag = Door.EGroup.KeyRear;

            foreach (var ctrl in mDoorControl)
            {
                ctrl.BtnDoorOpen.Click += BtnDoorOpen_Click;
                ctrl.BtnDoorLock.Click += BtnDoorLock_Click;
            }

            // 컨트롤 리스트 초기화
            {
                mControlUcCheckGroupItems = PnlCheckGroupItems.Controls.GetChildControlListByType(typeof(UcCell))
                    .Cast<UcCell>()
                    .ToArray();
                mControlUcCheckItems = PnlCheckItems.Controls.GetChildControlListByType(typeof(UcCell))
                    .Cast<UcCell>()
                    .ToArray();
            }
            InitializeAutoScale();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // ! Main Frame을 가리도록 크기와 위치를 조정함
            Size = m_objDocument.GetMainFrame().Size;
            Location = m_objDocument.GetMainFrame().Location;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogDoor_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogDoor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 해제
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        private bool Initialize()
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
        /// <returns></returns>
        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 패스워드는 표시 안되게
                TextPassword.PasswordChar = '＊';
                TextPassword.Text = "";
                // 설비 상태 점검 그룹 리스트 초기화
                SetCheckGroupItems();
                // 인터락 맵 리스트 초기화
                SetInterlockMapItems();
                // 버튼 태그 정의
                SetButtonTag();
                // 폼 중앙에서 생성
                CenterToParent();
                // 세이프티 IO 클래스 초기화
                m_objSafetyIO = new CSafetyIO(m_objDocument);
                if (false == m_objSafetyIO.Initialize())
                    break;
                // 이미지 투명도 설정
                PnlDoorControlLayout.BackgroundImage = SetImageOpacity(PnlDoorControlLayout.BackgroundImage, 0.3f);
                PnlInterlockMapBase.BackgroundImage = SetImageOpacity(PnlInterlockMapBase.BackgroundImage, 0.3f);
                PnlFfuMapBase.BackgroundImage = SetImageOpacity(PnlFfuMapBase.BackgroundImage, 0.3f);
                // 버튼 색상 정의
                SetButtonColor();
                // 언어 변경
                SetChangeLanguage();
                // 탭 컨트롤 기본 탭 버튼이 안보이게 설정
                TcContent.Appearance = TabAppearance.FlatButtons;
                TcContent.ItemSize = new Size(0, 1);
                TcContent.SizeMode = TabSizeMode.Fixed;
                // 탭 컨트롤 첫번째 탭 선택
                TcContent.SelectedIndex = 0;

                // 타이머 제어
                timer.Interval = 100;
                timer.Enabled = true;
                timerBlink.Interval = 1000;
                timerBlink.Enabled = true;

                Text = Program.ID;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설비 상태 점검 그룹 리스트 초기화
        /// </summary>
        private void SetCheckGroupItems()
        {
            foreach (CDefine.ECheckGroup groupIndex in Enum.GetValues(typeof(CDefine.ECheckGroup)))
            {
                // 필요한 정보만 필터링함
                switch (groupIndex)
                {
                    case CDefine.ECheckGroup.Door:
                    case CDefine.ECheckGroup.MaintKey:
                    case CDefine.ECheckGroup.EMS:
                    case CDefine.ECheckGroup.Sensor:
                    case CDefine.ECheckGroup.Etc:
                        break;

                    default:
                        continue;
                }

                var groupItems = m_objDocument.GetMainFrame().GetCheckGroupItemsFromGroupIndex(groupIndex);
                if (groupItems.Count() == 0)
                {
                    continue;
                }
                CheckGourpSet checkGroupItem = new CheckGourpSet();
                checkGroupItem.GroupIndex = groupIndex;
                checkGroupItem.Uiid = $"CheckGroup[{groupIndex}]";
                checkGroupItem.CheckItems.AddRange(groupItems);
                mCheckGroupItems.Add(checkGroupItem);
            }

            // 첫 번째 그룹 선택
            SetCheckGroupItemIndex(0);
        }

        private void SetInterlockMapItems()
        {
            var interlockItems = new[]
            {
                new { Control = UcCellInterlockMapSafetyPlcEms, IoIndex = CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_EMS_NORMAL },
                new { Control = UcCellInterlockMapSafetyPlcDoorForceOpened, IoIndex = CDeviceIODefine.EDigitalInput.X_SAFETY_STATUS_DOOR_FORCE_OPEN_NORMAL },

                new { Control = UcCellInterlockMapRearEms, IoIndex = CDeviceIODefine.EDigitalInput.X_REAR_EMERGENCY_SWITCH_PUSH },
                new { Control = UcCellInterlockMapFrontEms, IoIndex = CDeviceIODefine.EDigitalInput.X_FRONT_EMERGENCY_SWITCH_PUSH },

                new { Control = UcCellInterlockMapRearDoor1, IoIndex = CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_1_OPEN },
                new { Control = UcCellInterlockMapRearDoor2, IoIndex = CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_2_OPEN },
                new { Control = UcCellInterlockMapRearDoor3, IoIndex = CDeviceIODefine.EDigitalInput.X_REAR_SAFETY_DOOR_3_OPEN },
                new { Control = UcCellInterlockMapFrontDoor1, IoIndex = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_1_OPEN },
                new { Control = UcCellInterlockMapFrontDoor2, IoIndex = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_2_OPEN },
                new { Control = UcCellInterlockMapFrontDoor3, IoIndex = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_3_OPEN },
                new { Control = UcCellInterlockMapFrontDoor4, IoIndex = CDeviceIODefine.EDigitalInput.X_FRONT_SAFETY_DOOR_4_OPEN },
            };

            LineBetweenControls[] lineControls = components.Components
                .OfType<LineBetweenControls>()
                .ToArray();

            foreach (var item in interlockItems)
            {
                var addItem = new InterlockMapSet()
                {
                    Control = item.Control,
                    GroupIndex = m_objDocument.GetMainFrame().GetCheckGroupFromIoIndex(item.IoIndex),
                    IoIndicator = m_objDocument.GetMainFrame().GetCheckGroupItemFromIoIndex(item.IoIndex),
                    LineControl = lineControls.FirstOrDefault(i => ReferenceEquals(i.LineBeginControl, item.Control))
                };
                mInterlockMapItems.Add(addItem);
            }
        }

        /// <summary>
        /// 버튼 이벤트에서 사용될 Tag 인스턴스 초기화
        /// </summary>
        private void SetButtonTag()
        {
            // 점검 그룹 아이템 인덱스 초기화
            {
                int index = 0;
                foreach (var item in mControlUcCheckGroupItems)
                {
                    item.Tag = index++;
                }
            }
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
            SetButtonBackColor(BtnTitleDoorControl, m_colorLabel);
            foreach (var item in mDoorControl)
            {
                SetButtonBackColor(item.BtnTitle, m_colorLabel);
                SetButtonBackColor(item.BtnDoorOpen, m_colorNormal);
                SetButtonBackColor(item.BtnDoorLock, m_colorNormal);
            }
            SetButtonBackColor(BtnClose, m_colorNormal);
            foreach (var item in mDoorStatus)
            {
                item.TitleBackColor = m_colorLabelSub;
                item.MessageBackColor = m_colorLabelData;
            }

            PnlCheckBase.BackColor = m_colorLabelCategory;
            PnlCheckItems.BackColor = m_colorClick;
            PnlCheckItemPage.BackColor = m_colorClick;

            SetButtonBackColor(BtnTitleCheck.BtnCellData, m_colorLabel);
            SetButtonBackColor(BtnSubTitleCheckGroupName, m_colorLabel);

            foreach (var item in mControlUcCheckItems)
            {
                SetButtonBackColor(item.BtnCellData, m_colorLabelSub);
            }

            foreach (var item in mControlUcCheckGroupItems)
            {
                SetButtonBackColor(item.BtnCellData, m_colorNormal);
            }

            SetButtonBackColor(UcUpperDoorCheck.BtnCellData, m_colorLabelSub);
            SetButtonBackColor(UcLowerDoorCheck.BtnCellData, m_colorLabelSub);
            SetButtonBackColor(UcEnableSwitchCheck.BtnCellData, m_colorNormal);

            // Interlock Map
            {
                foreach (var item in mInterlockMapItems)
                {
                    SetButtonBackColor(item.Control.BtnCellData, m_colorLabelSub);
                    item.Control.BtnCellData.TextImageRelation = TextImageRelation.ImageBeforeText;
                    item.Control.BtnCellData.ImageList = imageListInterlockMapGroup;
                    item.Control.BtnCellData.ImageAlign = ContentAlignment.TopCenter;
                    if (item.GroupIndex == CDefine.ECheckGroup.EMS)
                    {
                        item.Control.BtnCellData.ImageIndex = 0;
                    }
                    else if (item.GroupIndex == CDefine.ECheckGroup.Door)
                    {
                        item.Control.BtnCellData.ImageIndex = 1;
                    }
                    else if (item.GroupIndex == CDefine.ECheckGroup.MaintKey)
                    {
                        item.Control.BtnCellData.ImageIndex = 2;
                    }
                    else if (item.GroupIndex == CDefine.ECheckGroup.Sensor)
                    {
                        item.Control.BtnCellData.ImageIndex = 3;
                    }
                }
            }

            SetButtonBackColor(BtnLanguage, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(nameof(UcCell.BtnCellData))
                    && false == btn.Name.Equals(nameof(BtnTitleDoorControl))
                    && false == btn.Name.Equals(nameof(BtnTitleInterlockMap))
                    && false == btn.Name.Equals(nameof(BtnTitleFfuMap))
                    && false == btn.Name.Equals(nameof(BtnPasswordDisplay))
                    && false == btn.Name.Equals(nameof(BtnPasswordShow))
                    && false == btn.Name.Equals(nameof(BtnTouch))
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }

            // !!! UserControl에 버튼 이름이 같아서 따로 처리해줌
            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var nonClickableButtons = mControlUcCheckItems
                .Select(item => item.BtnCellData)
                .Concat(new SpeedButton[] { BtnTitleCheck.BtnCellData })
                .Concat(new SpeedButton[] { UcUpperDoorCheck.BtnCellData })
                .Concat(new SpeedButton[] { UcLowerDoorCheck.BtnCellData })
                .Concat(new SpeedButton[] { UcEnableSwitchCheck.BtnCellData });
            foreach (var item in nonClickableButtons)
            {
                item.FlatAppearance.MouseOverBackColor = item.BackColor;
                item.FlatAppearance.MouseDownBackColor = item.BackColor;
                item.BackColorChanged += NonClickableButton_BackColorChanged;
                item.Cursor = Cursors.Default;
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
                SetButtonChangeLanguage(BtnTitleDoorControl);
                SetButtonChangeLanguage(BtnTitleInterlockMap);
                SetButtonChangeLanguage(BtnTitleFfuMap);
                foreach (var item in mDoorControl)
                {
                    string title = m_objDocument.GetDatabaseUIText(item.Name, Name);
                    item.Text = title;
                }
                SetButtonChangeLanguage(BtnLockAllDoors);
                SetButtonChangeLanguage(BtnUnlockAllDoors);
                SetButtonChangeLanguage(BtnClose);
                // 메세지 UI 갱신
                m_strMessageKeyNoOwn = m_objDocument.GetDatabaseUIText(nameof(m_strMessageKeyNoOwn), Name);
                m_strMessageInnerOperatorCheck = m_objDocument.GetDatabaseUIText(nameof(m_strMessageInnerOperatorCheck), Name);
                m_strMessageDoorLock = m_objDocument.GetDatabaseUIText(nameof(m_strMessageDoorLock), Name);
                m_strMessageDoorOpen = m_objDocument.GetDatabaseUIText(nameof(m_strMessageDoorOpen), Name);

                SetButtonText(BtnTitleCheck.BtnCellData, m_objDocument.GetDatabaseUIText(nameof(BtnTitleCheck), Name));
                SetButtonChangeLanguage(BtnSubTitleCheckGroupName);
                SetButtonChangeLanguage(BtnBuzzerOff);

                UcUpperDoorCheck.Title = m_objDocument.GetDatabaseUIText(UcUpperDoorCheck.Name, Name);
                UcLowerDoorCheck.Title = m_objDocument.GetDatabaseUIText(UcLowerDoorCheck.Name, Name);
                UcEnableSwitchCheck.Title = m_objDocument.GetDatabaseUIText(UcEnableSwitchCheck.Name, Name);

                foreach (var groupItem in mCheckGroupItems)
                {
                    groupItem.Name = m_objDocument.GetDatabaseUIText(groupItem.Uiid, "EquipmentInspection");
                    foreach (var item in groupItem.CheckItems)
                    {
                        item.SetChangeLanguage(m_objDocument.GetDatabaseUIText);
                    }
                }

                // Interlock Map
                {
                    foreach (var item in mInterlockMapItems)
                    {
                        item.IoIndicator.SetChangeLanguage(m_objDocument.GetDatabaseUIText);
                    }
                }

                // 언어 변경
                {
                    mLastLanguage = m_objDocument.m_objConfig.GetOptionParameter().eLanguage;
                    if (CDefine.ELanguage.LANGUAGE_KOREA == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "KOREA");
                    }
                    else if (CDefine.ELanguage.LANGUAGE_CHINA == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "CHINA");
                    }
                    else if (CDefine.ELanguage.LANGUAGE_ENGLISH == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "ENGLISH");
                    }
                    else if (CDefine.ELanguage.LANGUAGE_VIETNAM == mLastLanguage)
                    {
                        SetButtonText(BtnLanguage, "VIETNAM");
                    }
                }

                // 조작 금지 다국어 및 폰트 크기 맞춤
                {
                    SetLabelChangeLanguage(LblTitleTop);
                    SetLabelChangeLanguage(LblTitleRight1);
                    SetLabelChangeLanguage(LblTitleRight2);
                    if (m_objDocument.GetMainFrame().Size == new Size(1920, 1080))
                    {
                        if (m_objDocument.m_objConfig.GetOptionParameter().eLanguage == CDefine.ELanguage.LANGUAGE_KOREA)
                        {
                            LblTitleTop.Font = new Font("Cascadia Code", 80F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LblTitleRight1.Font = new Font("Cascadia Code", 28F, FontStyle.Bold);
                            LblTitleRight2.Font = new Font("Cascadia Code", 18F, FontStyle.Bold);
                        }
                        else
                        {
                            LblTitleTop.Font = new Font("Microsoft Sans Serif", 80F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LblTitleRight1.Font = new Font("Microsoft Sans Serif", 28F, FontStyle.Bold);
                            LblTitleRight2.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
                        }
                        SetControlTextFitting(LblTitleRight1, 28f, 32f);
                        SetControlTextFitting(LblTitleRight2, 14f, 18f);
                        SetControlTextFitting(LblTitleRight3, 20f, 32f);
                        SetControlTextFitting(LblTitleRight4, 14f, 18f);
                    }
                    else
                    {
                    }
                }

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

        private void SetLabelChangeLanguage(Label objLabel)
        {
            string uiText = m_objDocument.GetDatabaseUIText(objLabel.Name, Name);
            if (uiText != objLabel.Text)
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
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerActionUpdateTabSelection();
            TimerActionUpdateCheckItems();
            TimerActionUpdateDoorControlPage();
            TimerActionUpdateInterlockMapPage();
            TimerActionUpdateFfuMapPage();
            TimerActionUpdatePassword();
            if (mLastLanguage != m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
            {
                SetChangeLanguage();
            }
        }

        private void TimerActionUpdateTabSelection()
        {
            SetButtonBackColor(BtnTitleDoorControl, TcContent.SelectedIndex == 0 ? m_colorLabel : m_colorNormal);
            SetButtonBackColor(BtnTitleInterlockMap, TcContent.SelectedIndex == 1 ? m_colorLabel : m_colorNormal);
            SetButtonBackColor(BtnTitleFfuMap, TcContent.SelectedIndex == 2 ? m_colorLabel : m_colorNormal);
        }

        private void TimerActionUpdateCheckItems()
        {
            for (int i = 0; i < mControlUcCheckItems.Length; i++)
            {
                int index = i + (mControlUcCheckItems.Length * mCheckPageIndex);
                bool bVisible = index < mCheckGroupItems[mCheckGroupIndex].CheckItems.Count;
                if (mControlUcCheckItems[i].Visible != bVisible)
                {
                    mControlUcCheckItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                mCheckGroupItems[mCheckGroupIndex].CheckItems[index].Update(mControlUcCheckItems[i].BtnCellData, mControlUcCheckItems[i].BtnIndicator);
            }
            SetControlText(LblNaviCheckItemPage, $"{mCheckPageIndex + 1} / {mCheckPageCount}");
        }

        private void TimerActionUpdateDoorControlPage()
        {
            // 도어 상태 체크해서 언어 변경
            foreach (var door in DoorManager.Doors.Values)
            {
                int index = (int)door.DoorIndex;
                mDoorStatus[index].TitleText = Resource.Get(door.DoorIndex).ToString();
                if (CProcessSafetyDoor.ESafetyDoorScenario.KEY_NO_OWN == m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(door.DoorIndex))
                {
                    mDoorStatus[index].MessageText = m_strMessageKeyNoOwn;
                }
                else if (CProcessSafetyDoor.ESafetyDoorScenario.INNER_OPERATOR_CHECK == m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(door.DoorIndex))
                {
                    mDoorStatus[index].MessageText = m_strMessageInnerOperatorCheck;
                    mDoorStatus[index].MessageBackColor = m_colorLabelData;
                    mDoorStatus[index].MessageForeColor = Color.Black;
                }
                else if (CProcessSafetyDoor.ESafetyDoorScenario.DOOR_LOCK == m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(door.DoorIndex))
                {
                    mDoorStatus[index].MessageText = m_strMessageDoorLock;
                    mDoorStatus[index].MessageBackColor = m_colorGreen;
                    mDoorStatus[index].MessageForeColor = Color.Black;
                }
                else if (CProcessSafetyDoor.ESafetyDoorScenario.DOOR_UNLOCK == m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(door.DoorIndex))
                {
                    mDoorStatus[index].MessageText = m_strMessageDoorOpen;
                    mDoorStatus[index].MessageBackColor = m_colorYellow;
                    mDoorStatus[index].MessageForeColor = Color.Black;
                }
            }

            foreach (var group in DoorManager.Groups)
            {
                bool bUnlockAllDoorsGroup = group.Value.All(item => item.IsUnlockPermit);
                SetButtonBackColor(mDoorControl[(int)group.Key].BtnDoorOpen, bUnlockAllDoorsGroup ? m_colorOutputOn : m_colorOutputOff);
                SetButtonBackColor(mDoorControl[(int)group.Key].BtnDoorLock, bUnlockAllDoorsGroup ? m_colorNormal : m_colorClick);
                mDoorControl[(int)group.Key].HasOutsideKey = group.Value[0].HasOutsideKey;
            }

            bool bUnlockAllDoors = DoorManager.Doors.Values.All(item => item.IsUnlockPermit);
            bool bLockAllDoors = DoorManager.Doors.Values.All(item => item.IsUnlockPermit == false);
            SetButtonBackColor(BtnUnlockAllDoors, bUnlockAllDoors ? m_colorOutputOn : m_colorOutputOff);
            SetButtonBackColor(BtnLockAllDoors, bLockAllDoors ? m_colorClick : m_colorNormal);

            // 도어 언락 활성화/비활성화 상태 업데이트
            //bool bIsAutoMode = (
            //    (
            //    CDefine.ESafetyKey.SAFETY_KEY_AUTO == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_FRONT)
            //    && CDefine.ESafetyKey.SAFETY_KEY_AUTO == m_objSafetyIO.GetInputSafetyKey(CDefine.EMachineFrontRear.MACHINE_REAR)
            //    )
            //    && false == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH)
            //    );
            bool bIsAutoMode = false == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH);
            //bool bIsDoorLockKeyDetect = (
            //    CDefine.EDoorKeyDetect.DOOR_KEY_DETECT_ON == m_objSafetyIO.GetInputSafetyDoorLockKeyDetect(CDefine.EMachineFrontRear.MACHINE_FRONT)
            //    && CDefine.EDoorKeyDetect.DOOR_KEY_DETECT_ON == m_objSafetyIO.GetInputSafetyDoorLockKeyDetect(CDefine.EMachineFrontRear.MACHINE_REAR)
            //    );
            if (
                true == bIsAutoMode
                //|| true == bIsDoorLockKeyDetect
                )
            {
                //SetButtonEnable(BtnDoorOpen, false);
                mModeChangeDelayTick = 0;
            }
            else
            {
                // 모드 변경 직후 도어락을 해제하면 간헐적으로 Safety PLC가 오작동하여 바로 변경 할 수 없도록 딜레이 처리함
                if (5 <= mModeChangeDelayTick)
                {
                    //SetButtonEnable(BtnDoorOpen, true);
                }
                else
                {
                    mModeChangeDelayTick++;
                }
            }

            //// 도어 다이얼로그 닫기 버튼 활성화/비활성화 상태 업데이트
            //int nFrontScenarioIndex = (int)m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(CDefine.EMachineFrontRear.MACHINE_FRONT);
            //int nRearScenarioIndex = (int)m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(CDefine.EMachineFrontRear.MACHINE_REAR);
            //CProcessSafetyDoor.ESafetyDoorScenario scenario = CProcessSafetyDoor.ESafetyDoorScenario.DOOR_LOCK;
            //// 우선순위가 높은 시나리오를 기준으로 닫기 버튼을 활성화/비활성화 함
            //if (nFrontScenarioIndex > nRearScenarioIndex)
            //{
            //    scenario = m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(CDefine.EMachineFrontRear.MACHINE_FRONT);
            //}
            //else
            //{
            //    scenario = m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(CDefine.EMachineFrontRear.MACHINE_REAR);
            //}
            //switch (scenario)
            //{
            //    case CProcessSafetyDoor.ESafetyDoorScenario.DOOR_LOCK:
            //    case CProcessSafetyDoor.ESafetyDoorScenario.INNER_OPERATOR_CHECK:
            //        // 닫기 버튼 활성화
            //        SetButtonEnable(BtnClose, true);
            //        break;
            //    case CProcessSafetyDoor.ESafetyDoorScenario.DOOR_UNLOCK:
            //    case CProcessSafetyDoor.ESafetyDoorScenario.KEY_NO_OWN:
            //        // 닫기 버튼 비활성화
            //        SetButtonEnable(BtnClose, false);
            //        break;
            //}
            // 닫기 버튼 활성화
            SetButtonEnable(BtnClose, true);

            // Update Check Group Items
            for (int i = 0; i < mControlUcCheckGroupItems.Length; i++)
            {
                bool bVisible = i < mCheckGroupItems.Count && mCheckGroupItems[i].CheckItems.Count > 0;
                if (mControlUcCheckGroupItems[i].Visible != bVisible)
                {
                    mControlUcCheckGroupItems[i].Visible = bVisible;
                }
                if (bVisible == false)
                {
                    continue;
                }

                SetButtonText(mControlUcCheckGroupItems[i].BtnCellData, mCheckGroupItems[i].Name);
                SetButtonBackColor(mControlUcCheckGroupItems[i].BtnCellData, mCheckGroupIndex == i ? m_colorClick : m_colorNormal);
                SetButtonBackColor(mControlUcCheckGroupItems[i].BtnIndicator, mCheckGroupItems[i].CheckItems.All(item => item.GetIo.Invoke() == true) == true ? m_colorOn : m_colorOff);
            }
            SetButtonBackColor(BtnTitleCheck.BtnIndicator, mControlUcCheckGroupItems.Where(i => i.Visible).All(i => i.BtnIndicator.BackColor == m_colorOn) ? m_colorOn : m_colorOff);
            bool bEnableSwitchGrip = m_objDocument.m_objProcessMain.GetSafetyIO().IsAnyEnableSwitchGripped();
            bool bUpperEqDoorIsOpen = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceDoorOpened();
            bool bLowerEqDoorIsOpen = false; //m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.CheckLowerEqInterfaceDoorOpened();
            SetButtonBackColor(UcEnableSwitchCheck.BtnIndicator, bEnableSwitchGrip ? m_colorOn : m_colorOff);
            SetButtonBackColor(UcUpperDoorCheck.BtnIndicator, bUpperEqDoorIsOpen ? m_colorOff : m_colorOn);
            SetButtonBackColor(UcLowerDoorCheck.BtnIndicator, bLowerEqDoorIsOpen ? m_colorOff : m_colorOn);

            if (mbTriggerUpdateIndoorLamp == true)
            {
                mbTriggerUpdateIndoorLamp = false;
                m_objDocument.SetIOBit(CDeviceIODefine.EDigitalOutput.Y_EQUIPMENT_INSIDE_LAMP_ON, !bLockAllDoors);

                // 가상 모드 처리
                m_objDocument.m_objProcessMain.GetSafetyIO().SetSimulationSafetyAllDoorClosed(bLockAllDoors);
            }
        }

        private void TimerActionUpdateInterlockMapPage()
        {
            foreach (var item in mInterlockMapItems)
            {
                SetButtonBackColor(item.Control.BtnCellData, mCheckGroupItems[mCheckGroupIndex].GroupIndex == item.GroupIndex ? m_colorLabelSub : m_colorNormal);
                item.IoIndicator.Update(item.Control.BtnCellData, item.Control.BtnIndicator);

                if (item.LineControl != null)
                {
                    SetControlBackColor(item.LineControl.LineEndControl, mCheckGroupItems[mCheckGroupIndex].GroupIndex == item.GroupIndex ? mColorInterlockMapLocationSelected : mColorInterlockMapLocationNormal);
                    Color lineColor = mCheckGroupItems[mCheckGroupIndex].GroupIndex == item.GroupIndex ? mColorInterlockMapLineSelected : mColorInterlockMapLineNormal;
                    if (item.LineControl.LineColor != lineColor)
                    {
                        item.LineControl.LineColor = lineColor;
                    }
                }
            }
        }

        private void TimerActionUpdateFfuMapPage()
        {
            int[] readValues = CProcessMain.SerialReaders.Mcu[CDefine.EMcu.MainGruop].Value;
            SetControlText(LblFfuRpm1, $"RPM: {readValues[0]}");
        }

        private void TimerActionUpdatePassword()
        {
            if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
            {
                SetButtonBackColor(BtnBuzzerOff, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnBuzzerOff, m_colorClick);
            }

            if (ActiveControl == TextPassword)
            {
                SetButtonBackColor(BtnPasswordDisplay, m_colorNormal);
                SetButtonBackColor(BtnPasswordShow, m_colorNormal);
                SetButtonBackColor(BtnTouch, m_colorNormal);
            }
            else
            {
                SetButtonBackColor(BtnPasswordDisplay, Color.White);
                SetButtonBackColor(BtnPasswordShow, Color.White);
                SetButtonBackColor(BtnTouch, Color.White);
            }

            StringBuilder passwordString = new StringBuilder();
            if (mbPasswordShow == true)
            {
                passwordString.Append(TextPassword.Text);
            }
            else
            {
                for (int i = 0; i < TextPassword.Text.Length; i++)
                {
                    passwordString.Append('＊');
                }
            }
            SetButtonText(BtnPasswordDisplay, passwordString.ToString());
        }

        private new void SetControlVisible(Control control, bool bVisible)
        {
            if (control.Visible != bVisible)
            {
                control.Visible = bVisible;
            }
        }

        private void SetControlBackColor(Control control, Color backColor)
        {
            if (control.BackColor != backColor)
            {
                control.BackColor = backColor;
            }
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerBlink_Tick(object sender, EventArgs e)
        {
            mbBlinkSignal = !mbBlinkSignal;
            // 도어 상태 체크해서 언어 변경
            foreach (var door in DoorManager.Doors.Values)
            {
                int index = (int)door.DoorIndex;
                if (m_objDocument.m_objProcessMain.m_objProcessSafetyDoor.GetSafetyDoorScenario(door.DoorIndex) == CProcessSafetyDoor.ESafetyDoorScenario.KEY_NO_OWN)
                {
                    if (mbBlinkSignal == true)
                    {
                        mDoorStatus[index].MessageBackColor = m_colorRed;
                        mDoorStatus[index].MessageForeColor = m_colorLabelData;
                    }
                    else
                    {
                        mDoorStatus[index].MessageBackColor = m_colorLabelData;
                        mDoorStatus[index].MessageForeColor = m_colorRed;
                    }
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible == true)
            {
                TextPassword.Text = "";
                ActiveControl = TextPassword;
                SetTimer(true);
                SetChangeLanguage();
                m_objDocument.m_objProcessMain.m_objProcessTowerLamp.AddHistory(CProcessTowerLamp.ETowerLamp.PM);
                m_objDocument.m_ePMMode = CCIMDefine.EPmMode.PM_MODE_ON;
            }
            else
            {
                SetTimer(false);
                m_objDocument.m_objProcessMain.m_objProcessTowerLamp.ClearHistory(CProcessTowerLamp.ETowerLamp.PM);
                m_objDocument.m_ePMMode = CCIMDefine.EPmMode.PM_MODE_OFF;
            }

            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// 도어 창 닫음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (m_objDocument.IsMasterLogin == true)
            {
                Hide();
                TextPassword.Text = "";
                return;
            }

            do
            {
                string password = TextPassword.Text;
                // ! 도어락에서 하드웨어적으로 열쇠 제거시 락이 해제되는 기능이 있는 타입인 경우 `IsLocked`를 확인해야함
                //bool bAllDoorLock = DoorManager.Doors.Values.All(item => item.IsLocked == true)
                //    && DoorManager.Doors.Values.All(item => item.IsUnlockPermit == false);
                bool bAllDoorLock = DoorManager.Doors.Values.All(item => item.IsOpened == false)
                    && DoorManager.Doors.Values.All(item => item.IsUnlockPermit == false);
                bool bPlcDoorClose = m_objDocument.m_objProcessMain.GetSafetyIO().IsSafetyAllDoorClosed();
                bool bEnableSwitchGrip = m_objDocument.m_objProcessMain.GetSafetyIO().IsAnyEnableSwitchGripped();
                //bool bUpperEqDoorIsOpen = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.CheckUpperEqInterfaceDoorOpened();
                //bool bLowerEqDoorIsOpen = false; //m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.CheckLowerEqInterfaceDoorOpened();

                ActiveControl = TextPassword;

                if (
                    (
                    bAllDoorLock == false
                    || bPlcDoorClose == false
                    )
                    && bEnableSwitchGrip == false
                    )
                {
                    // Msg: 설비 도어가 열려 있습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.EQP_DOOR_IS_OPEN);
                    break;
                }

                //if (
                //    bUpperEqDoorIsOpen == true
                //    && bEnableSwitchGrip == false
                //    )
                //{
                //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_DOOROPEN_SIGNAL_PARAM1, "UPPER");
                //    break;
                //}

                //if (
                //    bLowerEqDoorIsOpen == true
                //    && bEnableSwitchGrip == false
                //    )
                //{
                //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.UNIT_EQUIPMENT_DOOROPEN_SIGNAL_PARAM1, "LOWER");
                //    break;
                //}

                if (string.IsNullOrWhiteSpace(password) == true)
                {
                    // Msg: 비밀번호가 없습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THERE_IS_NOT_PASSWORD);
                    break;
                }

                if (CDefine.EquipmentPasswordNumbers.Contains(password) == false)
                {
                    // Msg: 로그인 정보가 일치하지 않습니다.
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.THE_LOGIN_INFORMATION_DOES_NOT_MATCH);
                    TextPassword.Select(0, TextPassword.Text.Length);
                    break;
                }

                Hide();
                TextPassword.Text = "";
                // 버튼 로그 추가
                string strLog = string.Format("[{0}]", "BtnClose_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        /// <summary>
        /// 도어 잠금
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDoorLock_Click(object sender, EventArgs e)
        {
            Door.EGroup groupIndex = (Door.EGroup)((Control)sender).Tag;

            if (CanLockDoor() == false)
            {
                return;
            }

            // 도어 열림 상태 OFF
            //foreach (var door in DoorManager.Groups[groupIndex])
            //{
            //    // ! 도어락에서 하드웨어적으로 열쇠 제거시 락이 해제되는 기능이 있는 타입인 경우 `IsLocked`를 확인해야함
            //    //if (door.IsLocked == false)
            //    if (door.IsOpened == true)
            //    {
            //        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.DOOR_IS_OPENED_PARAM1, door.DoorIndex);
            //        return;
            //    }
            //    if (door.HasOutsideKey == false)
            //    {
            //        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.DOOR_KEY_IS_NOT_DETECTED_PARAM1, door.DoorIndex);
            //        return;
            //    }
            //}

            // 도어 잠금
            foreach (var door in DoorManager.Groups[groupIndex])
            {
                door.IsUnlockPermit = false;
            }
            Thread.Sleep(100);
            // 램프 상태 업데이트
            mbTriggerUpdateIndoorLamp = true;
        }

        /// <summary>
        /// 도어 해제
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDoorOpen_Click(object sender, EventArgs e)
        {
            Door.EGroup groupIndex = (Door.EGroup)((Control)sender).Tag;

            if (CanUnlockDoor() == false)
            {
                return;
            }

            // 도어 언락
            foreach (var door in DoorManager.Groups[groupIndex])
            {
                door.IsUnlockPermit = true;
            }
            Thread.Sleep(100);
            // 램프 상태 업데이트
            mbTriggerUpdateIndoorLamp = true;
        }

        private void CDialogDoor_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 프로그램이 켜져있는 동안 폼 닫지 않음
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus;
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }
            if (Keys.Alt == ModifierKeys || ModifierKeys == Keys.F4)
            {
                e.Cancel = true;
            }
        }

        private void BtnUnlockAllDoors_Click(object sender, EventArgs e)
        {
            if (CanUnlockDoor() == false)
            {
                return;
            }

            // 도어 언락
            foreach (var door in DoorManager.Doors.Values)
            {
                door.IsUnlockPermit = true;
            }

            Thread.Sleep(100);
            // 램프 상태 업데이트
            mbTriggerUpdateIndoorLamp = true;
        }

        private void BtnLockAllDoors_Click(object sender, EventArgs e)
        {
            if (CanLockDoor() == false)
            {
                return;
            }

            // 도어 열림 상태 OFF
            //foreach (var door in DoorManager.Doors.Values)
            //{
            //    // ! 도어락에서 하드웨어적으로 열쇠 제거시 락이 해제되는 기능이 있는 타입인 경우 `IsLocked`를 확인해야함
            //    //if (door.IsLocked == false)
            //    if (door.IsOpened == true)
            //    {
            //        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.DOOR_IS_OPENED_PARAM1, door.DoorIndex);
            //        return;
            //    }
            //    if (door.HasOutsideKey == false)
            //    {
            //        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.DOOR_KEY_IS_NOT_DETECTED_PARAM1, door.DoorIndex);
            //        return;
            //    }
            //}

            // 도어 잠금
            foreach (var door in DoorManager.Doors.Values)
            {
                door.IsUnlockPermit = false;
            }
            Thread.Sleep(100);
            // 램프 상태 업데이트
            mbTriggerUpdateIndoorLamp = true;
        }

        private bool CanLockDoor()
        {
            // 내부 키 체커 감지 확인
            foreach (var index in m_objDocument.m_objProcessMain.GetSafetyIO().MaintKeyCheckers)
            {
                if (m_objDocument.GetIOBit(index) == true)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.DOOR_EQUIPMENT_INSIDE_KEY_IS_DETECTED_PARAM1, index);
                    return false;
                }
            }

            // ! H/W 지원안함
            //// 실내등 소등 상태 확인
            //if (
            //    m_objDocument.IsMasterLogin == false
            //    && (m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_MAINT_LAMP_ON) == true
            //        )
            //    )
            //{
            //    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.INSIDE_LAMP_IS_ON);
            //    return false;
            //}

            // 내부 작업자 확인
            if (
                m_objDocument.IsMasterLogin == false
                && m_objDocument.m_objProcessMain.GetSafetyIO().HumanDetectSensers.Any(i => m_objDocument.GetIOBit(i) == true) == true
                )
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.LOCK_DOOR_FAIL_CHECK_INSIDE);
                return false;
            }

            return true;
        }

        private bool CanUnlockDoor()
        {
            bool bIsAutoMode = false == m_objDocument.GetIOBit(CDeviceIODefine.EDigitalInput.X_SAFETY_MODE_STATUS_SIGNAL_TEACH);
            // 도어 언락 조건
            // 정면, 후면 세이프티 키 모드 둘다 AUTO인 경우
            if (bIsAutoMode == true)
            {
                // Msg: 세이프티 키 모드 TEACH가 아닙니다.
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAFETY_KEY_MODE_IS_NOT_TEACH);
                return false;
            }

            SpinWait.SpinUntil(() => mModeChangeDelayTick >= 5, 500);

            return true;
        }

        /// <summary>
        /// 점검 그룹 아이템 선택
        /// </summary>
        /// <param name="itemIndex">그룹 아이템 인덱스</param>
        private void SetCheckGroupItemIndex(int itemIndex)
        {
            Utils.InRange(ref itemIndex, 0, mCheckGroupItems.Count - 1);
            mCheckGroupIndex = itemIndex;

            SetCheckPageIndex(0);
        }

        /// <summary>
        /// 점검 아이템 페이지 선택
        /// </summary>
        /// <param name="pageIndex">페이지 인덱스</param>
        private void SetCheckPageIndex(int pageIndex)
        {
            int pageDisplayItemCount = mControlUcCheckItems.Length;
            int itemCount = mCheckGroupItems[mCheckGroupIndex].CheckItems.Count;
            mCheckPageCount = itemCount / pageDisplayItemCount;
            if (itemCount % pageDisplayItemCount != 0)
            {
                mCheckPageCount++;
            }

            Utils.InRange(ref pageIndex, 0, mCheckPageCount - 1);

            mCheckPageIndex = pageIndex;
        }

        private void BtnNaviCheckItemFirst_Click(object sender, EventArgs e)
        {
            SetCheckPageIndex(0);
        }

        private void BtnNaviCheckItemPrivious_Click(object sender, EventArgs e)
        {
            int setIndex = mCheckPageIndex - 1;
            SetCheckPageIndex(setIndex);
        }

        private void BtnNaviCheckItemNext_Click(object sender, EventArgs e)
        {
            int setIndex = mCheckPageIndex + 1;
            SetCheckPageIndex(setIndex);
        }

        private void BtnNaviCheckItemLast_Click(object sender, EventArgs e)
        {
            SetCheckPageIndex(mCheckPageCount);
        }

        private void ucCellCheckGroupItem_Click(object sender, EventArgs e)
        {
            SetCheckGroupItemIndex(Convert.ToInt32(((Control)sender).Tag));
        }

        private void BtnTitleDoorControl_Click(object sender, EventArgs e)
        {
            TcContent.SelectedIndex = 0;
        }

        private void BtnTitleInterlockMap_Click(object sender, EventArgs e)
        {
            TcContent.SelectedIndex = 1;
        }

        private void BtnTitleFfuMap_Click(object sender, EventArgs e)
        {
            TcContent.SelectedIndex = 2;
        }

        private void UcCellInterlockMapItem_Click(object sender, EventArgs e)
        {
            var selectItem = mInterlockMapItems.FirstOrDefault(i => ReferenceEquals(sender, i.Control.BtnCellData));
            if (selectItem == null)
            {
                return;
            }

            int findIndex = mCheckGroupItems.FindIndex(i => i.GroupIndex == selectItem.GroupIndex);
            if (findIndex < 0)
            {
                return;
            }
            SetCheckGroupItemIndex(findIndex);
        }

        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            do
            {
                // 알람 없는 경우 
                if (false == m_objDocument.GetIsAlarmMessage())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                    break;
                }

                if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                }
                else
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                }

                // 버튼 로그 추가
                string strLog = string.Format("[{0} : {1}]", "BtnBuzzerOff_Click", m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus().ToString());
                m_objDocument.SetUpdateButtonLog(this, strLog);
            } while (false);
        }

        private void BtnPasswordShow_MouseDown(object sender, MouseEventArgs e)
        {
            mbPasswordShow = true;
        }

        private void BtnPasswordShow_MouseUp(object sender, MouseEventArgs e)
        {
            mbPasswordShow = false;
        }

        private void BtnPasswordDisplay_Click(object sender, EventArgs e)
        {
            TextPassword.Text = "";
            ActiveControl = TextPassword;
        }

        private void UcEnableSwitchCheck_Click(object sender, EventArgs e)
        {
            if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                return;
            }

            bool currentSignal = m_objDocument.m_objProcessMain.GetSafetyIO().IsAnyEnableSwitchGripped();
            foreach (var index in m_objDocument.m_objProcessMain.GetSafetyIO().EnableSwitchGrips)
            {
                m_objDocument.SetIOBit(index, !currentSignal);
            }
        }

        private void UcUpperDoorCheck_Click(object sender, EventArgs e)
        {
            if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                return;
            }

            var signalControllers = m_objDocument.m_objProcessMain.m_objProcessMotion.LoadInterface.SignalControllers as LoadInterfaceSignalController[];
            signalControllers[0].UpperBits.InterlockDoorClosed.Value = !signalControllers[0].UpperBits.InterlockDoorClosed.Value;
        }

        private void UcLowerDoorCheck_Click(object sender, EventArgs e)
        {
            if (CDefine.ESimulationMode.SIMULATION_MODE_OFF == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                return;
            }

            //var signalControllers = m_objDocument.m_objProcessMain.m_objProcessMotion.UnloadInterface.SignalControllers as UnloadInterfaceSignalController[];
            //signalControllers[0].LowerBits.InterlockDoorClosed.Value = !signalControllers[0].LowerBits.InterlockDoorClosed.Value;
        }

        private void BtnTouch_Click(object sender, EventArgs e)
        {
            TextPassword.Text = "";
            var dialog = new PopupKeyPad(m_objDocument)
            {
                Value = TextPassword.Text
            };
            dialog.ValueChanged += (s, ev) =>
            {
                TextPassword.Text = dialog.Value;
            };
            dialog.CheckOK += (s, ev) =>
            {
                BtnClose_Click(this, EventArgs.Empty);
            };
            dialog.OkButtonText = BtnClose.Text;
            dialog.InitializeFromBaseControl(BtnPasswordDisplay, ratioX: 3, ratioY: 4);
            dialog.Show(this);
        }

        private void BtnLanguage_Click(object sender, EventArgs e)
        {
            CDialogSelectLanguage objDialogSelectLanguage = new CDialogSelectLanguage(m_objDocument);
            objDialogSelectLanguage.InitializeFormBaseButton(BtnLanguage);
            objDialogSelectLanguage.Show(this);
        }

        private void TextPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                case (char)Keys.Space:
                    e.Handled = true;
                    BtnClose_Click(this, EventArgs.Empty);
                    break;

                default:
                    // 숫자와 백스페이스만 허용
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void TextPassword_LostFocus(object sender, EventArgs e)
        {
            // `TextPassword`가 포커스를 잃었을 때, 비밀번호를 초기화하고 다시 포커스를 설정합니다.
            ActiveControl = TextPassword;
            TextPassword.Text = "";
        }
    }
}