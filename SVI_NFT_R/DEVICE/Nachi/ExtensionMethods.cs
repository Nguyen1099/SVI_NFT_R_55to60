using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public static partial class ExtensionMethods
    {
        private static readonly Geometry ZERO_GEOMETRY_DATA = new Geometry(0d, 0d, 0d, 0d, 0d, 0d);

        public static Geometry GetRmsData(this IReadOnlyList<Geometry> geometries, ERobotProcess robotProcess, ERmsDataPosition rmsDataType)
        {
            int blockStartIndex = GetRmsProcessBlockStartIndex(robotProcess);
            switch (rmsDataType)
            {
                case ERmsDataPosition.Approach:
                    blockStartIndex = GetRmsProcessBlockStartIndex(robotProcess);
                    break;

                case ERmsDataPosition.InWait:
                    blockStartIndex = GetRmsProcessBlockStartIndex(robotProcess) + 1;
                    break;

                case ERmsDataPosition.OutWait:
                    blockStartIndex = GetRmsProcessBlockStartIndex(robotProcess) + 2;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            return blockStartIndex < geometries.Count ? geometries[blockStartIndex] : ZERO_GEOMETRY_DATA;
        }

        public static Geometry GetRmsAfterProcessData(this IReadOnlyList<Geometry> geometries, ERmsDataPosition rmsDataType)
        {
            int blockStartIndex = GetRmsProcessBlockStartIndex(ERobotProcess.P10) + 27;
            switch (rmsDataType)
            {
                case ERmsDataPosition.Home:
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            return blockStartIndex < geometries.Count ? geometries[blockStartIndex] : ZERO_GEOMETRY_DATA;
        }

        public static Geometry GetRmsUpData(this IReadOnlyList<Geometry> geometries, ERobotProcess robotProcess, ETool toolIndex, EStage stageIndex)
        {
            const int UP_DATA_OFFSET = 0;
            int itemIndex = GetRmsProcessBlockStartIndex(robotProcess) + GetRmsProcessDataOffset(toolIndex, stageIndex) + UP_DATA_OFFSET;
            return itemIndex < geometries.Count ? geometries[itemIndex] : ZERO_GEOMETRY_DATA;
        }

        public static Geometry GetRmsDownData(this IReadOnlyList<Geometry> geometries, ERobotProcess robotProcess, ETool toolIndex, EStage stageIndex)
        {
            const int DOWN_DATA_OFFSET = 1;
            int itemIndex = GetRmsProcessBlockStartIndex(robotProcess) + GetRmsProcessDataOffset(toolIndex, stageIndex) + DOWN_DATA_OFFSET;
            return itemIndex < geometries.Count ? geometries[itemIndex] : ZERO_GEOMETRY_DATA;
        }

        private static int GetRmsProcessDataOffset(ETool toolIndex, EStage stageIndex)
        {
            const int START_OFFSET = 3;
            const int ITEM_COUNT = 2;

            if (toolIndex == ETool.Tool1
                && stageIndex == EStage.Stage1
                )
            {
                return START_OFFSET + ITEM_COUNT * 0;
            }
            else if (toolIndex == ETool.Tool2
                && stageIndex == EStage.Stage2
                )
            {
                return START_OFFSET + ITEM_COUNT * 1;
            }
            else if (toolIndex == ETool.Tool3
                && stageIndex == EStage.Stage3
                )
            {
                return START_OFFSET + ITEM_COUNT * 2;
            }
            else if (toolIndex == ETool.Tool4
                && stageIndex == EStage.Stage4
                )
            {
                return START_OFFSET + ITEM_COUNT * 3;
            }
            else if (toolIndex == ETool.Tool12
                && stageIndex == EStage.Stage1
                )
            {
                return START_OFFSET + ITEM_COUNT * 4;
            }
            else if (toolIndex == ETool.Tool34
                && stageIndex == EStage.Stage3
                )
            {
                return START_OFFSET + ITEM_COUNT * 5;
            }
            else if (toolIndex == ETool.Tool1
                && stageIndex == EStage.Stage2
                )
            {
                return START_OFFSET + ITEM_COUNT * 6;
            }
            else if (toolIndex == ETool.Tool2
                && stageIndex == EStage.Stage1
                )
            {
                return START_OFFSET + ITEM_COUNT * 7;
            }
            else if (toolIndex == ETool.Tool3
                && stageIndex == EStage.Stage4
                )
            {
                return START_OFFSET + ITEM_COUNT * 8;
            }
            else if (toolIndex == ETool.Tool4
                && stageIndex == EStage.Stage3
                )
            {
                return START_OFFSET + ITEM_COUNT * 9;
            }
            else if (toolIndex == ETool.Tool12
                && stageIndex == EStage.Stage3
                )
            {
                return START_OFFSET + ITEM_COUNT * 10;
            }
            else if (toolIndex == ETool.Tool34
                && stageIndex == EStage.Stage1
                )
            {
                return START_OFFSET + ITEM_COUNT * 11;
            }
            else
            {
                Debug.Assert(false);
                return 0;
            }
        }

        private static int GetRmsProcessBlockStartIndex(ERobotProcess robotProcess)
        {
            int blockStartIndex = 0;
            foreach (ERobotProcess process in Enum.GetValues(typeof(ERobotProcess)))
            {
                if (process == robotProcess)
                {
                    break;
                }
                blockStartIndex += GetRmsProcessDataCount(process);
            }
            return blockStartIndex;
        }

        private static int GetRmsProcessDataCount(ERobotProcess robotProcess)
        {
            switch (robotProcess)
            {
                case ERobotProcess.P1:
                case ERobotProcess.P4:
                case ERobotProcess.P5:
                case ERobotProcess.P6:
                case ERobotProcess.P7:
                case ERobotProcess.P8:
                case ERobotProcess.P9:
                case ERobotProcess.P10:
                    // Approach
                    // InWait
                    // OutWait
                    // T1 Up
                    // T1 Down
                    // T2 Up
                    // T2 Down
                    // T3 Up
                    // T3 Down
                    // T4 Up
                    // T4 Down
                    // T12 Up
                    // T12 Down
                    // T34 Up
                    // T34 Down
                    // T1_S2 Up
                    // T1_S2 Down
                    // T2_S1 Up
                    // T2_S1 Down
                    // T3_S4 Up
                    // T3_S4 Down
                    // T4_S3 Up
                    // T4_S3 Down
                    // T12_S34 Up
                    // T12_S34 Down
                    // T34_S12 Up
                    // T34_S12 Down
                    return 3 + 24;

                case ERobotProcess.P2:
                case ERobotProcess.P3:
                    // Approach
                    // InWait
                    // OutWait
                    // T1 Up
                    // T1 Down
                    // T2 Up
                    // T2 Down
                    // T3 Up
                    // T3 Down
                    // T4 Up
                    // T4 Down
                    // T12 Up
                    // T12 Down
                    // T34 Up
                    // T34 Down
                    return 3 + 12;

                default:
                    break;
            }
            return 0;
        }
    }
}
