using System;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// Dll Version Parameter
        /// </summary>
        [Serializable]
        public class CDllVersionParameter
        {
            /// <summary>
            /// 저항 측정
            /// </summary>
            public string strDeviceElectrostaticPGMS;

            /// <summary>
            /// FFU
            /// </summary>
            public string strDeviceFFUMCU32;

            /// <summary>
            /// FTP 인터페이스
            /// </summary>
            public string strDeviceFileInterfaceFTP;

            /// <summary>
            /// CCLink
            /// </summary>
            public string strDeviceInterfaceCCLink;

            /// <summary>
            /// 멜섹
            /// </summary>
            public string strDeviceInterfaceMelsec;

            /// <summary>
            /// IO
            /// </summary>
            public string strDeviceIOAjin;

            /// <summary>
            /// MCR
            /// </summary>
            public string strDeviceMCRCognex;

            /// <summary>
            /// 모터
            /// </summary>
            public string strDeviceMotorAjin;

            /// <summary>
            /// 전력량계
            /// </summary>
            public string strDevicePowerMeterAccura;

            /// <summary>
            /// 온도 컨트롤러
            /// </summary>
            public string strDeviceTemperatureTK4S;
        }

        /// <summary>
        /// DLL 정보 리턴
        /// </summary>
        /// <returns></returns>
        public CDllVersionParameter GetDllVersionParameter() => m_objDllVersionParameter;

        /// <summary>
        /// DLL 버전 파라미터 선언
        /// </summary>
        private readonly CDllVersionParameter m_objDllVersionParameter = new CDllVersionParameter();

        /// <summary>
        /// DLL 버전 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadDllVersionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_DLL_VERSION_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"VERSION";
                m_objDllVersionParameter.strDeviceElectrostaticPGMS = objINI.GetString(sectionName, "strDeviceElectrostaticPGMS", "1.0.0.1");
                m_objDllVersionParameter.strDeviceFFUMCU32 = objINI.GetString(sectionName, "strDeviceFFUMCU32", "1.0.0.1");
                m_objDllVersionParameter.strDeviceFileInterfaceFTP = objINI.GetString(sectionName, "strDeviceFileInterfaceFTP", "1.0.0.1");
                m_objDllVersionParameter.strDeviceInterfaceCCLink = objINI.GetString(sectionName, "strDeviceInterfaceCCLink", "1.0.0.1");
                m_objDllVersionParameter.strDeviceInterfaceMelsec = objINI.GetString(sectionName, "strDeviceInterfaceMelsec", "1.0.0.1");
                m_objDllVersionParameter.strDeviceIOAjin = objINI.GetString(sectionName, "strDeviceIOAjin", "1.0.0.1");
                m_objDllVersionParameter.strDeviceMCRCognex = objINI.GetString(sectionName, "strDeviceMCRCognex", "1.0.0.1");
                m_objDllVersionParameter.strDeviceMotorAjin = objINI.GetString(sectionName, "strDeviceMotorAjin", "1.0.0.1");
                m_objDllVersionParameter.strDevicePowerMeterAccura = objINI.GetString(sectionName, "strDevicePowerMeterAccura", "1.0.0.1");
                m_objDllVersionParameter.strDeviceTemperatureTK4S = objINI.GetString(sectionName, "strDeviceTemperatureTK4S", "1.0.0.1");

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 설정 파라미터 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveDllVersionParameter()
        {
            bool bReturn = false;
            do
            {
                string strPath = string.Format("{0:S}\\{1:S}", m_strCurrentPath, CDefine.DEF_DLL_VERSION_INI);
                ClassINI objINI = new ClassINI(strPath);

                string sectionName = $"VERSION";
                objINI.WriteValue(sectionName, "strDeviceElectrostaticPGMS", m_objDllVersionParameter.strDeviceElectrostaticPGMS);
                objINI.WriteValue(sectionName, "strDeviceFFUMCU32", m_objDllVersionParameter.strDeviceFFUMCU32);
                objINI.WriteValue(sectionName, "strDeviceFileInterfaceFTP", m_objDllVersionParameter.strDeviceFileInterfaceFTP);
                objINI.WriteValue(sectionName, "strDeviceInterfaceCCLink", m_objDllVersionParameter.strDeviceInterfaceCCLink);
                objINI.WriteValue(sectionName, "strDeviceInterfaceMelsec", m_objDllVersionParameter.strDeviceInterfaceMelsec);
                objINI.WriteValue(sectionName, "strDeviceIOAjin", m_objDllVersionParameter.strDeviceIOAjin);
                objINI.WriteValue(sectionName, "strDeviceMCRCognex", m_objDllVersionParameter.strDeviceMCRCognex);
                objINI.WriteValue(sectionName, "strDeviceMotorAjin", m_objDllVersionParameter.strDeviceMotorAjin);
                objINI.WriteValue(sectionName, "strDevicePowerMeterAccura", m_objDllVersionParameter.strDevicePowerMeterAccura);
                objINI.WriteValue(sectionName, "strDeviceTemperatureTK4S", m_objDllVersionParameter.strDeviceTemperatureTK4S);

                bReturn = true;
            } while (false);
            return bReturn;
        }
    }
}