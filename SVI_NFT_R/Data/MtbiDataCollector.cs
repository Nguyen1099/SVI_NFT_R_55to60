using ENC.LogDLL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    internal static class MtbiDataCollector
    {
        public static bool IsStarted { get; private set; } = false;
        private static LogManager mMtbiLogManager;
        private static string mSaveFolderPath;
        private const string MTBI_LOG_NAME = "MTBi";
        private const string TACT_LOG_FOLDER_NAME = "01_Tact Time Log";
        private const string UI_IMAGE_FOLDER_NAME = "02_UI Image";
        private static CDocument mDocument;
        private static DateTime mStartTime;
        private static DateTime mEndTime = DateTime.MinValue;

        public static void Start(CDocument document, string saveFolderPath)
        {
            // 다시 시작한 경우 로그 매니저 정리 후 진행
            if (mMtbiLogManager != null)
            {
                mMtbiLogManager.DeInitialize();
                mMtbiLogManager = null;
            }

            if (string.IsNullOrEmpty(saveFolderPath) == true)
            {
                return;
            }
            try
            {
                if (Directory.Exists(saveFolderPath) == true)
                {
                    Directory.Delete(saveFolderPath, recursive: true);
                }
                if (Directory.Exists(saveFolderPath) == false)
                {
                    Directory.CreateDirectory(saveFolderPath);
                }
            }
            catch
            {
                return;
            }
            mSaveFolderPath = saveFolderPath;
            mDocument = document;
            mStartTime = DateTime.Now;
            mEndTime = DateTime.MinValue;

            // History DB 삭제            
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarm.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryAlarmEvent.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellInput.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellOutput.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentLowerLossTimeEvent.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentStopEvent.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentTPCodeEvent.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryEquipmentUpperLossTimeEvent.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackIn.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryCellTrackOut.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMachineResult.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryMCRStatus.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataCell.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataJudgement.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataReader.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataVisionResult.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistoryProcessDataWorkOrder.HLGetTableName()}");
            mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objSQLite.HLExecute($"DELETE FROM {mDocument.m_objProcessDatabase.m_objProcessDatabaseHistory.m_objManagerTableHistorySafetyPlcModifiedEvent.HLGetTableName()}");

            // 로그 매니저 초기화
            mMtbiLogManager = new LogManager();
            mMtbiLogManager.Initialize(MTBI_LOG_NAME, Path.Combine(mSaveFolderPath, TACT_LOG_FOLDER_NAME), bUsedHeader: true);
            mMtbiLogManager.SetHeader(MTBI_LOG_NAME, CDefine.TACT_LOG_HEADER);

            IsStarted = true;
        }

        public static void Finished()
        {
            if (IsStarted == false)
            {
                return;
            }

            // 로그 매니저 정리
            mMtbiLogManager.DeInitialize();
            mMtbiLogManager = null;

            // UI 스크린샷 저장
            // (1) 생산량 UI
            mDocument.GetMainFrame().SetChangeStatisticsProductionForm();
            Thread.Sleep(500);
            SaveScreenShot(Path.Combine(mSaveFolderPath, UI_IMAGE_FOLDER_NAME, "StatisticsProductionUI.png"));
            // (2) 알람 조회 UI
            mDocument.GetMainFrame().SetChangeStatisticsAlarmForm();
            Thread.Sleep(500);
            SaveScreenShot(Path.Combine(mSaveFolderPath, UI_IMAGE_FOLDER_NAME, "StatisticsAlarmUI.png"));
            // (3) 택타임 조회 UI
            mDocument.GetMainFrame().SetChangeStatisticsTactTimeForm();
            Thread.Sleep(500);
            SaveScreenShot(Path.Combine(mSaveFolderPath, UI_IMAGE_FOLDER_NAME, "StatisticsTactTimeUI.png"));

            MessageBox.Show("MTBi 데이터 수집이 완료되었습니다.", "MTBi 데이터 수집 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("explorer.exe", mSaveFolderPath);

            IsStarted = false;
            mEndTime = DateTime.Now;
        }

        public static void WriteLog(CDefine.ELogType logType, string log)
        {
            if (IsStarted == false)
            {
                return;
            }
            if (logType != CDefine.ELogType.LOG_TACT_TIME)
            {
                return;
            }

            mMtbiLogManager?.WriteLog(MTBI_LOG_NAME, log);
        }

        public static string GetElapsedTime()
        {
            if (IsStarted == false)
            {
                if (mEndTime == DateTime.MinValue)
                {
                    return string.Empty;
                }

                TimeSpan mtbiElapsedTime = mEndTime - mStartTime;
                return mtbiElapsedTime.ToString(@"hh\:mm\:ss");
            }

            TimeSpan elapsedTime = DateTime.Now - mStartTime;
            return elapsedTime.ToString(@"hh\:mm\:ss");
        }

        private static void SaveScreenShot(string fileName)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(fileName);
                if (Directory.Exists(directoryName) == false)
                {
                    Directory.CreateDirectory(directoryName);
                }

                // 주화면의 크기 정보 읽기
                Rectangle rect = Screen.PrimaryScreen.Bounds;
                // 픽셀 포맷 정보 얻기 (Optional)
                int bitsPerPixel = Screen.PrimaryScreen.BitsPerPixel;
                PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                if (bitsPerPixel <= 16)
                {
                    pixelFormat = PixelFormat.Format16bppRgb565;
                }
                if (bitsPerPixel == 24)
                {
                    pixelFormat = PixelFormat.Format24bppRgb;
                }
                // 화면 크기만큼의 Bitmap 생성
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, pixelFormat))
                {
                    // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                        gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
                    }
                    // Bitmap 데이타를 파일로 저장
                    bmp.Save(fileName);
                }
            }
            catch
            {
            }
        }
    }
}
