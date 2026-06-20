using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogTerminalMessage : CFormCommon, CFormInterface
    {
        /// <summary>
        /// 임의 인터락 데이터 리스트
        /// </summary>
        private List<CModelTerminal> m_ListTerminal;
        /// <summary>
        /// 알람 리스트 칼럼 정의
        /// </summary>
        private enum ETerminalListColumn
        {
            DATETIME,
            TERMINALID,
            MESSAGE,
            TERMINAL_LIST_COLUMN_FINAL
        };
        private CDocument m_objDocument;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CDialogTerminalMessage(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument as CDocument;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogTerminalMessage_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDialogTerminalMessage_FormClosed(object sender, FormClosedEventArgs e)
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
                if (false == InitializeDialog()) break;

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {

        }

        /// <summary>
        /// 다이얼로그 초기화
        /// </summary>
        /// <returns></returns>
        public bool InitializeDialog()
        {
            bool bReturn = false;

            do
            {
                // 알람 리스트 그리드 뷰 초기화
                string[] strOpCallList = { ETerminalListColumn.DATETIME.ToString(), ETerminalListColumn.TERMINALID.ToString(), ETerminalListColumn.MESSAGE.ToString() };
                if (false == InitializeGridView(this.dataGridViewTerminal, strOpCallList)) break;

                m_ListTerminal = new List<CModelTerminal>();

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
        /// 언어 변환
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            bool bReturn = false;

            do
            {
                // 설정 언어에 따라서 그리드 뷰나 버튼에 폰트가 변경되어야 할 수도 있음.
                // 그리드 뷰 크기 변경
                base.SetGridViewFont(this.dataGridViewTerminal, 9.0);
                //SetButtonChangeLanguage( this.BtnConfirm );
                //SetButtonChangeLanguage( this.BtnBuzzerOff );

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 언어 변경 적용
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, this.Name));
        }

        /// <summary>
        /// 언어 변경 적용
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
                if (false == base.InitializeGridView(objGridView)) break;
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
                //objGridView.Columns[0].Width = (int)(objGridView.Width * 0.22);
                //objGridView.Columns[1].Width = (int)(objGridView.Width * 0.15);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 그리드 뷰 데이터 적용
        /// </summary>
        /// <param name="objTerminalList"></param>
        public void SetDataGridViewData(List<CModelTerminal> objTerminalList)
        {
            bool bCompare = true;

            do
            {
                // 현재 그리드 뷰에 알람 리스트와 비교 (길이 다르면 바로 갱신)
                if (objTerminalList.Count == this.dataGridViewTerminal.RowCount)
                {
                    for (int iLoopInterlockList = 0; iLoopInterlockList < objTerminalList.Count; iLoopInterlockList++)
                    {
                        if (string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", objTerminalList[iLoopInterlockList].m_objTerminalTime) != this.dataGridViewTerminal[0, iLoopInterlockList].Value.ToString())
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
                if (true == bCompare) break;

                // 갱신하는 부분
                lock (this.dataGridViewTerminal)
                {
                    this.dataGridViewTerminal.Rows.Clear();

                    for (int iLoopInterlockList = 0; iLoopInterlockList < objTerminalList.Count; iLoopInterlockList++)
                    {
                        string[] strRowData = new string[(int)ETerminalListColumn.TERMINAL_LIST_COLUMN_FINAL];
                        strRowData[(int)ETerminalListColumn.DATETIME] = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", objTerminalList[iLoopInterlockList].m_objTerminalTime);
                        strRowData[(int)ETerminalListColumn.TERMINALID] = objTerminalList[iLoopInterlockList].m_strTerminalID;
                        strRowData[(int)ETerminalListColumn.MESSAGE] = objTerminalList[iLoopInterlockList].m_strTerminalMessage;
                        this.dataGridViewTerminal.Rows.Add(strRowData);
                    }
                }

            } while (false);
        }

        /// <summary>
        /// 타이머 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            SetDataGridViewData(m_ListTerminal);

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
        /// 리스트에 데이터 넣기
        /// </summary>
        /// <param name="time"></param>
        /// <param name="strTerminalID"></param>
        /// <param name="strMessage"></param>
        public void SetListData(DateTime time, string strTerminalID, string strMessage)
        {
            do
            {
                m_ListTerminal.Add(new CModelTerminal(
                    time,
                    strTerminalID,
                    strMessage));

            } while (false);
        }

        /// <summary>
        /// 컴펌 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            // 리스트 내용 삭제.
            m_ListTerminal.Clear();
            // 다이얼로그 닫기
            this.Hide();
        }

        /// <summary>
        /// 부저 오프 버튼 언어
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
        public class CModelTerminal
        {
            /// <summary>
            /// 인터락 발생 시간
            /// </summary>
            public DateTime m_objTerminalTime;
            /// <summary>
            /// 인터락 ID
            /// </summary>
            public string m_strTerminalID;
            /// <summary>
            /// 인터락 메시지
            /// </summary>
            public string m_strTerminalMessage;

            public CModelTerminal(DateTime objTerminalTime, string strTerminalID, string strTerminalMessage)
            {
                m_objTerminalTime = objTerminalTime;
                m_strTerminalID = strTerminalID;
                m_strTerminalMessage = strTerminalMessage;
            }
        }

        private void CDialogTerminalMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus objMachineStatus = new ENC.MemoryMap.Pages.CMMFPagesMachineStatus.CMachineStatus();
            var objMMFManagerMachineStatus = ENC.MemoryMap.Manager.CMMFManagerMachineStatus.Instance;
            objMMFManagerMachineStatus[0].GetMachineStatus(out objMachineStatus);
            if (CDefine.EProgramExitStatus.PROGRAM_EXIT_STATUS_ON != objMachineStatus.m_eProgramExitStatus)
            {
                e.Cancel = true;
            }
        }
    }
}
