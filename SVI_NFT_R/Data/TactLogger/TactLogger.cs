using SVI_NFT_R;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// 택타임 로거 베이스
    /// </summary>
    public abstract class TactLogger : IReadOnlyTactLogger
    {
        /// <summary>
        /// 태그
        /// </summary>
        public object Tag { get; set; } = null;
        /// <summary>
        /// 유닛 이름
        /// </summary>
        public string UnitName { get; protected set; } = string.Empty;
        /// <summary>
        /// CSV로그 파일 이름
        /// </summary>
        public string LogFileName { get; private set; }
        /// <summary>
        /// 마지막으로 로그를 기록한 시간
        /// </summary>
        public DateTime LastLoggingTime { get; private set; } = DateTime.MinValue;
        /// <summary>
        /// 대기 시간을 제외한 순수 동작 시간 (단위: 초)
        /// </summary>
        public double LastPureCycleTact { get; private set; } = -1d;
        /// <summary>
        /// [동기화용] 로그가 잠겨져 있는지 여부
        /// </summary>
        public bool IsLock => mSyncRoot.IsSet == false;
        /// <summary>
        /// 동작 리스트
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyTactLoggerSet> Items => mItems.ToDictionary(item => item.Key, item => (IReadOnlyTactLoggerSet)item.Value);
        /// <summary>
        /// 로그 쓰기 이벤트
        /// </summary>
        public event EventHandler<WritingLogEventArgs> WritingLog;
        protected readonly Dictionary<string, TactLoggerSet> mItems = new Dictionary<string, TactLoggerSet>();
        protected readonly string mLogPath;
        protected readonly ManualResetEventSlim mSyncRoot = new ManualResetEventSlim(true);

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="logPath">로그 경로</param>
        /// <param name="logFileName">로그 파일 이름</param>
        protected TactLogger(string logPath, string logFileName)
        {
            LogFileName = logFileName;
            mLogPath = logPath;
        }

        /// <summary>
        /// 시간을 포맷에 맞게 문자열로 반환함
        /// </summary>
        /// <param name="dateTime">시간</param>
        /// <returns>결과</returns>
        public static string GetFormatDateTime(DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd HH:mm}\"{dateTime.Second:00}'{dateTime.Millisecond:000}";
        }

        /// <summary>
        /// [동기화용] 로그 잠금이 풀릴 때까지 대기함
        /// </summary>
        /// <![CDATA[
        /// 로딩이 패시브인 유닛에서는 액티브 측에서 유닛에서 로그를 찍는데 패시브측에서 로그를 쓰기전에 액티브측이 로딩 시퀀스에 들어가게되면 로그가 누락되는 현상이 있어서 추가함
        /// (멀티 쓰레드 환경에서 로그 기록 시간 역전 현상을 막기 위한 동기화 기능)
        /// ]]>
        public void WaitingUnlock()
        {
            mSyncRoot.Wait();
        }

        /// <summary>
        /// [동기화용] 로그를 잠금
        /// </summary>
        /// <![CDATA[
        /// 로그를 쓰는 시퀀스를 Lock과 Unlock으로 감싸는걸 기본으로함
        /// ]]>
        public void Lock()
        {
            mSyncRoot.Reset();
        }

        /// <summary>
        /// [동기화용] 로그 잠금을 해제함
        /// </summary>
        /// <![CDATA[
        /// 로그를 쓰는 시퀀스를 Lock과 Unlock으로 감싸는걸 기본으로함
        /// ]]>
        public void Unlock()
        {
            mSyncRoot.Set();
        }

        /// <summary>
        /// 로그 쓰기
        /// </summary>
        /// <![CDATA[
        /// 로그를 쓰면서 내부 데이터가 업데이트됨으로 SetCellOutput와 순서에 유의할 것
        /// (SetCellOutput()가 WriteLog() 뒤에 호출 되야함)
        /// ]]>
        public void WriteLog()
        {
            DateTime lastLoggingTime = DateTime.Now;
            string folderPath = Path.Combine(mLogPath, $"{lastLoggingTime:yyyy-MM-dd}");

            // 폴더 유무 확인
            if (Directory.Exists(folderPath) == false)
            {
                // 없으면 폴더 생성
                Directory.CreateDirectory(folderPath);
            }

            // 모든 아이템 기록 완료 확인
            if (mItems.Values.All(item => item.CanLogging) == false)
            {
                string log = $"불완전한 데이터 로그 ({LogFileName}) => ";
                log += string.Join(",", mItems.Values.Where(item => !item.CanLogging).Select(item => item.ID));
                Console.WriteLine(log);
                return;
            }

            // 내부 데이터 업데이트
            {
                LastLoggingTime = lastLoggingTime;
                LastPureCycleTact = OnCalculationLastPureCycleTact();

                RaiseWritingLogEvent();
            }

            List<string> lines = new List<string>(2);
            // 데이터 생성
            lines.Add($"{GetFormatDateTime(lastLoggingTime)},{LastPureCycleTact:0.000},{string.Join(",", mItems.Values.Select(item => $"{item}"))}");

            // 파일에 쓰기
            try
            {
                string filePath = Path.Combine(folderPath, LogFileName);
                var fileInfo = new FileInfo(filePath);
                if (
                    fileInfo.Exists == false
                    || fileInfo.Length == 0
                    )
                {
                    lines.Insert(0, GetHeader());
                }
                File.AppendAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 모든 항목을 리셋함
        /// </summary>
        public void ResetAll()
        {
            foreach (var item in mItems.Values)
            {
                item.Reset();
            }
        }

        /// <summary>
        /// 모든 항목을 스킵 처리함
        /// </summary>
        public void SkipAll()
        {
            foreach (var item in mItems.Values)
            {
                item.SkipIfCantLogging();
            }
        }

        public void EmptyIfNotWriteAll()
        {
            foreach (var item in mItems.Values)
            {
                item.EmptyIfCantLogging();
            }
        }

        /// <summary>
        /// 대기 시간을 제외한 순수 동작 시간을 구할 때 호출되는 함수
        /// </summary>
        /// <returns>순수 동작 시간(단위: 초)</returns>
        protected virtual double OnCalculationLastPureCycleTact()
        {
            return mItems.Values
                .Where(item => item.Type.HasFlag(EAction.Moving))
                .Select(item => item.Duration.TotalSeconds)
                .Sum();
        }

        /// <summary>
        /// 모든 아이템을 정상적으로 초기화 했는지 확인하는 함수 (개발자의 실수를 확인하기 위한 함수)
        /// </summary>
        /// <param name="itemNames"></param>
        protected void CheckValidation(string[] itemNames)
        {
            bool bFailed = false;
            string log = string.Empty;
            foreach (var itemName in itemNames)
            {
                if (mItems.ContainsKey(itemName) == false)
                {
                    bFailed = true;
                    log += $" {itemName}";
                }
            }
            if (bFailed == true)
            {
                Debug.Assert(false, $"다음 항목이 누락되었습니다. 확인해주세요. ({log})");
            }
        }

        /// <summary>
        /// 로그 쓰기 이벤트 발생
        /// </summary>
        protected void RaiseWritingLogEvent()
        {
            WritingLog?.Invoke(this, new WritingLogEventArgs() { TactLogger = this });
        }

        /// <summary>
        /// CSV 로그 해더를 반환함
        /// </summary>
        /// <returns>로그 해더</returns>
        protected string GetHeader()
        {
            return $"Record Time,Cycle Time,{string.Join(",", mItems.Values.Select(item => item.HeaderName))}";
        }
    }
}