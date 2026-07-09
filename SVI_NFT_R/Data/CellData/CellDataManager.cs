using SVI_NFT_R.CellData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SVI_NFT_R
{
    public static class CellDataManager
    {
        public static bool IsInitailized { get; private set; } = false;
        public static IReadOnlyDictionary<ECellData, CellDataHandler> Cells => mCells;
        public static IReadOnlyDictionary<EProcess, IReadOnlyList<CellDataHandler>> ProcessCells => mProcessCells;
        public static readonly HashSet<string> AllInspectionKeys = new HashSet<string>();
        private static readonly Dictionary<ECellData, CellDataHandler> mCells = new Dictionary<ECellData, CellDataHandler>();
        private static readonly Dictionary<EProcess, IReadOnlyList<CellDataHandler>> mProcessCells = new Dictionary<EProcess, IReadOnlyList<CellDataHandler>>();
        private static Thread mThreadDataBackup;
        private static bool mbThreadExit = false;
        private static CDocument mDocument;
        private static readonly object mHashSync = new object();
        //////////////////////////////////////////////////////////////////////////
        // [셀 데이터 구조 변경시 mMapping에 변경사항 적용 해야함]
        // [맵핑 데이터를 기준으로 셀 데이터를 자동으로 생성함]
        private static readonly Dictionary<EProcess, IEnumerable<ECellData>> mMapping = new Dictionary<EProcess, IEnumerable<ECellData>>
        {
            { EProcess.InShuttle, new List<ECellData> { ECellData.InShuttleP1, ECellData.InShuttleP2 } },
            { EProcess.InRobot, new List<ECellData> { ECellData.InRobotP1, ECellData.InRobotP2 } },
            { EProcess.InspStage, new List<ECellData> { ECellData.InspStageP1, ECellData.InspStageP2 } },
            { EProcess.OutRobot, new List<ECellData> { ECellData.OutRobotP1, ECellData.OutRobotP2 } },
            { EProcess.OutFlip, new List<ECellData> { ECellData.OutFlipP1, ECellData.OutFlipP2 } },
            { EProcess.OutShuttle, new List<ECellData> { ECellData.OutShuttleP1, ECellData.OutShuttleP2 } },
        };
        private static readonly Dictionary<ECellData, ECellPlacement> mCellPlacementMapping = new Dictionary<ECellData, ECellPlacement>
        {
            { ECellData.InShuttleP1, ECellPlacement.Right | ECellPlacement.P1 },
            { ECellData.InShuttleP2, ECellPlacement.Left | ECellPlacement.P2 },
            { ECellData.InRobotP1, ECellPlacement.Right | ECellPlacement.P1 },
            { ECellData.InRobotP2, ECellPlacement.Left | ECellPlacement.P2 },
            { ECellData.InspStageP1, ECellPlacement.Right | ECellPlacement.P1 },
            { ECellData.InspStageP2, ECellPlacement.Left | ECellPlacement.P2 },
            { ECellData.OutRobotP1, ECellPlacement.Right | ECellPlacement.P1 },
            { ECellData.OutRobotP2, ECellPlacement.Left | ECellPlacement.P2 },
            { ECellData.OutFlipP1, ECellPlacement.Right | ECellPlacement.P1 },
            { ECellData.OutFlipP2, ECellPlacement.Left | ECellPlacement.P2 },
            { ECellData.OutShuttleP1, ECellPlacement.Right | ECellPlacement.P1 },
            { ECellData.OutShuttleP2, ECellPlacement.Left | ECellPlacement.P2 },
        };
        private static readonly Dictionary<EProcess, CDefine.ELogType> mProcessLogMapping = new Dictionary<EProcess, CDefine.ELogType>
        {
            { EProcess.InShuttle, CDefine.ELogType.LOG_PROCESS_01_IN_SHUTTLE },
            { EProcess.InRobot, CDefine.ELogType.LOG_PROCESS_02_IN_ROBOT},
            { EProcess.InspStage, CDefine.ELogType.LOG_PROCESS_03_INSP_STAGE},
            { EProcess.OutRobot, CDefine.ELogType.LOG_PROCESS_04_OUT_ROBOT},
            { EProcess.OutFlip, CDefine.ELogType.LOG_PROCESS_05_OUT_FLIP},
            { EProcess.OutShuttle, CDefine.ELogType.LOG_PROCESS_06_OUT_SHUTTLE},
        };
        //////////////////////////////////////////////////////////////////////////

        public static bool Initialize(CDocument document)
        {
            if (true == IsInitailized)
            {
                return false;
            }
            mDocument = document;

            // 맵핑 데이터를 기준으로 셀 데이터를 자동으로 생성함
            foreach (var process in mMapping)
            {
                List<CellDataHandler> group = new List<CellDataHandler>();
                int positionIndex = 0;
                foreach (var cellDataIndex in process.Value)
                {
                    mCells.Add(cellDataIndex, new CellDataHandler());
                    if (false == mCells[cellDataIndex].Initialize(cellDataIndex, process.Key, positionIndex, mCellPlacementMapping[cellDataIndex]))
                    {
                        throw new Exception($"{nameof(CellDataHandler)}({cellDataIndex}:{process.Key}:{positionIndex}) - CellDataManager Initialize Failed.");
                    }
                    group.Add(mCells[cellDataIndex]);
                    if (mCells[cellDataIndex].IsCellExist())
                    {
                        AllInspectionKeys.Add(mCells[cellDataIndex].GetInspectionKey());
                    }
                    positionIndex++;
                }
                mProcessCells[process.Key] = group;
            }

            // 개발자 실수를 막기 위한 체크
            var cellDataNames = Enum.GetNames(typeof(ECellData));
            if (mCells.Count != cellDataNames.Length)
            {
                throw new Exception($"if (mCells.Count != cellDataNames.Length) - CellDataManager Cell Data Count Missmatch.");
            }

            mbThreadExit = false;
            mThreadDataBackup = new Thread(ThreadDataBackup);
            mThreadDataBackup.Start();

            IsInitailized = true;
            return true;
        }

        public static void DeInitialize()
        {
            mbThreadExit = true;
            mThreadDataBackup.Join();
            mThreadDataBackup = null;

            foreach (ECellData index in Enum.GetValues(typeof(ECellData)))
            {
                mCells[index].DeInitialize();
            }
            mCells.Clear();
            mProcessCells.Clear();

            IsInitailized = false;
        }

        public static void UpdateID(CellDataHandler target)
        {
            if (target.IsCellExist())
            {
                lock (mHashSync)
                {
                    AllInspectionKeys.Add(target.GetInspectionKey());
                }
            }
        }

        public static void DataMove(CellDataHandler source, CellDataHandler destination)
        {
            destination.ToCopy(source.Data.DeepClone());
            source.Data.Reset();
            destination.IsChanged = true;
            source.IsChanged = true;
            if (destination.IsCellExist() == true)
            {
                lock (mHashSync)
                {
                    AllInspectionKeys.Add(destination.GetInspectionKey());
                }
            }
        }

        public static void DataReset(CellDataHandler target)
        {
            lock (target.DataLock)
            {
                if (target.IsCellExist())
                {
                    lock (mHashSync)
                    {
                        AllInspectionKeys.Remove(target.GetInspectionKey());
                    }
                }
                target.Data.Reset();
                target.IsChanged = true;
            }
        }

        public static bool TryGetCellDataHandlerFromInnerID(string innerID, out CellDataHandler resultOrNull)
        {
            foreach (var cellDataHandler in Cells.Values)
            {
                if (innerID == cellDataHandler.GetInnerID())
                {
                    resultOrNull = cellDataHandler;
                    return true;
                }
            }
            resultOrNull = null;
            return false;
        }

        public static bool TryGetLocationFromInnerID(string innerID, out KeyValuePair<EProcess, int> result)
        {
            result = new KeyValuePair<EProcess, int>();
            foreach (var item in ProcessCells)
            {
                for (int positionIndex = 0; positionIndex < item.Value.Count; positionIndex++)
                {
                    if (innerID == item.Value[positionIndex].GetInnerID())
                    {
                        result = new KeyValuePair<EProcess, int>(item.Key, positionIndex);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryGetCellDataHandlerFromCellID(string cellID, out CellDataHandler resultOrNull)
        {
            foreach (var cellDataHandler in Cells.Values)
            {
                if (
                    cellDataHandler.IsCellExist() == true
                    && cellDataHandler.GetCellID() == cellID
                    )
                {
                    resultOrNull = cellDataHandler;
                    return true;
                }
            }
            resultOrNull = null;
            return false;
        }

        public static bool TryGetLocationFromCellID(string cellID, out KeyValuePair<EProcess, int> result)
        {
            result = new KeyValuePair<EProcess, int>();
            foreach (var item in ProcessCells)
            {
                for (int positionIndex = 0; positionIndex < item.Value.Count; positionIndex++)
                {
                    if (
                        item.Value[positionIndex].IsCellExist() == true
                        && item.Value[positionIndex].GetCellID() == cellID
                        )
                    {
                        result = new KeyValuePair<EProcess, int>(item.Key, positionIndex);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryGetCellDataHandlerFromInspectionKey(string inspectionKey, out CellDataHandler resultOrNull, bool bUseJobID = true)
        {
            foreach (var cellDataHandler in Cells.Values)
            {
                if (
                    cellDataHandler.IsCellExist() == true
                    && cellDataHandler.GetInspectionKey(bUseJobID) == inspectionKey
                    )
                {
                    resultOrNull = cellDataHandler;
                    return true;
                }
            }
            resultOrNull = null;
            return false;
        }

        public static EInspectionResult GetEnumFromResultString(ref string inspectionResult)
        {
            EInspectionResult result;

            // 검사 결과 
            inspectionResult = inspectionResult.ToUpper();
            if ("BIN1" == inspectionResult)
            {
                result = EInspectionResult.BIN1;
            }
            else if ("BIN2" == inspectionResult)
            {
                result = EInspectionResult.BIN2;
            }
            else if ("BIN3" == inspectionResult)
            {
                result = EInspectionResult.BIN3;
            }
            else if ("NG" == inspectionResult)
            {
                result = EInspectionResult.REJECT;
            }
            else if ("REJECT" == inspectionResult || "RJ" == inspectionResult)
            {
                result = EInspectionResult.REJECT;
            }
            else
            {
                // 정의되지 않은 검사 결과는 BIN2로 처리한다
                result = EInspectionResult.BIN2;
            }

            switch (result)
            {
                case EInspectionResult.BIN1:
                    inspectionResult = "BIN1";
                    break;
                case EInspectionResult.BIN2:
                    inspectionResult = "BIN2";
                    break;
                case EInspectionResult.BIN3:
                    inspectionResult = "BIN3";
                    break;
                case EInspectionResult.REJECT:
                    inspectionResult = "REJECT";
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            return result;
        }

        public static ECellPlacement GetCellPlacementFromProcess(EProcess processIndex)
        {
            ECellPlacement result = 0;
            var cellDatas = ProcessCells[processIndex];
            var existCells = cellDatas.GetExistCellList();
            foreach (var cellDataHandler in existCells)
            {
                result |= cellDataHandler.CellPlacement;
            }
            if (existCells.Count == cellDatas.Count)
            {
                result |= ECellPlacement.Full;
            }
            if (existCells.Count == 0)
            {
                result |= ECellPlacement.Empty;
            }
            return result;
        }

        public static CDefine.ELogType GetLogTypeFromProcess(EProcess processIndex) => mProcessLogMapping[processIndex];

        private static void ThreadDataBackup()
        {
            while (false == mbThreadExit)
            {
                Thread.Sleep(10);
                foreach (var cellData in mCells.Values)
                {
                    if (true == cellData.IsChanged)
                    {
                        cellData.Backup();
                    }
                    Thread.Sleep(1);
                }
            }
        }
    }

}
