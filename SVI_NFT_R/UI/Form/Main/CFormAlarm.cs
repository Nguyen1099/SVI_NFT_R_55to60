using Mcc;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormAlarm : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 도큐먼트
        /// </summary>
        private readonly CDocument m_objDocument;
        /// <summary>
        /// 선택된 알람 리스트 행
        /// </summary>
        private int m_iSelectedRow = 0;
        /// <summary>
        /// 알람 리스트 칼럼 정의
        /// </summary>
        private enum EAlarmListColumn
        {
            ALID = 0,
            ALTX,
            LEVEL
        };
        /// <summary>
        /// 공유 메모리에 있는 알람 메세지를 읽어서 그리드 뷰 갱신
        /// </summary>
        public delegate void DelegateSetAlarmMessageUpdate();
        public DelegateSetAlarmMessageUpdate m_delegateSetAlarmMessageUpdate;
        private List<CDatabaseDefine.CAlarmData> m_objAlarmDataList;
        /// <summary>
        /// 이미지 박스 생성
        /// </summary>
        private List<PictureBox> m_objAlarmImageList;
        private Point mMouseStartLocation;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CFormAlarm(CDocument objDocument)
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
        private void CFormAlarm_Load(object sender, EventArgs e)
        {
            // 초기화
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFormAlarm_FormClosed(object sender, FormClosedEventArgs e)
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
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                // 버튼 색상 정의
                SetButtonColor();
                // 설비 이미지 화면 초기화
                if (false == InitializeEquipmentImage())
                {
                    break;
                }
                // 알람 리스트 그리드 뷰 초기화
                string[] strAlarmList = { EAlarmListColumn.ALID.ToString(), EAlarmListColumn.ALTX.ToString(), EAlarmListColumn.LEVEL.ToString() };
                if (false == InitializeGridView(GridViewAlarmList, strAlarmList))
                {
                    break;
                }
                // 알람 갱신 델리게이트 생성
                m_delegateSetAlarmMessageUpdate = new DelegateSetAlarmMessageUpdate(SetAlarmMessageUpdate);
                // 알람 데이터 리스트 초기화
                m_objAlarmDataList = new List<CDatabaseDefine.CAlarmData>();
                // 알람 이미지 리스트 초기화
                m_objAlarmImageList = new List<PictureBox>();
                // 타이머 외부에서 제어
                timer.Interval = 500;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(BtnTitleAlarmTime, m_colorLabelSub);
            base.SetButtonBackColor(BtnAlarmTime, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleAlarmPart, m_colorLabelSub);
            base.SetButtonBackColor(BtnAlarmPart, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleAlarmText, m_colorLabelSub);
            base.SetButtonBackColor(BtnAlarmText, m_colorLabelData);
            base.SetButtonBackColor(BtnTitleAlarmDescription, m_colorLabelSub);
            SetControlBackColor(pnlAlarmDescription, m_colorLabelData);
            SetControlBackColor(txtAlarmDescription, m_colorLabelData);
            base.SetButtonBackColor(BtnAlarmReset, m_colorNormal);
            base.SetButtonBackColor(BtnAlarmAllReset, m_colorNormal);
            base.SetButtonBackColor(BtnBuzzerOff, m_colorNormal);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    //&& false == btn.Name.Equals("")                          
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        private void SetControlBackColor(Control target, Color color)
        {
            if (target.BackColor != color)
            {
                target.BackColor = color;
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
                    base.SetControlButtonEnable(Controls, true);
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
                SetButtonChangeLanguage(BtnTitleAlarmTime);
                SetButtonChangeLanguage(BtnTitleAlarmPart);
                SetButtonChangeLanguage(BtnTitleAlarmText);
                SetButtonChangeLanguage(BtnTitleAlarmDescription);
                SetButtonChangeLanguage(BtnAlarmReset);
                SetButtonChangeLanguage(BtnAlarmAllReset);
                SetButtonChangeLanguage(BtnBuzzerOff);
                SetButtonChangeLanguage(BtnAlarmImage);
                SetButtonChangeLanguage(BtnAlarmSop);
                // 알람 리스트 갱신
                SetAlarmList();

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
            }
        }

        /// <summary>
        /// 그리드 뷰 초기화
        /// </summary>
        /// <param name="objGridView"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        private bool InitializeGridView(DataGridView objGridView, string[] strColumnName)
        {
            bool bReturn = false;

            do
            {
                // 그리드 뷰 기본 스타일 초기화
                if (false == base.InitializeGridView(objGridView))
                {
                    break;
                }
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(string.Format("{0}", iLoopColumn), strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                // ID, LEVEL 사이즈 조정
                objGridView.Columns[(int)EAlarmListColumn.ALID].Width = (int)(objGridView.Width * 0.07);
                objGridView.Columns[(int)EAlarmListColumn.LEVEL].Width = (int)(objGridView.Width * 0.13);
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, 10.0);

                bReturn = true;
            } while (false);

            return bReturn;
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
                pictureBoxEquipmentImage.BackgroundImage = SetImageOpacity(SVI_NFT_R.Properties.Resources.TOP_VIEW_FLOW, 0.3f);

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
        /// 알람 리스트 갱신
        /// </summary>
        private void SetAlarmList()
        {
            DataGridView objGridView = GridViewAlarmList;

            do
            {
                try
                {
                    if (
                        null == objGridView.CurrentCell
                        || 0 == m_objAlarmDataList.Count
                        )
                    {
                        break;
                    }
                    int iSelectedRow = objGridView.CurrentCell.RowIndex;
                    // 알람 리스트 데이터 갱신
                    SetAlarmListData(iSelectedRow);

                    if (pictureBoxEquipmentImage.Visible == true)
                    {
                        // 알람 리스트 이미지 갱신
                        SetAlarmListImage(iSelectedRow);
                    }

                    // 현재 행 갱신
                    m_iSelectedRow = iSelectedRow;

                    // 중 알람인 경우 텍스트 빨간색
                    Color objTextColor = m_colorRed;
                    if (CDatabaseDefine.EAlarmLevelList.LIGHT == m_objAlarmDataList[m_iSelectedRow].eAlarmLevel)
                    {
                        objTextColor = m_colorYellow;
                    }
                    SetButtonBackColor(BtnAlarmTime, objTextColor);
                    SetButtonBackColor(BtnAlarmPart, objTextColor);
                    SetButtonBackColor(BtnAlarmText, objTextColor);
                    SetControlBackColor(pnlAlarmDescription, objTextColor);
                    SetControlBackColor(txtAlarmDescription, objTextColor);
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 알람 데이터 클리어
        /// </summary>
        private void SetAlarmListDataClear()
        {
            // 그리드 뷰 셀이 선택되면 선택된 알람 리스트에 데이터를 버튼에 뿌려줌
            try
            {
                BtnAlarmTime.Text = "";
                BtnAlarmPart.Text = "";
                BtnAlarmText.Text = "";
                txtAlarmDescription.Text = "";

                // Alarm Image 화면으로 전환
                pictureBoxEquipmentImage.Visible = true;
                pdfViewer1.Document?.Dispose();
                pdfViewer1.Document = null;
                pdfViewer1.Visible = false;
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 알람 이미지 리스트 클리어
        /// </summary>
        private void SetAlarmListImageClear()
        {
            for (int iLoopImageList = 0; iLoopImageList < m_objAlarmImageList.Count; iLoopImageList++)
            {
                m_objAlarmImageList[iLoopImageList].Image = null;
                m_objAlarmImageList[iLoopImageList].Visible = false;
            }
            m_objAlarmImageList.Clear();
        }

        /// <summary>
        /// 알람 데이터 갱신
        /// </summary>
        /// <param name="iRow"></param>
        private void SetAlarmListData(int iRow)
        {
            if (0 == m_objAlarmDataList.Count)
            {
                return;
            }

            // 그리드 뷰 셀이 선택되면 선택된 알람 리스트에 데이터를 버튼에 뿌려줌
            try
            {
                BtnAlarmTime.Text = m_objAlarmDataList[iRow].strAlarmDate;
                BtnAlarmPart.Text = $"{m_objAlarmDataList[iRow].strAlarmUnit} - {getAlarmCategoryFromALTX(m_objAlarmDataList[iRow].ALTX)}";
                // 언어별로 뿌려줌
                BtnAlarmText.Text = m_objAlarmDataList[iRow].strAlarmText[(int)m_objDocument.m_objConfig.GetOptionParameter().eLanguage];
                txtAlarmDescription.Text = m_objAlarmDataList[iRow].strAlarmActionText[(int)m_objDocument.m_objConfig.GetOptionParameter().eLanguage];
                txtAlarmDescription.SelectAll();
                SetRichTextBoxFont(txtAlarmDescription, Font.Name, Font.Size, Font.Style);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 알람 이미지 갱신
        /// </summary>
        /// <param name="iRow"></param>
        private void SetAlarmListImage(int iRow)
        {
            if (0 == m_objAlarmDataList.Count)
            {
                return;
            }

            // 그리드 뷰 셀이 선택되면 선택된 알람 리스트에 이미지를 뿌려줌
            try
            {
                CDatabaseDefine.EAlarmBoxTypeList eAlarmBoxType = m_objAlarmDataList[iRow].eAlarmBoxType;
                int iX = Convert.ToInt32(m_objAlarmDataList[iRow].iAlarmBoxRectangleX * pictureBoxEquipmentImage.Width);
                int iY = Convert.ToInt32(m_objAlarmDataList[iRow].iAlarmBoxRectangleY * pictureBoxEquipmentImage.Height);
                int iWidth = Convert.ToInt32(m_objAlarmDataList[iRow].iAlarmBoxRectangleWidth * pictureBoxEquipmentImage.Width);
                int iHeight = Convert.ToInt32(m_objAlarmDataList[iRow].iAlarmBoxRectangleHeight * pictureBoxEquipmentImage.Height);

                for (int iLoopImageList = 0; iLoopImageList < m_objAlarmImageList.Count; iLoopImageList++)
                {
                    m_objAlarmImageList[iLoopImageList].Image = null;
                    m_objAlarmImageList[iLoopImageList].Visible = false;
                }

                m_objAlarmImageList[iRow].Parent = pictureBoxEquipmentImage;
                m_objAlarmImageList[iRow].BackColor = Color.Transparent;
                m_objAlarmImageList[iRow].BorderStyle = BorderStyle.None;
                m_objAlarmImageList[iRow].Location = new Point(iX, iY);
                m_objAlarmImageList[iRow].Width = iWidth;
                m_objAlarmImageList[iRow].Height = iHeight;

                if (
                    0 < iWidth
                    && 0 < iHeight
                    )
                {
                    Bitmap objBitmap = new Bitmap(iWidth, iHeight);
                    Graphics objGraphics = Graphics.FromImage(objBitmap);
                    int iPenWidth = 3;
                    Pen objPen = new Pen(Color.Red, iPenWidth);
                    objGraphics.DrawRectangle(objPen, iPenWidth, iPenWidth, iWidth - (iPenWidth * 2), iHeight - (iPenWidth * 2));
                    m_objAlarmImageList[iRow].Image = objBitmap;
                    objGraphics.Dispose();

                    m_objAlarmImageList[iRow].Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 알람 메세지 갱신
        /// </summary>
        private void SetAlarmMessageUpdate()
        {
            DataGridView objGridView = GridViewAlarmList;

            do
            {
                try
                {
                    if (null == objGridView)
                    {
                        break;
                    }
                    m_objAlarmDataList.Clear();
                    SetAlarmListImageClear();

                    // 알람 히스토리 테이블에서 셋된 알람 리스트를 읽어온다.
                    {
                        objGridView.RowCount = m_objDocument.m_objAlarmList.Count;
                        int rowIndex = 0;
                        foreach (var row in m_objDocument.m_objAlarmList.SetAlarms.Reverse())
                        {
                            int alarmID = (int)row.Value;
                            string alarmDateTime = row.Key.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            var alarmData = m_objDocument.AlarmDataTable.Select(string.Format("[{0}] = {1}",
                                m_objDocument.m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationAlarm.HLGetTableSchemaName()[(int)CDatabaseDefine.EInformationAlarm.ID],
                                alarmID));
                            if (
                                null != alarmData
                                && 0 < alarmData.Length
                                )
                            {
                                CDatabaseDefine.CAlarmData objAlarmData = new CDatabaseDefine.CAlarmData();
                                objAlarmData.tAlarmOccuredDate = row.Key;
                                objAlarmData.strAlarmDate = alarmDateTime;
                                objAlarmData.iAlarmID = alarmID;
                                objAlarmData.ALTX = (string)alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALTX];
                                objAlarmData.eAlarmLevel = m_objDocument.m_objProcessDatabase.GetAlarmLevel((string)alarmData[0][(int)CDatabaseDefine.EInformationAlarm.LEVEL]);
                                objAlarmData.strAlarmUnit = (string)alarmData[0][(int)CDatabaseDefine.EInformationAlarm.UNIT];
                                objAlarmData.eAlarmBoxType = m_objDocument.m_objProcessDatabase.GetAlarmBoxType((string)alarmData[0][(int)CDatabaseDefine.EInformationAlarm.BOX_TYPE]);
                                objAlarmData.iAlarmBoxRectangleX = Convert.ToDouble(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_X]);
                                objAlarmData.iAlarmBoxRectangleY = Convert.ToDouble(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_Y]);
                                objAlarmData.iAlarmBoxRectangleWidth = Convert.ToDouble(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_WIDTH]);
                                objAlarmData.iAlarmBoxRectangleHeight = Convert.ToDouble(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.BOX_RECTANGLE_HEIGHT]);
                                objAlarmData.strAlarmText[(int)CDefine.ELanguage.LANGUAGE_KOREA] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_KOREA]);
                                objAlarmData.strAlarmActionText[(int)CDefine.ELanguage.LANGUAGE_KOREA] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_KOREA]);
                                objAlarmData.strAlarmText[(int)CDefine.ELanguage.LANGUAGE_VIETNAM] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_VIETNAM]);
                                objAlarmData.strAlarmActionText[(int)CDefine.ELanguage.LANGUAGE_VIETNAM] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_VIETNAM]);
                                objAlarmData.strAlarmText[(int)CDefine.ELanguage.LANGUAGE_ENGLISH] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_ENGLISH]);
                                objAlarmData.strAlarmActionText[(int)CDefine.ELanguage.LANGUAGE_ENGLISH] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_ENGLISH]);
                                objAlarmData.strAlarmText[(int)CDefine.ELanguage.LANGUAGE_CHINA] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_TEXT_CHINA]);
                                objAlarmData.strAlarmActionText[(int)CDefine.ELanguage.LANGUAGE_CHINA] = Convert.ToString(alarmData[0][(int)CDatabaseDefine.EInformationAlarm.ALARM_ACTION_TEXT_CHINA]);
                                m_objAlarmDataList.Add(objAlarmData);

                                PictureBox objPictureBox = new PictureBox();
                                objPictureBox.Visible = false;
                                m_objAlarmImageList.Add(objPictureBox);

                                // 그리드 뷰에 데이터 표시
                                objGridView.Rows[rowIndex].Cells[(int)EAlarmListColumn.ALID].Value = string.Format("{0}", objAlarmData.iAlarmID);
                                objGridView.Rows[rowIndex].Cells[(int)EAlarmListColumn.ALTX].Value = objAlarmData.ALTX;
                                objGridView.Rows[rowIndex].Cells[(int)EAlarmListColumn.LEVEL].Value = string.Format("{0}", objAlarmData.eAlarmLevel.ToString());
                                rowIndex++;
                            }
                        }
                    }

                    for (int iLoopRow = 0; iLoopRow < objGridView.Rows.Count; iLoopRow++)
                    {
                        // 중 알람인 경우 텍스트 빨강
                        if (CDatabaseDefine.EAlarmLevelList.HEAVY.ToString() == objGridView[(int)EAlarmListColumn.LEVEL, iLoopRow].Value.ToString())
                        {
                            objGridView.Rows[iLoopRow].DefaultCellStyle.ForeColor = Color.Red;
                            objGridView.Rows[iLoopRow].DefaultCellStyle.BackColor = (iLoopRow % 2) == 1 ? Color.FromArgb(62, 62, 66) : Color.FromArgb(30, 30, 30);
                            objGridView.Rows[iLoopRow].DefaultCellStyle.SelectionForeColor = m_colorGridSelectionFore;
                        }
                        // 경 알람인 경우 텍스트 노랑
                        else if (CDatabaseDefine.EAlarmLevelList.LIGHT.ToString() == objGridView[(int)EAlarmListColumn.LEVEL, iLoopRow].Value.ToString())
                        {
                            objGridView.Rows[iLoopRow].DefaultCellStyle.ForeColor = Color.Yellow;
                            objGridView.Rows[iLoopRow].DefaultCellStyle.BackColor = (iLoopRow % 2) == 1 ? Color.FromArgb(62, 62, 66) : Color.FromArgb(30, 30, 30);
                            objGridView.Rows[iLoopRow].DefaultCellStyle.SelectionForeColor = m_colorGridSelectionFore;
                        }
                    }

                    // 첫 줄 선택된 상태로
                    if (
                        0 != objGridView.Rows.Count
                        && null != objGridView[0, 0]
                        )
                    {
                        objGridView[0, 0].Selected = true;
                    }
                    // 선택된 셀을 찾아서 데이터 & 이미지 갱신해줌
                    SetAlarmList();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (m_objAlarmDataList.Count != m_objDocument.m_objAlarmList.Count)
            {
                SetAlarmMessageUpdate();
            }

            // Alarm SOP가 있는지 체크하여 버튼 활성화
            {
                bool bVisible = false;
                DataGridView objGridView = GridViewAlarmList;
                if (
                    null != objGridView.CurrentCell
                    && m_objAlarmDataList.Count > 0
                    )
                {
                    string pdfPath = string.Format($"{m_objDocument.m_objConfig.GetAlarmSopFilePath()}\\{m_objAlarmDataList[objGridView.CurrentCell.RowIndex].ALTX}.PDF");
                    var pdfInfo = new FileInfo(pdfPath);
                    bVisible = pdfInfo.Exists == true ? true : false;
                }

                BtnAlarmSop.Visible = bVisible;

                SetButtonBackColor(BtnAlarmSop, pdfViewer1.Visible == true ? m_colorClick : m_colorNormal);
                SetButtonBackColor(BtnAlarmImage, pictureBoxEquipmentImage.Visible == true ? m_colorClick : m_colorNormal);
            }

            // 알람 사각박스 깜박이는 효과
            try
            {
                do
                {
                    if (
                        true == m_objDocument.m_objConfig.GetOptionParameter().bUseCIM
                        && CCIMDefine.EControlState.CRST_ONLINE_REMOTE != m_objDocument.m_eControlState
                        )
                    {
                        SetButtonEnable(BtnAlarmReset, false);
                        SetButtonEnable(BtnAlarmAllReset, false);
                    }
                    else
                    {
                        if (m_objAlarmDataList.Count > 0)
                        {
                            DataGridView objGridView = GridViewAlarmList;
                            CAlarmDefine.EAlarmList alarmID = (CAlarmDefine.EAlarmList)Convert.ToInt32(objGridView[(int)EAlarmListColumn.ALID, m_iSelectedRow].Value);
                            if (false == m_objDocument.m_objProcessMain.CheckSafetyAlarmOccured(alarmID))
                            {
                                SetButtonEnable(BtnAlarmReset, true);
                                SetButtonEnable(BtnAlarmAllReset, true);
                            }
                            else
                            {
                                SetButtonEnable(BtnAlarmReset, false);
                                SetButtonEnable(BtnAlarmAllReset, true);
                            }
                        }
                        else
                        {
                            SetButtonEnable(BtnAlarmReset, false);
                            SetButtonEnable(BtnAlarmAllReset, false);
                        }
                    }

                    if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
                    {
                        base.SetButtonBackColor(BtnBuzzerOff, m_colorNormal);
                    }
                    else
                    {
                        base.SetButtonBackColor(BtnBuzzerOff, m_colorClick);
                    }

                    // 알람 없는 경우
                    if (0 == m_objAlarmImageList.Count)
                    {
                        SetButtonBackColor(BtnAlarmTime, m_colorLabelData);
                        SetButtonBackColor(BtnAlarmPart, m_colorLabelData);
                        SetButtonBackColor(BtnAlarmText, m_colorLabelData);
                        SetControlBackColor(pnlAlarmDescription, m_colorLabelData);
                        SetControlBackColor(txtAlarmDescription, m_colorLabelData);
                        break;
                    }

                    // 중 알람인 경우 텍스트 빨간색
                    Color objTextColor = m_colorRed;
                    if (CDatabaseDefine.EAlarmLevelList.LIGHT == m_objAlarmDataList[m_iSelectedRow].eAlarmLevel)
                    {
                        objTextColor = m_colorYellow;
                    }
                    SetButtonBackColor(BtnAlarmTime, objTextColor);
                    SetButtonBackColor(BtnAlarmPart, objTextColor);
                    SetButtonBackColor(BtnAlarmText, objTextColor);
                    SetControlBackColor(pnlAlarmDescription, objTextColor);
                    SetControlBackColor(txtAlarmDescription, objTextColor);

                    // 깜빡깜빡
                    m_objAlarmImageList[m_iSelectedRow].Visible = !m_objAlarmImageList[m_iSelectedRow].Visible;

                } while (false);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 알람 리스트 셀 선택 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewAlarmList_CurrentCellChanged(object sender, EventArgs e)
        {
            // 선택된 셀을 찾아서 데이터 & 이미지 갱신해줌
            DataGridView objGridView = sender as DataGridView;
            if (null != objGridView.CurrentCell)
            {
                BtnAlarmImage_Click_1(null, null);
            }
        }

        /// <summary>
        /// 알람 리스트에서 선택된 셀만 리셋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarmReset_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = GridViewAlarmList;
            string strLog;
            do
            {
                try
                {
                    if (
                        CDefine.ERunStatus.Stopping == m_objDocument.GetRunStatus()
                        || 0 == m_objDocument.m_objAlarmList.Count
                        )
                    {
                        break;
                    }

                    // 중알람 발생시, 설비 이전상태가 Availability UP일경우 알람클리어 불가
                    if (
                       m_objDocument.GetIsHeavyAlarm() == true
                       && m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] == CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_UP
                       )
                    {
                        break;
                    }

                    // 설비 상태보고 대기
                    m_objDocument.m_objProcessCIM.WaitSyncEquipmentStateReport();

                    // 공유 메모리에 있는 알람 아이디에 일치하는 메세지 클리어
                    int iAlarmID = Convert.ToInt32(objGridView[(int)EAlarmListColumn.ALID, m_iSelectedRow].Value);
                    DateTime alarmDateTime = m_objAlarmDataList[m_iSelectedRow].tAlarmOccuredDate;

                    if (AlarmClearAbleCheck(iAlarmID) == false)
                    {
                        strLog = string.Format("[{0}] [Alarm ID : {1}] [Alarm Reset]", "BtnAlarmReset_Fail", iAlarmID);
                        m_objDocument.SetUpdateButtonLog(this, strLog);
                        return;
                    }

                    m_objDocument.SetAlarmClear(alarmDateTime, iAlarmID);
                    // 마지막 셀까지 지우고 리스트 데이터 클리어
                    if (0 == m_objDocument.m_objAlarmList.Count)
                    {
                        SetAlarmListDataClear();
                        m_objDocument.m_objProcessMain.m_objProcessTowerLamp.ClearHistory(CProcessTowerLamp.ETowerLamp.Fault);
                        m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                        if (MccLogManager.ShouldWriteAlarmActionEndLog == true)
                        {
                            MccLogManager.GetAlarmActions().BatchWriteEnd();
                            MccLogManager.ShouldWriteAlarmActionEndLog = false;
                        }
                    }
                    else
                    {
                        SetAlarmList();
                    }
                    // 버튼 로그 추가
                    strLog = string.Format("[{0}] [Alarm ID : {1}] [Alarm Reset]", "BtnAlarmReset_Click", iAlarmID);
                    m_objDocument.SetUpdateButtonLog(this, strLog);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);
        }

        /// <summary>
        /// 알람 리스트 전체 리셋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarmAllReset_Click(object sender, EventArgs e)
        {
            DataGridView objGridView = GridViewAlarmList;
            string strLog;

            // 공유 메모리에 있는 알람 메세지 클리어
            if (
                CDefine.ERunStatus.Stopping != m_objDocument.GetRunStatus()
                && objGridView.RowCount > 0
                )
            {
                // 중알람 발생시, 설비 이전상태가 Availability UP일경우 알람클리어 불가
                if (
                   m_objDocument.GetIsHeavyAlarm() == true
                   && m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.PREVIOUS_STATE] == CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_UP
                   )
                {
                    return;
                }

                // 설비 상태보고 대기
                m_objDocument.m_objProcessCIM.WaitSyncEquipmentStateReport();

                lock (m_objDocument.m_objAlarmList)
                {
                    Dictionary<DateTime, CAlarmDefine.EAlarmList> ableClearAlarm = new Dictionary<DateTime, CAlarmDefine.EAlarmList>();
                    List<CAlarmDefine.EAlarmList> canNotClearAlarm = new List<CAlarmDefine.EAlarmList>();
                    foreach (var alarm in m_objDocument.m_objAlarmList.SetAlarms)
                    {
                        if (AlarmClearAbleCheck((int)alarm.Value) == false)
                        {
                            strLog = string.Format("[{0}] [Alarm All Reset]", "BtnAlarmAllReset_Fail");
                            m_objDocument.SetUpdateButtonLog(this, strLog);
                            return;
                        }
                        if (true == m_objDocument.m_objProcessMain.CheckSafetyAlarmOccured(alarm.Value))
                        {
                            canNotClearAlarm.Add(alarm.Value);
                        }
                        else
                        {
                            ableClearAlarm.Add(alarm.Key, alarm.Value);
                        }
                    }

                    if (0 != canNotClearAlarm.Count)
                    {
                        foreach (var item in ableClearAlarm)
                        {
                            m_objDocument.SetAlarmClear(item.Key, (int)item.Value);
                        }
                        SetAlarmList();

                        StringBuilder sb = new StringBuilder();
                        if (CDefine.ELanguage.LANGUAGE_KOREA == m_objDocument.m_objConfig.GetOptionParameter().eLanguage)
                        {
                            sb.AppendLine("다음 알람들은 해결되지 않아 클리어 할 수 없습니다:");
                        }
                        else
                        {
                            sb.AppendLine("The following alarms have not been resolved and cannot be cleared:");
                        }
                        sb.AppendLine();
                        foreach (var item in canNotClearAlarm)
                        {
                            sb.AppendLine(" - " + item.ToString());
                        }
                        MessageBox.Show(sb.ToString(), Program.ID, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                m_objDocument.SetAlarmClear();
                // 그리드 뷰 클리어
                objGridView.Rows.Clear();
                m_objAlarmDataList.Clear();
                SetAlarmListDataClear();
                SetAlarmListImageClear();
                m_objDocument.m_objProcessMain.m_objProcessTowerLamp.ClearHistory(CProcessTowerLamp.ETowerLamp.Fault);
                m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                if (MccLogManager.ShouldWriteAlarmActionEndLog == true)
                {
                    MccLogManager.GetAlarmActions().BatchWriteEnd();
                    MccLogManager.ShouldWriteAlarmActionEndLog = false;
                }
                // 버튼 로그 추가
                strLog = string.Format("[{0}] [Alarm All Reset]", "BtnAlarmAllReset_Click");
                m_objDocument.SetUpdateButtonLog(this, strLog);
            }
        }

        private bool AlarmClearAbleCheck(int nAlarmID)
        {
            ////X01D	Smoke & Temp Reset Button	Reset Button을 눌렀을때 X01E X01C off시 alarm clear
            //if (nAlarmID == (int)CAlarmDefine.EAlarmList.UT_SVI_F_ELECTRONICBOX_TEMPERATURE_1_N_OVER40)
            //{
            //    if (m_objDocument.m_bTempSmokeAlarmHappened == true)
            //    {
            //        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_ALARM, CAlarmDefine.EMessageList.FIRST_PUSH_RESET_OVER_TEMPERATURE);
            //        return false;
            //    }
            //}
            //else if (nAlarmID == (int)CAlarmDefine.EAlarmList.UT_ELECTRONICBOX_SMOKE_SENSOR_1_N_DETECTED)
            //{
            //    if (m_objDocument.m_bTempSmokeAlarmHappened == true)
            //    {
            //        m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_ALARM, CAlarmDefine.EMessageList.FIRST_PUSH_RESET_SMOKE_DETECTION);
            //        return false;
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 부저 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            do
            {
                // 알람 없는 경우 
                if (0 == m_objAlarmDataList.Count)
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

        /// <summary>
        /// 좌표 얻는 기능 구현을 위해 사용된 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxEquipmentImage_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    mMouseStartLocation = e.Location;
                    break;
            }
        }

        /// <summary>
        /// 좌표 얻는 기능 구현을 위해 사용된 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxEquipmentImage_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    Size rectangleSize = new Size(e.Location.X - mMouseStartLocation.X, e.Location.Y - mMouseStartLocation.Y);
                    Clipboard.Clear();
                    Clipboard.SetText(string.Format("Location:{0}, Size:{1}", mMouseStartLocation.ToString(), rectangleSize.ToString()));

                    Trace.WriteLine(string.Format("Location:{0}, Size:{1}", mMouseStartLocation.ToString(), rectangleSize.ToString()));
                    break;
            }
        }

        private string getAlarmCategoryFromALTX(string altx)
        {
            string categorySymbol = altx.Substring(0, 2);
            switch (categorySymbol)
            {
                case "VC": return (string)Resource.Get("ALARM_CATEGORY_VACUUM");
                case "VA": return (string)Resource.Get("ALARM_CATEGORY_VALIDATION");
                case "MO": return (string)Resource.Get("ALARM_CATEGORY_MOTOR");
                case "IN": return (string)Resource.Get("ALARM_CATEGORY_INSPECTION");
                case "VI": return (string)Resource.Get("ALARM_CATEGORY_VISION");
                case "CY": return (string)Resource.Get("ALARM_CATEGORY_CYLINDER");
                case "SE": return (string)Resource.Get("ALARM_CATEGORY_SENSOR");
                case "CO": return (string)Resource.Get("ALARM_CATEGORY_COMMUNICATION");
                case "DO": return (string)Resource.Get("ALARM_CATEGORY_DOOR");
                case "TE": return (string)Resource.Get("ALARM_CATEGORY_TEMPERATURE");
                case "EM": return (string)Resource.Get("ALARM_CATEGORY_EMO");
                case "UT": return (string)Resource.Get("ALARM_CATEGORY_UTILITY");
                case "OP": return (string)Resource.Get("ALARM_CATEGORY_OPERATION");
                case "ET": return (string)Resource.Get("ALARM_CATEGORY_ETC");
                case "EF": return (string)Resource.Get("ALARM_CATEGORY_EFFICIENCY");
            }
            return (string)Resource.Get("ALARM_CATEGORY_RESERVED");
        }

        private void BtnAlarmSop_Click_1(object sender, EventArgs e)
        {
            pictureBoxEquipmentImage.Visible = false;
            DataGridView objGridView = GridViewAlarmList;
            int iSelectedRow = objGridView.CurrentCell.RowIndex;
            // 알람 리스트 데이터 갱신
            // 알람 SOP 갱신 (현재알람의 SOP가있을경우)
            string pdfPath = string.Format($"{m_objDocument.m_objConfig.GetAlarmSopFilePath()}\\{m_objAlarmDataList[iSelectedRow].ALTX}.PDF");
            var pdfInfo = new FileInfo(pdfPath);
            if (pdfInfo.Exists == false)
            {
                // File이 없거나 실패시 Alarm Image 화면으로 전환
                pictureBoxEquipmentImage.Visible = true;
                pdfViewer1.Visible = false;
            }
            else
            {
                PdfDocument pdfDocument = PdfDocument.Load(pdfPath);
                pdfViewer1.Document?.Dispose();
                pdfViewer1.Document = pdfDocument;
                pdfViewer1.Visible = true;
            }

            SetAlarmList();
        }

        private void BtnAlarmImage_Click_1(object sender, EventArgs e)
        {
            pictureBoxEquipmentImage.Visible = true;
            pdfViewer1.Document?.Dispose();
            pdfViewer1.Document = null;
            pdfViewer1.Visible = false;
            SetAlarmList();
        }
    }

}