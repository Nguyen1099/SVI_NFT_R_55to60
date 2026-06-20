using System;
using System.Diagnostics;
using System.Threading;

namespace SVI_NFT_R
{
    public class CProcessLogin
    {
        /// <summary>
        /// Document
        /// </summary>
        private CDocument m_objDocument;
        /// <summary>
        /// 스레드 종료 플래그
        /// </summary>
        private bool m_bThreadExit;
        /// <summary>
        /// 로그 아웃 상태면 로그인 다이얼로그 띄워주는 스레드
        /// </summary>
        private Thread m_ThreadLogin;
        /// <summary>
        /// 설비 시작 -> 알람 or 시작 -> 정지면 로그 아웃 상태로 바꾸는 스레드
        /// </summary>
        private Thread m_ThreadLogout;
        /// <summary>
        /// 로그인 시나리오
        /// </summary>
        public enum ELoginScenario
        {
            EQUIPMENT_STOP = 0,
            EQUIPMENT_START
        };
        private ELoginScenario m_eLoginScenario;
        private readonly Stopwatch mAutoLogoutStopwatch = new Stopwatch();
        private readonly TimeSpan mAutoLogoutTime = TimeSpan.FromMinutes(10);
        private readonly TimeSpan mUiInputIdleDetectTime = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public bool Initialize(CDocument objDocument)
        {
            bool bReturn = false;

            do
            {
                // 도큐먼트 이어줌
                m_objDocument = objDocument;
                m_eLoginScenario = ELoginScenario.EQUIPMENT_STOP;
                // 로그 아웃 상태면 로그인 다이얼로그 띄워주는 스레드
                m_bThreadExit = false;
                m_ThreadLogin = new Thread(ThreadLogin);
                m_ThreadLogin.IsBackground = true;
                m_ThreadLogin.Start(this);
                // 설비 시작 -> 알람 or 시작 -> 정지면 로그 아웃 상태로 바꾸는 스레드
                m_ThreadLogout = new Thread(ThreadLogout);
                m_ThreadLogout.IsBackground = true;
                m_ThreadLogout.Start(this);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_bThreadExit = true;
            m_ThreadLogin.Join(100);
            m_ThreadLogout.Join(100);
        }

        /// <summary>
        /// 오토 타임아웃 남은 시간 표시
        /// </summary>
        /// <returns></returns>
        public string GetAutoLogoutLesstimeString()
        {
            if (true == m_objDocument.IsAutoLogoutNotUsed)
            {
                return "99:99";
            }

            TimeSpan autoLogoutLesstime = Config.WaitTime.Etc.AutoLogout.ToTimeSpan() - mAutoLogoutStopwatch.Elapsed;
            if (autoLogoutLesstime < TimeSpan.FromSeconds(0))
            {
                return "00:00";
            }
            else
            {
                return autoLogoutLesstime.ToString(@"mm\:ss");
            }
        }

        /// <summary>
        /// 로그 아웃 상태면 로그인 다이얼로그 띄워주는 스레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadLogin(object state)
        {
            // 스레드 처음 시작할 때만 Sleep
            bool bFirstInitialize = true;

            while (false == m_bThreadExit)
            {
                // 마스터 로그인 유무 확인해서
                if (true == m_objDocument.IsMasterLogin)
                {
                    Thread.Sleep(100);
                    continue;
                }

                // 로그 아웃 상태 확인
                if (CDefine.EUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL == m_objDocument.GetUserInformation().m_eAuthorityLevel)
                {
                    if (null != m_objDocument.GetMainFrame())
                    {
                        if (null != m_objDocument.GetMainFrame().m_objDialogLogin)
                        {
                            // 로그인 다이얼로그 유무 체크
                            if (false == m_objDocument.GetMainFrame().m_objDialogLogin.Visible)
                            {
                                // 처음에 실행되면서 로그 아웃 창이 뜰 때 폼 생성 완료가 되고 떠야 하는데..
                                if (true == bFirstInitialize)
                                {
                                    Thread.Sleep(1000);
                                    bFirstInitialize = false;
                                }
                                // 로그인 창 띄워줌
                                m_objDocument.GetMainFrame().SetDelegateLoginDialogShow();
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 설비 시작 -> 알람 or 시작 -> 정지면 로그 아웃 상태로 바꾸는 스레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadLogout(object state)
        {
            while (false == m_bThreadExit)
            {
                if (true == m_objDocument.IsAutoLogoutNotUsed)
                {
                    mAutoLogoutStopwatch.Reset();
                    Thread.Sleep(100);
                    continue;
                }

                switch (m_eLoginScenario)
                {
                    case ELoginScenario.EQUIPMENT_STOP:
                        mAutoLogoutStopwatch.Reset();
                        // 설비가 시작이 되면 START 상태로 변경해줌
                        if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                        {
                            m_eLoginScenario = ELoginScenario.EQUIPMENT_START;
                            mAutoLogoutStopwatch.Start();
                        }
                        break;
                    case ELoginScenario.EQUIPMENT_START:
                        // 설비가 시작 상태에서 중 알람인 경우 or 정지 상태가 되면 설비 로그 아웃시켜주고 시나리오를 STOP으로 변경
                        if (
                            true == m_objDocument.GetIsHeavyAlarm()
                            || CDefine.ERunStatus.Stop == m_objDocument.GetRunStatus()
                            )
                        {
                            // CIM 다이얼로그 기다리는 최소 시간
                            Thread.Sleep(100);
                            // CDialogCellOutCode 띄어져 있으면 안됨
                            if (true == m_objDocument.GetIsForm(typeof(CDialogCellOutCode).Name))
                            {
                                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ETC, "[LOGOUT DENIED] " + typeof(CDialogCellOutCode).Name);
                                Thread.Sleep(500);
                                continue;
                            }
                            // CDialogCIMMessageTest 띄어져 있으면 안됨
                            if (true == m_objDocument.GetIsForm(typeof(CDialogCIMMessageTest).Name))
                            {
                                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ETC, "[LOGOUT DENIED] " + typeof(CDialogCIMMessageTest).Name);
                                Thread.Sleep(500);
                                continue;
                            }
                            // CDialogEQPFunctionList 띄어져 있으면 안됨
                            if (true == m_objDocument.GetIsForm(typeof(CDialogEQPFunctionList).Name))
                            {
                                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ETC, "[LOGOUT DENIED] " + typeof(CDialogEQPFunctionList).Name);
                                Thread.Sleep(500);
                                continue;
                            }
                            // CDialogLossCode 띄어져 있으면 안됨
                            if (true == m_objDocument.GetIsForm(typeof(CDialogLossCode).Name))
                            {
                                m_objDocument.SetUpdateLog(CDefine.ELogType.LOG_ETC, "[LOGOUT DENIED] " + typeof(CDialogLossCode).Name);
                                Thread.Sleep(500);
                                continue;
                            }
                            //// CDialogCIMConnectWait 띄어져 있으면 안됨
                            //if (true == m_objDocument.GetIsForm(typeof(CDialogCIMConnectWait).Name))
                            //{
                            //    Thread.Sleep(500);
                            //    continue;
                            //}
                            //// CDialogCIMDisconnected 띄어져 있으면 안됨
                            //if (true == m_objDocument.GetIsForm(typeof(CDialogCIMDisconnected).Name))
                            //{
                            //    Thread.Sleep(500);
                            //    continue;
                            //}
                            //// CDialogOperatorCallMessage 띄어져 있으면 안됨
                            //if (true == m_objDocument.GetIsForm(typeof(CDialogOperatorCallMessage).Name))
                            //{
                            //    Thread.Sleep(500);
                            //    continue;
                            //}
                            //// CDialogTerminalMessage 띄어져 있으면 안됨
                            //if (true == m_objDocument.GetIsForm(typeof(CDialogTerminalMessage).Name))
                            //{
                            //    Thread.Sleep(500);
                            //    continue;
                            //}
                            //// CDialogInterlockMessage 띄어져 있으면 안됨
                            //if (true == m_objDocument.GetIsForm(typeof(CDialogInterlockMessage).Name))
                            //{
                            //    Thread.Sleep(500);
                            //    continue;
                            //}
                            //// CDialogUnitInterlockMessage 띄어져 있으면 안됨
                            //if (true == m_objDocument.GetIsForm(typeof(CDialogUnitInterlockMessage).Name))
                            //{
                            //    Thread.Sleep(500);
                            //    continue;
                            //}

                            // 로그 아웃
                            if (mAutoLogoutStopwatch.Elapsed > Config.WaitTime.Etc.AutoLogout.ToTimeSpan())
                            {
                                m_objDocument.SetLogout();
                            }
                            m_eLoginScenario = ELoginScenario.EQUIPMENT_STOP;
                        }
                        else if (CDefine.ERunStatus.Start == m_objDocument.GetRunStatus())
                        {
                            // 오토런중 설정 시간이 지나면 자동으로 로그아웃 처리함
                            if (
                                mAutoLogoutStopwatch.Elapsed > Config.WaitTime.Etc.AutoLogout.ToTimeSpan()
                                && "AUTO_LOGOUT" != m_objDocument.GetUserInformation().m_strName
                                && CDefine.EUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL != m_objDocument.GetUserInformation().m_eAuthorityLevel
                                )
                            {
                                var objUserInformation = new CUserInformation();
                                objUserInformation.m_strID = m_objDocument.GetUserInformation().m_strID;
                                objUserInformation.m_strPassword = "1";
                                objUserInformation.m_strName = "AUTO_LOGOUT";
                                objUserInformation.m_eAuthorityLevel = CDefine.EUserAuthorityLevel.OPERATOR;
                                m_objDocument.SetLogin(objUserInformation);
                                m_objDocument.IsMasterLogin = false;

                                // 자동 로그아웃시 UI를 메인 플로우 화면으로 변경함
                                var viewForm = m_objDocument.GetMainFrame().GetFormView() as CFormView;
                                if (null != viewForm)
                                {
                                    viewForm.Invoke(new Action(() =>
                                    {
                                        viewForm.SetChangeMainFlowForm();
                                    }));
                                }
                            }
                        }

                        if (CDefine.EUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL == m_objDocument.GetUserInformation().m_eAuthorityLevel)
                        {
                            // 사용자가 UI에서 로그아웃 버튼을 눌렀을 때 자동 로그아웃 타이머 초기화
                            mAutoLogoutStopwatch.Reset();
                        }
                        else
                        {
                            if (
                                Utils.GetUiInputIdleTime() < mUiInputIdleDetectTime
                                && "AUTO_LOGOUT" != m_objDocument.GetUserInformation().m_strName
                                )
                            {
                                mAutoLogoutStopwatch.Reset();
                            }
                            else
                            {
                                mAutoLogoutStopwatch.Start();
                            }
                        }
                        break;
                    default:
                        break;
                }

                Thread.Sleep(100);
            }
        }
    }
}