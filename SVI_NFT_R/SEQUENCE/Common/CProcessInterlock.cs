using System;

namespace SVI_NFT_R
{
    public class CProcessInterlock
    {
        private readonly CProcessInterlockAbstract m_objInterlockAbstract;

        /// <summary>
        /// 생성자 함수
        /// </summary>
        /// <param name="objInterlock"></param>
        /// <returns></returns>
        public CProcessInterlock(CProcessInterlockAbstract objInterlock)
        {
            m_objInterlockAbstract = objInterlock;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            return m_objInterlockAbstract.Initialize();
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_objInterlockAbstract.DeInitialize();
        }

        /// <summary>
        /// 모션 Class 인터락 ( 상대 위치 이동 )
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <param name="dRelativePosition"></param>
        /// <returns></returns>
        public bool CheckMotionClassInterlock(string strMotorName, double dRelativePosition)
        {
            return m_objInterlockAbstract.CheckMotionClassInterlock(strMotorName, dRelativePosition);
        }

        /// <summary>
        /// 모션 Class 인터락 ( Teaching Position 기준 )
        /// </summary>
        /// <param name="strMotorName"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        public bool CheckMotionClassInterlock(string strMotorName, int iPosition)
        {
            return m_objInterlockAbstract.CheckMotionClassInterlock(strMotorName, iPosition);
        }

        /// <summary>
        /// 모터 인터락 ( Jog 중속 이상 및 움직일 Motor에 대한 모든 Interlock를 체크 )
        /// </summary>
        /// <returns></returns>
        public bool CheckForcedInterlock(string strMotorName, bool bPositiveDirection, Action callbackInterlockPreActionOrNull)
        {
            return m_objInterlockAbstract.CheckForcedInterlock(strMotorName, bPositiveDirection, callbackInterlockPreActionOrNull);
        }

        /// <summary>
        /// 실린더 인터락
        /// </summary>
        /// <param name="eCylinder"></param>
        /// <param name="eCommand"></param>
        /// <returns></returns>
        public bool CheckCylinderInterlock(CProcessMotion.ECylinder eCylinder, CCylinder.ECylinderCommand eCommand)
        {
            return m_objInterlockAbstract.CheckCylinderInterlock(eCylinder, eCommand);
        }

        /// <summary>
        /// 진공 인터락
        /// </summary>
        /// <param name="eVacuum"></param>
        /// <param name="eCommand"></param>
        /// <returns></returns>
        public bool CheckVacuumInterlock(CProcessMotion.EVacuum eVacuum, CVacuum.EVacuumCommand eCommand)
        {
            return m_objInterlockAbstract.CheckVacuumInterlock(eVacuum, eCommand);
        }

        /// <summary>
        /// 모터 포지션별 설정 범위 인터락 (옵션)
        /// </summary>
        /// <param name="motorName">모터 이름</param>
        /// <param name="positionIndex">포지션 인덱스</param>
        /// <param name="setValue">포지션 설정값</param>
        /// <returns>true=정상, false=인터락</returns>
        public virtual bool CheckMotorSettingPositionInterlock(string motorName, int positionIndex, double setValue)
        {
            return m_objInterlockAbstract.CheckMotorSettingPositionInterlock(motorName, positionIndex, setValue);
        }
    }
}