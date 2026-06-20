using System;
using System.IO;

namespace Utils
{
    /// <summary>
    /// 택타임 항목
    /// </summary>
    public sealed class TactLoggerSet : IReadOnlyTactLoggerSet
    {
        /// <summary>
        /// 로그를 쓸 수있는지 여부 (Begin과 End를 정상적으로 진행했거나, Skip 이나 Empty를 진행한 경우 로그를 쓸 수 있음)
        /// </summary>
        public bool CanLogging => (mTimeRange.HasStart && mTimeRange.HasEnd) || mbSkipData || mbEmptyData;
        /// <summary>
        /// Skip 이나 Empty를 진행한지 여부
        /// </summary>
        public bool IsSkipOrEmpty => mbSkipData || mbEmptyData;
        /// <summary>
        /// Begin 기록 여부
        /// </summary>
        public bool IsWriteBegin => mTimeRange.HasStart;
        /// <summary>
        /// End 기록 여부
        /// </summary>
        public bool IsWriteEnd => mTimeRange.HasEnd;
        /// <summary>
        /// 아이템을 구별할 고유 이름
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// CSV 로그 해더 제목
        /// </summary>
        public string HeaderName { get; private set; }
        /// <summary>
        /// 동작 구분
        /// </summary>
        public EAction Type { get; private set; }
        /// <summary>
        /// 시간을 반환함
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                if (mbSkipData == true
                    || mbEmptyData == true
                    || mTimeRange.HasStart == false
                    || mTimeRange.HasEnd == false
                    )
                {
                    return TimeSpan.Zero;
                }

                return mTimeRange.Duration;
            }
        }
        /// <summary>
        /// 시간 계산용 객체
        /// </summary>
        public Itenso.TimePeriod.TimeRange TimeRange => new Itenso.TimePeriod.TimeRange(mTimeRange.Start, mTimeRange.End, isReadOnly: true);
        /// <summary>
        /// 택타임 로거 (로깅용)
        /// </summary>
        public TactLogger Parent { get; set; }
        // https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET
        private readonly Itenso.TimePeriod.TimeRange mTimeRange = new Itenso.TimePeriod.TimeRange();
        private bool mbEmptyData = false;
        private bool mbSkipData = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="headerName"></param>
        public TactLoggerSet(EAction type, string id, string headerName)
        {
            ID = id;
            HeaderName = headerName;
            Type = type;
            Reset();
        }

        /// <summary>
        /// 동작 시작 시점 기록
        /// </summary>
        public void Begin()
        {
            // 정상인 경우 설비가 정지 됐다 다시 시작될 경우 동작 시간을 덮어쓰는 경우가 나옴.
            if (mTimeRange.HasStart == true)
            {
                /// !!! 만약 반복적으로 로그가 남으면 시퀀스상에 버그가 있는것으로 시퀀스를 수정해야함
                Console.WriteLine($"'Begin' 시간 덮어쓰기 ({Parent.LogFileName}) => ID: {ID}");

                // End가 이미 써진 상태에서 Start 시간을 등록하면 시간 역전이 발생하여 익셉션이 발생함으로 등록된 시간을 리셋함.
                mTimeRange.Reset();
            }
            if (mTimeRange.HasEnd == true)
            {
                string destinationFolder = $@"{Constants.BACKUP_PATH}\TactTimeNG\{DateTime.Now:yyyy-MM-dd}\({Parent.LogFileName}){ID}.txt";
                if (Directory.Exists(Path.GetDirectoryName(destinationFolder)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFolder));
                }
                if (!File.Exists(destinationFolder))
                {
                    File.Create(destinationFolder);
                }

                Console.WriteLine($"'이상현상' ({Parent.LogFileName}) => ID: {ID}");
                mTimeRange.Reset();
            }
            // !Exception: 시간 역전(End 후 Start 등록)시 익셉션 발생함. 시퀀스가 잘 못된 것으로 시퀀스를 고쳐줘야함.
            mTimeRange.Start = DateTime.Now;
        }

        /// <summary>
        /// 동작 완료 시점 기록
        /// </summary>
        public void End()
        {
            // 정상인 경우 설비가 정지 됐다 다시 시작될 경우 동작 시간을 덮어쓰는 경우가 나옴.
            if (mTimeRange.HasEnd == true)
            {
                /// !!! 만약 반복적으로 로그가 남으면 시퀀스상에 버그가 있는것으로 시퀀스를 수정해야함
                Console.WriteLine($"'End' 시간 덮어쓰기 ({Parent.LogFileName}) => ID: {ID}");
            }

            mTimeRange.End = DateTime.Now;
        }

        /// <summary>
        /// 시작 시점을 기록한적이 없으면 시작 시점을 기록
        /// </summary>
        public void BeginIfNotWrite()
        {
            if (mTimeRange.HasStart == true)
            {
                return;
            }

            Begin();
        }

        /// <summary>
        /// 완료 시점을 기록한적이 없으면 완료 시점을 기록
        /// </summary>
        public void EndIfNotWrite()
        {
            if (mTimeRange.HasEnd == true)
            {
                return;
            }

            End();
        }

        /// <summary>
        /// 정상적으로 처리되지 않은 로그를 "0.000"으로 기록함
        /// </summary>
        public void SkipIfCantLogging()
        {
            if (mTimeRange.HasStart && mTimeRange.HasEnd)
            {
                return;
            }

            mbSkipData = true;
        }

        /// <summary>
        /// 정상적으로 처리되지 않은 로그를 공백으로 기록함
        /// </summary>
        public void EmptyIfCantLogging()
        {
            if (mTimeRange.HasStart && mTimeRange.HasEnd)
            {
                return;
            }

            mbEmptyData = true;
        }

        /// <summary>
        /// 리셋
        /// </summary>
        public void Reset()
        {
            mTimeRange.Reset();
            mbEmptyData = false;
            mbSkipData = false;
        }

        /// <summary>
        /// 시간을 초단위로 형식에 맞게 문자열을 반환함
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (mbEmptyData == true)
            {
                return string.Empty;
            }
            if (Duration == TimeSpan.Zero)
            {
                return "0.000";
            }

            return $"{Duration.TotalSeconds:0.000}";
        }
    }
}