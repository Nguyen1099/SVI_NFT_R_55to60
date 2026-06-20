/// <summary>
/// 메모리맵 페이지들이 정의 되어 있는 네임스페이스 입니다.
/// </summary>
namespace ENC.MemoryMap.Pages
{
    using Define;
    using System.IO.MemoryMappedFiles;

    // [메모리맵 페이지 클래스]
    //
    // 1. MMData_PageBase를 상속 받아 개별 페이지를 정의합니다.
    // 2. 프로퍼티를 정의하여 내부 데이터에 접근이 용이하게 합니다.
    //

    /// <summary>
    /// 설비상태를 정의한 클래스
    /// </summary>
    sealed class CMMFPagesMachineStatus : MMData_PageBase
    {
        /// <summary>
        /// 설비상태 생성자 입니다.
        /// </summary>
        /// <param name="memMap">메모리 맵 파일 클래스</param>
        /// <param name="pageIndex">할당 할 페이지 인덱스</param>
        /// <param name="pageCount">메모리 맵에 모든 페이지 개수</param>
        public CMMFPagesMachineStatus(MemoryMappedFile memMap, uint pageIndex, uint pageCount)
            : base(memMap, pageIndex, pageCount)
        {

        }

        /// <summary>
        /// 기본값을 씁니다.
        /// </summary>
        public void DefaultValue()
        {
            // 내부 데이터를 클리어 한다.
            ByteData.Clear();
            BoolData.Clear();
            IntData.Clear();
            FloatData.Clear();
            DoubleData.Clear();
            StringData.Clear();
        }

        public class CMachineStatus
        {
            // CIM 보고용 장비 상태
            public SVI_NFT_R.CCIMDefine.EAvailabilityState[] m_eAvailabilityState = new SVI_NFT_R.CCIMDefine.EAvailabilityState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.EInterlockState[] m_eInterlockState = new SVI_NFT_R.CCIMDefine.EInterlockState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.EMoveState[] m_eMoveState = new SVI_NFT_R.CCIMDefine.EMoveState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.ERunState[] m_eRunState = new SVI_NFT_R.CCIMDefine.ERunState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.EFrontEquipmentState[] m_eFrontState = new SVI_NFT_R.CCIMDefine.EFrontEquipmentState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.ERearEquipmentState[] m_eRearState = new SVI_NFT_R.CCIMDefine.ERearEquipmentState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.EPP_SPLState m_ePP_SPLState = new SVI_NFT_R.CCIMDefine.EPP_SPLState();
            public SVI_NFT_R.CCIMDefine.EControlState m_eControlState = new SVI_NFT_R.CCIMDefine.EControlState();
            public SVI_NFT_R.CCIMDefine.EOpcallState m_eOpCallState = new SVI_NFT_R.CCIMDefine.EOpcallState();
            public SVI_NFT_R.CCIMDefine.EConveyorMoveState[] m_eConveyorMoveState = new SVI_NFT_R.CCIMDefine.EConveyorMoveState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.EConveyorStopReason[] m_eConveyorStopReason = new SVI_NFT_R.CCIMDefine.EConveyorStopReason[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
            public SVI_NFT_R.CCIMDefine.EHeartBeat m_eHeartBeat = new SVI_NFT_R.CCIMDefine.EHeartBeat();
            public SVI_NFT_R.CDefine.ERunStatus m_eRunMode = new SVI_NFT_R.CDefine.ERunStatus();
            public SVI_NFT_R.CDefine.EProgramExitStatus m_eProgramExitStatus = new SVI_NFT_R.CDefine.EProgramExitStatus();
            public SVI_NFT_R.CCIMDefine.ESilenceStop m_eSilenceStop = new SVI_NFT_R.CCIMDefine.ESilenceStop();
            public SVI_NFT_R.CCIMDefine.EMoveState[][] m_eUnitMoveState = new SVI_NFT_R.CCIMDefine.EMoveState[System.Enum.GetNames(typeof(SVI_NFT_R.CCIMDefine.EUnit)).Length][];
            public SVI_NFT_R.CCIMDefine.EInterlockState[][] m_eUnitInterlockState = new SVI_NFT_R.CCIMDefine.EInterlockState[System.Enum.GetNames(typeof(SVI_NFT_R.CCIMDefine.EUnit)).Length][];
            public CMachineStatus()
            {
                for (int i = 0; i < m_eUnitMoveState.Length; i++)
                {
                    m_eUnitMoveState[i] = new SVI_NFT_R.CCIMDefine.EMoveState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
                }
                for (int i = 0; i < m_eUnitInterlockState.Length; i++)
                {
                    m_eUnitInterlockState[i] = new SVI_NFT_R.CCIMDefine.EInterlockState[(int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL];
                }
            }
        }


        // 설비상태는 2개씩 존재함 1페이지에 2개의 데이터가 담김 CURRENT_STATE, PREVIOUS_STATE,
        private const int DEF_POSITION_ELEMENT_COUNT = 100;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 공유메모리에 CMachineStatus 구조체의 값 Write ( 설비상태 )
        //설명 : 공유메모리 Int, Double, String 자료형에 인덱스로 접근하여 Write / 설비상태는 현재와 이전상태가 존재하기 때문에 DEF_POSITION_ELEMENT_COUNT을 이용하여 구분
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetMachineStatus(CMachineStatus objMachineStatus)
        {
            for (int iLoopPosition = 0; iLoopPosition < (int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL; iLoopPosition++)
            {
                int iIntDataIndex = 0;
                int iIndex = DEF_POSITION_ELEMENT_COUNT * iLoopPosition;

                // structureCell;
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eAvailabilityState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eInterlockState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eMoveState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eRunState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eFrontState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eRearState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eConveyorMoveState[iLoopPosition];
                IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eConveyorStopReason[iLoopPosition];
                for (int i = 0; i < objMachineStatus.m_eUnitMoveState.Length; i++)
                {
                    IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eUnitMoveState[i][iLoopPosition];
                }
                for (int i = 0; i < objMachineStatus.m_eUnitInterlockState.Length; i++)
                {
                    IntData[(iIntDataIndex++) + iIndex] = (int)objMachineStatus.m_eUnitInterlockState[i][iLoopPosition];
                }
                if (0 == iLoopPosition)
                {
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_ePP_SPLState;
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_eControlState;
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_eOpCallState;
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_eHeartBeat;
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_eRunMode;
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_eProgramExitStatus;
                    IntData[(iIntDataIndex++)] = (int)objMachineStatus.m_eSilenceStop;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //생성 : 
        //추가 : 
        //목적 : 공유메모리에 CMachineStatus 구조체의 값 Read ( 설비상태 )
        //설명 : 공유메모리 Int, Double, String 자료형에 인덱스로 접근하여 Read / 설비상태는 현재와 이전상태가 존재하기 때문에 DEF_POSITION_ELEMENT_COUNT을 이용하여 구분
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GetMachineStatus(out CMachineStatus objMachineStatus)
        {
            objMachineStatus = new CMachineStatus();

            for (int iLoopPosition = 0; iLoopPosition < (int)SVI_NFT_R.CCIMDefine.EPresentState.PRESENT_STATE_FINAL; iLoopPosition++)
            {
                int iIntDataIndex = 0;
                int iIndex = DEF_POSITION_ELEMENT_COUNT * iLoopPosition;

                // structureCell;
                objMachineStatus.m_eAvailabilityState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.EAvailabilityState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eInterlockState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.EInterlockState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eMoveState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.EMoveState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eRunState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.ERunState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eFrontState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.EFrontEquipmentState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eRearState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.ERearEquipmentState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eConveyorMoveState[iLoopPosition] = (SVI_NFT_R.CCIMDefine.EConveyorMoveState)IntData[(iIntDataIndex++) + iIndex];
                objMachineStatus.m_eConveyorStopReason[iLoopPosition] = (SVI_NFT_R.CCIMDefine.EConveyorStopReason)IntData[(iIntDataIndex++) + iIndex];
                for (int i = 0; i < objMachineStatus.m_eUnitMoveState.Length; i++)
                {
                    objMachineStatus.m_eUnitMoveState[i][iLoopPosition] = (SVI_NFT_R.CCIMDefine.EMoveState)IntData[(iIntDataIndex++) + iIndex];
                }
                for (int i = 0; i < objMachineStatus.m_eUnitInterlockState.Length; i++)
                {
                    objMachineStatus.m_eUnitInterlockState[i][iLoopPosition] = (SVI_NFT_R.CCIMDefine.EInterlockState)IntData[(iIntDataIndex++) + iIndex];
                }
                if (0 == iLoopPosition)
                {
                    objMachineStatus.m_ePP_SPLState = (SVI_NFT_R.CCIMDefine.EPP_SPLState)IntData[(iIntDataIndex++)];
                    objMachineStatus.m_eControlState = (SVI_NFT_R.CCIMDefine.EControlState)IntData[(iIntDataIndex++)];
                    objMachineStatus.m_eOpCallState = (SVI_NFT_R.CCIMDefine.EOpcallState)IntData[(iIntDataIndex++)];
                    objMachineStatus.m_eHeartBeat = (SVI_NFT_R.CCIMDefine.EHeartBeat)IntData[(iIntDataIndex++)];
                    objMachineStatus.m_eRunMode = (SVI_NFT_R.CDefine.ERunStatus)IntData[(iIntDataIndex++)];
                    objMachineStatus.m_eProgramExitStatus = (SVI_NFT_R.CDefine.EProgramExitStatus)IntData[(iIntDataIndex++)];
                    objMachineStatus.m_eSilenceStop = (SVI_NFT_R.CCIMDefine.ESilenceStop)IntData[(iIntDataIndex++)];
                }
            }
        }


        #region MachineStatus 에 접근 할 수 있는 프로퍼티를 정의합니다.
        #endregion
    }
}
