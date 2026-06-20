using SVI_NFT_R.CellData;
using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    internal static class Resource
    {
        public static bool IsInitialize { get; private set; } = false;
        private static CDocument mDocument;
        private static Dictionary<string, string[]> mResources = new Dictionary<string, string[]>();

        public static void Initialize(CDocument document)
        {
            mDocument = document;
            mResources.Clear();

            // Regist All Process
            mResources[nameof(EProcess.InShuttle)] = new string[] { "IN셔틀", "IN-SHUTTLE" };
            mResources[nameof(EProcess.InRobot)] = new string[] { "IN로봇", "IN-ROBOT" };
            mResources[nameof(EProcess.InspStage)] = new string[] { "검사스테이지", "INSPECTION-STAGE" };
            mResources[nameof(EProcess.OutRobot)] = new string[] { "OUT로봇", "OUT-ROBOT" };
            mResources[nameof(EProcess.OutFlip)] = new string[] { "OUT플립", "OUT-FLIP" };
            // Regist All Motors
            mResources[nameof(CProcessMotion.EMotor.IN_SHUTTLE_X1)] = new string[] { "IN셔틀 X1 축", "IN-SHUTTLE X1 AXIS" };
            mResources[nameof(CProcessMotion.EMotor.INSP_STAGE_Y)] = new string[] { "검사스테이지 Y 축", "INSPECTION-STAGE Y AXIS" };
            mResources[nameof(CProcessMotion.EMotor.OUT_FLIP_R1)] = new string[] { "OUT플립 R1 축", "OUT-FLIP R1 AXIS" };
            mResources[nameof(CProcessMotion.EMotor.OUT_FLIP_R2)] = new string[] { "OUT플립 R2 축", "OUT-FLIP R2 AXIS" };
            mResources[nameof(CProcessMotion.EMotor.OUT_FLIP_Z)] = new string[] { "OUT플립 Z 축", "OUT-FLIP Z AXIS" };
            mResources[nameof(CProcessMotion.EMotor.OUT_CONVEYOR_X2)] = new string[] { "아웃컨베이어 X2 축", "OUT-CONVEYOR X2 AXIS" };
            // Regist All Robots
            mResources[nameof(CProcessMotion.ERobot.IN_ROBOT)] = new string[] { "IN로봇", "IN-ROBOT" };
            mResources[nameof(CProcessMotion.ERobot.OUT_ROBOT)] = new string[] { "OUT로봇", "OUT-ROBOT" };
            // Regist All Doors
            mResources[nameof(Door.EDoor.MainFront1)] = new string[] { "정면 도어1", "FRONT DOOR1" };
            mResources[nameof(Door.EDoor.MainFront2)] = new string[] { "정면 도어2", "FRONT DOOR2" };
            mResources[nameof(Door.EDoor.MainFront3)] = new string[] { "정면 도어3", "FRONT DOOR3" };
            mResources[nameof(Door.EDoor.MainFront4)] = new string[] { "정면 도어4", "FRONT DOOR4" };
            mResources[nameof(Door.EDoor.MainRear1)] = new string[] { "후면 도어1", "REAR DOOR1" };
            mResources[nameof(Door.EDoor.MainRear2)] = new string[] { "후면 도어2", "REAR DOOR2" };
            mResources[nameof(Door.EDoor.MainRear3)] = new string[] { "후면 도어3", "REAR DOOR3" };
            // Regist Etc.
            mResources["LOAD"] = new string[] { "로드", "LOAD" };
            mResources["UNLOAD"] = new string[] { "언로드", "UNLOAD" };
            mResources["UPPER"] = new string[] { "상류", "UPPER" };
            mResources["LOWER"] = new string[] { "하류", "LOWER" };
            mResources["SAFETY KEY"] = new string[] { "다인수", "SAFETY" };
            mResources["INPUT BOX"] = new string[] { "투입부", "INPUT BOX" };
            mResources["OUTPUT BOX"] = new string[] { "배출부", "OUTPUT BOX" };
            mResources["EDIT"] = new string[] { "편집", "EDIT" };
            mResources["NOT SURPPORT"] = new string[] { "미지원", "NOT SURPPORT" };
            // Regist Alarm Category
            mResources["ALARM_CATEGORY_VACUUM"] = new string[] { "진공", "Vacuum" };
            mResources["ALARM_CATEGORY_VALIDATION"] = new string[] { "확인", "Validation" };
            mResources["ALARM_CATEGORY_MOTOR"] = new string[] { "모터", "Motor" };
            mResources["ALARM_CATEGORY_INSPECTION"] = new string[] { "검사", "Inspection" };
            mResources["ALARM_CATEGORY_VISION"] = new string[] { "비전", "Vision" };
            mResources["ALARM_CATEGORY_CYLINDER"] = new string[] { "실린더", "Cylinder" };
            mResources["ALARM_CATEGORY_SENSOR"] = new string[] { "감지", "Sens" };
            mResources["ALARM_CATEGORY_COMMUNICATION"] = new string[] { "통신", "Communication" };
            mResources["ALARM_CATEGORY_DOOR"] = new string[] { "문", "Door" };
            mResources["ALARM_CATEGORY_TEMPERATURE"] = new string[] { "온도", "Temperature" };
            mResources["ALARM_CATEGORY_EMO"] = new string[] { "비상 정지", "Emergency Stop" };
            mResources["ALARM_CATEGORY_UTILITY"] = new string[] { "유틸리티", "Utility" };
            mResources["ALARM_CATEGORY_OPERATION"] = new string[] { "운영", "Operation" };
            mResources["ALARM_CATEGORY_ETC"] = new string[] { "기타", "Etc." };
            mResources["ALARM_CATEGORY_EFFICIENCY"] = new string[] { "무언 정지", "Run Down" };
            mResources["ALARM_CATEGORY_RESERVED"] = new string[] { "오보고", "Miss Report" };

            mResources["EQUIPMENT"] = new string[] { "설비", "EQUIPMENT" };
            mResources["INSPECT"] = new string[] { "AMO검사기", "AMO-INSPECT" };
            mResources["OP_MODE_DRY_RUN"] = new string[] { "드라이런 모드", "DRY-RUN MODE" };
            mResources["INSP_MODE_SKIP"] = new string[] { "스킵 모드", "SKIP MODE" };

            IsInitialize = true;
        }

        public static void DeInitialize()
        {
            mDocument = null;
            IsInitialize = false;
        }

        public static object Get(object index)
        {
            string key = index.ToString();
            if (mResources.ContainsKey(key) == true)
            {
                if (mDocument.m_objConfig.GetOptionParameter().eLanguage == CDefine.ELanguage.LANGUAGE_KOREA)
                {
                    return mResources[key][0];
                }
                else
                {
                    return mResources[key][1];
                }
            }
            else
            {
                return index;
            }
        }

        public static object[] GetAll(params object[] args)
        {
            return args.Select(item => Get(item)).ToArray();
        }
    }
}
