using System;
using System.Collections.Generic;
using System.IO;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        private class ConfigInitializeSet
        {
            public Action Load { get; private set; }
            public Action Save { get; private set; }

            public ConfigInitializeSet(Action load, Action save)
            {
                Load = load;
                Save = save;
            }
        }

        /// <summary>
        /// 모델 파일 경로
        /// </summary>
        private string m_strModelPath = string.Empty;

        /// <summary>
        /// 로그 파일 경로
        /// </summary>
        private string m_strLogPath = string.Empty;

        /// <summary>
        /// TrackOut 폴더 경로
        /// </summary>
        private string m_strTrackOutPath = string.Empty;

        /// <summary>
        /// 시스템 파일 경로
        /// </summary>
        private string m_strCurrentPath = string.Empty;

        /// <summary>
        /// Cell Information 저장 경로 (AR 설비 TrackOut에 사용함)
        /// </summary>
        private string m_strCellInformationPath = string.Empty;

        /// <summary>
        /// Alarm SOP PDF 파일모음 폴더 저장 경로
        /// </summary>
        private string m_strAlarmSopFilePath = string.Empty;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            // 경로 정보 초기화
            m_strCurrentPath = Directory.GetCurrentDirectory();
            ClassINI objINI = new ClassINI(string.Format(@"{0}\{1}", m_strCurrentPath, CDefine.DEF_CONFIG_INI));
            string sectionName = $"SYSTEM_PATH";
            m_strModelPath = objINI.GetString(sectionName, "strModelPath", string.Format(@"D:\ITEM\{0}", Program.ID));
            m_strLogPath = objINI.GetString(sectionName, "strLogPath", @"D:\ITEM\Log");
            m_strTrackOutPath = objINI.GetString(sectionName, "strTrackOutPath", @""); // D:\TrackOutInfo
            m_strCellInformationPath = objINI.GetString(sectionName, "strCellInformationPath", @"D:\ITEM\CellInformation");
            m_strAlarmSopFilePath = objINI.GetString(sectionName, "strAlarmSopFilePath", @"D:\ITEM\SOP");
            objINI.WriteValue(sectionName, "strModelPath", m_strModelPath);
            objINI.WriteValue(sectionName, "strLogPath", m_strLogPath);
            objINI.WriteValue(sectionName, "strTrackOutPath", m_strTrackOutPath);
            objINI.WriteValue(sectionName, "strCellInformationPath", m_strCellInformationPath);
            objINI.WriteValue(sectionName, "strAlarmSopFilePath", m_strAlarmSopFilePath);

            // 디렉토리 확인 및 생성
            if (
                false == string.IsNullOrWhiteSpace(m_strTrackOutPath)
                && false == Directory.Exists(m_strTrackOutPath)
                )
            {
                Directory.CreateDirectory(m_strTrackOutPath);
            }
            string[] checkPathList = new string[]
            {
                m_strModelPath,
                m_strLogPath,
                m_strAlarmSopFilePath,
                GetIniPath(),
                GetDatabaseTablePath(),
                GetDatabaseRecordPath(),
                GetDatabaseHistoryPath()
            };
            foreach (string path in checkPathList)
            {
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
            }

            var configInitItems = new List<ConfigInitializeSet>()
            {
                new ConfigInitializeSet(() => LoadSystemParameter(), () => SaveSystemParameter()),
                new ConfigInitializeSet(() => LoadOptionParameter(), () => SaveOptionParameter()),
                new ConfigInitializeSet(() => LoadCrucialOptionParameter(), () => SaveCrucialOptionParameter()),
                new ConfigInitializeSet(() => LoadCimParameter(), () => SaveCimParameter()),
                new ConfigInitializeSet(() => LoadModelParameter(), () => SaveModelParameter()),
                new ConfigInitializeSet(() => LoadModelOffsetParameter(), DoNothing),
                new ConfigInitializeSet(() => LoadSignalTowerParameter(), () => SaveSignalTowerParameter()),
                new ConfigInitializeSet(() => LoadTemperatureParameter(), () => SaveTemperatureParameter()),
                new ConfigInitializeSet(() => LoadPowerMeterParameter(), () => SavePowerMeterParameter()),
                new ConfigInitializeSet(() => LoadFfuParameter(), () => SaveFfuParameter()),
                new ConfigInitializeSet(() => LoadMCRParameter(), () => SaveMCRParameter()),
                new ConfigInitializeSet(() => LoadCCLinkIEParameter(), () => SaveCCLinkIEParameter()),
                new ConfigInitializeSet(() => LoadCCLinkVer2Parameter(), () => SaveCCLinkVer2Parameter()),
                new ConfigInitializeSet(() => LoadElectrostaticParameter(), () => SaveElectrostaticParameter()),
                new ConfigInitializeSet(() => LoadIOParameter(), () => SaveIOParameter()),
                new ConfigInitializeSet(() => LoadDatabaseParameter(), () => SaveDatabaseParameter()),
                new ConfigInitializeSet(() => LoadAlignOptionParameter(), () => SaveAlignOptionParameter()),
                new ConfigInitializeSet(() => LoadInspOptionParameter(), () => SaveInspOptionParameter()),
                new ConfigInitializeSet(() => LoadInspInitializeParameter(), () => SaveInspInitializeParameter()),
                new ConfigInitializeSet(() => LoadNachiModelParameter(), () => SaveNachiModelParameter()),
                new ConfigInitializeSet(() => LoadNachiInitializeParameter(), () => SaveNachiInitializeParameter()),
                new ConfigInitializeSet(() => LoadMccOptionParameter(), () => SaveMccOptionParameter()),
                new ConfigInitializeSet(() => LoadMccInitializeParameter(), () => SaveMccInitializeParameter()),
                new ConfigInitializeSet(() => LoadDllVersionParameter(), () => SaveDllVersionParameter()),
                new ConfigInitializeSet(() => LoadAnalogCalibrationParameterAirPressure(), () => SaveAnalogCalibrationParameterAirPressure()),
                new ConfigInitializeSet(() => LoadAlignInitializeParameter(), () => SaveAlignInitializeParameter()),
                new ConfigInitializeSet(() => LoadAngleSensorParameter(), () => SaveAngleSensorParameter()),
                new ConfigInitializeSet(() => LoadMeasurementInitializeParameter(), () => SaveMeasurementInitializeParameter()),
                new ConfigInitializeSet(() => LoadSpecificationVersionStateParameter(), DoNothing),
                new ConfigInitializeSet(() => LoadSafetyPlcParameter(), () => SaveSafetyPlcParameter()),
                new ConfigInitializeSet(() => LoadEcmParameter(), () => SaveEcmParameter()),
                new ConfigInitializeSet(() => LoadVirtualParameter(), () => SaveVirtualParameter()),
            };
            // 모든 파라메터 불러오기
            foreach (var item in configInitItems)
            {
                item.Load?.Invoke();
            }
            // 모든 파라메터 저장
            foreach (var item in configInitItems)
            {
                item.Save?.Invoke();
            }

            // 대기시간 매니저 초기화
            Config.WaitTime.Initialize();

            return true;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
        }

        /// <summary>
        /// 모델 경로 리턴 (레시피 폴더 위치까지 경로 리턴)
        /// </summary>
        /// <returns></returns>
        public string GetModelPath() => $@"{m_strModelPath}\DATA";

        /// <summary>
        /// INI 경로 리턴 (INI 폴더 위치까지 경로 리턴)
        /// </summary>
        /// <returns></returns>
        public string GetIniPath() => $@"{m_strModelPath}\INI";

        /// <summary>
        /// 모델의 베이스 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetModelBasePath() => m_strModelPath;

        /// <summary>
        /// 로그 경로 리턴 (로그 폴더 위치까지 경로 리턴)
        /// </summary>
        /// <returns></returns>
        public string GetLogPath() => m_strLogPath;

        /// <summary>
        /// TrackOut 폴더 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetTrackOutPath() => m_strTrackOutPath;

        /// <summary>
        /// 데이터베이스 테이블 파일 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseTablePath() => $@"{m_strCurrentPath}\Database\TABLE";

        /// <summary>
        /// 데이터베이스 레코드 파일 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseRecordPath() => $@"{m_strCurrentPath}\Database\RECORD";

        /// <summary>
        /// 데이터베이스 이력 파일 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseHistoryPath() => $@"{m_strModelPath}\DATABASE";

        /// <summary>
        /// 시스템 파일 경로
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPath() => m_strCurrentPath;

        /// <summary>
        /// Cell Information 폴더 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetCellInformationPath() => m_strCellInformationPath;

        /// <summary>
        /// Alarm SOP 폴더 경로 리턴
        /// </summary>
        /// <returns></returns>
        public string GetAlarmSopFilePath() => m_strAlarmSopFilePath;

        private void DoNothing()
        {
        }
    }
}