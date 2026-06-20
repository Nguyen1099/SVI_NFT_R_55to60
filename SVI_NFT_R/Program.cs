using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    static class Program
    {
        public const string ID = "SVI_NFT_R";

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 중복 실행 방지
            bool bDupliacte = false;
            Mutex hMutex = new Mutex(true, ID, out bDupliacte);

            // 뮤텍스가 생성되면 최초 한번 실행
            if (true == bDupliacte)
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += CurrentDomain_UnhandledException;

                using (Process currentProcess = Process.GetCurrentProcess())
                {
                    currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var mainFrame = new CMainFrame();
                Application.Run(mainFrame);
                mainFrame.DeInitialize();
            }
            else
            {
                // 뮤텍스 생성 실패되면 이미 프로세스 상주 중
                MessageBox.Show(string.Format("Already {0}.exe Running...", ID));
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // 예외 처리되지 않은 예외에 대한 로그를 남긴다.
            LogWrite.Exception((Exception)e.ExceptionObject);
            ExceptionTool.ShowExceptionDialog((Exception)e.ExceptionObject, "UnhandledException");
            if (e.IsTerminating == true)
            {
                LogWrite.Dump("UnhandledException");
            }
        }
    }
}