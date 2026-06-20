using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using urLinkDll;

namespace SVI_NFT_R
{
    public abstract class CCIMImplementAbstract
    {
        /// <summary>
        /// 쓰레드 종료
        /// </summary>
        protected bool m_bThreadExit;
        /// <summary>
        /// 쓰레드
        /// </summary>
        protected Thread[] m_ThreadProcess;
        /// <summary>
        /// 동기화 객체
        /// </summary>
        protected readonly object[] m_objLock;
        /// <summary>
        /// 통신 객체
        /// </summary>
        protected urLinkDllCommunicator m_objCommunicator;
        /// <summary>
        /// 도튜먼트
        /// </summary>
        protected CDocument m_objDocument;
        /// <summary>
        /// XML 직렬화시 네임스페이스
        /// </summary>
        public XmlSerializerNamespaces ns;
        /// <summary>
        /// string UTF8 변환
        /// </summary>
        public sealed class ExtentendStringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get
                {
                    return new UTF8Encoding(false);
                }
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        protected virtual bool Initialize()
        {
            bool bResult = false;
            do
            {
                m_bThreadExit = false;
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                bResult = true;
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public virtual void DeInitialize()
        {
            m_bThreadExit = false;
        }

        public abstract void SetEqStateChangeEvent(object objEQStateChangeEvent);
        public abstract void SetAvailabityStateChangeEvent(object objAvailabilityStateChangeEvent);
        public abstract void SetInterlockStateChangeEvent(object objInterlockStateChangeEvent);
        public abstract void SetMoveStateChangeEvent(object objMoveStateChangeEvent);
        public abstract void SetRunStateChangeEvent(object objRunStateChangeEvent);
        public abstract void SetFrontStateChangeEvent(object objFrontStateChangeEvent);
        public abstract void SetRearStateChangeEvent(object objRearStateChangeEvent);
        public abstract void SetSampleLotStateChangeEvent(object objPP_SPLStateChangeEvent);
        public abstract void SetUnitStateChangeEvent(object objUnitStateChangeEvent);
        public abstract void SetConveyorMoveStateChangeEvent(object objConveyorMoveStateChangeEvent);
        public abstract void SetCellInEvent(object objCellTrackingEvent);
        public abstract void SetCellOutEvent(object objCellTrackingEvent);
        public abstract void SetInspectionResultEvent(object objInspectionResultEvent);
        public abstract void SetPPChangeEvent(object objPPChangeEvent);
        public abstract void SetCurrentPPIDChangeEvent(object objCurrentPPIDChangeEvent);
        public abstract void SetCurrentPPIDParamChangeEvent(object objCurrentPPIDParamChangeEvent);
        public abstract void SetOPCallConfirmEvent(object objOpCallConfirmEvent);
        public abstract void SetInterlockAndMoveStateChangeEvnet(object objInterlockAndMoveStateChangeEvent);
        public abstract void SetInterlockConfirmEvent(object objInterlockConfirmEvent);
        public abstract void SetFunctionChangeEvent(object objFunctionChangeEvent);
        public abstract void SetUnitInterlockConfirmEvent(object objUnitInterlockConfirmEvent);
        public abstract void SetAlarmReport(object objAlarmReport);
        public abstract void SetAvailabilityAndMoveStateChangeEvent(object objAvailabilityAndMoveStateChangeEvent);
        public abstract void SetEcChangeEvent(object objECChangeEvent);
        public abstract void SetTraceDataReport(object objTraceDataReport);
        public abstract void SetLossCodeEvent(object objLossCodeEvent);
        public abstract void SetOperatorInfoEvent(object objOperatorInfoEvent);
        public abstract void SetTactEvent(object objTactEvent);
        public abstract void SetPPParamReply(object objPPParamReply);
        public abstract void SetCellJobProcessReply(object objCellJobProcessReply);
        public abstract void SetControlStateReply(object objControlStateSetReply);
        public abstract void SetConveyorMoveStateReply(object objConveyorMoveStateReply);
        public abstract void SetCurrentAlarmListReply(object objCurrentAlarmListReply);
        public abstract void SetCurrentPPIDReply(object objCurrentPPIDReply);
        public abstract void SetDateAndTimeSetReply(object objDateAndTimeSetReply);
        public abstract void SetECListReply(object objECListReply);
        public abstract void SetECSetReply(object objECSetReply);
        public abstract void SetEQStateReply(object objEQStateReply);
        public abstract void SetFunctionChangeReply(object objFunctionChangeReply);
        public abstract void SetFunctionStateReply(object objFunctionStateReply);
        public abstract void SetInterlockReply(object objInterlockReply);
        public abstract void SetOPCallReply(object objOpCallReply);
        public abstract void SetPPIDListReply(object objPPListReply);
        public abstract void SetTerminalDisplayReply(object objTerminalDisplayReply);
        public abstract void SetUnitInterlockReply(object objUnitInterlockReply);
        public abstract void SetUnitStateReply(object objUnitStateReply);
        public abstract void SetTraceStartReply(object objTraceStartReply);
        public abstract void SetTraceStopReply(object objTraceStopReply);
        public abstract void SetCellInfomationEvent(object objCellInformationEvent);
        public abstract void SetCarrierValidationCheckRequest(object objCarrierValidationCheck);
        public abstract void SetPPParamSetReply(object objPPParamRmoteRequestReply);
        public abstract void SetEquipmentSpecificStepEvent(object objEquipmentSpecificStep);
        public abstract void SetPerformanceLossEvent(object objEquipmentSpecificStep);
        public abstract void SetCellLotInformationRequest(object objCellLotInformationRequest);
        public abstract void SetEquipmentApproveReply(object objEquipmentApproveReply);
        public abstract void SetSpecificationversionStateReply(object objSpecificationversionStateReply);
    }
}
