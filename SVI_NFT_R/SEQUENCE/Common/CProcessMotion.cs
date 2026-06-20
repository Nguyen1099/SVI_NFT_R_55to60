using EqToEq;
using HLDevice;
using HLDevice.Abstract;
using HLDevice.Motor;
using SVI_NFT_R.CellData;
using SVI_NFT_R.EHS;
using SVI_NFT_R.EqToEq.SdvGammaFoldableLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SVI_NFT_R
{
    public class CProcessMotion : CProcessAbstract
    {
        public enum EVacuum
        {
            IN_SHUTTLE_VACUUM_P1 = 0,
            IN_SHUTTLE_VACUUM_P2,
            IN_ROBOT_VACUUM_P1,
            IN_ROBOT_VACUUM_P2,
            INSP_STAGE_VACUUM_P1,
            INSP_STAGE_VACUUM_P2,
            OUT_ROBOT_VACUUM_P1,
            OUT_ROBOT_VACUUM_P2,
            OUT_FLIP_VACUUM_P1_1,
            OUT_FLIP_VACUUM_P1_2,
            OUT_FLIP_VACUUM_P2_1,
            OUT_FLIP_VACUUM_P2_2,
        }

        public enum ECylinder
        {
            IN_ROBOT_TURN_CYLINDER_P1 = 0,
            IN_ROBOT_TURN_CYLINDER_P2,
            OUT_ROBOT_TURN_CYLINDER_P1,
            OUT_ROBOT_TURN_CYLINDER_P2,
        }

        public enum EMotor
        {
            IN_SHUTTLE_X1 = 0,
            INSP_STAGE_Y,
            OUT_FLIP_R1,
            OUT_FLIP_R2,
            OUT_FLIP_Z,
            OUT_CONVEYOR_X2,
            IN_SHUTTLE_X1_TRIGGER_MODULE,
        }

        public enum ERobot
        {
            IN_ROBOT = 0,
            OUT_ROBOT
        }

        public InShuttle InShuttle { get; private set; }
        public InRobot InRobot { get; private set; }
        public InspStage InspStage { get; private set; }
        public OutRobot OutRobot { get; private set; }
        public OutFlip OutFlip { get; private set; }
        public IProcessManagerLoadInterface LoadInterface { get; private set; }
        public TrackOut TrackOut { get; private set; }
        public Dictionary<EMotor, CProcessInterlock> MotorInterlocks { get; private set; } = new Dictionary<EMotor, CProcessInterlock>(Enum.GetNames(typeof(EMotor)).Length);
        public Dictionary<EMotor, CDeviceMotor> m_objMotor { get; private set; }
        public Dictionary<EVacuum, CVacuum> m_objVacuum { get; private set; }
        public Dictionary<ECylinder, CCylinder> m_objCylinder { get; private set; }
        public Dictionary<ERobot, CDeviceNachi> m_objRobot { get; private set; }
        public List<CProcessAbstract> AllMotions { get; private set; } = new List<CProcessAbstract>();
        public bool IsRunningMotorRepeatMove => mMotorRepeatMove != null;
        private readonly object mSyncRootMotorRepeatMove = new object();
        private SubBatchMotorRepeatMove mMotorRepeatMove = null;

        #region Board Index
        private const int BOARD_INDEX_IN_SHUTTLE_X1 = 0;
        private const int MOTOR_INDEX_IN_SHUTTLE_X1 = 0;
        private const int BOARD_INDEX_INSP_STAGE_Y = 0;
        private const int MOTOR_INDEX_INSP_STAGE_Y = 1;
        private const int BOARD_INDEX_OUT_FLIP_R1 = 0;
        private const int MOTOR_INDEX_OUT_FLIP_R1 = 2;
        private const int BOARD_INDEX_OUT_FLIP_R2 = 0;
        private const int MOTOR_INDEX_OUT_FLIP_R2 = 3;
        private const int BOARD_INDEX_OUT_FLIP_Z = 0;
        private const int MOTOR_INDEX_OUT_FLIP_Z = 4;
        private const int BOARD_INDEX_OUT_CONVEYOR_X2 = 0;
        private const int MOTOR_INDEX_OUT_CONVEYOR_X2 = 5;
        private const int BOARD_INDEX_IN_SHUTTLE_X1_TRIGGER_MODULE = 0;
        private const int MOTOR_INDEX_IN_SHUTTLE_X1_TRIGGER_MODULE = 6;
        #endregion

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="document"></param>
        /// <param name="position"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override bool Initialize(CDocument document, int position = 0, params object[] args)
        {
            bool bReturn = false;
            do
            {
                // 알람 구조체 생성
                m_objAlarmStructure = new CDefine.structureAlarmInformation
                {
                    strAlarmObject = typeof(CProcessMotion).Name,
                    strAlarmFunction = MethodBase.GetCurrentMethod().Name
                };

                m_objDocument = document;
                m_objConfig = m_objDocument.m_objConfig;
                m_objIO = m_objDocument.m_objProcessMain.m_objIO;

                // 컴포넌트 생성 및 초기화
                if (
                    false == InitializeMotorComponent()
                    || false == InitializeVacuumComponent()
                    || false == InitializeCylinderComponent()
                    || false == InitializeRobotComponent()
                    )
                {
                    break;
                }
                // 소프트웨어 리미트 설정 모터
                SetSoftwareLimit();

                // 매니저 객체 생성 및 초기화
                InShuttle = new InShuttle();
                InRobot = new InRobot();
                InspStage = new InspStage();
                OutRobot = new OutRobot();
                OutFlip = new OutFlip();
                LoadInterface = new LoadInterface();
                TrackOut = new TrackOut();
                if (false == InShuttle.Initialize(m_objDocument)
                    || false == InRobot.Initialize(m_objDocument)
                    || false == InspStage.Initialize(m_objDocument)
                    || false == OutRobot.Initialize(m_objDocument)
                    || false == OutFlip.Initialize(m_objDocument)
                    || false == LoadInterface.Initialize(m_objDocument, 0)
                    || false == TrackInManager.Initialize(m_objDocument)
                    || false == TrackOut.Initialize(m_objDocument)
                    || false == UnitManager.Initialize(m_objDocument)
                    )
                {
                    break;
                }

                // 초기화 쓰레드 생성
                mThreadInitialize = new Thread(ThreadInitializeProcess);
                mThreadInitialize.Start();

                // 재시작 스레드 생성
                mThreadRestart = new Thread(ThreadRestartProcess);
                mThreadRestart.Start();

                // 부팅 동작 동시 처리
                Parallel.ForEach(AllMotions, motion => motion.BootUpAction());

                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {
            mbThreadExit = true;
            mThreadInitialize.Join();
            mThreadRestart.Join();

            UnitManager.DeInitialize();
            TrackOut.DeInitialize();
            TrackInManager.DeInitialize();
            LoadInterface.DeInitialize();
            OutRobot.DeInitialize();
            OutFlip.DeInitialize();
            InspStage.DeInitialize();
            InRobot.DeInitialize();
            InShuttle.DeInitialize();

            foreach (var motor in m_objMotor)
            {
                if (null != motor.Value)
                {
                    motor.Value.HLDeInitialize();
                }
            }
            foreach (var cylinder in m_objCylinder)
            {
                if (null != cylinder.Value)
                {
                    cylinder.Value.DeInitialize();
                }
            }
            foreach (var vacuum in m_objVacuum)
            {
                if (null != vacuum.Value)
                {
                    vacuum.Value.DeInitialize();
                }
            }
            foreach (var robot in m_objRobot)
            {
                if (null != robot.Value)
                {
                    robot.Value.DeInitialize();
                }
            }
        }

        /// <summary>
        /// 설비 초기화 전 예외 상태 확인 및 메시지 출력(기타등등 체크하여 설비 초기화 전에 메시지 출력)
        /// </summary>
        /// <returns></returns>
        public bool CheckPreConditionBeforeInitialize()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            m_objAlarmStructure.eAlarmLevel = CDefine.EAlarmType.ALARM_INTERLOCK;
            m_objAlarmStructure.iAlarmCode = (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM;

            do
            {
                if (CDefine.ERunMode.RealRun != m_objDocument.GetRunMode())
                {
                    // 가상 모드에서는 초기화시 셀 데이터를 초기화 해준다.
                    foreach (var cell in CellDataManager.Cells.Values)
                    {
                        cell.Data.Reset();
                        cell.IsChanged = true;
                    }
                    // 베큠을 꺼준다.
                    Parallel.ForEach(m_objVacuum.Values, vacuum =>
                    {
                        vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
                    });
                }
                // 인터페이스 신호를 초기화 한다
                LoadInterface.DryRunSignalClear();
                bReturn = true;
            } while (false);

            if (m_objAlarmStructure.iAlarmCode != (int)CAlarmDefine.EAlarmList.NOT_REGISTED_ALARM)
            {
                m_objDocument.SetAlarmEvent(m_objAlarmStructure);
            }
            return bReturn;
        }

        /// <summary>
        /// 설비 돌리기 전 예외 상태 확인 및 메시지 출력(데이터 불일치 및 기타등등 체크하여 설비 런 전에 메시지 출력)
        /// </summary>
        /// <returns></returns>
        public bool CheckPreConditionBeforeRun()
        {
            // 1차 필터링 (진공이 있는 구간에 셀 카운트 비교)
            {
                var vacuumStatus = Enum.GetValues(typeof(EVacuum))
                        .Cast<EVacuum>()
                        .Where(v => !(v.ToString().StartsWith("INSP_STAGE") && v.ToString().EndsWith("_1")))
                        .ToDictionary(key => key, value => m_objVacuum[value].Status);
                int vacuumCellCount = CellDataManager.ProcessCells[EProcess.InShuttle].GetExistCellCount()
                       + CellDataManager.ProcessCells[EProcess.InRobot].GetExistCellCount()
                       + CellDataManager.ProcessCells[EProcess.InspStage].GetExistCellCount()
                       + CellDataManager.ProcessCells[EProcess.OutRobot].GetExistCellCount()
                       + CellDataManager.ProcessCells[EProcess.OutFlip].GetExistCellCount();
                int vacuumOnCount = vacuumStatus.Count(item => item.Value == CVacuumAbstract.EVacuumStatus.STS_ON);

                // Cell 개수와 Vacuum 개수 체크
                if (
                    m_objDocument.IsManualInputMode == false
                    && vacuumCellCount != vacuumOnCount
                    )
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.CELL_COUNT_MISSMATCH);
                    return false;
                }
            }

            // 2차 필터링
            {
                // 최종적으로 Vacuum Sensor가 OFF면 Restart 할 때 OFF 해 준다
                {
                    Parallel.ForEach(m_objVacuum.Values, vacuum =>
                    {
                        if (vacuum.Status == CVacuumAbstract.EVacuumStatus.STS_ON)
                        {
                            return;
                        }

                        vacuum.SetVacuumCommand(CVacuumAbstract.EVacuumCommand.CMD_OFF, CVacuumAbstract.ESensorCheck.IGNORE);
                    });
                }
            }
            return true;
        }

        /// <summary>
        /// 모터 알람 및 홈 상태 확인 (서보 알람이나, 홈이 완료되지 않았으면 메시지 출력)
        /// </summary>
        /// <returns>설비 동작 전 체크 true = 정상, false = 알람</returns>
        public bool CheckServoStatus()
        {
            var checkItems = new[]
            {
                new { ProcessIndex = EProcess.InShuttle, IsAvaliable = new Func<bool>(() => IsMotorAvaliable(m_objMotor[EMotor.IN_SHUTTLE_X1])) },
                new { ProcessIndex = EProcess.InRobot, IsAvaliable = new Func<bool>(() => IsRobotAvaliable(m_objRobot[ERobot.IN_ROBOT])) },
                new { ProcessIndex = EProcess.InspStage, IsAvaliable = new Func<bool>(() => IsMotorAvaliable(m_objMotor[EMotor.INSP_STAGE_Y])) },
                new { ProcessIndex = EProcess.OutRobot, IsAvaliable = new Func<bool>(() => IsRobotAvaliable(m_objRobot[ERobot.OUT_ROBOT])) },
                new { ProcessIndex = EProcess.OutFlip, IsAvaliable = new Func<bool>(() => IsMotorAvaliable(m_objMotor[EMotor.OUT_FLIP_R1])) },
                new { ProcessIndex = EProcess.OutFlip, IsAvaliable = new Func<bool>(() => IsMotorAvaliable(m_objMotor[EMotor.OUT_FLIP_R2])) },
                new { ProcessIndex = EProcess.OutFlip, IsAvaliable = new Func<bool>(() => IsMotorAvaliable(m_objMotor[EMotor.OUT_FLIP_Z])) },
                new { ProcessIndex = EProcess.OutFlip, IsAvaliable = new Func<bool>(() => IsMotorAvaliable(m_objMotor[EMotor.OUT_CONVEYOR_X2])) },
            };

            foreach (var item in checkItems)
            {
                if (item.IsAvaliable.Invoke() == false)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_NOT_INITIALIZED_PARAM1, item.ProcessIndex);
                    return false;
                }
            }

            return true;
        }

        public Task<bool> MotorRepeatMoveRun(CProcessInterlock interlock, EMotor motorIndex, int repeatCount, int[] positions)
        {
            return Task.Run(() =>
            {
                lock (mSyncRootMotorRepeatMove)
                {
                    if (mMotorRepeatMove != null)
                    {
                        return false;
                    }
                    mMotorRepeatMove = new SubBatchMotorRepeatMove(m_objDocument, interlock);
                }
                try
                {
                    return mMotorRepeatMove.Execute(motorIndex, repeatCount, positions);
                }
                finally
                {
                    mMotorRepeatMove = null;
                }
            });
        }

        /// <summary>
        /// 현재 설비가 이동중인지 확인
        /// </summary>
        /// <returns>true = 무빙 , false = 정지</returns>
        public bool CheckMovingStatus()
        {
            foreach (var motor in m_objMotor.Values)
            {
                var motorStatus = motor.HLGetMotorStatus();
                if (
                    true == motorStatus.bAlarm
                    || true == motorStatus.bInposition
                    || false == motorStatus.bServo
                    )
                {
                    continue;
                }
                if (motor == m_objMotor[EMotor.OUT_CONVEYOR_X2])
                {
                    // 컨베이어 모터는 제외한다.
                    continue;
                }
                // 이동중인 모터가 하나라도 있으면 true
                return true;
            }
            foreach (var robot in m_objRobot.Values)
            {
                if (
                    true == robot.Status.IsFailure
                    || false == robot.Status.IsBusy
                    || false == robot.Status.IsMotorEnergized
                    )
                {
                    continue;
                }
                // 이동중인 로봇이 하나라도 있으면 true
                return true;
            }
            // 모든 모터가 정지중이면 false
            return false;
        }

        /// <summary>
        /// 모든 시퀀스 매니져중에 배치 동작이 진행중인 매니져가 있는지 확인함
        /// </summary>
        /// <returns>true=배치 동작중인 매니져가 있음, false=모든 매니져가 배치 동작중이 아님</returns>
        public bool CheckAllSequenceManagerStateIsBatchMoving()
        {
            bool bReturn = false;

            do
            {
                if (
                    0 == InShuttle.BatchCommand
                    && 0 == InRobot.BatchCommand
                    && 0 == InspStage.BatchCommand
                    && 0 == OutRobot.BatchCommand
                    && 0 == OutFlip.BatchCommand
                    && mMotorRepeatMove == null
                    )
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 설비 및 매니져 초기화 상태 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckInitialize()
        {
            //  설비 전체 초기화 상태 확인
            if (GetInitializeStatus() != EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE)
            {
                m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.EQUIPMENT_IS_NOT_INITIALIZED);
                return false;
            }

            var allProcesses = new[]
            {
                new { Process = (CProcessAbstract)InShuttle, AlarmCode = EProcess.InShuttle.ToString() },
                new { Process = (CProcessAbstract)InRobot, AlarmCode = EProcess.InRobot.ToString() },
                new { Process = (CProcessAbstract)InspStage, AlarmCode = EProcess.InspStage.ToString() },
                new { Process = (CProcessAbstract)OutRobot, AlarmCode = EProcess.OutRobot.ToString() },
                new { Process = (CProcessAbstract)OutFlip, AlarmCode = EProcess.OutFlip.ToString() }
            };
            foreach (var item in allProcesses)
            {
                if (item.Process.GetInitializeStatus() != EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE)
                {
                    m_objDocument.SetMessage(CDefine.EAlarmType.ALARM_INTERLOCK, CAlarmDefine.EMessageList.UNIT_IS_NOT_INITIALIZED_PARAM1, item.AlarmCode);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 설비 및 매니져 초기화 상태만 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckInitializeStatus()
        {
            //  설비 전체 초기화 상태 확인
            if (GetInitializeStatus() != EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE)
            {
                return false;
            }

            var allProcesses = new[]
            {
                new { Process = (CProcessAbstract)InShuttle },
                new { Process = (CProcessAbstract)InRobot },
                new { Process = (CProcessAbstract)InspStage },
                new { Process = (CProcessAbstract)OutRobot },
                new { Process = (CProcessAbstract)OutFlip }
            };
            foreach (var item in allProcesses)
            {
                if (item.Process.GetInitializeStatus() != EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 모든 시퀀스 매니져의 상태가 IDLE인지 확인함
        /// </summary>
        /// <returns>true=모두 IDLE상태, false=동작중</returns>
        public bool CheckAllSequenceManagerStateIsIdle()
        {
            bool bResult = false;

            do
            {
                // LOAD INTERFACE
                if (
                    LoadInterface.IsInitialized == true
                    && LoadInterface.IsHandShaking == true
                    )
                {
                    break;
                }

                // IN SHUTTLE
                if (InShuttle.IsIdle == false)
                {
                    break;
                }

                // IN ROBOT
                if (InRobot.IsIdle == false)
                {
                    break;
                }

                // INSP STAGE
                if (InspStage.IsIdle == false)
                {
                    break;
                }

                // OUT ROBOT
                if (OutRobot.IsIdle == false)
                {
                    break;
                }

                // OUT FLIP
                if (OutFlip.IsIdle == false)
                {
                    break;
                }

                // Track In/Out
                if (TrackInManager.IsBusy == true
                    || TrackOut.IsBusy == true
                    )
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 모터 티칭 정보 업데이트 (모델이 변경 될 경우 호출 한다.)
        /// </summary>
        /// <returns></returns>
        public bool LoadTeachParameter()
        {
            bool bReturn = false;
            string strFilePath = m_objConfig.GetModelPath();
            string strModelName = m_objConfig.GetSystemParameter().strPPID;
            do
            {
                // 모터 운용 파라미터 로드
                foreach (var motor in m_objMotor.Values)
                {
                    // 새로운 모델 이름 및 경로 설정
                    var param = motor.HLGetMotorInitializeParameter();
                    param.strFilePath = strFilePath;
                    param.strModelName = strModelName;
                    motor.HLSetMotorInitializeParameter(param);

                    // 파라메터 로드
                    motor.HLLoadMotorOperationParameter("", "");
                    motor.HLLoadMotorPositionParameter("", "");
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 진공 옵션 적용 (모델이 변경 될 경우 호출 한다.)
        /// </summary>
        public void ApplyVacuumOptions()
        {
        }

        /// <summary>
        /// 모터 소프트 웨어 리미트 설정
        /// </summary>
        /// <returns></returns>
        public bool SetSoftwareLimit()
        {
            foreach (var item in m_objMotor)
            {
                // 컨베이어 축같은 경우 SW Limit를 걸면 에러가 발생함으로 예외처리한다
                // { 없음 }
                if (false == item.Value.HLSetSoftwareLimit(CDeviceMotorAbstract.EUse.ENABLE))
                {
                    //return false;
                }
            }
            return true;
        }

        private bool IsMotorAvaliable(CDeviceMotor motor)
        {
            if (motor.HLGetMotorStatus().bAlarm == true
                || motor.HLGetMotorStatus().bServo == false
                || (motor.HLGetMotorStatus().bHome == false && motor.HLGetMotorOperationParameter().bUseHome == true)
                )
            {
                return false;
            }
            return true;
        }

        private bool IsRobotAvaliable(CDeviceNachi robot)
        {
            if (robot.Status.IsFailure == true)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 모터 객체 생성 및 초기화
        /// </summary>
        /// <returns>초기화 결과</returns>
        private bool InitializeMotorComponent()
        {
            bool bResult = true;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            CDeviceMotorAbstract.CMotorInitializeParameter objMotorInitialize = null;
            do
            {
                m_objMotor = new Dictionary<EMotor, CDeviceMotor>();
                foreach (EMotor index in Enum.GetValues(typeof(EMotor)))
                {
                    CDeviceMotor objMotor = null;
                    objMotorInitialize = GetMotorInitializeParameter(index);
                    if (null != objMotorInitialize)
                    {
                        if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objConfig.GetSystemParameter().eSimulationMode)
                        {
                            objMotor = new CDeviceMotor(new CDeviceMotorVirtual());
                        }
                        else
                        {
                            switch (index)
                            {
                                case EMotor.IN_SHUTTLE_X1:
                                case EMotor.INSP_STAGE_Y:
                                case EMotor.OUT_FLIP_R1:
                                case EMotor.OUT_FLIP_R2:
                                case EMotor.OUT_FLIP_Z:
                                case EMotor.OUT_CONVEYOR_X2:
                                case EMotor.IN_SHUTTLE_X1_TRIGGER_MODULE:
                                    objMotor = new CDeviceMotor(new CDeviceMotorAjin());
                                    break;
                                default:
                                    // 지정하지 않은 객체는 생성되지 않음으로 다음 루틴에서 NullReferenceException이 발생한다
                                    break;
                            }
                        }
                        // 모터 가상 모드 적용
                        if (objMotor != null
                            && m_objConfig.IsVirtualMotor(index) == true
                            )
                        {
                            objMotor = new CDeviceMotor(new CDeviceMotorVirtual());
                        }
                        if (false == objMotor.HLInitialize(objMotorInitialize))
                        {
                            Debug.Assert(false, string.Format("{0} 모터 객체 초기화 실패! ", index.ToString()));
                            bResult = false;
                        }
                    }
                    m_objMotor[index] = objMotor;
                }
            } while (false);
            return bResult;
        }

        /// <summary>
        /// 입력한 모터의 초기화 파라메터를 불러온다
        /// </summary>
        /// <param name="index">모터 인덱스</param>
        /// <returns>null=사용할 수 없는 모터, other=성공</returns>
        private CDeviceMotorAbstract.CMotorInitializeParameter GetMotorInitializeParameter(EMotor index)
        {
            var objInitialize = new CDeviceMotorAbstract.CMotorInitializeParameter
            {
                strFilePath = m_objConfig.GetModelPath(),
                strModelName = m_objConfig.GetSystemParameter().strPPID,
                strMotorName = index.ToString()
            };

            switch (index)
            {
                case EMotor.IN_SHUTTLE_X1:
                    objInitialize.iBoardNo = BOARD_INDEX_IN_SHUTTLE_X1;
                    objInitialize.iAxisNo = MOTOR_INDEX_IN_SHUTTLE_X1;
                    break;

                case EMotor.INSP_STAGE_Y:
                    objInitialize.iBoardNo = BOARD_INDEX_INSP_STAGE_Y;
                    objInitialize.iAxisNo = MOTOR_INDEX_INSP_STAGE_Y;
                    break;

                case EMotor.OUT_FLIP_R1:
                    objInitialize.iBoardNo = BOARD_INDEX_OUT_FLIP_R1;
                    objInitialize.iAxisNo = MOTOR_INDEX_OUT_FLIP_R1;
                    break;

                case EMotor.OUT_FLIP_R2:
                    objInitialize.iBoardNo = BOARD_INDEX_OUT_FLIP_R2;
                    objInitialize.iAxisNo = MOTOR_INDEX_OUT_FLIP_R2;
                    break;

                case EMotor.OUT_FLIP_Z:
                    objInitialize.iBoardNo = BOARD_INDEX_OUT_FLIP_Z;
                    objInitialize.iAxisNo = MOTOR_INDEX_OUT_FLIP_Z;
                    break;

                case EMotor.OUT_CONVEYOR_X2:
                    objInitialize.iBoardNo = BOARD_INDEX_OUT_CONVEYOR_X2;
                    objInitialize.iAxisNo = MOTOR_INDEX_OUT_CONVEYOR_X2;
                    break;

                case EMotor.IN_SHUTTLE_X1_TRIGGER_MODULE:
                    objInitialize.iBoardNo = BOARD_INDEX_IN_SHUTTLE_X1_TRIGGER_MODULE;
                    objInitialize.iAxisNo = MOTOR_INDEX_IN_SHUTTLE_X1_TRIGGER_MODULE;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            return objInitialize;
        }

        /// <summary>
        /// 진공 객체 생성 및 초기화
        /// </summary>
        /// <returns></returns>
        private bool InitializeVacuumComponent()
        {
            bool bReturn = true;
            CVacuumAbstract.CVacuumInitializeParameter objVacuumInitialize = null;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            do
            {
                m_objVacuum = new Dictionary<EVacuum, CVacuum>();
                foreach (EVacuum index in Enum.GetValues(typeof(EVacuum)))
                {
                    CVacuum objVacuum = null;
                    objVacuumInitialize = GetVacuumInitiliazeParameter(index);
                    if (null != objVacuumInitialize)
                    {
                        objVacuum = new CVacuum(m_objDocument);
                        if (false == objVacuum.Initialize(m_objIO, objVacuumInitialize))
                        {
                            Debug.Assert(false, string.Format("{0} 베큠 객체 초기화 실패!", index.ToString()));
                            bReturn = false;
                        }
                    }
                    m_objVacuum[index] = objVacuum;
                }
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 입력한 베큠의 초기화 파라메터를 불러온다
        /// </summary>
        /// <param name="index">베큠 인덱스</param>
        /// <returns>null=사용할 수 없는 베큠,other=성공</returns>
        private CVacuumAbstract.CVacuumInitializeParameter GetVacuumInitiliazeParameter(EVacuum index)
        {
            CVacuumAbstract.CVacuumInitializeParameter objVacuumInitialize = new CVacuumAbstract.CVacuumInitializeParameter
            {
                strFilePath = m_objDocument.m_objConfig.GetModelBasePath(),
                strFileName = CDefine.DEF_VACUUM_DAT,
                strVacuumName = index.ToString()
            };

            switch (index)
            {
                case EVacuum.IN_SHUTTLE_VACUUM_P1:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_SHUTTLE_P1_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_SHUTTLE_P1_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_IN_SHUTTLE_P1_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_IN_SHUTTLE_P1_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.IN_SHUTTLE_VACUUM_P2:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_SHUTTLE_P2_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_SHUTTLE_P2_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_IN_SHUTTLE_P2_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_IN_SHUTTLE_P2_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.IN_ROBOT_VACUUM_P1:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P1_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P1_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P1_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_IN_ROBOT_P1_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.IN_ROBOT_VACUUM_P2:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P2_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P2_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P2_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_IN_ROBOT_P2_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.INSP_STAGE_VACUUM_P1:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_INSP_STAGE_P1_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_INSP_STAGE_P1_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_INSP_STAGE_P1_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_INSP_STAGE_P1_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.INSP_STAGE_VACUUM_P2:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_INSP_STAGE_P2_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_INSP_STAGE_P2_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_INSP_STAGE_P2_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_INSP_STAGE_P2_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.OUT_ROBOT_VACUUM_P1:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P1_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P1_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P1_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_OUT_ROBOT_P1_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.OUT_ROBOT_VACUUM_P2:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P2_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P2_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P2_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_OUT_ROBOT_P2_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.OUT_FLIP_VACUUM_P1_1:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P1_1_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P1_1_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROTATE_P1_1_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P1_1_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.OUT_FLIP_VACUUM_P1_2:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P1_2_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P1_2_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROTATE_P1_2_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P1_2_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.OUT_FLIP_VACUUM_P2_1:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P2_1_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P2_1_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROTATE_P2_1_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P2_1_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                case EVacuum.OUT_FLIP_VACUUM_P2_2:
                    objVacuumInitialize.strVacuumOnOutputIO[(int)CVacuumAbstract.EVacuumOnOutputIO.ON_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P2_2_VACUUM.ToString();
                    objVacuumInitialize.strVacuumBlowOutputIO[(int)CVacuumAbstract.EVacuumOffOutputIO.OFF_OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROTATE_P2_2_BLOW.ToString();
                    objVacuumInitialize.strVacuumInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROTATE_P2_2_VACUUM_PRESSURE_SENSOR.ToString();
                    objVacuumInitialize.strVacuumAnalogInputIO[(int)CVacuumAbstract.EVacuumInputIO.INPUT_IO_1] = CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P2_2_VACUUM_PRESSURE.ToString();
                    objVacuumInitialize.eVacuumSolenoidType = CVacuumAbstract.EVacuumSolenoidType.SINGLE_SOLENOID;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return objVacuumInitialize;
        }

        /// <summary>
        /// 실린더 객체 생성 및 초기화
        /// </summary>
        /// <returns></returns>
        private bool InitializeCylinderComponent()
        {
            bool bReturn = true;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            CCylinderAbstract.CCylinderInitializeParameter objCylinderInitialize = null;

            do
            {
                m_objCylinder = new Dictionary<ECylinder, CCylinder>();
                foreach (ECylinder index in Enum.GetValues(typeof(ECylinder)))
                {
                    CCylinder objCylinder = null;
                    objCylinderInitialize = GetCylinderInitializeParameter(index);
                    if (null != objCylinderInitialize)
                    {
                        objCylinder = new CCylinder(m_objDocument);
                        if (false == objCylinder.Initialize(m_objIO, objCylinderInitialize))
                        {
                            Debug.Assert(false, string.Format("{0} 실린더 객체 초기화 실패!", index.ToString()));
                            bReturn = false;
                        }
                    }
                    m_objCylinder[index] = objCylinder;
                }
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 실린더 초기화 파라메터를 불러온다
        /// </summary>
        /// <param name="index">실린더 인덱스</param>
        /// <returns>null=사용할 수 없는 Cylinder, other=성공</returns>
        private CCylinderAbstract.CCylinderInitializeParameter GetCylinderInitializeParameter(ECylinder index)
        {
            CCylinderAbstract.CCylinderInitializeParameter objCylinderInitialize = new CCylinderAbstract.CCylinderInitializeParameter
            {
                strFilePath = m_objDocument.m_objConfig.GetModelBasePath(),
                strFileName = CDefine.DEF_CYLINDER_DAT,
                strCylinderName = index.ToString()
            };

            switch (index)
            {
                case ECylinder.IN_ROBOT_TURN_CYLINDER_P1:
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P1_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_2] = CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P1_CYLINDER_TURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P1_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_2] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P1_CYLINDER_TURN.ToString();
                    objCylinderInitialize.eCylinderSolenoidType = CCylinderAbstract.ECylinderSolenoidType.DOUBLE_SOLENOID;
                    objCylinderInitialize.eCylinderType = CCylinderAbstract.ECylinderType.ReturnTurn;
                    break;

                case ECylinder.IN_ROBOT_TURN_CYLINDER_P2:
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P2_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_2] = CDeviceIODefine.EDigitalInput.X_IN_ROBOT_P2_CYLINDER_TURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P2_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_2] = CDeviceIODefine.EDigitalOutput.Y_IN_ROBOT_P2_CYLINDER_TURN.ToString();
                    objCylinderInitialize.eCylinderSolenoidType = CCylinderAbstract.ECylinderSolenoidType.DOUBLE_SOLENOID;
                    objCylinderInitialize.eCylinderType = CCylinderAbstract.ECylinderType.ReturnTurn;
                    break;

                case ECylinder.OUT_ROBOT_TURN_CYLINDER_P1:
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P1_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_2] = CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P1_CYLINDER_TURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P1_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_2] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P1_CYLINDER_TURN.ToString();
                    objCylinderInitialize.eCylinderSolenoidType = CCylinderAbstract.ECylinderSolenoidType.DOUBLE_SOLENOID;
                    objCylinderInitialize.eCylinderType = CCylinderAbstract.ECylinderType.ReturnTurn;
                    break;

                case ECylinder.OUT_ROBOT_TURN_CYLINDER_P2:
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_1] = CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P2_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderInputIO[(int)CCylinderAbstract.ECylinderInputIO.INPUT_IO_2] = CDeviceIODefine.EDigitalInput.X_OUT_ROBOT_P2_CYLINDER_TURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_1] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P2_CYLINDER_RETURN.ToString();
                    objCylinderInitialize.strCylinderOutputIO[(int)CCylinderAbstract.ECylinderOutputIO.OUTPUT_IO_2] = CDeviceIODefine.EDigitalOutput.Y_OUT_ROBOT_P2_CYLINDER_TURN.ToString();
                    objCylinderInitialize.eCylinderSolenoidType = CCylinderAbstract.ECylinderSolenoidType.DOUBLE_SOLENOID;
                    objCylinderInitialize.eCylinderType = CCylinderAbstract.ECylinderType.ReturnTurn;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return objCylinderInitialize;
        }

        /// <summary>
        /// 로봇 객체 생성 및 초기화
        /// </summary>
        /// <returns></returns>
        private bool InitializeRobotComponent()
        {
            bool bReturn = false;
            m_objAlarmStructure.strAlarmFunction = MethodBase.GetCurrentMethod().Name;
            RobotInitialOption robotInitialOption;
            CDeviceNachi objRobot;

            do
            {
                m_objRobot = new Dictionary<ERobot, CDeviceNachi>();
                bReturn = true;
                foreach (ERobot index in Enum.GetValues(typeof(ERobot)))
                {
                    objRobot = null;
                    robotInitialOption = GetRobotInitializeParameter(index);
                    if (null != robotInitialOption)
                    {
                        objRobot = new CDeviceNachi();
                        if (false == objRobot.Initialize(m_objDocument, robotInitialOption))
                        {
                            Debug.Assert(false, string.Format("{0} 로봇 객체 초기화 실패!", index.ToString()));
                            bReturn = false;
                        }
                    }
                    m_objRobot[index] = objRobot;
                }
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 로봇 초기화 파라메터를 불러온다
        /// </summary>
        /// <param name="index">로봇 인덱스</param>
        /// <returns>null=사용할 수 없는 Robot, other=성공</returns>
        private RobotInitialOption GetRobotInitializeParameter(ERobot index)
        {
            RobotInitialOption robotInitialOption = new RobotInitialOption();

            switch (index)
            {
                case ERobot.IN_ROBOT:
                    robotInitialOption.LogTarget = CDefine.ELogType.LOG_ROBOT_IN_ROBOT;
                    robotInitialOption.RobotPrefix = "IN";
                    robotInitialOption.OffsetInterface = m_objDocument.m_objProcessMain.m_objNachiInterface[(int)CDefine.ENachiComm.InNachiOffset];
                    robotInitialOption.RmsInterfaceOrNull = m_objDocument.m_objProcessMain.m_objNachiInterface[(int)CDefine.ENachiComm.InNachiRms];
                    robotInitialOption.AllRobotProcess = new DEVICE.Nachi.ERobotProcess[] {
                        DEVICE.Nachi.ERobotProcess.P1,
                        DEVICE.Nachi.ERobotProcess.P3,
                        DEVICE.Nachi.ERobotProcess.P4
                    };
                    break;

                case ERobot.OUT_ROBOT:
                    robotInitialOption.LogTarget = CDefine.ELogType.LOG_ROBOT_OUT_ROBOT;
                    robotInitialOption.RobotPrefix = "OUT";
                    robotInitialOption.OffsetInterface = m_objDocument.m_objProcessMain.m_objNachiInterface[(int)CDefine.ENachiComm.OutNachiOffset];
                    robotInitialOption.RmsInterfaceOrNull = m_objDocument.m_objProcessMain.m_objNachiInterface[(int)CDefine.ENachiComm.OutNachiRms];
                    robotInitialOption.AllRobotProcess = new DEVICE.Nachi.ERobotProcess[] {
                        DEVICE.Nachi.ERobotProcess.P1,
                        DEVICE.Nachi.ERobotProcess.P4
                    };
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            robotInitialOption.IsVirtual = m_objDocument.m_objConfig.IsVirtualRobot(index);
            return robotInitialOption;
        }

        private Task<bool> InitializeProcessAsync(CProcessAbstract processManager, string processName)
        {
            return Task.Factory.StartNew(() =>
            {
                m_objDocument.SetInitializeProcessLog($"`{processName}` Initialize Start");
                processManager.SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_START);
                while (EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_START == processManager.GetInitializeStatus())
                {
                    Thread.Sleep(10);
                }
                if (
                    EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE != processManager.GetInitializeStatus()
                    || CDefine.ERunStatus.Initialize != m_objDocument.GetRunStatus()
                    )
                {
                    m_objDocument.SetInitializeErrorLog($"`{processName}` Initialize Fail");
                    return false;
                }
                m_objDocument.SetInitializeProcessLog($"`{processName}` Initialize Complete");
                return true;
            });
        }

        private Task<bool> RestartProcessAsync(CProcessAbstract processManager)
        {
            return Task.Factory.StartNew(() =>
            {
                processManager.SetRestartCommand(EProcessManagerRestart.PROCESS_MANAGER_RESTART_START);
                while (EProcessManagerRestart.PROCESS_MANAGER_RESTART_START == processManager.GetRestartStatus())
                {
                    Thread.Sleep(10);
                }
                if (
                    EProcessManagerRestart.PROCESS_MANAGER_RESTART_DONE != processManager.GetRestartStatus()
                    || CDefine.ERunStatus.Setup != m_objDocument.GetRunStatus()
                    )
                {
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 모터 초기화 시퀀스 동작 정의
        /// </summary>
        /// <returns>동작 성공 여부</returns>
        private bool DoProcessInitialize()
        {
            // 설비에 모든 모터를 정지시키고 매니져 초기화 상태를 리셋함
            this.EmergencyStopAll();

            // 모든 매니져 초기화 상태 리셋
            var allManagers = new CProcessAbstract[]
            {
                InShuttle,
                InRobot,
                InspStage,
                OutRobot,
                OutFlip,
            };
            foreach (var manager in allManagers)
            {
                manager.SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);
            }

            List<Task<bool>> initializeTasks = new List<Task<bool>>();
            initializeTasks.Clear();
            initializeTasks.Add(InitializeProcessAsync(InRobot, Resource.Get(EProcess.InRobot).ToString()));
            initializeTasks.Add(InitializeProcessAsync(OutRobot, Resource.Get(EProcess.OutRobot).ToString()));
            Task.WaitAll(initializeTasks.ToArray());
            foreach (var task in initializeTasks)
            {
                if (task.Result == false)
                {
                    return false;
                }
            }
            initializeTasks.Clear();
            initializeTasks.Add(InitializeProcessAsync(InShuttle, Resource.Get(EProcess.InShuttle).ToString()));
            initializeTasks.Add(InitializeProcessAsync(InspStage, Resource.Get(EProcess.InspStage).ToString()));
            initializeTasks.Add(InitializeProcessAsync(OutFlip, Resource.Get(EProcess.OutFlip).ToString()));
            Task.WaitAll(initializeTasks.ToArray());
            foreach (var task in initializeTasks)
            {
                if (task.Result == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 초기화 쓰레드 동작
        /// </summary>
        /// <param name="state"></param>
        private void ThreadInitializeProcess()
        {
            while (false == mbThreadExit)
            {
                if (CDefine.ERunStatus.Initialize == m_objDocument.GetRunStatus())
                {
                    if (true == CheckPreConditionBeforeInitialize())
                    {
                        m_objDocument.GetMainFrame().ShowWaitMessage(true, "EQUIPMENT INITIALIZE");
                        SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_START);
                        if (true == DoProcessInitialize())
                        {
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
                            SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_DONE);
                            m_objDocument.GetMainFrame().ShowWaitMessage(false);
                        }
                        else
                        {
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
                            SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);
                            m_objDocument.GetMainFrame().ShowWaitMessage(false);
                        }
                    }
                    else
                    {
                        m_objDocument.SetRunStatus(CDefine.ERunStatus.Stop);
                        SetInitializeCommand(EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);
                        m_objDocument.GetMainFrame().ShowWaitMessage(false);
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 재시작 동작을 정의합니다.
        /// </summary>
        /// <returns>함수 실행 결과</returns>
        private bool DoProcessRestart()
        {
            List<Task<bool>> restartTasks = new List<Task<bool>>();
            restartTasks.Clear();
            restartTasks.Add(RestartProcessAsync(InRobot));
            restartTasks.Add(RestartProcessAsync(OutRobot));
            Task.WaitAll(restartTasks.ToArray());
            foreach (var task in restartTasks)
            {
                if (task.Result == false)
                {
                    return false;
                }
            }

            restartTasks.Clear();
            restartTasks.Add(RestartProcessAsync(InShuttle));
            restartTasks.Add(RestartProcessAsync(InspStage));
            restartTasks.Add(RestartProcessAsync(OutFlip));
            Task.WaitAll(restartTasks.ToArray());
            foreach (var task in restartTasks)
            {
                if (task.Result == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 초기화 스레드 동작을 정의합니다.
        /// </summary>
        private void ThreadRestartProcess()
        {
            while (false == mbThreadExit)
            {
                if (CDefine.ERunStatus.Setup == m_objDocument.GetRunStatus())
                {
                    if (EProcessManagerRestart.PROCESS_MANAGER_RESTART_START == GetRestartStatus())
                    {
                        m_objDocument.GetMainFrame().ShowWaitMessage(true, "EQUIPMENT RESTART");
                        if (true == DoProcessRestart())
                        {
                            m_objDocument.m_objProcessCIM.BeginSyncEquipmentStateReport();
                            // 장비 상태 변경
                            m_objDocument.m_eInterlockState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EInterlockState.INTERLOCK_STATE_OFF;
                            m_objDocument.m_eAvailabilityState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EAvailabilityState.AVAILABILITY_STATE_UP;
                            m_objDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE] = CCIMDefine.EMoveState.MOVE_STATE_RUNNING;
                            m_objDocument.m_objProcessCIM.EndSyncEquipmentStateReport();

                            SetRestartCommand(EProcessManagerRestart.PROCESS_MANAGER_RESTART_DONE);
                            m_objDocument.GetMainFrame().ShowWaitMessage(false);
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Start);
                        }
                        else
                        {
                            SetRestartCommand(EProcessManagerRestart.PROCESS_MANAGER_RESTART_ERROR);
                            m_objDocument.GetMainFrame().ShowWaitMessage(false);
                            m_objDocument.SetRunStatus(CDefine.ERunStatus.Stopping);
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}