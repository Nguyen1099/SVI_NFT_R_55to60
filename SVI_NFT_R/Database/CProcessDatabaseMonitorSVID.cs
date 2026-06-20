using HLDevice.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace SVI_NFT_R
{
    public class CProcessDatabaseMonitorSVID : CDatabaseAbstract
    {
        public bool IsSvidMonitorLock { get; set; }
        private string ReportInnerId => string.Format("{0:yyyyMMddHHmm}00", mLastInnerIdUpdateTime);
        private Dictionary<CDatabaseDefine.ESvidList, SVCheck> m_objSVCheck;
        private DataTable mSvidInformationTable;
        private DataRow[] mSvidInformationRows;
        private const int STEP_STOP = 0;
        private const int STEP_IDLE = 1;
        private const int STEP_RUN = 2;
        private readonly int mSvidItemCount = Enum.GetNames(typeof(CDatabaseDefine.ESvidList)).Length;
        private DateTime mLastInnerIdUpdateTime = DateTime.Now;
        private readonly TimeSpan mInnerIdUpdatePeriodTime = TimeSpan.FromSeconds(1);
        private FdcReportDataSet mPowerMeterData = new FdcReportDataSet();

        /// <summary>
        /// SV 필터링을 위한 클래스
        /// </summary>
        private class SVCheck
        {
            /// <summary>
            /// 최대값 (null == N/A)
            /// </summary>
            private readonly double? mMaxValue;
            /// <summary>
            /// 최소값 (null == N/A)
            /// </summary>
            private readonly double? mMinValue;
            /// <summary>
            /// 마지막 정상 보고 값
            /// </summary>
            private object mLastValue;
            /// <summary>
            /// 기준 초과 값 시작 시간
            /// </summary>
            private DateTime? mOverStartTime;
            /// <summary>
            /// 기준 초과 값 보고 시작 시간 (이 이간 이상 경과하면 실제 값을 TC에 올린다)
            /// </summary>
            private TimeSpan mOverTime;

            /// <summary>
            /// 생성자
            /// </summary>                                                         
            /// <param name="maxValueOrNull"></param>
            /// <param name="minValueOrNull"></param>
            /// <param name="overTimeOrNull"></param>
            public SVCheck(double? maxValueOrNull = null, double? minValueOrNull = null, TimeSpan? overTimeOrNull = null)
            {
                if (null == overTimeOrNull)
                {
                    mOverTime = new TimeSpan(0, 0, 5);
                }
                else
                {
                    mOverTime = overTimeOrNull.Value;
                }
                mMaxValue = maxValueOrNull;
                mMinValue = minValueOrNull;
                mLastValue = null;
                mOverStartTime = null;
            }

            /// <summary>
            /// 입력된 SV값을 확인하여 범위를 초과했으면 설정한 시간까지는 이전에 정상값을 보고하다가 시간이 지나면 실제 값을 보고한다.
            /// </summary>
            /// <param name="checkValue">SV 값</param>
            /// <returns>보고할 값</returns>
            public object GetSVCheck(object checkValue)
            {
                object objResult = checkValue;
                bool bResult = false;
                if (null == mLastValue)
                {
                    mLastValue = checkValue;
                }

                do
                {
                    if ("String" == checkValue.GetType().Name)
                    {
                        // 더블형이 아니면 체크 안함
                        mLastValue = checkValue;
                        mOverStartTime = null;
                        bResult = true;
                        break;
                    }

                    double dValue = 0.0;
                    try
                    {
                        dValue = Convert.ToDouble(checkValue);
                    }
                    catch
                    {
                        // 더블형이 아니면 체크 안함
                        mLastValue = checkValue;
                        mOverStartTime = null;
                        bResult = true;
                        break;
                    }

                    if (null != mMaxValue)
                    {
                        if (dValue > mMaxValue.Value)
                        {
                            break;
                        }
                    }
                    if (null != mMinValue)
                    {
                        if (dValue < mMinValue.Value)
                        {
                            break;
                        }
                    }

                    mLastValue = checkValue;
                    mOverStartTime = null;
                    bResult = true;
                } while (false);

                if (false == bResult)
                {
                    if (null == mOverStartTime)
                    {
                        mOverStartTime = new DateTime?(DateTime.Now);
                    }

                    TimeSpan elapseTime = DateTime.Now - mOverStartTime.Value;
                    if (elapseTime < mOverTime)
                    {
                        objResult = mLastValue;
                    }
                }
                return objResult;
            }
        }

        /// <summary>
        /// 초기화 함수
        /// </summary>
        /// <param name="objProcessDatabase"></param>
        /// <returns></returns>
        public bool Initialize(CProcessDatabase objProcessDatabase)
        {
            bool bReturn = false;

            do
            {
                IsSvidMonitorLock = false;
                // 프로세스 데이터베이스 이어줌
                m_objProcessDatabase = objProcessDatabase;

                // SVID Min, Max값 갖고 있어야 함.
                mSvidInformationTable = m_objProcessDatabase.m_objProcessDatabaseInformation.m_objManagerTableInformationSVID.HLGetDataTable();
                // SVID 테이블 행 값으로 갖음
                mSvidInformationRows = mSvidInformationTable.Select();
                // SV Checker 초기화
                int iEnd = mSvidInformationRows.Length;
                m_objSVCheck = new Dictionary<CDatabaseDefine.ESvidList, SVCheck>();
                for (int iLoop = 0; iLoop < iEnd; iLoop++)
                {
                    string strMaxValue = (string)mSvidInformationRows[iLoop][(int)CDatabaseDefine.EInformationSvid.MAX_VALUE];
                    string strMinValue = (string)mSvidInformationRows[iLoop][(int)CDatabaseDefine.EInformationSvid.MIN_VALUE];
                    double? dMaxValue = null;
                    double? dMinValue = null;
                    double dValue;
                    if (false == string.IsNullOrEmpty(strMaxValue) && "N/A" != strMaxValue)
                    {
                        double.TryParse(strMaxValue, out dValue);
                        dMaxValue = new double?(dValue);
                    }
                    if (false == string.IsNullOrEmpty(strMinValue) && "N/A" != strMinValue)
                    {
                        double.TryParse(strMinValue, out dValue);
                        dMinValue = new double?(dValue);
                    }
                    m_objSVCheck.Add((CDatabaseDefine.ESvidList)iLoop, new SVCheck(dMaxValue, dMinValue));
                }

                m_bThreadExit = false;
                m_ThreadProcess = new Thread(ThreadProcess);
                m_ThreadProcess.IsBackground = true;
                m_ThreadProcess.Start();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
            m_bThreadExit = true;
            m_ThreadProcess.Join(1000);
        }

        /// <summary>
        /// 쓰레드
        /// </summary>
        /// <param name="state"></param>
        private void ThreadProcess()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            TimeSpan updatePeriod = TimeSpan.FromSeconds(1);
            sw.Start();

            while (false == m_bThreadExit)
            {
                if (sw.Elapsed > updatePeriod)
                {
                    SetUpdateReportInnerId();
                    sw.Restart();
                    if (false == IsSvidMonitorLock)
                    {
                        SetSvidListUpdate();
                    }
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// SVID 리스트 갱신해서 지속적으로 CIM에 날려줌 & CFormMainCIM에도 정보 날려줌
        /// </summary>
        private void SetSvidListUpdate()
        {
            try
            {
                // 디바이스 & 제어에서 사용하는 데이터 넣어줌
                var objSvidList = new Dictionary<CDatabaseDefine.ESvidList, object>(mSvidItemCount);

                // Air Supply Preesure
                {
                    var pressures = new Dictionary<CDatabaseDefine.ESvidList, CDeviceIODefine.EAnalogInput>()
                    {
                        { CDatabaseDefine.ESvidList.MAIN_CDA_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_MAIN_CDA_PRESSURE },
                    };
                    foreach (var item in pressures)
                    {
                        objSvidList[item.Key] = m_objProcessDatabase.GetDocument().GetVoltageToMPa(item.Value);
                    }
                }

                // Vacuum Supply Pressure
                {
                    var pressures = new Dictionary<CDatabaseDefine.ESvidList, CDeviceIODefine.EAnalogInput>()
                    {
                        { CDatabaseDefine.ESvidList.MAIN_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_MAIN_VACUUM_PRESSURE },
                        { CDatabaseDefine.ESvidList.INSP_STAGE_MAIN_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_INSP_STAGE_VAC_PRESSURE},
                    };
                    foreach (var item in pressures)
                    {
                        objSvidList[item.Key] = m_objProcessDatabase.GetDocument().GetVoltageTokPa(item.Value);
                    }
                }

                // Vacuum Pressure
                {
                    var pressures = new Dictionary<CDatabaseDefine.ESvidList, CDeviceIODefine.EAnalogInput>()
                    {
                         { CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_IN_SHUTTLE_P1_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_IN_SHUTTLE_P2_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.IN_ROBOT_P1_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_IN_ROBOT_P1_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.IN_ROBOT_P2_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_IN_ROBOT_P2_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.INSP_STAGE_P1_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_INSP_STAGE_P1_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.INSP_STAGE_P2_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_INSP_STAGE_P2_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.OUT_ROBOT_P1_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_OUT_ROBOT_P1_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.OUT_ROBOT_P2_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_OUT_ROBOT_P2_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.OUT_FLIP_P1_1_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P1_1_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.OUT_FLIP_P1_2_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P1_2_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.OUT_FLIP_P2_1_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P2_1_VACUUM_PRESSURE },
                         { CDatabaseDefine.ESvidList.OUT_FLIP_P2_2_VACUUM_PRESSURE, CDeviceIODefine.EAnalogInput.X_AD_OUT_ROTATE_P2_2_VACUUM_PRESSURE },
                    };
                    foreach (var item in pressures)
                    {
                        objSvidList[item.Key] = m_objProcessDatabase.GetDocument().GetVoltageTokPaVacuum(item.Value);
                    }
                }

                // Temperature
                {
                    objSvidList[CDatabaseDefine.ESvidList.MAIN_ELECTRICAL_TEMP] = CProcessMain.SerialReaders.Temperature[CDefine.ETemperature.MainElectronicBox].Value;
                    objSvidList[CDatabaseDefine.ESvidList.MAIN_PCRACK_TEMP] = CProcessMain.SerialReaders.Temperature[CDefine.ETemperature.PcRack].Value;
                }

                // EFU
                {
                    bool bIsRunningAllEFU = true;
                    const int MIN_RPM = 10;
                    int[] readValues = CProcessMain.SerialReaders.Mcu[CDefine.EMcu.MainGruop].Value;
                    int valueIndex = 0;
                    objSvidList[CDatabaseDefine.ESvidList.EFU_RPM_1] = (double)readValues[valueIndex];
                    bIsRunningAllEFU &= (readValues[valueIndex++] > MIN_RPM);
                    objSvidList[CDatabaseDefine.ESvidList.RUN_EFU] = (bIsRunningAllEFU == true) ? 1 : 0;
                }

                // 적산 전력계
                {
                    // aggrigation 데이터 업데이트
                    foreach (var powerMeter in m_objProcessDatabase.GetDocument().m_objProcessMain.m_objPowerMeter.Values)
                    {
                        powerMeter.HLDataFetch();
                    }

                    int unitID = m_objProcessDatabase.GetDocument().m_objConfig.GetPowerMeterParameter(CDefine.EPowerMeter.POWER_METER_GPS).iUnitID;
                    m_objProcessDatabase.GetDocument().m_objProcessMain.m_objPowerMeter[CDefine.EPowerMeter.POWER_METER_GPS].TryReadFdcReportData(unitID, ref mPowerMeterData);
                    objSvidList[CDatabaseDefine.ESvidList.SINGLE_PHASE_POWER_1] = (double)mPowerMeterData.PHASE_POWER;
                    objSvidList[CDatabaseDefine.ESvidList.INSTANTANEOUS_POWER_1] = (double)mPowerMeterData.INSTANTANEOUS_POWER;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_VOLTAGE_1] = (double)mPowerMeterData.R_PHASE_VOLTAGE;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_CURRENT_1] = (double)mPowerMeterData.R_PHASE_CURRENT;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_VOLTAGE_VTHD_1] = (double)mPowerMeterData.R_PHASE_VOLTAGE_VTHD;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_CURRENT_ITDD_1] = (double)mPowerMeterData.R_PHASE_CURRENT_ITDD;

                    unitID = m_objProcessDatabase.GetDocument().m_objConfig.GetPowerMeterParameter(CDefine.EPowerMeter.POWER_METER_UPS).iUnitID;
                    m_objProcessDatabase.GetDocument().m_objProcessMain.m_objPowerMeter[CDefine.EPowerMeter.POWER_METER_UPS].TryReadFdcReportData(unitID, ref mPowerMeterData);
                    objSvidList[CDatabaseDefine.ESvidList.SINGLE_PHASE_POWER_2] = (double)mPowerMeterData.PHASE_POWER;
                    objSvidList[CDatabaseDefine.ESvidList.INSTANTANEOUS_POWER_2] = (double)mPowerMeterData.INSTANTANEOUS_POWER;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_VOLTAGE_2] = (double)mPowerMeterData.R_PHASE_VOLTAGE;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_CURRENT_2] = (double)mPowerMeterData.R_PHASE_CURRENT;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_VOLTAGE_VTHD_2] = (double)mPowerMeterData.R_PHASE_VOLTAGE_VTHD;
                    objSvidList[CDatabaseDefine.ESvidList.R_PHASE_CURRENT_ITDD_2] = (double)mPowerMeterData.R_PHASE_CURRENT_ITDD;
                }

                // Main Vision
                {
                    objSvidList[CDatabaseDefine.ESvidList.USE_INSP_STAGE_MAIN_VISION] = (m_objProcessDatabase.GetDocument().m_objConfig.GetInspOptionParameter(CDefine.EInspectType.Main).eInspectionMode != CConfig.CInspOptionParameter.EInspectionMode.INSPECT_NOT_USE) ? 1 : 0;
                }

                // Align
                {
                    var inShuttle = m_objProcessDatabase.GetDocument().m_objProcessMain.m_objProcessMotion.InShuttle;
                    objSvidList[CDatabaseDefine.ESvidList.USE_IN_SHUTTLE_PRE_ALIGN] = (m_objProcessDatabase.GetDocument().m_objConfig.GetAlignOptionParameter(CDefine.EAlign.PreAlign).bUseVision == true) ? 1 : 0;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_PRE_ALIGN_SCORE_SETTING] = m_objProcessDatabase.GetDocument().m_objConfig.GetAlignOptionParameter(CDefine.EAlign.PreAlign).iVisionScore;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_DATA_SCORE] = inShuttle.CellContainer[0].IsCellExist() == true ? inShuttle.CellContainer[0].Data.PreAlignResult.Score : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_DATA_X] = inShuttle.CellContainer[0].IsCellExist() == true ? inShuttle.CellContainer[0].Data.PreAlignResult.TotalRevisionX : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_DATA_Y] = inShuttle.CellContainer[0].IsCellExist() == true ? inShuttle.CellContainer[0].Data.PreAlignResult.TotalRevisionY : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_DATA_T] = inShuttle.CellContainer[0].IsCellExist() == true ? inShuttle.CellContainer[0].Data.PreAlignResult.TotalRevisionT : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_DATA_SCORE] = inShuttle.CellContainer[1].IsCellExist() == true ? inShuttle.CellContainer[1].Data.PreAlignResult.Score : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_DATA_X] = inShuttle.CellContainer[1].IsCellExist() == true ? inShuttle.CellContainer[1].Data.PreAlignResult.TotalRevisionX : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_DATA_Y] = inShuttle.CellContainer[1].IsCellExist() == true ? inShuttle.CellContainer[1].Data.PreAlignResult.TotalRevisionY : 0d;
                    objSvidList[CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_DATA_T] = inShuttle.CellContainer[1].IsCellExist() == true ? inShuttle.CellContainer[1].Data.PreAlignResult.TotalRevisionT : 0d;
                }

                // MCR
                {
                    objSvidList[CDatabaseDefine.ESvidList.USE_IN_ROBOT_MCR] = m_objProcessDatabase.GetDocument().m_objConfig.GetOptionParameter().bUseAutoMCR == true ? 1 : 0;
                }

                {
                    objSvidList[CDatabaseDefine.ESvidList.OUT_FLIP_CELL_OUT_DELAY_TIME] = m_objProcessDatabase.GetDocument().m_objProcessMain.m_objProcessMotion.OutFlip.CellOutputDelayTimeForFdcData.TotalSeconds;
                }

                // Cell ID and Step ID
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.IN_ROBOT_P1_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.IN_ROBOT_P2_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.INSP_STAGE_P1_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.INSP_STAGE_P2_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.OUT_ROBOT_P1_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.OUT_ROBOT_P2_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.OUT_FLIP_P1_1_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.OUT_FLIP_P1_2_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.OUT_FLIP_P2_1_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.OUT_FLIP_P2_2_CELL_ID, ref objSvidList);
                // Vacuum 외
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_CELL_ID, ref objSvidList);
                GetCellIDAndStepID(CDatabaseDefine.ESvidList.UTIL_CELL_ID, ref objSvidList);

                // SV Report Value Checking
                List<object> objSvid = new List<object>(mSvidItemCount);
                for (int i = 0; i < mSvidItemCount; i++)
                {
                    CDatabaseDefine.ESvidList key = (CDatabaseDefine.ESvidList)i;
                    objSvid.Add(m_objSVCheck[key].GetSVCheck(objSvidList[key]));
                }

                // CFormMainCIM에 정보 날려줌
                m_objProcessDatabase.GetDocument().SetSvidList(objSvid);
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        private void SetUpdateReportInnerId()
        {
            TimeSpan span = DateTime.Now - mLastInnerIdUpdateTime;
            if (span > mInnerIdUpdatePeriodTime)
            {
                mLastInnerIdUpdateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 해당 인덱스에 셀 ID를 구해서 넣는다.
        /// </summary>
        /// <param name="svidCellIDIndex">셀 ID 인덱스</param>
        /// <param name="svidList">SVID 리스트</param>
        /// <param name="processData">프로세스 데이터</param>
        private void GetCellIDAndStepID(CDatabaseDefine.ESvidList svidCellIDIndex, ref Dictionary<CDatabaseDefine.ESvidList, object> svidList)
        {
            string cellID = string.Empty;

            // Cell ID
            {
                CellData.ECellData cellDataIndex = CellData.ECellData.InRobotP1;
                switch (svidCellIDIndex)
                {
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_CELL_ID:
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_CELL_ID:
                        cellDataIndex = CellData.ECellData.InShuttleP1;
                        break;
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_CELL_ID:
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_CELL_ID:
                        cellDataIndex = CellData.ECellData.InShuttleP2;
                        break;
                    case CDatabaseDefine.ESvidList.IN_ROBOT_P1_CELL_ID:
                        cellDataIndex = CellData.ECellData.InRobotP1;
                        break;
                    case CDatabaseDefine.ESvidList.IN_ROBOT_P2_CELL_ID:
                        cellDataIndex = CellData.ECellData.InRobotP2;
                        break;
                    case CDatabaseDefine.ESvidList.INSP_STAGE_P1_CELL_ID:
                        cellDataIndex = CellData.ECellData.InspStageP1;
                        break;
                    case CDatabaseDefine.ESvidList.INSP_STAGE_P2_CELL_ID:
                        cellDataIndex = CellData.ECellData.InspStageP2;
                        break;
                    case CDatabaseDefine.ESvidList.OUT_ROBOT_P1_CELL_ID:
                        cellDataIndex = CellData.ECellData.OutRobotP1;
                        break;
                    case CDatabaseDefine.ESvidList.OUT_ROBOT_P2_CELL_ID:
                        cellDataIndex = CellData.ECellData.OutRobotP2;
                        break;
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P1_1_CELL_ID:
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P1_2_CELL_ID:
                        cellDataIndex = CellData.ECellData.OutFlipP1;
                        break;
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P2_1_CELL_ID:
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P2_2_CELL_ID:
                        cellDataIndex = CellData.ECellData.OutFlipP2;
                        break;
                    // COMMON, LIGHT, UTIL, GMS 영역은 셀이 머무르지 않고 상시 체크해야하는 부분임으로 CELL_ID는 'yyyyMMddHHmmss' 형식으로 넣고, STEP_ID는 '1'로 고정하여 보고함.
                    //case CDatabaseDefine.ESvidList.LIGHT_CELL_ID:
                    case CDatabaseDefine.ESvidList.UTIL_CELL_ID:
                        //case CDatabaseDefine.ESvidList.GMS_CELL_ID:
                        cellID = ReportInnerId;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "등록되지 않은 SVID 인덱스에 대한 셀 ID를 요청했습니다. " + svidCellIDIndex.ToString());
                        break;
                }

                if (string.IsNullOrWhiteSpace(cellID) == true)
                {
                    // MCR 미촬상시 공백 리턴
                    cellID = CellDataManager.Cells[cellDataIndex].GetCellID();

                    // [MCR 촬상전 보고하는 케이스 처리]
                    // V3 G-Project 처리 방식 - MCR 촬상 전에는 투입시간으로 고정하여 보고함
                    string innerID = CellDataManager.Cells[cellDataIndex].Data.Cell.InnerID;
                    if (
                        string.IsNullOrEmpty(cellID) == true
                        && string.IsNullOrEmpty(innerID) == false
                        && innerID.Length > 15
                        )
                    {
                        // InnerID Format = {'A' or 'B' or 'C' or 'D'}+{yyyyMMddHHmmssfff}
                        cellID = innerID.Substring(1, 14);
                    }
                    //// 기존 처리 방식 - MCR 촬상 전에는 설정한 간격으로 현재 시간을 갱신하여 보고함
                    //if (string.IsNullOrEmpty(cellID) == true)
                    //{
                    //    cellID = mReportInnerId;
                    //}
                }

                svidList[svidCellIDIndex++] = cellID;
            }

            // Step ID
            {
                // [설비 정지] = [STEP_STOP]
                // [설비 런 & 셀 체크 안함] = [STEP_IDLE]
                // [설비 런 & 셀 체크 시작] = [STEP_RUN]
                int stepID = STEP_STOP;
                switch (svidCellIDIndex)
                {

                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.IN_ROBOT_P1_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.IN_ROBOT_P1_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.IN_ROBOT_P2_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.IN_ROBOT_P2_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.INSP_STAGE_P1_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.INSP_STAGE_P1_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.INSP_STAGE_P2_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.INSP_STAGE_P2_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.OUT_ROBOT_P1_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.OUT_ROBOT_P1_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.OUT_ROBOT_P2_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.OUT_ROBOT_P2_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P1_1_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.OUT_FLIP_P1_1_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P1_2_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.OUT_FLIP_P1_2_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P2_1_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.OUT_FLIP_P2_1_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.OUT_FLIP_P2_2_STEP_ID:
                        stepID = GetStepID(cellID, CDatabaseDefine.ESvidList.OUT_FLIP_P2_2_VACUUM_PRESSURE, svidList);
                        break;
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P1_PRE_ALIGN_STEP_ID:
                        stepID = GetAlignStepID(CellData.ECellData.InShuttleP1, CDefine.EAlign.PreAlign);
                        break;
                    case CDatabaseDefine.ESvidList.IN_SHUTTLE_P2_PRE_ALIGN_STEP_ID:
                        stepID = GetAlignStepID(CellData.ECellData.InShuttleP2, CDefine.EAlign.PreAlign);
                        break;
                    // COMMON, LIGHT, UTIL, GMS 영역은 셀이 머무르지 않고 상시 체크해야하는 부분임으로 CELL_ID는 'yyyyMMddHHmmss' 형식으로 넣고, STEP_ID는 '1'로 고정하여 보고함.
                    // 2021-03-31 UTIL_STEP_ID & LIGHT_STEP_ID RUN 시 2보고 STOP 시 0 보고 (천안 마더 라인)
                    //case CDatabaseDefine.ESvidList.LIGHT_STEP_ID:
                    case CDatabaseDefine.ESvidList.UTIL_STEP_ID:
                        //case CDatabaseDefine.ESvidList.GMS_STEP_ID:
                        {
                            bool bRunningAuto = (
                                m_objProcessDatabase.GetDocument().GetRunStatus() == CDefine.ERunStatus.Start
                                || m_objProcessDatabase.GetDocument().GetRunStatus() == CDefine.ERunStatus.Stopping
                                || m_objProcessDatabase.GetDocument().GetRunStatus() == CDefine.ERunStatus.LoadingStop
                                );
                            stepID = bRunningAuto ? STEP_RUN : STEP_STOP;
                            //stepID = STEP_IDLE;
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "등록되지 않은 SVID 인덱스에 대한 스텝 ID를 요청했습니다. " + svidCellIDIndex.ToString());
                        break;
                }
                svidList[svidCellIDIndex] = stepID;
            }
        }

        /// <summary>
        /// 현재 베큠값과 최소 적정값을 비교하여 스텝 ID를 구한다
        /// </summary>
        /// <param name="cellID">CELL ID</param>
        /// <param name="svidVacuumIndex">현재 베큠값의 인덱스</param>
        /// <param name="svidList">SVID 리스트</param>
        /// <returns>스텝 ID</returns>
        private int GetStepID(string cellID, CDatabaseDefine.ESvidList svidVacuumIndex, Dictionary<CDatabaseDefine.ESvidList, object> svidList)
        {
            int result;

            do
            {
                // AUTO RUN 상태가 아니면 STOP
                if (
                    CDefine.ERunStatus.Start != m_objProcessDatabase.GetDocument().GetRunStatus()
                    && CDefine.ERunStatus.Stopping != m_objProcessDatabase.GetDocument().GetRunStatus()
                    && CDefine.ERunStatus.LoadingStop != m_objProcessDatabase.GetDocument().GetRunStatus()
                    )
                {
                    result = STEP_STOP;
                    break;
                }

                // 현재 CELL ID가 없는 경우 IDLE
                if (true == string.IsNullOrWhiteSpace(cellID))
                {
                    result = STEP_IDLE;
                    break;
                }

                // 현재 베큠값이 최소 허용값보다 작은 경우 IDLE
                double vacuumPressure = Convert.ToDouble(svidList[svidVacuumIndex]);
                double vacuumMaxPressure = Convert.ToDouble(mSvidInformationRows[(int)svidVacuumIndex][(int)CDatabaseDefine.EInformationSvid.APPROPRIATE_MAX_VALUE]);
                if (vacuumMaxPressure < vacuumPressure)
                {
                    result = STEP_IDLE;
                    break;
                }

                result = STEP_RUN;
            } while (false);

            return result;
        }

        private int GetAlignStepID(CellData.ECellData cellDataIndex, CDefine.EAlign eAlign)
        {
            int result;

            do
            {
                // AUTO RUN 상태가 아니면 STOP
                if (CDefine.ERunStatus.Start != m_objProcessDatabase.GetDocument().GetRunStatus()
                    && CDefine.ERunStatus.Stopping != m_objProcessDatabase.GetDocument().GetRunStatus()
                    && CDefine.ERunStatus.LoadingStop != m_objProcessDatabase.GetDocument().GetRunStatus()
                    )
                {
                    result = STEP_STOP;
                    break;
                }

                // Align 미사용시 STOP
                if (m_objProcessDatabase.GetDocument().m_objConfig.GetAlignOptionParameter(eAlign).bUseVision == false)
                {
                    result = STEP_STOP;
                    break;
                }

                if (eAlign == CDefine.EAlign.PreAlign)
                {
                    // Cell Data가 없거나 Align 미완료시 STOP
                    if (CellDataManager.Cells[cellDataIndex].IsCellExist() == false
                        || CellDataManager.Cells[cellDataIndex].Data.Cell.PreAlignStatus != CellData.EStatus.Done)
                    {
                        result = STEP_STOP;
                        break;
                    }
                }

                result = STEP_RUN;
            } while (false);

            return result;
        }
    }
}
