using System;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        public bool UsingVirtualConfig => mUsingVirtualConfig;
        private bool mUsingVirtualConfig = false;

        public bool IsVirtualMotor(CProcessMotion.EMotor motorIndex)
        {
            string strPath = Path.Combine(m_strCurrentPath, CDefine.DEF_VIRTUAL_INI);
            ClassINI objINI = new ClassINI(strPath);
            string sectionName = $"VIRTUAL_MOTOR";
            bool bIsVirtual = objINI.GetBool(sectionName, motorIndex.ToString(), false);
            mUsingVirtualConfig |= bIsVirtual;
            return bIsVirtual;
        }

        public bool IsVirtualRobot(CProcessMotion.ERobot robotIndex)
        {
            string strPath = Path.Combine(m_strCurrentPath, CDefine.DEF_VIRTUAL_INI);
            ClassINI objINI = new ClassINI(strPath);
            string sectionName = $"VIRTUAL_ROBOT";
            bool bIsVirtual = objINI.GetBool(sectionName, robotIndex.ToString(), false);
            mUsingVirtualConfig |= bIsVirtual;
            return bIsVirtual;
        }

        /// <summary>
        /// 가상 모드 구성 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadVirtualParameter()
        {
            // 셋업 기간 종료시 파일 삭제
            string strPath = Path.Combine(m_strCurrentPath, CDefine.DEF_VIRTUAL_INI);
            try
            {
                if (CDocument.IsAfterShipping == true)
                {
                    if (File.Exists(strPath) == true)
                    {
                        File.Delete(strPath);
                    }
                }
            }
            catch
            {
                // 삭제 안되도 무시
            }
            return true;
        }

        /// <summary>
        /// 가상 모드 구성 파라미터 로드 (항목이 없으면 기본 값을 써줌)
        /// </summary>
        /// <returns></returns>
        private bool SaveVirtualParameter()
        {
            if (CDocument.IsAfterShipping == true)
            {
                return true;
            }

            string strPath = Path.Combine(m_strCurrentPath, CDefine.DEF_VIRTUAL_INI);
            ClassINI objINI = new ClassINI(strPath);
            string sectionName = $"VIRTUAL_MOTOR";
            string[] keyNames = objINI.GetKeyNames(sectionName);
            foreach (var motorName in Enum.GetNames(typeof(CProcessMotion.EMotor)))
            {
                string keyName = $"{motorName}";
                if (keyNames.Contains(keyName) == true)
                {
                    continue;
                }
                objINI.WriteValue(sectionName, keyName, "0");
            }
            sectionName = $"VIRTUAL_ROBOT";
            keyNames = objINI.GetKeyNames(sectionName);
            foreach (var robotName in Enum.GetNames(typeof(CProcessMotion.ERobot)))
            {
                string keyName = $"{robotName}";
                if (keyNames.Contains(keyName) == true)
                {
                    continue;
                }
                objINI.WriteValue(sectionName, keyName, "0");
            }
            return true;
        }
    }
}