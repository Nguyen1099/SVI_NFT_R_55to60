using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SVI_NFT_R
{
    public sealed partial class CConfig
    {
        public enum ERobotOffset
        {
            Spare1 = 0,
            Spare2,
            Spare3,
            Spare4,

            P1_T1_X,
            P1_T1_Y,
            P1_T1_Z,
            P1_T1_Rz,
            P1_T2_X,
            P1_T2_Y,
            P1_T2_Z,
            P1_T2_Rz,
            P1_T3_X,
            P1_T3_Y,
            P1_T3_Z,
            P1_T3_Rz,
            P1_T4_X,
            P1_T4_Y,
            P1_T4_Z,
            P1_T4_Rz,
            P1_T12_X,
            P1_T12_Y,
            P1_T12_Z,
            P1_T12_Rz,
            P1_T34_X,
            P1_T34_Y,
            P1_T34_Z,
            P1_T34_Rz,
            P1_T1_S2_X,
            P1_T1_S2_Y,
            P1_T1_S2_Z,
            P1_T1_S2_Rz,
            P1_T2_S1_X,
            P1_T2_S1_Y,
            P1_T2_S1_Z,
            P1_T2_S1_Rz,
            P1_T3_S4_X,
            P1_T3_S4_Y,
            P1_T3_S4_Z,
            P1_T3_S4_Rz,
            P1_T4_S3_X,
            P1_T4_S3_Y,
            P1_T4_S3_Z,
            P1_T4_S3_Rz,
            P1_T12_S34_X,
            P1_T12_S34_Y,
            P1_T12_S34_Z,
            P1_T12_S34_Rz,
            P1_T34_S12_X,
            P1_T34_S12_Y,
            P1_T34_S12_Z,
            P1_T34_S12_Rz,

            P2_T1_X,
            P2_T1_Y,
            P2_T1_Z,
            P2_T1_Rz,
            P2_T2_X,
            P2_T2_Y,
            P2_T2_Z,
            P2_T2_Rz,
            P2_T3_X,
            P2_T3_Y,
            P2_T3_Z,
            P2_T3_Rz,
            P2_T4_X,
            P2_T4_Y,
            P2_T4_Z,
            P2_T4_Rz,
            P2_T12_X,
            P2_T12_Y,
            P2_T12_Z,
            P2_T12_Rz,
            P2_T34_X,
            P2_T34_Y,
            P2_T34_Z,
            P2_T34_Rz,

            P3_T1_X,
            P3_T1_Y,
            P3_T1_Z,
            P3_T1_Rz,
            P3_T2_X,
            P3_T2_Y,
            P3_T2_Z,
            P3_T2_Rz,
            P3_T3_X,
            P3_T3_Y,
            P3_T3_Z,
            P3_T3_Rz,
            P3_T4_X,
            P3_T4_Y,
            P3_T4_Z,
            P3_T4_Rz,
            P3_T12_X,
            P3_T12_Y,
            P3_T12_Z,
            P3_T12_Rz,
            P3_T34_X,
            P3_T34_Y,
            P3_T34_Z,
            P3_T34_Rz,

            P4_T1_X,
            P4_T1_Y,
            P4_T1_Z,
            P4_T1_Rz,
            P4_T2_X,
            P4_T2_Y,
            P4_T2_Z,
            P4_T2_Rz,
            P4_T3_X,
            P4_T3_Y,
            P4_T3_Z,
            P4_T3_Rz,
            P4_T4_X,
            P4_T4_Y,
            P4_T4_Z,
            P4_T4_Rz,
            P4_T12_X,
            P4_T12_Y,
            P4_T12_Z,
            P4_T12_Rz,
            P4_T34_X,
            P4_T34_Y,
            P4_T34_Z,
            P4_T34_Rz,
            P4_T1_S2_X,
            P4_T1_S2_Y,
            P4_T1_S2_Z,
            P4_T1_S2_Rz,
            P4_T2_S1_X,
            P4_T2_S1_Y,
            P4_T2_S1_Z,
            P4_T2_S1_Rz,
            P4_T3_S4_X,
            P4_T3_S4_Y,
            P4_T3_S4_Z,
            P4_T3_S4_Rz,
            P4_T4_S3_X,
            P4_T4_S3_Y,
            P4_T4_S3_Z,
            P4_T4_S3_Rz,
            P4_T12_S34_X,
            P4_T12_S34_Y,
            P4_T12_S34_Z,
            P4_T12_S34_Rz,
            P4_T34_S12_X,
            P4_T34_S12_Y,
            P4_T34_S12_Z,
            P4_T34_S12_Rz,

            P5_T1_X,
            P5_T1_Y,
            P5_T1_Z,
            P5_T1_Rz,
            P5_T2_X,
            P5_T2_Y,
            P5_T2_Z,
            P5_T2_Rz,
            P5_T3_X,
            P5_T3_Y,
            P5_T3_Z,
            P5_T3_Rz,
            P5_T4_X,
            P5_T4_Y,
            P5_T4_Z,
            P5_T4_Rz,
            P5_T12_X,
            P5_T12_Y,
            P5_T12_Z,
            P5_T12_Rz,
            P5_T34_X,
            P5_T34_Y,
            P5_T34_Z,
            P5_T34_Rz,
            P5_T1_S2_X,
            P5_T1_S2_Y,
            P5_T1_S2_Z,
            P5_T1_S2_Rz,
            P5_T2_S1_X,
            P5_T2_S1_Y,
            P5_T2_S1_Z,
            P5_T2_S1_Rz,
            P5_T3_S4_X,
            P5_T3_S4_Y,
            P5_T3_S4_Z,
            P5_T3_S4_Rz,
            P5_T4_S3_X,
            P5_T4_S3_Y,
            P5_T4_S3_Z,
            P5_T4_S3_Rz,
            P5_T12_S34_X,
            P5_T12_S34_Y,
            P5_T12_S34_Z,
            P5_T12_S34_Rz,
            P5_T34_S12_X,
            P5_T34_S12_Y,
            P5_T34_S12_Z,
            P5_T34_S12_Rz,

            P6_T1_X,
            P6_T1_Y,
            P6_T1_Z,
            P6_T1_Rz,
            P6_T2_X,
            P6_T2_Y,
            P6_T2_Z,
            P6_T2_Rz,
            P6_T3_X,
            P6_T3_Y,
            P6_T3_Z,
            P6_T3_Rz,
            P6_T4_X,
            P6_T4_Y,
            P6_T4_Z,
            P6_T4_Rz,
            P6_T12_X,
            P6_T12_Y,
            P6_T12_Z,
            P6_T12_Rz,
            P6_T34_X,
            P6_T34_Y,
            P6_T34_Z,
            P6_T34_Rz,
            P6_T1_S2_X,
            P6_T1_S2_Y,
            P6_T1_S2_Z,
            P6_T1_S2_Rz,
            P6_T2_S1_X,
            P6_T2_S1_Y,
            P6_T2_S1_Z,
            P6_T2_S1_Rz,
            P6_T3_S4_X,
            P6_T3_S4_Y,
            P6_T3_S4_Z,
            P6_T3_S4_Rz,
            P6_T4_S3_X,
            P6_T4_S3_Y,
            P6_T4_S3_Z,
            P6_T4_S3_Rz,
            P6_T12_S34_X,
            P6_T12_S34_Y,
            P6_T12_S34_Z,
            P6_T12_S34_Rz,
            P6_T34_S12_X,
            P6_T34_S12_Y,
            P6_T34_S12_Z,
            P6_T34_S12_Rz,

            P7_T1_X,
            P7_T1_Y,
            P7_T1_Z,
            P7_T1_Rz,
            P7_T2_X,
            P7_T2_Y,
            P7_T2_Z,
            P7_T2_Rz,
            P7_T3_X,
            P7_T3_Y,
            P7_T3_Z,
            P7_T3_Rz,
            P7_T4_X,
            P7_T4_Y,
            P7_T4_Z,
            P7_T4_Rz,
            P7_T12_X,
            P7_T12_Y,
            P7_T12_Z,
            P7_T12_Rz,
            P7_T34_X,
            P7_T34_Y,
            P7_T34_Z,
            P7_T34_Rz,
            P7_T1_S2_X,
            P7_T1_S2_Y,
            P7_T1_S2_Z,
            P7_T1_S2_Rz,
            P7_T2_S1_X,
            P7_T2_S1_Y,
            P7_T2_S1_Z,
            P7_T2_S1_Rz,
            P7_T3_S4_X,
            P7_T3_S4_Y,
            P7_T3_S4_Z,
            P7_T3_S4_Rz,
            P7_T4_S3_X,
            P7_T4_S3_Y,
            P7_T4_S3_Z,
            P7_T4_S3_Rz,
            P7_T12_S34_X,
            P7_T12_S34_Y,
            P7_T12_S34_Z,
            P7_T12_S34_Rz,
            P7_T34_S12_X,
            P7_T34_S12_Y,
            P7_T34_S12_Z,
            P7_T34_S12_Rz,

            P8_T1_X,
            P8_T1_Y,
            P8_T1_Z,
            P8_T1_Rz,
            P8_T2_X,
            P8_T2_Y,
            P8_T2_Z,
            P8_T2_Rz,
            P8_T3_X,
            P8_T3_Y,
            P8_T3_Z,
            P8_T3_Rz,
            P8_T4_X,
            P8_T4_Y,
            P8_T4_Z,
            P8_T4_Rz,
            P8_T12_X,
            P8_T12_Y,
            P8_T12_Z,
            P8_T12_Rz,
            P8_T34_X,
            P8_T34_Y,
            P8_T34_Z,
            P8_T34_Rz,
            P8_T1_S2_X,
            P8_T1_S2_Y,
            P8_T1_S2_Z,
            P8_T1_S2_Rz,
            P8_T2_S1_X,
            P8_T2_S1_Y,
            P8_T2_S1_Z,
            P8_T2_S1_Rz,
            P8_T3_S4_X,
            P8_T3_S4_Y,
            P8_T3_S4_Z,
            P8_T3_S4_Rz,
            P8_T4_S3_X,
            P8_T4_S3_Y,
            P8_T4_S3_Z,
            P8_T4_S3_Rz,
            P8_T12_S34_X,
            P8_T12_S34_Y,
            P8_T12_S34_Z,
            P8_T12_S34_Rz,
            P8_T34_S12_X,
            P8_T34_S12_Y,
            P8_T34_S12_Z,
            P8_T34_S12_Rz,

            P9_T1_X,
            P9_T1_Y,
            P9_T1_Z,
            P9_T1_Rz,
            P9_T2_X,
            P9_T2_Y,
            P9_T2_Z,
            P9_T2_Rz,
            P9_T3_X,
            P9_T3_Y,
            P9_T3_Z,
            P9_T3_Rz,
            P9_T4_X,
            P9_T4_Y,
            P9_T4_Z,
            P9_T4_Rz,
            P9_T12_X,
            P9_T12_Y,
            P9_T12_Z,
            P9_T12_Rz,
            P9_T34_X,
            P9_T34_Y,
            P9_T34_Z,
            P9_T34_Rz,
            P9_T1_S2_X,
            P9_T1_S2_Y,
            P9_T1_S2_Z,
            P9_T1_S2_Rz,
            P9_T2_S1_X,
            P9_T2_S1_Y,
            P9_T2_S1_Z,
            P9_T2_S1_Rz,
            P9_T3_S4_X,
            P9_T3_S4_Y,
            P9_T3_S4_Z,
            P9_T3_S4_Rz,
            P9_T4_S3_X,
            P9_T4_S3_Y,
            P9_T4_S3_Z,
            P9_T4_S3_Rz,
            P9_T12_S34_X,
            P9_T12_S34_Y,
            P9_T12_S34_Z,
            P9_T12_S34_Rz,
            P9_T34_S12_X,
            P9_T34_S12_Y,
            P9_T34_S12_Z,
            P9_T34_S12_Rz,

            P10_T1_X,
            P10_T1_Y,
            P10_T1_Z,
            P10_T1_Rz,
            P10_T2_X,
            P10_T2_Y,
            P10_T2_Z,
            P10_T2_Rz,
            P10_T3_X,
            P10_T3_Y,
            P10_T3_Z,
            P10_T3_Rz,
            P10_T4_X,
            P10_T4_Y,
            P10_T4_Z,
            P10_T4_Rz,
            P10_T12_X,
            P10_T12_Y,
            P10_T12_Z,
            P10_T12_Rz,
            P10_T34_X,
            P10_T34_Y,
            P10_T34_Z,
            P10_T34_Rz,
            P10_T1_S2_X,
            P10_T1_S2_Y,
            P10_T1_S2_Z,
            P10_T1_S2_Rz,
            P10_T2_S1_X,
            P10_T2_S1_Y,
            P10_T2_S1_Z,
            P10_T2_S1_Rz,
            P10_T3_S4_X,
            P10_T3_S4_Y,
            P10_T3_S4_Z,
            P10_T3_S4_Rz,
            P10_T4_S3_X,
            P10_T4_S3_Y,
            P10_T4_S3_Z,
            P10_T4_S3_Rz,
            P10_T12_S34_X,
            P10_T12_S34_Y,
            P10_T12_S34_Z,
            P10_T12_S34_Rz,
            P10_T34_S12_X,
            P10_T34_S12_Y,
            P10_T34_S12_Z,
            P10_T34_S12_Rz,
            Spare5,
            Spare6,
            Spare7,
            Spare8,
            Spare9,
            Spare10,
            Spare11,
            Spare12,
            Spare13,
            Spare14,
            Spare15,
            Spare16,
            Spare17,
            Spare18,
            Spare19,
            Spare20,
            Spare21,
            Spare22,
            Spare23,
            Spare24,
            Spare25,
            Spare26,
            Spare27,
            Spare28,
            Spare29,
            Spare30,
            Spare31,
            Spare32,
            Spare33,
            Spare34,
            Spare35,
            Spare36,
            Spare37,
            Spare38,
            Spare39,
        }

        [Serializable]
        public class RobotModelParameter
        {
            public int RecipeNo { get; set; } = 1;
            public int OverrideSpeed { get; set; } = 100;
            public Dictionary<ERobotOffset, double> OffsetData { get; set; } = Enumerable.Cast<ERobotOffset>(Enum.GetValues(typeof(ERobotOffset)))
                .ToDictionary(offsetIndex => offsetIndex, offsetIndex => 0d);
        }
        private readonly Dictionary<CProcessMotion.ERobot, RobotModelParameter> mNachiModelParameters = Enumerable.Cast<CProcessMotion.ERobot>(Enum.GetValues(typeof(CProcessMotion.ERobot)))
            .ToDictionary(robotIndex => robotIndex, robotIndex => new RobotModelParameter());

        public RobotModelParameter GetNachiModelParameter(CProcessMotion.ERobot robotIndex) => mNachiModelParameters[robotIndex];

        public bool LoadNachiModelParameter()
        {
            string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_ROBOT_MODEL_DAT);
            if (LoadNachiModelParameterOld() == false)
            {
                try
                {
                    var readParameters = JsonConvert.DeserializeObject<Dictionary<CProcessMotion.ERobot, RobotModelParameter>>(File.ReadAllText(strPath));
                    foreach (CProcessMotion.ERobot robotIndex in Enum.GetValues(typeof(CProcessMotion.ERobot)))
                    {
                        if (readParameters.ContainsKey(robotIndex) == true
                            && readParameters[robotIndex] != null
                            )
                        {
                            mNachiModelParameters[robotIndex] = readParameters[robotIndex].DeepClone();
                        }
                        else
                        {
                            mNachiModelParameters[robotIndex] = new RobotModelParameter();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    return false;
                }
            }
            return true;
        }

        public bool SaveNachiModelParameter()
        {
            string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_ROBOT_MODEL_DAT);
            try
            {
                File.WriteAllText(strPath, JsonConvert.SerializeObject(mNachiModelParameters, Formatting.Indented), Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
                return false;
            }
        }

        public bool SaveNachiModelParameter(CProcessMotion.ERobot robotIndex, RobotModelParameter objRobotParameter)
        {
            mNachiModelParameters[robotIndex] = objRobotParameter.DeepClone();
            return SaveNachiModelParameter();
        }

        private bool LoadNachiModelParameterOld()
        {
            // ! ini 방식과 호환성을 유지하기 위해 남겨둠
            // ! ini 방식으로 저장된 경우 읽어서 csv 형식으로 덮어씌움

            string strPath = string.Format(@"{0}\{1}\{2}", GetModelPath(), m_objSystemParameter.strPPID, CDefine.DEF_ROBOT_MODEL_DAT);
            ClassINI objINI = new ClassINI(strPath);

            string[] sectionNames = objINI.GetSectionNames();
            if (sectionNames.Length == 0)
            {
                return false;
            }

            foreach (CProcessMotion.ERobot robotIndex in Enum.GetValues(typeof(CProcessMotion.ERobot)))
            {
                if (mNachiModelParameters[robotIndex] == null)
                {
                    mNachiModelParameters[robotIndex] = new RobotModelParameter();
                }
                string sectionName = $"{robotIndex}";
                mNachiModelParameters[robotIndex].RecipeNo = objINI.GetInt32(sectionName, nameof(RobotModelParameter.RecipeNo), 1);
                mNachiModelParameters[robotIndex].OverrideSpeed = objINI.GetInt32(sectionName, nameof(RobotModelParameter.OverrideSpeed), 100);
                foreach (ERobotOffset offsetIndex in Enum.GetValues(typeof(ERobotOffset)))
                {
                    mNachiModelParameters[robotIndex].OffsetData[offsetIndex] = objINI.GetDouble(sectionName, $"{offsetIndex}", 0d);
                }
            }

            return true;
        }
    }
}