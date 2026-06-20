using System;
using System.Collections.Generic;

namespace SVI_NFT_R.TactTime
{
    public class Tact
    {
        public int CellCount
        {
            get
            {
                int result = 0;
                for (int i = 0; i < InnerID.Length; i++)
                {
                    if (false == string.IsNullOrWhiteSpace(InnerID[i]))
                    {
                        result++;
                    }
                }
                return result;
            }
        }
        public string[] InnerID { get; set; }
        public string[] CellID { get; set; }
        public TimeSpan? OutTact { get; set; }
        public TimeSpan? InspectionTact { get; set; }
        public TimeSpan? InputDelayTime { get; set; }
        public TimeSpan? OutputDelayTime { get; set; }
        public string FastPureTactUnitName
        {
            get
            {
                string result = "";
                TimeSpan minTime = TimeSpan.MaxValue;
                foreach (var item in UnitTacts)
                {
                    if (minTime > item.Value.PureUnitTactTime)
                    {
                        minTime = item.Value.PureUnitTactTime;
                        result = item.Key.ToString().Replace("PROCESS_", "");
                    }
                }
                return result;
            }
        }
        public TimeSpan FastPureTactUnitTime
        {
            get
            {
                TimeSpan minTime = TimeSpan.MaxValue;
                foreach (var item in UnitTacts)
                {
                    if (minTime > item.Value.PureUnitTactTime)
                    {
                        minTime = item.Value.PureUnitTactTime;
                    }
                }
                return minTime;
            }
        }
        public string SlowPureTactUnitName
        {
            get
            {
                string result = "";
                TimeSpan maxTime = TimeSpan.MinValue;
                foreach (var item in UnitTacts)
                {
                    if (maxTime < item.Value.PureUnitTactTime)
                    {
                        maxTime = item.Value.PureUnitTactTime;
                        result = item.Key.ToString().Replace("PROCESS_", "");
                    }
                }
                return result;
            }
        }
        public TimeSpan SlowPureTactUnitTime
        {
            get
            {
                TimeSpan maxTime = TimeSpan.MinValue;
                foreach (var item in UnitTacts)
                {
                    if (maxTime < item.Value.PureUnitTactTime)
                    {
                        maxTime = item.Value.PureUnitTactTime;
                    }
                }
                return maxTime;
            }
        }
        public Dictionary<CellData.EProcess, UnitTact> UnitTacts
        {
            get
            {
                return mUnitTacts;
            }
        }
        public bool IsIncomplete
        {
            get
            {
                bool bResult = true;
                do
                {
                    if (
                        0 == InnerID.Length
                        || 0 == CellID.Length
                        || false == InspectionTact.HasValue
                        || false == OutTact.HasValue
                        || false == InputDelayTime.HasValue
                        || false == OutputDelayTime.HasValue
                        // ! 프로그램을 구동 후 첫 사이클에서는 이전 배출 시점이 없어서 'OutTact'이 0초로 나옴으로 예외 처리함
                        || TimeSpan.Zero == OutTact.Value
                        )
                    {
                        break;
                    }

                    bResult = false;
                } while (false);
                return bResult;
            }
        }
        public bool IsTimeoutData
        {
            get
            {
                TimeSpan time = DateTime.Now - mRegistDateTime;
                TimeSpan timeout = TimeSpan.FromDays(1);
                return (time > timeout);
            }
        }
        private DateTime mRegistDateTime;
        private Dictionary<CellData.EProcess, UnitTact> mUnitTacts;
        private int mProcessCount = Enum.GetNames(typeof(CellData.EProcess)).Length;

        public Tact()
        {
            InnerID = new string[0];
            CellID = new string[0];
            mUnitTacts = new Dictionary<CellData.EProcess, UnitTact>();
            mRegistDateTime = DateTime.Now;
            InspectionTact = null;
            OutTact = null;
            InputDelayTime = null;
            OutputDelayTime = null;
        }
    }
}
