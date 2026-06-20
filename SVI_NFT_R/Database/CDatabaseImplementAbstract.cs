namespace SVI_NFT_R
{
    public abstract class CDatabaseImplementAbstract
    {
        /// <summary>도큐먼트</summary>
        public CDocument m_objDocument;

        public abstract void SetInsertHistoryAlarm(object objAlarm);
        public abstract void SetInsertHistoryOPCall(object objOPCall);
        public abstract void SetInsertHistoryInterlockCall(object objInterlockCall);
        public abstract void SetInsertHistoryTerminalCall(object objTerminalCall);
        public abstract void SetInsertHistoryCellInput(object objCellInput);
        public abstract void SetInsertHistoryCellOutput(object objCellOutput);
        public abstract void SetInsertHistoryCellTrackIn(object objCellTrackIn);
        public abstract void SetInsertHistoryCellTrackOut(object objCellTrackOut);
        public abstract void SetInsertHistoryMCRStatus(object objMCRStatus);
        public abstract void SetInsertHistoryMachineResult(object objMachineResult);
        public abstract void SetInsertHistoryAlarmEvent(object objAlarmEvent);
        public abstract void SetInsertHistoryEquipmentStopEvent(object objEquipmentStopEvent);
        public abstract void SetInsertHistoryEquipmentTPCodeEvent(object objEquipmentTPCodeEvent);
        public abstract void SetInsertHistoryEquipmentUpperLossTimeEvent(object objEquipmentUpperLossTimeEvent);
        public abstract void SetInsertHistoryEquipmentLowerLossTimeEvent(object objEquipmentLowerLossTimeEvent);
        public abstract void SetInsertHistorySafetyPlcModifiedEvent(object objSafetyPlcModifiedEvent);
        public abstract void SetInsertHistoryProcessData(object objProcessData);
        public abstract void SetUpdateHistoryProcessData(object objProcessData);
    }
}