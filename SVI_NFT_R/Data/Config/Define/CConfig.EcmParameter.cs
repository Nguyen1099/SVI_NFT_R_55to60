using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        /// <summary>
        /// EC 리스트
        /// </summary>
        public class CECListData
        {
            public string strECID;
            public string strECItemName;
            public string strECDef;
            public string strECSll;
            public string strECSul;
            public string strECWll;
            public string strECWul;

            public CECListData()
            {
                strECID = "";
                strECItemName = "";
                strECDef = "";
                strECSll = "";
                strECSul = "";
                strECWll = "";
                strECWul = "";
            }

            public object Clone()
            {
                CECListData objECListData = new CECListData();
                objECListData.strECID = this.strECID;
                objECListData.strECItemName = this.strECItemName;
                objECListData.strECDef = this.strECDef;
                objECListData.strECSll = this.strECSll;
                objECListData.strECSul = this.strECSul;
                objECListData.strECWll = this.strECWll;
                objECListData.strECWul = this.strECWul;
                return objECListData;
            }
        }

        public enum EEquipmentEcmList
        {
            IN_SHUTTLE_LOAD_X_POS,
            IN_SHUTTLE_LOAD_X_SPEED,
            IN_SHUTTLE_WAIT_SCAN_X_POS,
            IN_SHUTTLE_WAIT_SCAN_X_SPEED,
            IN_SHUTTLE_SCAN_TRIGGER_START_X_POS,
            IN_SHUTTLE_SCAN_TRIGGER_END_X_POS,
            IN_SHUTTLE_SCAN_X_SPEED,
            IN_SHUTTLE_ALIGN_X_POS,
            IN_SHUTTLE_ALIGN_X_SPEED,
            IN_SHUTTLE_UNLOAD_X_POS,
            IN_SHUTTLE_UNLOAD_X_SPEED,
            INSP_STAGE_LOAD_Y_POS,
            INSP_STAGE_LOAD_Y_SPEED,
            INSP_STAGE_P1_GRAB_Y_POS,
            INSP_STAGE_P1_GRAB_Y_SPEED,
            INSP_STAGE_P2_GRAB_Y_POS,
            INSP_STAGE_P2_GRAB_Y_SPEED,
            INSP_STAGE_UNLOAD_Y_POS,
            INSP_STAGE_UNLOAD_Y_SPEED,
            OUT_FLIP_LOAD_R1_POS,
            OUT_FLIP_LOAD_R1_SPEED,
            OUT_FLIP_UNLOAD_R1_POS,
            OUT_FLIP_UNLOAD_R1_SPEED,
            OUT_FLIP_LOAD_R2_POS,
            OUT_FLIP_LOAD_R2_SPEED,
            OUT_FLIP_UNLOAD_R2_POS,
            OUT_FLIP_UNLOAD_R2_SPEED,
            OUT_FLIP_UP_Z_POS,
            OUT_FLIP_UP_Z_SPEED,
            OUT_FLIP_DOWN_Z_POS,
            OUT_FLIP_DOWN_Z_SPEED,
            //OUT_CONVEYOR_UNLOAD_X_POS,
            //OUT_CONVEYOR_UNLOAD_X_SPEED,
            OUT_SHUTTLE_LOAD_X_POS,
            OUT_SHUTTLE_LOAD_X_SPEED,
            OUT_SHUTTLE_UNLOAD_X_POS,
            OUT_SHUTTLE_UNLOAD_X_SPEED,
            IN_ROBOT_HOME_POS_X,
            IN_ROBOT_HOME_POS_Y,
            IN_ROBOT_HOME_POS_T,
            IN_ROBOT_HOME_POS_Z,
            IN_ROBOT_PICKUP_APP_POS_X,
            IN_ROBOT_PICKUP_APP_POS_Y,
            IN_ROBOT_PICKUP_APP_POS_T,
            IN_ROBOT_PICKUP_APP_POS_Z,
            IN_ROBOT_PICKUP_IN_WAIT_POS_X,
            IN_ROBOT_PICKUP_IN_WAIT_POS_Y,
            IN_ROBOT_PICKUP_IN_WAIT_POS_T,
            IN_ROBOT_PICKUP_IN_WAIT_POS_Z,
            IN_ROBOT_PICKUP_OUT_WAIT_POS_X,
            IN_ROBOT_PICKUP_OUT_WAIT_POS_Y,
            IN_ROBOT_PICKUP_OUT_WAIT_POS_T,
            IN_ROBOT_PICKUP_OUT_WAIT_POS_Z,
            IN_ROBOT_T1_S1_PICKUP_UP_POS_X,
            IN_ROBOT_T1_S1_PICKUP_UP_POS_Y,
            IN_ROBOT_T1_S1_PICKUP_UP_POS_T,
            IN_ROBOT_T1_S1_PICKUP_UP_POS_Z,
            IN_ROBOT_T1_S1_PICKUP_DOWN_POS_X,
            IN_ROBOT_T1_S1_PICKUP_DOWN_POS_Y,
            IN_ROBOT_T1_S1_PICKUP_DOWN_POS_T,
            IN_ROBOT_T1_S1_PICKUP_DOWN_POS_Z,
            IN_ROBOT_T2_S2_PICKUP_UP_POS_X,
            IN_ROBOT_T2_S2_PICKUP_UP_POS_Y,
            IN_ROBOT_T2_S2_PICKUP_UP_POS_T,
            IN_ROBOT_T2_S2_PICKUP_UP_POS_Z,
            IN_ROBOT_T2_S2_PICKUP_DOWN_POS_X,
            IN_ROBOT_T2_S2_PICKUP_DOWN_POS_Y,
            IN_ROBOT_T2_S2_PICKUP_DOWN_POS_T,
            IN_ROBOT_T2_S2_PICKUP_DOWN_POS_Z,
            IN_ROBOT_MCR_APP_POS_X,
            IN_ROBOT_MCR_APP_POS_Y,
            IN_ROBOT_MCR_APP_POS_T,
            IN_ROBOT_MCR_APP_POS_Z,
            IN_ROBOT_MCR_IN_WAIT_POS_X,
            IN_ROBOT_MCR_IN_WAIT_POS_Y,
            IN_ROBOT_MCR_IN_WAIT_POS_T,
            IN_ROBOT_MCR_IN_WAIT_POS_Z,
            IN_ROBOT_MCR_OUT_WAIT_POS_X,
            IN_ROBOT_MCR_OUT_WAIT_POS_Y,
            IN_ROBOT_MCR_OUT_WAIT_POS_T,
            IN_ROBOT_MCR_OUT_WAIT_POS_Z,
            IN_ROBOT_T1_S1_MCR_UP_POS_X,
            IN_ROBOT_T1_S1_MCR_UP_POS_Y,
            IN_ROBOT_T1_S1_MCR_UP_POS_T,
            IN_ROBOT_T1_S1_MCR_UP_POS_Z,
            IN_ROBOT_T1_S1_MCR_DOWN_POS_X,
            IN_ROBOT_T1_S1_MCR_DOWN_POS_Y,
            IN_ROBOT_T1_S1_MCR_DOWN_POS_T,
            IN_ROBOT_T1_S1_MCR_DOWN_POS_Z,
            IN_ROBOT_T2_S2_MCR_UP_POS_X,
            IN_ROBOT_T2_S2_MCR_UP_POS_Y,
            IN_ROBOT_T2_S2_MCR_UP_POS_T,
            IN_ROBOT_T2_S2_MCR_UP_POS_Z,
            IN_ROBOT_T2_S2_MCR_DOWN_POS_X,
            IN_ROBOT_T2_S2_MCR_DOWN_POS_Y,
            IN_ROBOT_T2_S2_MCR_DOWN_POS_T,
            IN_ROBOT_T2_S2_MCR_DOWN_POS_Z,
            IN_ROBOT_T12_S12_MCR_UP_POS_X,
            IN_ROBOT_T12_S12_MCR_UP_POS_Y,
            IN_ROBOT_T12_S12_MCR_UP_POS_T,
            IN_ROBOT_T12_S12_MCR_UP_POS_Z,
            IN_ROBOT_T12_S12_MCR_DOWN_POS_X,
            IN_ROBOT_T12_S12_MCR_DOWN_POS_Y,
            IN_ROBOT_T12_S12_MCR_DOWN_POS_T,
            IN_ROBOT_T12_S12_MCR_DOWN_POS_Z,
            IN_ROBOT_PLACE_APP_POS_X,
            IN_ROBOT_PLACE_APP_POS_Y,
            IN_ROBOT_PLACE_APP_POS_T,
            IN_ROBOT_PLACE_APP_POS_Z,
            IN_ROBOT_PLACE_IN_WAIT_POS_X,
            IN_ROBOT_PLACE_IN_WAIT_POS_Y,
            IN_ROBOT_PLACE_IN_WAIT_POS_T,
            IN_ROBOT_PLACE_IN_WAIT_POS_Z,
            IN_ROBOT_PLACE_OUT_WAIT_POS_X,
            IN_ROBOT_PLACE_OUT_WAIT_POS_Y,
            IN_ROBOT_PLACE_OUT_WAIT_POS_T,
            IN_ROBOT_PLACE_OUT_WAIT_POS_Z,
            IN_ROBOT_T1_S1_PLACE_UP_POS_X,
            IN_ROBOT_T1_S1_PLACE_UP_POS_Y,
            IN_ROBOT_T1_S1_PLACE_UP_POS_T,
            IN_ROBOT_T1_S1_PLACE_UP_POS_Z,
            IN_ROBOT_T1_S1_PLACE_DOWN_POS_X,
            IN_ROBOT_T1_S1_PLACE_DOWN_POS_Y,
            IN_ROBOT_T1_S1_PLACE_DOWN_POS_T,
            IN_ROBOT_T1_S1_PLACE_DOWN_POS_Z,
            IN_ROBOT_T2_S2_PLACE_UP_POS_X,
            IN_ROBOT_T2_S2_PLACE_UP_POS_Y,
            IN_ROBOT_T2_S2_PLACE_UP_POS_T,
            IN_ROBOT_T2_S2_PLACE_UP_POS_Z,
            IN_ROBOT_T2_S2_PLACE_DOWN_POS_X,
            IN_ROBOT_T2_S2_PLACE_DOWN_POS_Y,
            IN_ROBOT_T2_S2_PLACE_DOWN_POS_T,
            IN_ROBOT_T2_S2_PLACE_DOWN_POS_Z,
            OUT_ROBOT_HOME_POS_X,
            OUT_ROBOT_HOME_POS_Y,
            OUT_ROBOT_HOME_POS_T,
            OUT_ROBOT_HOME_POS_Z,
            OUT_ROBOT_PICKUP_APP_POS_X,
            OUT_ROBOT_PICKUP_APP_POS_Y,
            OUT_ROBOT_PICKUP_APP_POS_T,
            OUT_ROBOT_PICKUP_APP_POS_Z,
            OUT_ROBOT_PICKUP_IN_WAIT_POS_X,
            OUT_ROBOT_PICKUP_IN_WAIT_POS_Y,
            OUT_ROBOT_PICKUP_IN_WAIT_POS_T,
            OUT_ROBOT_PICKUP_IN_WAIT_POS_Z,
            OUT_ROBOT_PICKUP_OUT_WAIT_POS_X,
            OUT_ROBOT_PICKUP_OUT_WAIT_POS_Y,
            OUT_ROBOT_PICKUP_OUT_WAIT_POS_T,
            OUT_ROBOT_PICKUP_OUT_WAIT_POS_Z,
            OUT_ROBOT_T1_S1_PICKUP_UP_POS_X,
            OUT_ROBOT_T1_S1_PICKUP_UP_POS_Y,
            OUT_ROBOT_T1_S1_PICKUP_UP_POS_T,
            OUT_ROBOT_T1_S1_PICKUP_UP_POS_Z,
            OUT_ROBOT_T1_S1_PICKUP_DOWN_POS_X,
            OUT_ROBOT_T1_S1_PICKUP_DOWN_POS_Y,
            OUT_ROBOT_T1_S1_PICKUP_DOWN_POS_T,
            OUT_ROBOT_T1_S1_PICKUP_DOWN_POS_Z,
            OUT_ROBOT_T2_S2_PICKUP_UP_POS_X,
            OUT_ROBOT_T2_S2_PICKUP_UP_POS_Y,
            OUT_ROBOT_T2_S2_PICKUP_UP_POS_T,
            OUT_ROBOT_T2_S2_PICKUP_UP_POS_Z,
            OUT_ROBOT_T2_S2_PICKUP_DOWN_POS_X,
            OUT_ROBOT_T2_S2_PICKUP_DOWN_POS_Y,
            OUT_ROBOT_T2_S2_PICKUP_DOWN_POS_T,
            OUT_ROBOT_T2_S2_PICKUP_DOWN_POS_Z,
            OUT_ROBOT_PLACE_APP_POS_X,
            OUT_ROBOT_PLACE_APP_POS_Y,
            OUT_ROBOT_PLACE_APP_POS_T,
            OUT_ROBOT_PLACE_APP_POS_Z,
            OUT_ROBOT_PLACE_IN_WAIT_POS_X,
            OUT_ROBOT_PLACE_IN_WAIT_POS_Y,
            OUT_ROBOT_PLACE_IN_WAIT_POS_T,
            OUT_ROBOT_PLACE_IN_WAIT_POS_Z,
            OUT_ROBOT_PLACE_OUT_WAIT_POS_X,
            OUT_ROBOT_PLACE_OUT_WAIT_POS_Y,
            OUT_ROBOT_PLACE_OUT_WAIT_POS_T,
            OUT_ROBOT_PLACE_OUT_WAIT_POS_Z,
            OUT_ROBOT_T1_S1_PLACE_UP_POS_X,
            OUT_ROBOT_T1_S1_PLACE_UP_POS_Y,
            OUT_ROBOT_T1_S1_PLACE_UP_POS_T,
            OUT_ROBOT_T1_S1_PLACE_UP_POS_Z,
            OUT_ROBOT_T1_S1_PLACE_DOWN_POS_X,
            OUT_ROBOT_T1_S1_PLACE_DOWN_POS_Y,
            OUT_ROBOT_T1_S1_PLACE_DOWN_POS_T,
            OUT_ROBOT_T1_S1_PLACE_DOWN_POS_Z,
            OUT_ROBOT_T2_S2_PLACE_UP_POS_X,
            OUT_ROBOT_T2_S2_PLACE_UP_POS_Y,
            OUT_ROBOT_T2_S2_PLACE_UP_POS_T,
            OUT_ROBOT_T2_S2_PLACE_UP_POS_Z,
            OUT_ROBOT_T2_S2_PLACE_DOWN_POS_X,
            OUT_ROBOT_T2_S2_PLACE_DOWN_POS_Y,
            OUT_ROBOT_T2_S2_PLACE_DOWN_POS_T,
            OUT_ROBOT_T2_S2_PLACE_DOWN_POS_Z,
        }
        [Serializable]
        public class CEcmParameter
        {
            public Dictionary<EEquipmentEcmList, int> ECID { get; set; } = new Dictionary<EEquipmentEcmList, int>();
            public Dictionary<EEquipmentEcmList, string> ECItemName { get; set; } = new Dictionary<EEquipmentEcmList, string>();
            public Dictionary<EEquipmentEcmList, double> ECDef { get; set; } = new Dictionary<EEquipmentEcmList, double>();
            public Dictionary<EEquipmentEcmList, double> ECSll { get; set; } = new Dictionary<EEquipmentEcmList, double>();
            public Dictionary<EEquipmentEcmList, double> ECSul { get; set; } = new Dictionary<EEquipmentEcmList, double>();
            public Dictionary<EEquipmentEcmList, double> ECWll { get; set; } = new Dictionary<EEquipmentEcmList, double>();
            public Dictionary<EEquipmentEcmList, double> ECWul { get; set; } = new Dictionary<EEquipmentEcmList, double>();
        }

        /// <summary>
        /// ECM 파라미터 리턴
        /// </summary>
        /// <param name="eCamera"></param>
        /// <returns></returns>
        public CEcmParameter GetEcmParameter() => m_objEcmParameter;

        /// <summary>
        /// ECM 데이터 파라미터 선언
        /// </summary>
        private CEcmParameter m_objEcmParameter = new CEcmParameter();

        /// <summary>
        /// ECM 파라미터 로드
        /// </summary>
        /// <returns></returns>
        public bool LoadEcmParameter()
        {
            if (LoadEcmParameterOld() == false)
            {
                try
                {
                    string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_ECM_DAT);
                    using (var fileReader = File.OpenText(strPath))
                    {
                        while (fileReader.EndOfStream == false)
                        {
                            string[] values = fileReader.ReadLine().Split(',');
                            if (values.Length < 7)
                            {
                                continue;
                            }
                            if (Enum.TryParse(values[1], out EEquipmentEcmList ecmIndex) == false)
                            {
                                continue;
                            }
                            int intValue;
                            double doubleValue;
                            int.TryParse(values[0], out intValue);
                            m_objEcmParameter.ECID[ecmIndex] = intValue;
                            m_objEcmParameter.ECItemName[ecmIndex] = values[1];
                            double.TryParse(values[2], out doubleValue);
                            m_objEcmParameter.ECDef[ecmIndex] = doubleValue;
                            double.TryParse(values[3], out doubleValue);
                            m_objEcmParameter.ECSll[ecmIndex] = doubleValue;
                            double.TryParse(values[4], out doubleValue);
                            m_objEcmParameter.ECSul[ecmIndex] = doubleValue;
                            double.TryParse(values[5], out doubleValue);
                            m_objEcmParameter.ECWll[ecmIndex] = doubleValue;
                            double.TryParse(values[6], out doubleValue);
                            m_objEcmParameter.ECWul[ecmIndex] = doubleValue;
                        }
                    }
                }
                catch
                {
                    // 파일을 못 읽었을 경우 기본 값으로 채움
                }
            }

            // 비어 있는 항목은 기본 값으로 채움
            foreach (EEquipmentEcmList ecmIndex in Enum.GetValues(typeof(EEquipmentEcmList)))
            {
                if (m_objEcmParameter.ECID.ContainsKey(ecmIndex) == true)
                {
                    continue;
                }

                m_objEcmParameter.ECID[ecmIndex] = (int)ecmIndex + 1;
                m_objEcmParameter.ECItemName[ecmIndex] = ecmIndex.ToString();
                m_objEcmParameter.ECDef[ecmIndex] = 0d;
                m_objEcmParameter.ECSll[ecmIndex] = 0d;
                m_objEcmParameter.ECSul[ecmIndex] = 0d;
                m_objEcmParameter.ECWll[ecmIndex] = 0d;
                m_objEcmParameter.ECWul[ecmIndex] = 0d;
            }
            return true;
        }

        /// <summary>
        /// ECM 파라미터 저장
        /// </summary>
        /// <returns></returns>
        public bool SaveEcmParameter()
        {
            try
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_ECM_DAT);
                StringBuilder sb = new StringBuilder(1024);
                foreach (EEquipmentEcmList ecmIndex in Enum.GetValues(typeof(EEquipmentEcmList)))
                {
                    sb.AppendLine($"{m_objEcmParameter.ECID[ecmIndex]},{m_objEcmParameter.ECItemName[ecmIndex]},{m_objEcmParameter.ECDef[ecmIndex]},{m_objEcmParameter.ECSll[ecmIndex]},{m_objEcmParameter.ECSul[ecmIndex]},{m_objEcmParameter.ECWll[ecmIndex]},{m_objEcmParameter.ECWul[ecmIndex]}");
                }
                File.WriteAllText(strPath, sb.ToString(), Encoding.UTF8);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ECM 파라미터 저장
        /// </summary>
        /// <returns></returns>
        public bool SaveEcmParameter(CEcmParameter objEcmParameter)
        {
            try
            {
                string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_ECM_DAT);
                StringBuilder sb = new StringBuilder(1024);
                foreach (EEquipmentEcmList ecmIndex in Enum.GetValues(typeof(EEquipmentEcmList)))
                {
                    sb.AppendLine($"{objEcmParameter.ECID[ecmIndex]},{objEcmParameter.ECItemName[ecmIndex]},{objEcmParameter.ECDef[ecmIndex]},{objEcmParameter.ECSll[ecmIndex]},{objEcmParameter.ECSul[ecmIndex]},{objEcmParameter.ECWll[ecmIndex]},{objEcmParameter.ECWul[ecmIndex]}");
                }
                File.WriteAllText(strPath, sb.ToString(), Encoding.UTF8);
            }
            catch
            {
                return false;
            }
            m_objEcmParameter = objEcmParameter.DeepClone();
            return true;
        }

        /// <summary>
        /// ECM 파라미터 로드
        /// </summary>
        /// <returns></returns>
        private bool LoadEcmParameterOld()
        {
            // ! ini 방식과 호환성을 유지하기 위해 남겨둠
            // ! ini 방식으로 저장된 경우 읽어서 csv 형식으로 덮어씌움

            string strPath = string.Format(@"{0}\{1}", GetModelPath(), CDefine.DEF_ECM_DAT);
            ClassINI objINI = new ClassINI(strPath);

            string[] sectionNames = objINI.GetSectionNames();
            if (sectionNames.Length == 0)
            {
                return false;
            }

            int iLoopCount = 0;
            foreach (EEquipmentEcmList ecmIndex in Enum.GetValues(typeof(EEquipmentEcmList)))
            {
                string sectionName = ecmIndex.ToString();
                m_objEcmParameter.ECID[ecmIndex] = objINI.GetInt32(sectionName, $"ECID", ++iLoopCount);
                m_objEcmParameter.ECItemName[ecmIndex] = objINI.GetString(sectionName, $"ECItemName", $"{ecmIndex}");
                m_objEcmParameter.ECDef[ecmIndex] = objINI.GetDouble(sectionName, $"ECDef", 0d);
                m_objEcmParameter.ECSll[ecmIndex] = objINI.GetDouble(sectionName, $"ECSll", 0d);
                m_objEcmParameter.ECSul[ecmIndex] = objINI.GetDouble(sectionName, $"ECSul", 0d);
                m_objEcmParameter.ECWll[ecmIndex] = objINI.GetDouble(sectionName, $"ECWll", 0d);
                m_objEcmParameter.ECWul[ecmIndex] = objINI.GetDouble(sectionName, $"ECWul", 0d);
            }

            return true;
        }
    }
}