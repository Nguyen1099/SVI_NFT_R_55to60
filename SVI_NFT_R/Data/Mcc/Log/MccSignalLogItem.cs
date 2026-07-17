using SVI_NFT_R;
using SVI_NFT_R.CellData;
using System;

namespace Mcc
{
    public sealed class MccSignalLogItem : IMccLogItem
    {
        public bool IsExist => CellDataManager.ProcessCells[mProcess][mPosition].IsCellExist();
        private readonly CDocument mDocument;
        private readonly EProcess mProcess;
        private readonly int mPosition;
        private readonly string mModuleID;
        private readonly string mActionName;
        private readonly string mFromPosition;
        private readonly string mToPosition;
        private readonly string mActionNamePrefix;
        private readonly bool mbUseEmptyCell = false;
        private bool mbStartExist;
        private string mStartCellID;
        private string mStartStepID;
        private string mStartJobID;

        public MccSignalLogItem(CDocument document, EProcess process, int position, string actionNamePrefix, string actionName, string moduleID, string fromPosition, string toPosition)
        {
            mDocument = document;
            mProcess = process;
            mPosition = position;
            mModuleID = moduleID;
            mActionName = actionName;
            mFromPosition = fromPosition;
            mToPosition = toPosition;
            mActionNamePrefix = actionNamePrefix;
        }

        public MccSignalLogItem(CDocument document, string actionNamePrefix, string actionName, string moduleID, string fromPosition, string toPosition)
        {
            mDocument = document;
            mProcess = 0;
            mPosition = 0;
            mModuleID = moduleID;
            mActionName = actionName;
            mFromPosition = fromPosition;
            mToPosition = toPosition;
            mActionNamePrefix = actionNamePrefix;
            mbUseEmptyCell = true;
        }

        public void WriteStart(OneCell cellDataOrNull = null)
        {
            OneCell cellData;
            if (cellDataOrNull == null)
            {
                cellData = CellDataManager.ProcessCells[mProcess][mPosition].Data;
            }
            else
            {
                cellData = cellDataOrNull;
            }
            mStartCellID = mbUseEmptyCell ? "" : cellData.Cell.CellID;
            mStartStepID = mbUseEmptyCell ? "" : cellData.Cell.StepID;
            mStartJobID = mbUseEmptyCell ? "" : cellData.Cell.JobID;
            AddMccLogQueue(DateTime.Now, "START");
        }

        public void WriteEnd()
        {
            AddMccLogQueue(DateTime.Now, "END");
        }

        public void WriteStartExist(OneCell cellDataOrNull = null)
        {
            OneCell cellData;
            if (cellDataOrNull == null)
            {
                cellData = CellDataManager.ProcessCells[mProcess][mPosition].Data;
            }
            else
            {
                cellData = cellDataOrNull;
            }
            mbStartExist = cellData.IsCellExist();
            if (
                mbStartExist == false
                && mbUseEmptyCell == false
                )
            {
                return;
            }
            mStartCellID = mbUseEmptyCell ? "" : cellData.Cell.CellID;
            mStartStepID = mbUseEmptyCell ? "" : cellData.Cell.StepID;
            mStartJobID = mbUseEmptyCell ? "" : cellData.Cell.JobID;
            AddMccLogQueue(DateTime.Now, "START");
        }

        public void WriteEndExist()
        {
            if (mbStartExist == false)
            {
                return;
            }
            AddMccLogQueue(DateTime.Now, "END");
        }

        private void AddMccLogQueue(DateTime writeDateTime, string startOrEnd)
        {
            // 큐에 로그 메시지 삽입
            var addItem = new MccLogData(writeDateTime, MakeLogMessage(writeDateTime, mStartCellID, mStartStepID, mStartJobID, startOrEnd));
            mDocument.AddMccLogData(addItem);
        }

        private string MakeLogMessage(DateTime writeDateTime, string glassID, string stepID, string lotID, string startOrEnd)
        {
            return string.Format("{0:MMdd_HHmm_ss.fff},{1},{2},{3},{4},{5},{6}={7},{8}={9}={10}={11}",
                writeDateTime,
                mModuleID,
                "S",
                stepID,
                glassID,
                lotID,
                mDocument.m_objConfig.GetSystemParameter().strPPID,
                mDocument.m_objConfig.GetSystemParameter().strPPID,
                $"{mActionNamePrefix}{mActionName}",
                mFromPosition,
                mToPosition,
                startOrEnd);
        }
    }
}
