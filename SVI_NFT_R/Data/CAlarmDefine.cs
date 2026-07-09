namespace SVI_NFT_R
{
    public static class CAlarmDefine
    {
        /// <summary>
        /// 알람 & 인터락 리스트
        /// </summary>
        public enum EAlarmList
        {
            /// <summary>
            /// 등록되지 않은 알람
            /// </summary>
            NOT_REGISTED_ALARM = 0,


            //////////////////////////////////////////////////////////////////////////
            // LOAD_EQ_TO_EQ
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 상류설비에서 EMERGENCY가 발생했습니다
            /// </summary>
            EM_UPPER_EQUIPMENT_N_EMERGENCY_STATE = 10000,
            /// <summary>
            /// 상류설비에 DOOR가 열렸습니다
            /// </summary>
            EM_UPPER_EQUIPMENT_DOOR_OPEN = 10001,
            //////////////////////////////////////////////////////////////////////////
            // IN_SHUTTLE
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// IN셔틀 X1축이 홈 동작 진행에 실패 했습니다
            /// </summary>
            MO_IN_SHUTTLE_X1_HOME_MOTION = 10500,
            /// <summary>
            /// IN셔틀 X1축 로드 위치 이동에 실패 했습니다 (상류설비 위치)
            /// </summary>
            MO_IN_SHUTTLE_X1_LOAD_MOVE = 10501,
            /// <summary>
            /// IN셔틀 X1축 스캔 시작 대기 위치 이동에 실패 했습니다
            /// </summary>
            MO_IN_SHUTTLE_X1_SCAN_START_WAIT_MOVE = 10502,
            /// <summary>
            /// IN셔틀 X1축 스캔 트리거 시작 위치 이동에 실패 했습니다
            /// </summary>
            MO_IN_SHUTTLE_X1_SCAN_TRIGGER_START_MOVE = 10503,
            /// <summary>
            /// IN셔틀 X1축 스캔 트리거 종료 위치 이동에 실패 했습니다
            /// </summary>
            MO_IN_SHUTTLE_X1_SCAN_TRIGGER_END_MOVE = 10504,
            /// <summary>
            /// IN셔틀 X1축 얼라인 위치 이동에 실패 했습니다
            /// </summary>
            MO_IN_SHUTTLE_X1_ALIGN_MOVE = 10505,
            /// <summary>
            /// IN셔틀 X1축 언로드 위치 이동에 실패 했습니다 (IN로봇 위치)
            /// </summary>
            MO_IN_SHUTTLE_X1_UNLOAD_MOVE = 10506,
            /// <summary>
            /// IN셔틀 P1 진공 흡착에 실패 했습니다
            /// </summary>
            VC_IN_SHUTTLE_P1_VACUUM = 10600,
            /// <summary>
            /// IN셔틀 P1 진공 파기에 실패 했습니다
            /// </summary>
            VC_IN_SHUTTLE_P1_BLOW = 10601,
            /// <summary>
            /// IN셔틀 P1 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_IN_SHUTTLE_P1_VACUUM_CHECK = 10602,
            /// <summary>
            /// IN셔틀 P1 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_IN_SHUTTLE_P1_SENSOR_CHECK = 10603,
            /// <summary>
            /// IN셔틀 P1 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_IN_SHUTTLE_P1_VACUUM_OVERLAP = 10604,
            /// <summary>
            /// IN셔틀 P1 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_IN_SHUTTLE_P1_SENSOR_OVERLAP = 10605,
            /// <summary>
            /// IN셔틀 P2 진공 흡착에 실패 했습니다
            /// </summary>
            VC_IN_SHUTTLE_P2_VACUUM = 10610,
            /// <summary>
            /// IN셔틀 P2 진공 파기에 실패 했습니다
            /// </summary>
            VC_IN_SHUTTLE_P2_BLOW = 10611,
            /// <summary>
            /// IN셔틀 P2 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_IN_SHUTTLE_P2_VACUUM_CHECK = 10612,
            /// <summary>
            /// IN셔틀 P2 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_IN_SHUTTLE_P2_SENSOR_CHECK = 10613,
            /// <summary>
            /// IN셔틀 P2 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_IN_SHUTTLE_P2_VACUUM_OVERLAP = 10614,
            /// <summary>
            /// IN셔틀 P2 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_IN_SHUTTLE_P2_SENSOR_OVERLAP = 10615,
            //////////////////////////////////////////////////////////////////////////
            // IN_ROBOT
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// IN로봇 초기화 동작에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_IN_N_INITIALIZE = 11000,
            /// <summary>
            /// IN로봇 동작중인 프로세스 스킵에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_IN_PROCESS_SKIP = 11001,
            /// <summary>
            /// IN로봇 JCM 모드 진입(ENTER)에 실패 했습니다. 
            /// </summary>
            MO_TRANSFER_ROBOT_IN_ENTER_JCM_MODE = 11002,
            /// <summary>
            /// IN로봇 JCM 모드 이동(MOVE)에 실패 했습니다.
            /// </summary>
            MO_TRANSFER_ROBOT_IN_JCM_MODE_MOVE = 11003,
            /// <summary>
            /// IN로봇 JCM 모드 출입(EXIT)에 실패 했습니다. 
            /// </summary>
            MO_TRANSFER_ROBOT_IN_EXIT_JCM_MODE = 11004,
            /// <summary>
            /// IN로봇 로드 프로세스 진입에 실패 했습니다 (IN셔틀 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_IN_LOAD_BEGIN = 11005,
            /// <summary>
            /// IN로봇 로드 프로세스 퍼밋 동작에 실패 했습니다 (IN셔틀 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_IN_LOAD_PERMIT = 11006,
            /// <summary>
            /// IN로봇 로트 프로세스 종료에 실패 했습니다 (IN셔틀 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_IN_LOAD_EXIT = 11007,
            /// <summary>
            /// IN로봇 MCR 프로세스 진입에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_IN_MCR_BEGIN = 11008,
            /// <summary>
            /// IN로봇 MCR 프로세스 퍼밋 동작에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_IN_MCR_PERMIT = 11009,
            /// <summary>
            /// IN로봇 MCR 프로세스 종료에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_IN_MCR_EXIT = 11010,
            /// <summary>
            /// IN로봇 언로드 프로세스 진입에 실패 했습니다 (검사스테이지 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_IN_UNLOAD_BEGIN = 11011,
            /// <summary>
            /// IN로봇 언로드 프로세스 퍼밋 동작에 실패 했습니다 (검사스테이지 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_IN_UNLOAD_PERMIT = 11012,
            /// <summary>
            /// IN로봇 언로드 프로세스 종료에 실패 했습니다 (검사스테이지 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_IN_UNLOAD_EXIT = 11013,
            /// <summary>
            /// IN로봇 실린더 P1 회전 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_IN_P1_T_TURN = 11100,
            /// <summary>
            /// IN로봇 실린더 P1 복귀 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_IN_P1_T_RETURN = 11101,
            /// <summary>
            /// IN로봇 실린더 P2 회전 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_IN_P2_T_TURN = 11110,
            /// <summary>
            /// IN로봇 실린더 P2 복귀 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_IN_P2_T_RETURN = 11111,
            /// <summary>
            /// IN로봇 피커 P1 흡착에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_IN_P1_VACUUM = 11120,
            /// <summary>
            /// IN로봇 피커 P1 파기에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_IN_P1_BLOW = 11121,
            /// <summary>
            /// IN로봇 P1 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P1_VACUUM_CHECK = 11122,
            /// <summary>
            /// IN로봇 P1 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P1_SENSOR_CHECK = 11123,
            /// <summary>
            /// IN로봇 P1 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P1_VACUUM_OVERLAP = 11124,
            /// <summary>
            /// IN로봇 P1 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P1_SENSOR_OVERLAP = 11125,
            /// <summary>
            /// IN로봇 피커 P2 흡착에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_IN_P2_VACUUM = 11130,
            /// <summary>
            /// IN로봇 피커 P2 파기에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_IN_P2_BLOW = 11131,
            /// <summary>
            /// IN로봇 P2 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P2_VACUUM_CHECK = 11132,
            /// <summary>
            /// IN로봇 P2 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P2_SENSOR_CHECK = 11133,
            /// <summary>
            /// IN로봇 P2 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P2_VACUUM_OVERLAP = 11134,
            /// <summary>
            /// IN로봇 P2 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_IN_P2_SENSOR_OVERLAP = 11135,
            /// <summary>
            /// IN로봇 얼라인 데이터 전송에 실패 했습니다
            /// </summary>
            CO_TRANSFER_ROBOT_IN_ALIGN_DATA = 11400,
            /// <summary>
            /// IN로봇 얼라인 초기화 데이터 전송에 실패 했습니다
            /// </summary>
            CO_TRANSFER_ROBOT_IN_RESET_DATA = 11401,
            /// <summary>
            /// IN로봇 켈리브레이션 데이터 전송에 실패 했습니다
            /// </summary>
            CO_TRANSFER_ROBOT_IN_CALIBRATION_DATA = 11402,
            //////////////////////////////////////////////////////////////////////////
            // INSPECTION_STAGE
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 검사스테이지 Y축이 홈 동작 진행에 실패 했습니다
            /// </summary>
            MO_INSP_STAGE_Y_HOME_MOTION = 11500,
            /// <summary>
            /// 검사스테이지 Y축 로드 위치 이동에 실패 했습니다 (IN로봇 위치)
            /// </summary>
            MO_INSP_STAGE_Y_LOAD_MOVE = 11501,
            /// <summary>
            /// 검사스테이지 Y축 P1 촬상 위치 이동에 실패 했습니다
            /// </summary>
            MO_INSP_STAGE_Y_GRAB_P1_MOVE = 11502,
            /// <summary>
            /// 검사스테이지 Y축 P2 촬상 위치 이동에 실패 했습니다
            /// </summary>
            MO_INSP_STAGE_Y_GRAB_P2_MOVE = 11503,
            /// <summary>
            /// 검사스테이지 Y축 언로드 위치 이동에 실패 했습니다 (OUT로봇 위치)
            /// </summary>
            MO_INSP_STAGE_Y_UNLOAD_MOVE = 11504,
            /// <summary>
            /// 검사스테이지 P1 진공 흡착에 실패 했습니다
            /// </summary>
            VC_INSP_STAGE_P1_VACUUM = 11600,
            /// <summary>
            /// 검사스테이지 P1 진공 파기에 실패 했습니다
            /// </summary>
            VC_INSP_STAGE_P1_BLOW = 11601,
            /// <summary>
            /// 검사스테이지 P1 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_INSP_STAGE_P1_VACUUM_CHECK = 11602,
            /// <summary>
            /// 검사스테이지 P1 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_INSP_STAGE_P1_SENSOR_CHECK = 11603,
            /// <summary>
            /// 검사스테이지 P1 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_INSP_STAGE_P1_VACUUM_OVERLAP = 11604,
            /// <summary>
            /// 검사스테이지 P1 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_INSP_STAGE_P1_SENSOR_OVERLAP = 11605,
            /// <summary>
            /// 검사스테이지 P2 진공 흡착에 실패 했습니다
            /// </summary>
            VC_INSP_STAGE_P2_VACUUM = 11610,
            /// <summary>
            /// 검사스테이지 P2 진공 파기에 실패 했습니다
            /// </summary>
            VC_INSP_STAGE_P2_BLOW = 11611,
            /// <summary>
            /// 검사스테이지 P2 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_INSP_STAGE_P2_VACUUM_CHECK = 11612,
            /// <summary>
            /// 검사스테이지 P2 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_INSP_STAGE_P2_SENSOR_CHECK = 11613,
            /// <summary>
            /// 검사스테이지 P2 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_INSP_STAGE_P2_VACUUM_OVERLAP = 11614,
            /// <summary>
            /// 검사스테이지 P2 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_INSP_STAGE_P2_SENSOR_OVERLAP = 11615,
            //////////////////////////////////////////////////////////////////////////
            // OUT_ROBOT
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// OUT로봇 초기화 동작에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_N_INITIALIZE = 12000,
            /// <summary>
            /// OUT로봇 동작중인 프로세스 스킵에 실패 했습니다
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_PROCESS_SKIP = 12001,
            /// <summary>
            /// OUT로봇 JCM 모드 진입(ENTER)에 실패 했습니다. 
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_ENTER_JCM_MODE = 12002,
            /// <summary>
            /// OUT로봇 JCM 모드 이동(MOVE)에 실패 했습니다.
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_JCM_MODE_MOVE = 12003,
            /// <summary>
            /// OUT로봇 JCM 모드 출입(EXIT)에 실패 했습니다. 
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_EXIT_JCM_MODE = 12004,
            /// <summary>
            /// OUT로봇 로드 프로세스 진입에 실패 했습니다 (검사스테이지 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_LOAD_BEGIN = 12005,
            /// <summary>
            /// OUT로봇 로드 프로세스 퍼밋 동작에 실패 했습니다 (검사스테이지 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_LOAD_PERMIT = 12006,
            /// <summary>
            /// OUT로봇 로드 프로세스 종료에 실패 했습니다 (검사스테이지 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_LOAD_EXIT = 12007,
            /// <summary>
            /// OUT로봇 언로드 프로세스 진입에 실패 했습니다 (하류설비 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_UNLOAD_BEGIN = 12008,
            /// <summary>
            /// OUT로봇 언로드 프로세스 퍼밋 동작에 실패 했습니다 (하류설비 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_UNLOAD_PERMIT = 12009,
            /// <summary>
            /// OUT로봇 언로드 프로세스 종료에 실패 했습니다 (하류설비 위치)
            /// </summary>
            MO_TRANSFER_ROBOT_OUT_UNLOAD_EXIT = 12010,
            /// <summary>
            /// OUT로봇 실린더 P1 회전 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_OUT_P1_T_TURN = 12100,
            /// <summary>
            /// OUT로봇 실린더 P1 복귀 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_OUT_P1_T_RETURN = 12101,
            /// <summary>
            /// OUT로봇 실린더 P2 회전 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_OUT_P2_T_TURN = 12110,
            /// <summary>
            /// OUT로봇 실린더 P2 복귀 동작에 실패 했습니다
            /// </summary>
            CY_TRANSFER_ROBOT_OUT_P2_T_RETURN = 12111,
            /// <summary>
            /// OUT로봇 피커 P1 흡착에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_OUT_P1_VACUUM = 12120,
            /// <summary>
            /// OUT로봇 피커 P1 파기에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_OUT_P1_BLOW = 12121,
            /// <summary>
            /// OUT로봇 P1 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P1_VACUUM_CHECK = 12122,
            /// <summary>
            /// OUT로봇 P1 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P1_SENSOR_CHECK = 12123,
            /// <summary>
            /// OUT로봇 P1 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P1_VACUUM_OVERLAP = 12124,
            /// <summary>
            /// OUT로봇 P1 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P1_SENSOR_OVERLAP = 12125,
            /// <summary>
            /// OUT로봇 피커 P2 흡착에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_OUT_P2_VACUUM = 12130,
            /// <summary>
            /// OUT로봇 피커 P2 파기에 실패 했습니다
            /// </summary>
            VC_TRANSFER_ROBOT_OUT_P2_BLOW = 12131,
            /// <summary>
            /// OUT로봇 P2 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P2_VACUUM_CHECK = 12132,
            /// <summary>
            /// OUT로봇 P2 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P2_SENSOR_CHECK = 12133,
            /// <summary>
            /// OUT로봇 P2 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P2_VACUUM_OVERLAP = 12134,
            /// <summary>
            /// OUT로봇 P2 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_TRANSFER_ROBOT_OUT_P2_SENSOR_OVERLAP = 12135,
            /// <summary>
            /// OUT로봇 얼라인 데이터 전송에 실패 했습니다
            /// </summary>
            CO_TRANSFER_ROBOT_OUT_ALIGN_DATA = 12400,
            /// <summary>
            /// OUT로봇 얼라인 초기화 데이터 전송에 실패 했습니다
            /// </summary>
            CO_TRANSFER_ROBOT_OUT_RESET_DATA = 12401,
            /// <summary>
            /// OUT로봇 켈리브레이션 데이터 전송에 실패 했습니다
            /// </summary>
            CO_TRANSFER_ROBOT_OUT_CALIBRATION_DATA = 12402,
            //////////////////////////////////////////////////////////////////////////
            // OUT_FLIP
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// OUT플립 R1축이 홈 동작 진행에 실패 했습니다
            /// </summary>
            MO_OUT_FLIP_R1_HOME_MOTION = 13000,
            /// <summary>
            /// OUT플립 R1축 로드 위치 이동에 실패 했습니다 (OUT로봇 위치)
            /// </summary>
            MO_OUT_FLIP_R1_LOAD_MOVE = 13001,
            /// <summary>
            /// OUT플립 R1축 언로드 위치 이동에 실패 했습니다 (OUT컨베이어 위치)
            /// </summary>
            MO_OUT_FLIP_R1_UNLOAD_MOVE = 13002,
            /// <summary>
            /// OUT플립 R2축이 홈 동작 진행에 실패 했습니다
            /// </summary>
            MO_OUT_FLIP_R2_HOME_MOTION = 13010,
            /// <summary>
            /// OUT플립 R2축 로드 위치 이동에 실패 했습니다 (OUT로봇 위치)
            /// </summary>
            MO_OUT_FLIP_R2_LOAD_MOVE = 13011,
            /// <summary>
            /// OUT플립 R2축 언로드 위치 이동에 실패 했습니다 (OUT컨베이어 위치)
            /// </summary>
            MO_OUT_FLIP_R2_UNLOAD_MOVE = 13012,
            /// <summary>
            /// OUT플립 Z축이 홈 동작 진행에 실패 했습니다
            /// </summary>
            MO_OUT_FLIP_Z_HOME_MOTION = 13020,
            /// <summary>
            /// OUT플립 Z축 상승 위치 이동에 실패 했습니다
            /// </summary>
            MO_OUT_FLIP_Z_UP_MOVE = 13021,
            /// <summary>
            /// OUT플립 Z축 하강 위치 이동에 실패 했습니다
            /// </summary>
            MO_OUT_FLIP_Z_DOWN_MOVE = 13022,
            /// <summary>
            /// OUT플립 피커 P1 흡착에 실패 했습니다
            /// </summary>
            VC_OUT_FLIP_P1_VACUUM = 13100,
            /// <summary>
            /// OUT플립 피커 P1 파기에 실패 했습니다
            /// </summary>
            VC_OUT_FLIP_P1_BLOW = 13101,
            /// <summary>
            /// OUT플립 P1 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_OUT_FLIP_P1_VACUUM_CHECK = 13102,
            /// <summary>
            /// OUT플립 P1 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_OUT_FLIP_P1_SENSOR_CHECK = 13103,
            /// <summary>
            /// OUT플립 P1 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_OUT_FLIP_P1_VACUUM_OVERLAP = 13104,
            /// <summary>
            /// OUT플립 P1 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_OUT_FLIP_P1_SENSOR_OVERLAP = 13105,
            /// <summary>
            /// OUT플립 피커 P2 흡착에 실패 했습니다
            /// </summary>
            VC_OUT_FLIP_P2_VACUUM = 13110,
            /// <summary>
            /// OUT플립 피커 P2 파기에 실패 했습니다
            /// </summary>
            VC_OUT_FLIP_P2_BLOW = 13111,
            /// <summary>
            /// OUT플립 P2 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_OUT_FLIP_P2_VACUUM_CHECK = 13112,
            /// <summary>
            /// OUT플립 P2 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_OUT_FLIP_P2_SENSOR_CHECK = 13113,
            /// <summary>
            /// OUT플립 P2 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_OUT_FLIP_P2_VACUUM_OVERLAP = 13114,
            /// <summary>
            /// OUT플립 P2 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_OUT_FLIP_P2_SENSOR_OVERLAP = 13115,
            //////////////////////////////////////////////////////////////////////////
            // OUT_SHUTTLE
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// OUT셔틀 X1축이 홈 동작 진행에 실패 했습니다
            /// </summary>
            MO_OUT_SHUTTLE_X1_HOME_MOTION = 13500,
            /// <summary>
            /// OUT셔틀 X1축 로드 위치 이동에 실패 했습니다 
            /// </summary>
            MO_OUT_SHUTTLE_X1_LOAD_MOVE = 13501,
            /// <summary>
            /// OUT셔틀 X1축 언로드 위치 이동에 실패 했습니다 
            /// </summary>
            MO_OUT_SHUTTLE_X1_UNLOAD_MOVE = 13502,
            /// <summary>
            /// OUT셔틀 P1 진공 흡착에 실패 했습니다
            /// </summary>
            VC_OUT_SHUTTLE_P1_VACUUM = 13600,
            /// <summary>
            /// OUT셔틀 P1 진공 파기에 실패 했습니다
            /// </summary>
            VC_OUT_SHUTTLE_P1_BLOW = 13601,
            /// <summary>
            /// OUT셔틀 P1 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_OUT_SHUTTLE_P1_VACUUM_CHECK = 13602,
            /// <summary>
            /// OUT셔틀 P1 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_OUT_SHUTTLE_P1_SENSOR_CHECK = 13603,
            /// <summary>
            /// OUT셔틀 P1 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_OUT_SHUTTLE_P1_VACUUM_OVERLAP = 13604,
            /// <summary>
            /// OUT셔틀 P1 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_OUT_SHUTTLE_P1_SENSOR_OVERLAP = 13605,
            /// <summary>
            /// OUT셔틀 P2 진공 흡착에 실패 했습니다
            /// </summary>
            VC_OUT_SHUTTLE_P2_VACUUM = 13610,
            /// <summary>
            /// OUT셔틀 P2 진공 파기에 실패 했습니다
            /// </summary>
            VC_OUT_SHUTTLE_P2_BLOW = 13612,
            /// <summary>
            /// OUT셔틀 P2 셀 확인에 실패 했습니다 (진공)
            /// </summary>
            SE_OUT_SHUTTLE_P2_VACUUM_CHECK = 13614,
            /// <summary>
            /// OUT셔틀 P2 셀 확인에 실패 했습니다 (센서)
            /// </summary>
            SE_OUT_SHUTTLE_P2_SENSOR_CHECK = 13616,
            /// <summary>
            /// OUT셔틀 P2 셀 겹침 확인되었습니다 (진공)
            /// </summary>
            SE_OUT_SHUTTLE_P2_VACUUM_OVERLAP = 13618,
            /// <summary>
            /// OUT셔틀 P2 셀 겹침 확인되었습니다 (센서)
            /// </summary>
            SE_OUT_SHUTTLE_P2_SENSOR_OVERLAP = 13620,
            //////////////////////////////////////////////////////////////////////////
            // UNLOAD_EQ_TO_EQ
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 하류설비에서 EMERGENCY가 발생했습니다
            /// </summary>
            EM_LOWER_EQUIPMENT_N_EMERGENCY_STATE = 14000,
            /// <summary>
            /// 하류설비에 DOOR가 열렸습니다
            /// </summary>
            EM_LOWER_EQUIPMENT_DOOR_OPEN = 14001,
            //////////////////////////////////////////////////////////////////////////
            // MCR
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// MCR P1 촬상에 실패 했습니다
            /// </summary>
            VI_LOADER_MCR_P1_GRAB_TIMEOUT = 20000,
            /// <summary>
            /// MCR P1 결과 대기 시간을 초과 했습니다
            /// </summary>
            VI_LOADER_MCR_P1_RESULT_TIMEOUT = 20001,
            /// <summary>
            /// MCR P1 촬상을 연속으로 실패 했습니다
            /// </summary>
            VI_LOADER_MCR_P1_CONTINUOUS_FAILURE = 20002,
            /// <summary>
            /// MCR P2 촬상에 실패 했습니다
            /// </summary>
            VI_LOADER_MCR_P2_GRAB_TIMEOUT = 20010,
            /// <summary>
            /// MCR P2 결과 대기 시간을 초과 했습니다
            /// </summary>
            VI_LOADER_MCR_P2_RESULT_TIMEOUT = 20011,
            /// <summary>
            /// MCR P2 촬상을 연속으로 실패 했습니다
            /// </summary>
            VI_LOADER_MCR_P2_CONTINUOUS_FAILURE = 20012,
            //////////////////////////////////////////////////////////////////////////
            // PRE_ALIGN_VISION
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 프리얼라인 Program 통신 연결이 끊어 졌습니다
            /// </summary>
            CO_PRE_ALIGN_N_DISCONNECTED = 20500,
            /// <summary>
            /// 프리얼라인 Program에 에러가 발생했습니다
            /// </summary>
            VI_PRE_ALIGN_N_RECEIVE_ALIGN_ERROR = 20501,
            /// <summary>
            /// 프리얼라인 Program 모델 리스트 요청 응답 대기 시간을 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_N_MODEL_LIST_TIMEOUT = 20502,
            /// <summary>
            /// 프리얼라인 Program에 현재 모델 정보가 등록되어 있지 않습니다
            /// </summary>
            VI_PRE_ALIGN_N_MODEL_NOT_FOUND = 20503,
            /// <summary>
            /// 프리얼라인 촬상 시작 응답 대기 시간을 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_START_TIMEOUT = 20504,
            /// <summary>
            /// 프리얼라인 촬상 완료 대기 시간을 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_COMPLETE_TIMEOUT = 20505,
            /// <summary>
            /// 프리얼라인 촬상 완료 데이터 정합성이 맞지 않습니다
            /// </summary>
            CO_PRE_ALIGN_COMPLETE_VALIDATION_FAIL = 20506,
            /// <summary>
            /// 프리얼라인 P1 X 총 보정량이 인터락 범위를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_P1_OVERRANGE_X = 20507,
            /// <summary>
            /// 프리얼라인 P1 Y 총 보정량이 인터락 범위를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_P1_OVERRANGE_Y = 20508,
            /// <summary>
            /// 프리얼라인 P1 T 총 보정량이 인터락 범위를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_P1_OVERRANGE_T = 20509,
            /// <summary>
            /// 프리얼라인 P1 카메라 그랩 스코어가 설정값 보다 낮습니다
            /// </summary>
            VI_PRE_ALIGN_P1_OVERRANGE_SCORE = 20510,
            /// <summary>
            /// 프리얼라인 P2 X 총 보정량이 인터락 범위를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_P2_OVERRANGE_X = 20511,
            /// <summary>
            /// 프리얼라인 P2 Y 총 보정량이 인터락 범위를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_P2_OVERRANGE_Y = 20512,
            /// <summary>
            /// 프리얼라인 P2 T 총 보정량이 인터락 범위를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_P2_OVERRANGE_T = 20513,
            /// <summary>
            /// 프리얼라인 P2 카메라 그랩 스코어가 설정값 보다 낮습니다
            /// </summary>
            VI_PRE_ALIGN_P2_OVERRANGE_SCORE = 20514,
            /// <summary>
            /// 프리얼라인 카메라 재시도 횟수를 초과 했습니다
            /// </summary>
            VI_PRE_ALIGN_RETRY_COUNTOVER = 20515,
            /// <summary>
            /// 프리얼라인 시스템이 정지 상태입니다
            /// </summary>
            VI_PRE_ALIGN_ERROR_SYSTEM_NOT_RUNNING = 20550,
            /// <summary>
            /// 프리얼라인 카메라에 이상이 발생했습니다
            /// </summary>
            VI_PRE_ALIGN_ERROR_CAMERA = 20551,
            /// <summary>
            /// 프리얼라인 조명 컨트롤러에 이상이 발생했습니다
            /// </summary>
            VI_PRE_ALIGN_ERROR_LIGHT_CONTROLLER = 20552,
            /// <summary>
            /// 프리얼라인 조명이 정상 온도를 벗어났습니다
            /// </summary>
            VI_PRE_ALIGN_ERROR_LIGHT_TEMP_OVER = 20553,
            //////////////////////////////////////////////////////////////////////////
            // INSPECT_VISION
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 외관 검사기 통신 연결이 끊어 졌습니다
            /// </summary>
            CO_INSP_PC_MAIN_N_DISCONNECTED = 21000,
            /// <summary>
            /// 외관 검사 조명 광량 요청 Send 실패 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_LIGHT_INTENCITY_REQUEST = 21001,
            /// <summary>
            /// 외관 검사기 조명 온도 요청 Send 실패 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_LIGHT_TEMPERATURE_REQUEST = 21002,
            /// <summary>
            /// 외관 검사기 인터페이스 상태 확인 Send 실패 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_CHECK_STATUS_SEND = 21003,
            /// <summary>
            /// 외관 검사 스테이지 검사 준비 상태가 아닙니다
            /// </summary>
            CO_INSP_PC_MAIN_NOT_READY = 21004,
            /// <summary>
            /// 외관 검사기 인터페이스 그랩 시작 Send 실패 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_GRAB_START_SEND = 21005,
            /// <summary>
            /// 외관 검사기 인터페이스 그랩 시작 ReceiveData가 정확하지 않습니다
            /// </summary>
            CO_INSP_PC_MAIN_GRAB_START_VALIDATION = 21006,
            /// <summary>
            /// 외관 검사기 인터페이스 그랩 완료 응답 타임아웃 입니다
            /// </summary>
            CO_INSP_PC_MAIN_GRAB_END_TIMEOUT = 21007,
            /// <summary>
            /// 외관 검사기 인터페이스 그랩 완료 ReceiveData가 정확하지 않습니다
            /// </summary>
            CO_INSP_PC_MAIN_GRAB_END_VALIDATION = 21008,
            /// <summary>
            /// 외관 검사기 종합 결과 응답 타임아웃 입니다
            /// </summary>
            IN_INSP_PC_MAIN_TOTAL_RESULT_TIMEOUT = 21009,
            /// <summary>
            /// 외관 검사기 종합 결과 응답 연속 타임아웃 입니다
            /// </summary>
            IN_INSP_PC_MAIN_TOTAL_RESULT_NORESPONSE = 21010,
            /// <summary>
            /// JAVAS 외관 검사기에서 Error가 발생 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_ERROR = 21011,
            /// <summary>
            /// JAVAS 외관 검사 결과 동일한 위치에 손상이 발생 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_ERROR_SAME_POSITION_DEFECT = 21012,
            /// <summary>
            /// 외관 검사PC 카메라에 이상이 있습니다
            /// </summary>
            CO_INSP_PC_MAIN_ERROR_CAMERA = 21013,
            /// <summary>
            /// 외관 검사PC 조명 컨트롤러에 이상이 있습니다
            /// </summary>
            CO_INSP_PC_MAIN_ERROR_LIGHT_CONTROLLER = 21014,
            /// <summary>
            /// 외관 검사기 인터페이스 셀 데이터 동기화 Send 실패 하였습니다
            /// </summary>
            CO_INSP_PC_MAIN_CELL_DATA_SYNC_SEND = 21015,
            /// <summary>
            /// 외관 검사기 인터페이스 셀 데이터 동기화 ReceiveData가 정확하지 않습니다
            /// </summary>
            CO_INSP_PC_MAIN_CELL_DATA_SYNC_VALIDATION = 21016,
            /// <summary>
            /// 외관 검사기 인터페이스 AMO 그랩 시작 Send 실패 하였습니다 (간이 광학계)
            /// </summary>
            CO_INSP_PC_MAIN_AMO_GRAB_START_SEND = 21017,
            /// <summary>
            /// 외관 검사기 인터페이스 AMO 그랩 시작 ReceiveData가 정확하지 않습니다 (간이 광학계)
            /// </summary>
            CO_INSP_PC_MAIN_AMO_GRAB_START_VALIDATION = 21018,
            /// <summary>
            /// 외관 검사기 인터페이스 AMO 그랩 완료 Send 실패 하였습니다 (간이 광학계)
            /// </summary>
            CO_INSP_PC_MAIN_AMO_GRAB_END_SEND = 21019,
            /// <summary>
            /// 외관 검사기 인터페이스 AMO 그랩 완료 ReceiveData가 정확하지 않습니다 (간이 광학계)
            /// </summary>
            CO_INSP_PC_MAIN_AMO_GRAB_END_VALIDATION = 21020,
            //////////////////////////////////////////////////////////////////////////
            // CIM
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// P1 트랙킹에 실패했습니다
            /// </summary>
            VA_TRANSFER_CIM_P1_TRACKING_FAILED = 29500,
            /// <summary>
            /// P1 트랙킹 시간이 초과했습니다
            /// </summary>
            VA_TRANSFER_CIM_P1_TRACKING_TIMEOUT = 29501,
            /// <summary>
            /// P1 트랙킹 CELL ID와 JOB PROCESS REQUEST CELL ID와 다릅니다
            /// </summary>
            VA_TRANSFER_CIM_P1_TRACKING_MISSMATCH = 29502,
            /// <summary>
            /// P1 셀 정보 다운로드에 실패했습니다
            /// </summary>
            VA_TRANSFER_CIM_P1_CELL_INFORMATION_DOWNLOAD_FAILED = 29510,
            /// <summary>
            /// P1 셀 정보 다운로드 시간이 초과했습니다
            /// </summary>
            VA_TRANSFER_CIM_P1_CELL_INFORMATION_DOWNLOAD_TIMEOUT = 29511,
            /// <summary>
            /// P1 셀 정보 다운로드 CELL ID와 JOB PROCESS REQUEST CELL ID와 다릅니다
            /// </summary>
            VA_TRANSFER_CIM_P1_CELL_INFORMATION_DOWNLOAD_MISSMATCH = 29512,
            /// <summary>
            /// P2 트랙킹에 실패했습니다
            /// </summary>
            VA_TRANSFER_CIM_P2_TRACKING_FAILED = 29600,
            /// <summary>
            /// P2 트랙킹 시간이 초과했습니다
            /// </summary>
            VA_TRANSFER_CIM_P2_TRACKING_TIMEOUT = 29601,
            /// <summary>
            /// P2 트랙킹 CELL ID와 JOB PROCESS REQUEST CELL ID와 다릅니다
            /// </summary>
            VA_TRANSFER_CIM_P2_TRACKING_MISSMATCH = 29602,
            /// <summary>
            /// P2 셀 정보 다운로드에 실패했습니다
            /// </summary>
            VA_TRANSFER_CIM_P2_CELL_INFORMATION_DOWNLOAD_FAILED = 29610,
            /// <summary>
            /// P2 셀 정보 다운로드 시간이 초과했습니다
            /// </summary>
            VA_TRANSFER_CIM_P2_CELL_INFORMATION_DOWNLOAD_TIMEOUT = 29611,
            /// <summary>
            /// P2 셀 정보 다운로드 CELL ID와 JOB PROCESS REQUEST CELL ID와 다릅니다
            /// </summary>
            VA_TRANSFER_CIM_P2_CELL_INFORMATION_DOWNLOAD_MISSMATCH = 29612,
            /// <summary>
            /// HOST 통신 연결이 끊겼습니다
            /// </summary>
            CO_TRANSFER_CIM_N_HOST_DISCONNECTED = 29998,
            /// <summary>
            /// CIM 통신 연결이 끊겼습니다
            /// </summary>
            CO_TRANSFER_CIM_N_CIM_DISCONNECTED = 29999,
            //////////////////////////////////////////////////////////////////////////
            // SAFETY
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 전장반 온도가 40도를 초과 했습니다
            /// </summary>
            UT_MAIN_ELECTRONICBOX_TEMPERATURE_1_N_OVER40 = 30000,
            /// <summary>
            /// PC 랙 온도가 40도를 초과 했습니다
            /// </summary>
            UT_MAIN_PCRACK_TEMPERATURE_1_N_OVER40 = 30001,
            /// <summary>
            /// 전장반에 연기가 감지되었습니다
            /// </summary>
            UT_MAIN_ELECTRONICBOX_SMOKE_SENSOR_1_N_DETECTED = 30002,
            /// <summary>
            /// 세이프티 PLC 문제 발생하여 설비 MAGNET 전원이 OFF 되었습니다
            /// </summary>
            UT_MAIN_SAFETYPLC_GPS_MC_POWER_OFF = 30003,
            /// <summary>
            /// FFU컨트롤러 연결이 끊어졌습니다.
            /// </summary>
            CO_MAIN_DEVICE_MCU_DISCONNECTED = 30004,
            /// <summary>
            /// 접지 저항 측정기 연결이 끊어졌습니다.
            /// </summary>
            CO_MAIN_DEVICE_GMS_DISCONNECTED = 30005,
            /// <summary>
            /// 전장반 온도컨트롤러 연결이 끊어졌습니다.
            /// </summary>
            CO_MAIN_ELECTRONICBOX_TEMPERATURE_DISCONNECTED = 30006,
            /// <summary>
            /// PC 랙 온도컨트롤러 연결이 끊어졌습니다.
            /// </summary>
            CO_MAIN_PCRACK_TEMPERATURE_DISCONNECTED = 30007,
            /// <summary>
            /// 설비 구동중에 세이프티키가 TEACH로 변경되었습니다
            /// </summary>
            OP_MAIN_MODEKEY_N_TEACH = 30008,
            /// <summary>
            /// 설비에 모든 유닛이 정지 상태입니다
            /// </summary>
            OP_MAIN_UNIT_ALL_STOP = 30009,
            /// <summary>
            /// 메인 진공 공압이 설정 값보다 낮아졌습니다
            /// </summary>
            VC_MAIN_VACUUM_MAIN_SUPPLY = 30010,
            /// <summary>
            /// 검사스테이지 메인 진공 공압이 설정 값보다 낮아졌습니다
            /// </summary>
            VC_INSP_STAGE_VACUUM_MAIN_SUPPLY = 30011,
            /// <summary>
            /// 메인 공압이 설정 값보다 낮아졌습니다
            /// </summary>
            VC_MAIN_AIR_MAIN_N_SUPPLY = 30012,
            /// <summary>
            /// MAIN 전장반 FAN1이 정지했습니다
            /// </summary>
            UT_MAIN_ELECTRONICBOX_FAN_1_STOP = 30013,
            /// <summary>
            /// MAIN 전장반 FAN2가 정지했습니다
            /// </summary>
            UT_MAIN_ELECTRONICBOX_FAN_2_STOP = 30014,
            /// <summary>
            /// MAIN MCU 그룹에 FFU#1에 FAN이 정지했습니다
            /// </summary>
            UT_MAIN_MCU_FFU_1_STOP = 30015,
            /// <summary>
            /// MAIN MCU 그룹에 FFU#1에 FAN에 알람이 발생했습니다
            /// </summary>
            UT_MAIN_MCU_FFU_1_ALARM_OCCURRED = 30016,
            /// <summary>
            /// 정면 비상 정지 버튼#1이 눌렸습니다
            /// </summary>
            EM_MAIN_CONTROLPANEL_FRONT_EMS_X000 = 30100,
            /// <summary>
            /// 후면 비상 정지 버튼#2이 눌렸습니다
            /// </summary>
            EM_MAIN_CONTROLPANEL_REAR_EMS_X001 = 30101,
            /// <summary>
            /// IN로봇 팬던트에 비상 정지 버튼이 눌렸습니다
            /// </summary>
            EM_TRANSFER_ROBOT_IN_PENDANT_EMS = 30102,
            /// <summary>
            /// IN로봇 컨트롤러에 비상 정지 버튼이 눌렸습니다
            /// </summary>
            EM_TRANSFER_ROBOT_IN_CONTROLLER_EMS = 30103,
            /// <summary>
            /// OUT로봇 팬던트에 비상 정지 버튼이 눌렸습니다
            /// </summary>
            EM_TRANSFER_ROBOT_OUT_PENDANT_EMS = 30104,
            /// <summary>
            /// OUT로봇 컨트롤러에 비상 정지 버튼이 눌렸습니다
            /// </summary>
            EM_TRANSFER_ROBOT_OUT_CONTROLLER_EMS = 30105,
            /// <summary>
            /// 도어락이 강제로 해제되었습니다
            /// </summary>
            DO_MAIN_ANY_DOOR_FORECE_RELEASED_X02A = 30106,
            //////////////////////////////////////////////////////////////////////////
            // ETC
            //////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// 설비에서 무언 정지가 발생 했습니다
            /// </summary>
            EF_PREFORM4_MAIN_EQUIPMENT_SILENT_STOP = 40000,
            /// <summary>
            /// 투입 셔틀 셀의 손상 유무를 확인 하세요
            /// </summary>
            EF_IN_SHUTTLE_CELL_DAMAGED_CHECK = 40001,
            /// <summary>
            /// IN로봇 셀의 손상 유무를 확인 하세요
            /// </summary>
            EF_IN_ROBOT_CELL_DAMAGED_CHECK = 40002,
            /// <summary>
            /// 검사 스테이지 셀의 손상 유무를 확인 하세요
            /// </summary>
            EF_INSP_STAGE_CELL_DAMAGED_CHECK = 40003,
            /// <summary>
            /// OUT로봇 셀의 손상 유무를 확인 하세요
            /// </summary>
            EF_OUT_ROBOT_CELL_DAMAGED_CHECK = 40004,
            /// <summary>
            /// OUT플립 셀의 손상 유무를 확인 하세요
            /// </summary>
            EF_OUT_FLIP_CELL_DAMAGED_CHECK = 40005,
        }

        /// <summary>
        /// User Message 리스트
        /// </summary>
        public enum EMessageList
        {
            /// <summary>
            /// 시작하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_START = 90000,
            /// <summary>
            /// 프로그램을 종료하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_EXIT_THE_PROGRAM = 90001,
            /// <summary>
            /// Cell Out 리스트에 등록 하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_REGISTER_IN_THE_CELL_OUT_LIST = 90002,
            /// <summary>
            /// Cell Out 리스트에 삭제 하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_DELETE_IT_FROM_THE_CELL_OUT_LIST = 90003,
            /// <summary>
            /// 해당 스테이지에서 CELL을 제거해 주세요.
            /// </summary>
            REMOVE_THE_CELL_FROM_THE_STAGE = 90004,
            /// <summary>
            /// 해당 스테이지에서 TRAY를 제거해 주세요.
            /// </summary>
            REMOVE_THE_TRAY_FROM_THE_STAGE = 90005,
            /// <summary>
            /// Cell Out을 진행하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_RUN_CELL_OUT = 90006,
            /// <summary>
            /// 아이디가 없습니다.
            /// </summary>
            THERE_IS_NOT_ID = 90007,
            /// <summary>
            /// 비밀번호가 없습니다.
            /// </summary>
            THERE_IS_NOT_PASSWORD = 90008,
            /// <summary>
            /// 유저 아이디가 일치하지 않습니다.
            /// </summary>
            USER_ID_DO_NOT_MATCH = 90009,
            /// <summary>
            /// 로그인 정보가 일치하지 않습니다.
            /// </summary>
            THE_LOGIN_INFORMATION_DOES_NOT_MATCH = 90010,
            /// <summary>
            /// PPID가 없습니다.
            /// </summary>
            THERE_IS_NOT_PPID = 90011,
            /// <summary>
            /// PPID가 중복됩니다.
            /// </summary>
            THE_PPID_IS_DUPLICATED = 90012,
            /// <summary>
            /// PPID 번호가 중복됩니다.
            /// </summary>
            THE_PPID_NUMBER_IS_DUPLICATED = 90013,
            /// <summary>
            /// 아직 수정된 결과를 커밋하지 않았습니다. 커밋하시겠습니까? (아니요를 누르면 롤백)
            /// </summary>
            YOU_HAVE_NOT_COMMITTED_THE_RESULTS_YET_DO_YOU_WANT_TO_COMMIT = 90014,
            /// <summary>
            /// 배출 택타임 최대 제한에 걸립니다.
            /// </summary>
            OUTPUT_TACT_TIME_TAKES_THE_MAXIMUM_LIMIT = 90015,
            /// <summary>
            /// 저장이 완료되었습니다.
            /// </summary>
            SAVING_IS_COMPLETE = 90016,
            /// <summary>
            /// 불러오기가 완료되었습니다.
            /// </summary>
            LOADING_IS_COMPLETE = 90017,
            /// <summary>
            /// 설비 알람 상태를 확인하여 주십시오.
            /// </summary>
            CHECK_THE_EQUIPMENT_ALARM_STATUS = 90018,
            /// <summary>
            /// 설비가 정지 상태가 아니면 도어창에 접근할 수 없습니다.
            /// </summary>
            YOU_CAN_NOT_ACCESS_THE_DOOR_DIALOG_UNLESS_THE_EQUIPMENT_IS_STOPPED = 90019,
            /// <summary>
            /// 레시피를 저장하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_SAVE_THE_RECIPE = 90020,
            /// <summary>
            /// 레시피를 불러오시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_LOAD_THE_RECIPE = 90021,
            /// <summary>
            /// 레시피를 삭제하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_DELETE_THE_RECIPE = 90022,
            /// <summary>
            /// 레시피를 생성하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_CREATE_THE_RECIPE = 90023,
            /// <summary>
            /// 오프셋 데이터를 저장하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_SAVE_THE_OFFSET_DATA = 90024,
            /// <summary>
            /// 오프셋 데이터를 불러오시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_LOAD_THE_OFFSET_DATA = 90025,
            /// <summary>
            /// 오프셋 데이터를 삭제하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_DELETE_THE_OFFSET_DATA = 90026,
            /// <summary>
            /// 오프셋 데이터를 생성하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_CREATE_THE_OFFSET_DATA = 90027,
            /// <summary>
            /// 로깅 시간은 01 ~ 99분 사이로 설정해야 합니다.
            /// </summary>
            LOADING_TIME_01_99_RANGE_SET = 90028,
            /// <summary>
            /// 로깅 스캔 시간은 01 ~ 60초 사이로 설정해야 합니다.
            /// </summary>
            LOADING_SCAN_TIME_01_60_RANGE_SET = 90029,
            /// <summary>
            /// 로그 보관 기간은 30 ~ 365일 사이로 설정해야 합니다.
            /// </summary>
            LOG_DELETE_PERIOD_30_365_RANGE_SET = 90030,
            /// <summary>
            /// 문을 잠글 수 없습니다. 설비 내부에 작업자가 있는지 확인해주세요.
            /// </summary>
            LOCK_DOOR_FAIL_CHECK_INSIDE = 90031,
            /// <summary>
            /// 설비 도어를 잠그고 세이프티 키 상태를 오토로 변경해 주십시오.
            /// </summary>
            LOCK_THE_EQUIPMENT_DOOR_AND_CHANGE_THE_SAFETY_KEY_STATUS_TO_AUTO = 90032,
            /// <summary>
            /// 설비 도어를 잠그고 세이프티 키 상태를 티치로 변경해 주십시오.
            /// </summary>
            LOCK_THE_EQUIPMENT_DOOR_AND_CHANGE_THE_SAFETY_KEY_STATUS_TO_TEACH = 90033,
            /// <summary>
            /// 정면 세이프티 키 모드 AUTO가 아닙니다.
            /// </summary>
            FRONT_SAFETY_KEY_IS_NOT_AUTO_MODE = 90034,
            /// <summary>
            /// 후면 세이프티 키 모드 AUTO가 아닙니다.
            /// </summary>
            REAR_SAFETY_KEY_IS_NOT_AUTO_MODE = 90035,
            /// <summary>
            /// 정면 세이프티 키 모드 TEACH가 아닙니다.
            /// </summary>
            FRONT_SAFETY_KEY_MODE_IS_NOT_TEACH = 90036,
            /// <summary>
            /// 후면 세이프티 키 모드 TEACH가 아닙니다.
            /// </summary>
            REAR_SAFETY_KEY_MODE_IS_NOT_TEACH = 90037,
            /// <summary>
            /// 세이프티 키 모드 AUTO가 아닙니다.
            /// </summary>
            SAFETY_KEY_IS_NOT_AUTO_MODE = 90038,
            /// <summary>
            /// 세이프티 키 모드 TEACH가 아닙니다.
            /// </summary>
            SAFETY_KEY_MODE_IS_NOT_TEACH = 90039,
            /// <summary>
            /// 설비 자동 운전 상태 입니다.
            /// </summary>
            MACHINE_IS_AUTO_RUN_STATUS = 90040,
            /// <summary>
            /// 메인 진공 공압 센서가 감지 되지 않았습니다.
            /// </summary>
            MAIN_VACUUM_AIR_SENSOR_NOT_DETECT = 90041,
            /// <summary>
            /// 메인 에어 공압 센서가 감지 되지 않았습니다.
            /// </summary>
            MAIN_AIR_SENSOR_NOT_DETECT = 90042,
            /// <summary>
            /// 전면 부 비상 정지 상태 입니다.
            /// </summary>
            FRONT_SUB_EMG_STOP_STATE = 90043,
            /// <summary>
            /// 후면 부 비상 정지 상태 입니다.
            /// </summary>
            REAR_SUB_EMG_STOP_STATE = 90044,
            /// <summary>
            /// 설비 메인 MC OFF 상태 입니다.
            /// </summary>
            MACHINE_MAIN_MC_OFF_STATE = 90045,
            /// <summary>
            /// 설비 메인 FIRST CP OFF 상태 입니다.
            /// </summary>
            MACHINE_FIRST_CP_OFF_STATE = 90046,
            /// <summary>
            /// 설비 메인 SECOND CP OFF 상태 입니다.
            /// </summary>
            MACHINE_SECOND_CP_OFF_STATE = 90047,
            /// <summary>
            /// 설비 전장 박스 온도 알람 상태 입니다.
            /// </summary>
            MACHINE_BOX_TEMPETURE_ALARM_STATE = 90048,
            /// <summary>
            /// 설비 연기 감지 상태 입니다.
            /// </summary>
            MACHINE_SMOKE_DETECT_STATE = 90049,
            /// <summary>
            /// 광학 쿨링 공압 센서가 감지 되지 않았습니다.
            /// </summary>
            OPTIAL_COOLING_AIR_SENSOR_NOT_DETECT = 90050,
            /// <summary>
            /// 설비 SAFETY_PLC_MODE OFF 상태가 아닙니다.
            /// </summary>
            EQP_SAFTTY_PLC_MODE_STATUS_IS_NOT_OFF = 90051,
            /// <summary>
            /// 설비 SAFETY_PLC_SIGNAL_NORMAL ON 상태가 아닙니다.
            /// </summary>
            EQP_SAFTTY_PLC_SIGNAL_NORMAL_STATUS_IS_NOT_ON = 90052,
            /// <summary>
            /// 설비 도어가 열려 있습니다.
            /// </summary>
            EQP_DOOR_IS_OPEN = 90053,
            /// <summary>
            /// 저장하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_SAVE = 90054,
            /// <summary>
            /// 불러오시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_LOAD = 90055,
            /// <summary>
            /// LOT END 실행 하겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_LOT_END = 90056,
            /// <summary>
            /// 라이트 커튼 센서가 감지되어 인터락이 발생했습니다.
            /// </summary>
            IN_SHUTTLE_LIGHT_CURTAIN_DETECT_INTRLOCK = 90057,
            /// <summary>
            /// MAIN FAN 1이 미작동 중입니다.
            /// </summary>
            MACHINE_X_MAIN_FAN_1_ALARM = 90058,
            /// <summary>
            /// MAIN FAN 2이 미작동 중입니다.
            /// </summary>
            MACHINE_X_MAIN_FAN_2_ALARM = 90059,
            /// <summary>
            /// MAIN FAN 3이 미작동 중입니다.
            /// </summary>
            MACHINE_X_MAIN_FAN_3_ALARM = 90060,
            /// <summary>
            /// MAIN FAN 4이 미작동 중입니다.
            /// </summary>
            MACHINE_X_MAIN_FAN_4_ALARM = 90061,
            /// <summary>
            /// SUB FAN 1이 미작동 중입니다.
            /// </summary>
            MACHINE_X_SUB_FAN_1_ALARM = 90062,
            /// <summary>
            /// SUB FAN 2이 미작동 중입니다.
            /// </summary>
            MACHINE_X_SUB_FAN_2_ALARM = 90063,
            /// <summary>
            /// SUB FAN 3이 미작동 중입니다.
            /// </summary>
            MACHINE_X_SUB_FAN_3_ALARM = 90064,
            /// <summary>
            /// SUB FAN 4이 미작동 중입니다.
            /// </summary>
            MACHINE_X_SUB_FAN_4_ALARM = 90065,
            /// <summary>
            /// [{0}] 이 미작동 중입니다.
            /// </summary>
            MACHINE_FAN_ALARM = 90066,
            /// <summary>
            /// EFU 가동중이 아닙니다.
            /// </summary>
            MACHINE_EFU_NOT_RUN = 90067,
            /// <summary>
            /// 실내등이 켜져 있습니다.
            /// </summary>
            INSIDE_LAMP_IS_ON = 90068,
            /// <summary>
            /// 얼라인 데이터를 클리어 하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_ALIGN_DATA_CLEAR = 90069,
            /// <summary>
            /// 출력 신호 변경 잠금 상태입니다.
            /// </summary>
            OUTPUT_SIGNAL_CHANGE_LOCK_STATUS = 90070,
            /// <summary>
            /// 레시피 변경 사항이 없습니다.
            /// </summary>
            THERE_ARE_NO_CHANGES_TO_THE_RECIPE = 90071,
            /// <summary>
            /// 오토 켈리브레이션 모드 진입에 실패 했습니다.
            /// </summary>
            FAILED_TO_ENTER_AUTO_CALIBRATION_MODE = 90072,
            /// <summary>
            /// 오토 켈리브레이션에 실패 했습니다.
            /// </summary>
            AUTO_CALIBRATION_FAILED = 90073,
            /// <summary>
            /// 오토 켈리브레이션 진행중에는 변경 할 수 없습니다.
            /// </summary>
            IT_CANNOT_BE_CHANGED_WHILE_AUTO_CALIBRATION_IS_IN_PROGRESS = 90074,
            /// <summary>
            /// 로봇 쪽 신호가 먼저 준비 되어야 진행 할 수 있습니다.
            /// </summary>
            THE_ROBOT_SIDE_SIGNAL_MUST_BE_PREPARED_FIRST_TO_PROCEED = 90075,
            /// <summary>
            /// 로봇 툴에 셀이 없습니다. 로봇 툴에 셀을 로드 한 뒤 다시 시도해 주세요.
            /// </summary>
            THERE_IS_NO_CELL_IN_THE_ROBOT_TOOL_LOAD_THE_CELL_INTO_THE_ROBOT_TOOL_AND_TRY_AGAIN = 90076,
            /// <summary>
            /// 로봇 위치가 원점이 아닙니다. 로봇 초기화를 실행 한 뒤 다시 시도해 주세요.
            /// </summary>
            THE_ROBOT_POSITION_IS_NOT_THE_ORIGIN_PLEASE_INITIALIZE_THE_ROBOT_AND_TRY_AGAIN = 90077,
            /// <summary>
            /// 설비 내부에 셀이 존재하면 옵션을 변경 할 수 없습니다. 설비 내부에 셀을 제거 한 뒤 다시 시도해 주세요.
            /// </summary>
            IF_A_CELL_EXISTS_INSIDE_THE_MACHINE_THE_OPTION_CANNOT_BE_CHANGED_REMOVE_THE_CELLS_INSIDE_THE_MACHINE_AND_TRY_AGAIN = 90078,
            /// <summary>
            /// 해당 유닛에 셀 정보가 존재하여 배치 동작을 실행 할 수 없습니다.
            /// </summary>
            THE_BATCH_MOVE_CANNOT_BE_PERFORMED_BECAUSE_CELL_INFORMATION_EXISTS_IN_THE_UNIT = 90079,
            /// <summary>
            /// 데이터가 변경 되었습니다. 저장 하시겠습니까?
            /// </summary>
            YOUR_DATA_HAS_BEEN_CHANGED_DO_YOU_WANT_TO_SAVE_THE_CHANGES = 90080,
            /// <summary>
            /// 우선 과열 리셋 버튼을 누른 후 알람을 클리어 해주세요.
            /// </summary>
            FIRST_PUSH_RESET_OVER_TEMPERATURE = 90081,
            /// <summary>
            /// 우선 연기 감지 리셋 버튼을 누른 후 알람을 클리어 해주세요.
            /// </summary>
            FIRST_PUSH_RESET_SMOKE_DETECTION = 90082,
            /// <summary>
            /// 이동하시겠습니까?
            /// </summary>
            DO_YOU_WANT_MOVING = 90083,
            /// <summary>
            /// 라이선스 동글키를 감지할 수 없습니다.
            /// </summary>
            LISENCE_KEY_RECOGNITION_FAILED = 90084,
            /// <summary>
            /// 설비 전체 초기화 상태가 아닙니다.
            /// </summary>
            EQUIPMENT_IS_NOT_INITIALIZED = 90085,
            /// <summary>
            /// [{0}] 초기화 상태가 아닙니다.
            /// </summary>
            UNIT_IS_NOT_INITIALIZED_PARAM1 = 90086,
            /// <summary>
            /// [{0}] 잠김 상태가 아닙니다.
            /// </summary>
            DOOR_IS_NOT_LOCKED_PARAM1 = 90087,
            /// <summary>
            /// [{0}] 키를 소지하십시오.
            /// </summary>
            DOOR_CARRY_THE_KEY_PARAM1 = 90088,
            /// <summary>
            /// [{0}] 내부 작업자 확인 후 도어를 잠그십시오.
            /// </summary>
            DOOR_CHECK_THE_INNER_OPERATOR_AND_LOCK_THE_DOOR_PARAM1 = 90089,
            /// <summary>
            /// [{0}] 열림 상태입니다.
            /// </summary>
            DOOR_IS_OPENED_PARAM1 = 90090,
            /// <summary>
            /// [{0}] 키 미감지 상태입니다.
            /// </summary>
            DOOR_KEY_IS_NOT_DETECTED_PARAM1 = 90091,
            /// <summary>
            /// 로봇 오프셋 {0} 데이터 인터락 값을 초과 했습니다.
            /// </summary>
            ROBOT_OFFSET_X_VALUE_IS_EXCEEDS_INTERLOCK_RANGE_PARAM1 = 90092,
            /// <summary>
            /// 나찌 로봇에서 지원하지 않는 조합으로 요청 했습니다.
            /// </summary>
            ROBOT_NOT_SUPPORT_SELECT_COMBINATION_REQUEST = 90093,
            /// <summary>
            /// 셀 데이터와 셀 실물의 개수가 일치하지 않습니다.
            /// </summary>
            CELL_COUNT_MISSMATCH = 90094,
            /// <summary>
            /// 셀 실물 위치와 셀 데이터가 일치하지 않습니다.
            /// </summary>
            CELL_INFORMATION_AND_POSITION_MISSMATCH = 90095,
            /// <summary>
            /// 트레이 실물 위치와 데이터가 일치하지 않습니다.
            /// </summary>
            TRAY_INFORMATION_AND_POSITION_MISSMATCH = 90096,
            /// <summary>
            /// [{0}]이(가) [{1}]과(와) 충돌 위치에 있습니다.
            /// </summary>
            IT_IS_COLLISION_POSITION_BETWEEN_A_AND_B_PARAM2 = 90097,
            /// <summary>
            /// [{0}]이(가) {1} 위치가 아닙니다. [{0}]의 위치를 확인해 주세요.
            /// </summary>
            UNIT_IS_NOT_TARGET_POSITION_PARAM2 = 90098,
            /// <summary>
            /// [{0}]이(가) 서보 알람 상태입니다. 서보 알람을 리셋 해주세요.
            /// </summary>
            UNIT_IS_SERVO_ALARM_PARAM1 = 90099,
            /// <summary>
            /// [{0}]이(가) 서보 오프 상태입니다. 서보 온 상태로 변경해주세요.
            /// </summary>
            UNIT_IS_SERVO_OFF_PARAM1 = 90100,
            /// <summary>
            /// [{0}]이(가) 자동 모드가 아닙니다. 로봇에 상태를 확인해주세요.
            /// </summary>
            ROBOT_IS_NOT_IN_AUTOMATIC_MODE_PARAM1 = 90101,
            /// <summary>
            /// [{0}]이(가) 매뉴얼 모드 진입에 실패했습니다. 로봇에 상태를 확인해주세요.
            /// </summary>
            ROBOT_IS_FAILED_TO_ENTRY_MANUAL_MODE_PARAM1 = 90102,
            /// <summary>
            /// [{0}]이(가) 매뉴얼 모드 해제에 실패했습니다. 로봇에 상태를 확인해주세요.
            /// </summary>
            ROBOT_IS_FAILED_TO_EXIT_MANUAL_MODE_PARAM1 = 90103,
            /// <summary>
            /// [{0}]이(가) 자동 재생 모드가 아닙니다. 로봇에 상태를 확인 후 초기화를 진행해 주세요.
            /// </summary>
            ROBOT_IS_NOT_ENTRY_PLAYBACK_MODE_PARAM1 = 90104,
            /// <summary>
            /// [{0}]이(가) 서보 온 상태가 아닙니다. 로봇에 상태를 확인 후 초기화를 진행해 주세요.
            /// </summary>
            ROBOT_IS_NOT_SERVO_ON_STATUS_PARAM1 = 90105,
            /// <summary>
            /// [{0}]이(가) 알람 발생 상태입니다. 로봇에 상태를 확인 후 초기화를 진행해 주세요.
            /// </summary>
            ROBOT_IS_ALARM_OCCURRED_STATUS_PARAM1 = 90106,
            /// <summary>
            /// [{0}]이(가) 이동중입니다. 로봇이 멈출 때까지 기다려주세요.
            /// </summary>
            ROBOT_IS_KEEP_MOVING_STATUS_PARAM1 = 90107,
            /// <summary>
            /// [{0}]이(가) 서보 배터리 경고가 발생했습니다. 로봇에 서보 배터리를 점검해 주세요.
            /// </summary>
            ROBOT_IS_SERVO_BATTERY_WARNING_OCCURRED_STATUS_PARAM1 = 90108,
            /// <summary>
            /// [{0}]이(가) 비상 정지 상태입니다. 비상 정지 상태를 해제한 후 초기화를 진행해 주세요.
            /// </summary>
            ROBOT_IS_EMERGENCY_STOP_STATUS_PARAM1 = 90109,
            /// <summary>
            /// [{0}]이(가) 매뉴얼 모드 진입 상태가 아닙니다. 로봇을 매뉴얼 모드로 전환 해주세요.
            /// </summary>
            ROBOT_IS_NOT_ENTRY_MANUAL_MODE_STATUS_PARAM1 = 90110,
            /// <summary>
            /// [{0}]이(가) {1} 인터페이스 알람 상태 입니다. 로봇에 상태를 확인 후 초기화를 진행해 주세요.
            /// </summary>
            ROBOT_IS_INTERFACE_ALARM_STATUS_PARAM2 = 90111,
            /// <summary>
            /// [{0}]이(가) 일시 정지 상태 입니다. 로봇에 상태를 확인 후 초기화를 진행해 주세요.
            /// </summary>
            ROBOT_IS_PAUSE_STOP_STATUS_PARAM1 = 90112,
            /// <summary>
            /// [{0}] 연결에 실패 했습니다. 디바이스를 점검해 주세요.
            /// </summary>
            UNIT_CONNECTION_FAILED_PARAM1 = 90113,
            /// <summary>
            /// GPS MC가 꺼져있어 [{0}]을(를) 구동 할 수 없습니다.
            /// </summary>
            CAN_NOT_OPERATE_THE_UNIT_BECAUSE_THE_GPS_MC_IS_OFF = 90114,
            /// <summary>
            /// 설비 [{0}] 내부에 다인수 키가 감지되었습니다. 설비 내부에 키를 확인해 주세요.
            /// </summary>
            DOOR_EQUIPMENT_INSIDE_KEY_IS_DETECTED_PARAM1 = 90115,
            /// <summary>
            /// [{0}] 설비에 인터락 신호가 감지되었습니다. [{0}] 설비를 확인해 주세요.
            /// </summary>
            UNIT_EQUIPMENT_INTERLOCK_SIGNAL_PARAM1 = 90116,
            /// <summary>
            /// [{0}] 설비에 문 열림 신호가 감지되었습니다. [{0}] 설비를 확인해 주세요.
            /// </summary>
            UNIT_EQUIPMENT_DOOROPEN_SIGNAL_PARAM1 = 90117,
            /// <summary>
            /// [{0}] 설비에 비상정지 신호가 감지되었습니다. [{0}] 설비를 확인해 주세요.
            /// </summary>
            UNIT_EQUIPMENT_EMERGENCY_SIGNAL_PARAM1 = 90118,
            /// <summary>
            /// AR 설비의 시작 대기 상태가 완료되지 않았습니다. AR 설비를 시작대기 상태로 만들어 주세요.
            /// </summary>
            REVIEW_EQUIPMENT_NOT_READY = 90119,
            /// <summary>
            /// [{0}] 팬던트를 확인해 주시기 바랍니다. [{0}] 티칭 후 원점 위치에 복귀하지 않아서 명령을 실행 할 수 없습니다.
            /// </summary>
            CHECK_ROBOT_TEACHING_AFTER_HOMMING_PARAM1 = 90120,
            /// <summary>
            /// 현재 설비가 [{0}]이(가) 아니라서 명령을 수행 할 수 없습니다. 설비를 [{0}]로 변경 후 다시 시도해주세요.
            /// </summary>
            EQUIPMENT_CANNOT_EXECUTED_COMMAND_PARAM1 = 90121,
            /// <summary>
            /// [{0}] 라이트 커튼이 뮤팅 상태입니다.
            /// </summary>
            LIGHT_CURTAIN_IS_MUTING_STATE_PARAM1 = 90122,
            /// <summary>
            /// [{0}] 라이트 커튼이 감지 상태입니다.
            /// </summary>
            LIGHT_CURTAIN_IS_DETECTED_PARAM1 = 90123,
            /// <summary>
            /// TC에서 로그인 정보를 받지 못했습니다.
            /// </summary>
            LOGIN_FAILED_TC_TIMEOUT = 90124,
            /// <summary>
            /// TC에서 로그아웃 정보를 받지 못했습니다.
            /// </summary>
            LOGOUT_FAILED_TC_TIMEOUT = 90125,
            /// <summary>
            /// 레시피를 생성 할 수 없습니다. 설비에서 생성하는 PPID는 TT_로 시작해야 합니다.
            /// </summary>
            RECIPE_CREATE_FAILED_PPID_WRONG_FORMAT = 90126,
            /// <summary>
            /// 레시피를 생성 할 수 없습니다. 설비에서 수동으로 생성 할 수 있는 레시피 개수를 초과 했습니다.
            /// </summary>
            RECIPE_CREATE_FAILED_MANUAL_RECIPE_IS_FULL = 90127,
            /// <summary>
            /// 레시피를 편집 할 수 없습니다. TPM Loss Code를 J/C로 선택하여 주세요.
            /// </summary>
            RECIPE_EDIT_FAILED_SELECT_TPM_LOSS_CODE_JC = 90128,
            /// <summary>
            /// 레시피를 편집 할 수 없습니다. 선택된 레시피는 RMS에서 관리하는 영역입니다.
            /// </summary>
            RECIPE_EDIT_FAILED_RMS_ONLY = 90129,
            /// <summary>
            /// 트레이 레시피를 편집 할 수 없습니다. 설비 내부에 트레이 정보가 있습니다.
            /// </summary>
            TRAY_RECIPE_EDIT_FAILED_PARAM1 = 90130,
            /// <summary>
            /// 오프셋 데이터를 복사하시겠습니까?
            /// </summary>
            DO_YOU_WANT_TO_COPY_THE_OFFSET_DATA = 90131,
            /// <summary>
            /// [{0}] - [{1}]에 설정 가능한 위치 값의 범위는 [{2}] ~ [{3}] 입니다. (입력 값: [{4}])
            /// </summary>
            MOTOR_SETTING_POSITION_OVER_RANGE_PARAM5 = 90132,
            /// <summary>
            /// [{0}]이(가) 홈 프로세스 실패 입니다.
            /// </summary>
            UNIT_IS_HOME_PROCESS_FAIL = 90133,
            /// <summary>
            /// [{0}]이(가) 홈 프로세스 타임아웃 입니다.
            /// </summary>
            UNIT_IS_HOME_PROCESS_TIMEOUT = 90134,
            /// <summary>
            /// 사용자가 작업을 취소 했습니다.
            /// </summary>
            USER_CANCEL_OPERATION = 90135,
            /// <summary>
            /// ENABLE 스위치를 GRIP 해주세요.
            /// </summary>
            ENABLE_SWITCH_IS_NOT_DETECT = 90136,
            /// <summary>
            /// [{0}] 로봇 팬던트 EMS 버튼이 눌려있습니다. 확인 후 알람을 클리어 해주세요.
            /// </summary>
            ROBOT_PENDANT_EMS_BUTTON_IS_PRESSED_CHECK_EMS_BUTTON = 90137,
            /// <summary>
            /// [{0}] 로봇 컨트롤러 EMS 버튼이 눌려있습니다. 확인 후 알람을 클리어 해주세요.
            /// </summary>
            ROBOT_CONTROLLER_EMS_BUTTON_IS_PRESSED_CHECK_EMS_BUTTON = 90138,
            /// <summary>
            /// EQ TO EQ 상위설비에서 현재 RUN 가능한 상태가 아닙니다.
            /// </summary>
            EQ_TO_EQ_UPPER_IS_NOT_READY = 90139,
            /// <summary>
            /// EQ TO EQ 하위설비에서 현재 RUN 가능한 상태가 아닙니다.
            /// </summary>
            EQ_TO_EQ_LOWER_IS_NOT_READY = 90140,
            /// <summary>
            /// EQ TO EQ 상위설비가 현재 가동중 입니다.
            /// </summary>
            EQ_TO_EQ_UPPER_RUNNING = 90141,
            /// <summary>
            /// EQ TO EQ 하위설비가 현재 가동중 입니다.
            /// </summary>
            EQ_TO_EQ_LOWER_RUNNING = 90142,
            /// <summary>
            /// [{0}] 동작을 할 수 없습니다. (확인: [{1}])
            /// </summary>
            CAN_NOT_DO_THE_ACTION_PARAM2 = 90143,
            /// <summary>
            /// 현재 설비에서 일부 모터나 로봇을 가상으로 사용하고 있습니다.
            /// </summary>
            USING_VIRTUAL_CONFIG = 90144,
            /// <summary>
            /// 현재 트레이에 작업을 완료했습니다.\n\n트레이를 교체해주세요.
            /// </summary>
            CHANGE_THE_TRAY = 90145,
            /// <summary>
            /// 현재 트레이 포트가 비어있습니다.\n\n트레이를 투입해주세요.
            /// </summary>
            INSERT_THE_TRAY = 90146,
            /// <summary>
            /// 설비 내부에 셀의 총합이 트레이 포켓 개수를 초과했습니다.\n\n트레이를 확인해주세요.
            /// </summary>
            OVERFLOW_THE_TRAY = 90147,
            /// <summary>
            /// 트레이 포트가 UNLOCK상태입니다. 트레이 포트를 확인해주세요.
            /// </summary>
            TRAY_PORT_IS_UNLOCK = 90148,
            /// <summary>
            /// 트레이 포트가 열려있습니다. 트레이 포트를 확인해주세요.
            /// </summary>
            TRAY_PORT_IS_OPEN = 90149,
            /// <summary>
            /// 트레이 포트에 트레이가 감지되지 않았습니다.
            /// </summary>
            TRAY_IS_NOT_DETECT = 90150,
            /// <summary>
            /// 설정 변경 잠금 상태입니다.
            /// </summary>
            SETTING_IS_LOCK = 90151,
            /// <summary>
            /// 하류 설비로 인해 인터페이스가 지연되고 있습니다.\n\n하류 설비를 확인해 주세요. [{0}]
            /// </summary>
            LOWER_EQUIPMENT_INTERFACE_PENDING_PARAM1 = 90152,
            /// <summary>
            /// 하류 설비로 인해 인터페이스가 지연되고 있습니다.\n\n하류 설비의 인터페이스 신호 클리어 상태를 확인해 주세요. [{0}]
            /// </summary>
            LOWER_EQUIPMENT_SIGNAL_CLEAR_TIMEOUT_PARAM1 = 90153,
            /// <summary>
            /// 경고!\n\n[{0}]에 운영 모드가 [{1}]로 설정된 상태입니다.
            /// </summary>
            WARNING_DEVICE_OPERATING_MODE_SET_PARAM2 = 90154,
        }
    }
}
