using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogInterlockMessage : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 임의 인터락 데이터 리스트
        /// </summary>
        public List<CModelInterlock> m_ListInterlock;
        /// <summary>
        /// 알람 리스트 칼럼 정의
        /// </summary>
        private enum EInterlockListColumn
        {
            DATETIME,
            INTERLOCKID,
            MESSAGE,
            INTERLOCK_LIST_COLUMN_FINAL
        };
        private CDocument m_objDocument;
        private FileStream mBackupFile;
        private const string BACKUP_FILE_NAME = @".\cache\InterlockMessage.bak";

        /// <summary>
        /// 생성자.
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogInterlockMessage(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument as CDocument;
            m_ListInterlock = new List<CModelInterlock>();

            var directoryName = Path.GetDirectoryName(BACKUP_FILE_NAME);
            if (
                false == string.IsNullOrEmpty(directoryName)
                && false == Directory.Exists(directoryName)
                )
            {
                Directory.CreateDirectory(directoryName);
            }
            mBackupFile = File.Open(BACKUP_FILE_NAME, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            restore();
        }

        /// <summary>
        /// 다이얼로그 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogInterlockMessage_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 다이얼로그 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogInterlockMessage_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeInitialize();
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bResult = false;
            do
            {
                // 폼 초기화
                if (false == InitializeDialog())
                    break;

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            SetTimer(false);
        }

        /// <summary>
        /// 폼 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeDialog()
        {
            bool bReturn = false;

            do
            {
                // 알람 리스트 그리드 뷰 초기화
                string[] strInterlockList = { EInterlockListColumn.DATETIME.ToString(), EInterlockListColumn.INTERLOCKID.ToString(), EInterlockListColumn.MESSAGE.ToString() };
                if (false == InitializeGridView(this.dataGridViewInterlock, strInterlockList))
                    break;

                // 언어변환
                SetChangeLanguage();

                SetButtonColor();

                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                SetTimer(true);

                bReturn = true;
            } while (false);

            return bReturn;
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
                // 설정 언어에 따라서 그리드 뷰나 버튼에 폰트가 변경되어야 할 수도 있음.
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(this.dataGridViewInterlock, 9.0);
                //SetButtonChangeLanguage( this.BtnConfirm );
                //SetButtonChangeLanguage( this.BtnBuzzerOff );

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 버튼 언어변경 적용
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 버튼 언어변경 적용
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 버튼 색상 정의
        /// </summary>
        private void SetButtonColor()
        {
            base.SetButtonBackColor(this.BtnConfirm, m_colorNormal);
            base.SetButtonBackColor(this.BtnBuzzerOff, m_colorNormal);
        }

        /// <summary>
        /// 타이머 시작
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            if (true == bTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            if (bVisible == true)
                m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
        }

        /// <summary>
        /// 타이머 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 알람 리스트 데이터가 있는지 확인해서 뿌려줌
            SetDataGridViewData(m_ListInterlock);

            if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
            {
                base.SetButtonBackColor(this.BtnBuzzerOff, m_colorNormal);
            }
            else
            {
                base.SetButtonBackColor(this.BtnBuzzerOff, m_colorClick);
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
                    break;
                // 그리드 높이 조정
                objGridView.RowTemplate.Height = 30;
                // 그리드 뷰 ReadOnly
                objGridView.ReadOnly = true;
                // 그리드 뷰 다중 선택 x
                objGridView.MultiSelect = false;
                // 그리드 뷰 선택 모드 (행 전체 선택)
                objGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // 그리드 뷰 칼럼 추가
                for (int iLoopColumn = 0; iLoopColumn < strColumnName.Length; iLoopColumn++)
                {
                    objGridView.Columns.Add(strColumnName[iLoopColumn], strColumnName[iLoopColumn]);
                    // 칼럼 정렬 기능 x
                    objGridView.Columns[iLoopColumn].SortMode = DataGridViewColumnSortMode.NotSortable;

                }

                objGridView.Columns[0].Width = (int)(objGridView.Width * 0.14);
                objGridView.Columns[1].Width = (int)(objGridView.Width * 0.08);
                objGridView.Columns[2].Width = (int)(objGridView.Width * 0.78);

                // 그리드 뷰 크기 변경
                base.SetGridViewFont(objGridView, "신명조", 9.0, FontStyle.Regular);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 그리드 뷰 리스트 데이터 적용
        /// </summary>
        /// <param name="objInterlockList"></param>
        public void SetDataGridViewData(List<CModelInterlock> objInterlockList)
        {
            bool bCompare = true;

            do
            {
                // 현재 그리드 뷰에 알람 리스트와 비교 (길이 다르면 바로 갱신)
                if (objInterlockList.Count == this.dataGridViewInterlock.RowCount)
                {
                    for (int iLoopInterlockList = 0; iLoopInterlockList < objInterlockList.Count; iLoopInterlockList++)
                    {
                        if (string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", objInterlockList[iLoopInterlockList].m_objInterlockTime) != this.dataGridViewInterlock[0, iLoopInterlockList].Value.ToString())
                        {
                            bCompare = false;
                            break;
                        }
                    }
                }
                else
                {
                    bCompare = false;
                }

                // 데이터 같으면 다시 쓸 필요 없음
                if (true == bCompare)
                    break;

                // 갱신하는 부분
                lock (this.dataGridViewInterlock)
                {
                    this.dataGridViewInterlock.Rows.Clear();

                    for (int iLoopInterlockList = 0; iLoopInterlockList < objInterlockList.Count; iLoopInterlockList++)
                    {
                        string[] strRowData = new string[(int)EInterlockListColumn.INTERLOCK_LIST_COLUMN_FINAL];
                        strRowData[(int)EInterlockListColumn.DATETIME] = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", objInterlockList[iLoopInterlockList].m_objInterlockTime);
                        strRowData[(int)EInterlockListColumn.INTERLOCKID] = objInterlockList[iLoopInterlockList].m_strInterlockID;
                        strRowData[(int)EInterlockListColumn.MESSAGE] = objInterlockList[iLoopInterlockList].m_strInterlockMessage;
                        this.dataGridViewInterlock.Rows.Add(strRowData);
                    }
                }

            } while (false);
        }

        /// <summary>
        /// 리스트에 데이터 넣기
        /// </summary>
        /// <param name="time"></param>
        /// <param name="strInterlockID"></param>
        /// <param name="strMessage"></param>
        public void SetListData(DateTime time, string strInterlockID, string strMessage)
        {
            do
            {
                m_ListInterlock.Add(new CModelInterlock(
                    time,
                    strInterlockID,
                    strMessage));
                backup();

            } while (false);
        }

        /// <summary>
        /// 컴펌 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            // 데이터 그리드 뷰에서 인터락 모두 가져와서 CIM에 보고.
            InterlockConfirmEvent objInterlockConfirmEvent = new InterlockConfirmEvent();
            objInterlockConfirmEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;

            // 그리드 뷰에 있는 데이터 갯수 가져오기.
            int iItemCount = dataGridViewInterlock.Rows.Count;

            objInterlockConfirmEvent.BODY.INTERLOCKS = new InterlockConfirmEvent._InterlockConfirmEvent.InterlockConfirm[iItemCount];

            for (int iLoopCount = 0; iLoopCount < iItemCount; iLoopCount++)
            {
                objInterlockConfirmEvent.BODY.INTERLOCKS[iLoopCount].INTERLOCKID = dataGridViewInterlock.Rows[iLoopCount].Cells[(int)EInterlockListColumn.INTERLOCKID].Value.ToString();
                objInterlockConfirmEvent.BODY.INTERLOCKS[iLoopCount].MESSAGE = dataGridViewInterlock.Rows[iLoopCount].Cells[(int)EInterlockListColumn.MESSAGE].Value.ToString();
            }

            // CIM  Interlock 보고.
            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetInterlockConfirmEvent(objInterlockConfirmEvent);

            m_objDocument.GetMainFrame().GetFormView().Enabled = true;

            // 부저 Off 시킨다.
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();

            // 타워 램프 히스토리 삭제
            m_objDocument.m_objProcessMain.m_objProcessTowerLamp.ClearHistory(CProcessTowerLamp.ETowerLamp.Interlock);

            // 리스트 내용 삭제.
            m_ListInterlock.Clear();
            backup();

            // 다이얼로그 닫기.
            this.Hide();
        }

        /// <summary>
        /// 부저 오프 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            do
            {
                if (CDefine.EBuzzerValue.BUZZER_VALUE_ON == m_objDocument.m_objProcessMain.m_objProcessTowerLamp.GetBuzzerStatus())
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStop();
                }
                else
                {
                    m_objDocument.m_objProcessMain.m_objProcessTowerLamp.SetBuzzerStart();
                }
            } while (false);
        }

        /// <summary>
        /// 임의 인터락 데이터 클래스 구현
        /// </summary>
        [Serializable]
        public class CModelInterlock
        {
            /// <summary>
            /// 인터락 발생 시간
            /// </summary>
            public DateTime m_objInterlockTime;
            /// <summary>
            /// 인터락 ID
            /// </summary>
            public string m_strInterlockID;
            /// <summary>
            /// 인터락 메시지
            /// </summary>
            public string m_strInterlockMessage;

            public CModelInterlock(DateTime objInterlockTime, string strInterlockID, string strInterlockMessage)
            {
                m_objInterlockTime = objInterlockTime;
                m_strInterlockID = strInterlockID;
                m_strInterlockMessage = strInterlockMessage;
            }
        }

        private void CDialogInterlockMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }

            mBackupFile.Dispose();
        }

        private void backup()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            mBackupFile.SetLength(0);
            mBackupFile.Seek(0, SeekOrigin.Begin);
            serializer.Serialize(mBackupFile, m_ListInterlock);
        }

        private void restore()
        {
            if (0 == mBackupFile.Length)
            {
                backup();
            }

            BinaryFormatter serializer = new BinaryFormatter();
            mBackupFile.Seek(0, SeekOrigin.Begin);
            m_ListInterlock = (List<CModelInterlock>)serializer.Deserialize(mBackupFile);
        }
    }
}
