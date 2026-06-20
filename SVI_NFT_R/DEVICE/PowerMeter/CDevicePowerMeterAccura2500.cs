using HLDevice.Abstract;
using HLDeviceDLL.PowerMeter.Accura;

namespace HLDevice.PowerMeter
{
    public class CDevicePowerMeterAccura2500 : CDevicePowerMeterAbstract
    {
        private readonly CPowerMeterError m_objError = new CPowerMeterError();
        private readonly HLDeviceDLL.PowerMeter.Accura.CDevicePowerMeterAccura2500 m_objPowerMeter = new HLDeviceDLL.PowerMeter.Accura.CDevicePowerMeterAccura2500();

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                CDevicePowerMeterAccura2500Define.CInitializeParameter objParameter = new CDevicePowerMeterAccura2500Define.CInitializeParameter();
                objParameter.eType = (CDevicePowerMeterAccura2500Define.CInitializeParameter.EType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (CDevicePowerMeterAccura2500Define.CInitializeParameter.ESerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (CDevicePowerMeterAccura2500Define.CInitializeParameter.ESerialPortStopBits)objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;

                if (m_objPowerMeter.HLInitialize(objParameter) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objPowerMeter.HLDeInitialize();
        }

        /// <summary>
        /// DLL버전
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objPowerMeter.GetVersion();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return m_objPowerMeter.HLIsConnected();
        }

        /// <summary>
        /// 통신 원격 설정 잠금 (Setup)
        /// </summary>
        /// <returns></returns>
        public override bool HLRemoteSetupLock()
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.RemoteSetupLock() == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 해제 (Setup)
        /// </summary>
        /// <returns></returns>
        public override bool HLRemoteSetupUnlock()
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.RemoteSetupUnlock() == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 (Control)
        /// </summary>
        /// <returns></returns>
        public override bool HLRemoteControlLock()
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.RemoteControlLock() == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 해제 (Control)
        /// </summary>
        /// <returns></returns>
        public override bool HLRemoteControlUnlock()
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.RemoteControlUnlock() == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// aggrigation 타입에 데이터를 업데이트한다 (aggrigation 타입에 데이터를 Fetch 없이 읽으면 같은 데이터를 계속 읽게된다)
        /// </summary>
        /// <returns></returns>
        /// <![CDATA[
        /// 'Accura 2500_2550_AC_Communication User Guide_Rev1_20_Korean_220705.pdf' Page71 참조
        /// 
        ///  Register 9911을 읽으면, aggregation과 index selection으로 지정된「Measurement Header」, 「Measurement Data」,
        /// 「Measurement Max/ Min Data」 의 계측 데이터가 register 9914 - 52500 으로 fetch 되고 이에 따라 index selection이
        /// 갱신된다.
        /// ]]>
        public override bool HLDataFetch()
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.DataFetch() == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 계측 데이터의 aggregation 구간의 시작 및 종료 지점에 time-stamp와 Accura 2500/2550 AC 모듈의 각 ID 별 계측 데이터의 유형 및 유효성을 확인 할 수 있다
        /// </summary>
        /// <param name="measurementHeaderOrNull"></param>
        /// <returns></returns>
        public override bool HLGetMeasurementHeader(out MeasurementHeaderSet measurementHeaderOrNull)
        {
            bool bReturn = false;
            do
            {
                if (m_objPowerMeter.GetMeasurementHeader(out measurementHeaderOrNull) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2500M 데이터 조회
        /// </summary>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(IAccura2500_MeasureItem<ushort> item, out ushort measureData)
        {
            bool bReturn = false;
            do
            {
                if (m_objPowerMeter.GetMeasureData(item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2500M 데이터 조회
        /// </summary>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(IAccura2500_MeasureItem<float> item, out float measureData)
        {
            bool bReturn = false;
            do
            {
                if (m_objPowerMeter.GetMeasureData(item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<ushort> item, out ushort measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<float> item, out float measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<int> item, out int measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<uint> item, out uint measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 3P (단상 or 삼상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<ushort> item, out ushort measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 3P (단상 or 삼상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<float> item, out float measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 3P (단상 or 삼상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public override bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<int> item, out int measureData)
        {
            bool bReturn = false;

            do
            {
                if (m_objPowerMeter.GetMeasureData(unitId, item, out measureData) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override CPowerMeterError HLGetErrorCode()
        {
            return (CPowerMeterError)m_objError.Clone();
        }

        public override bool TryReadFdcReportData(int unitID, ref FdcReportDataSet data)
        {
            switch (UnitType)
            {
                case 0: // 단상
                    {
                        int readIntValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Net_active_energy, out readIntValue) == false)
                        {
                            return false;
                        }
                        data.PHASE_POWER = readIntValue;
                        float readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Demand_power, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.INSTANTANEOUS_POWER = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Leakage_current, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.LEAKAGE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_A_Van, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Current, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_A_THD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE_VTHD = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Current_TDD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT_ITDD = readFloatValue;
                        data.S_PHASE_VOLTAGE = null;
                        data.S_PHASE_CURRENT = null;
                        data.S_PHASE_VOLTAGE_VTHD = null;
                        data.S_PHASE_CURRENT_ITDD = null;
                        data.T_PHASE_VOLTAGE = null;
                        data.T_PHASE_CURRENT = null;
                        data.T_PHASE_VOLTAGE_VTHD = null;
                        data.T_PHASE_CURRENT_ITDD = null;
                    }
                    break;

                case 1: // 3상
                    {
                        int readIntValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Net_active_energy, out readIntValue) == false)
                        {
                            return false;
                        }
                        data.PHASE_POWER = readIntValue;
                        float readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Total_active_power_Ptot, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.INSTANTANEOUS_POWER = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Leakage_current, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.LEAKAGE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_AB_Vab, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Current_A_Ia, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_AB_THD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE_VTHD = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Current_A_TDD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT_ITDD = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_BC_Vbc, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_VOLTAGE = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Current_B_Ib, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_BC_THD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_VOLTAGE_VTHD = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Current_B_TDD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_CURRENT_ITDD = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_CA_Vca, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_VOLTAGE = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Current_C_Ic, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(MeasureItems.Accura2500M.Voltage_CA_THD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_VOLTAGE_VTHD = readFloatValue;
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Current_C_TDD_Percent, out readFloatValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_CURRENT_ITDD = readFloatValue;
                    }
                    break;

                default:
                    return false;
            }

            return true;
        }

        public override bool TryReadLeakageCurrent(int unitID, ref float leakageCurrent)
        {
            float readFloatValue;
            switch (UnitType)
            {
                case 0: // 단상
                    {
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Leakage_current, out readFloatValue) == false)
                        {
                            return false;
                        }
                        leakageCurrent = readFloatValue;
                    }
                    break;

                case 1: // 3상
                    {
                        if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Leakage_current, out readFloatValue) == false)
                        {
                            return false;
                        }
                        leakageCurrent = readFloatValue;
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {
            CDevicePowerMeterAccura2500Define.CAccuraError objError = m_objPowerMeter.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }
    }
}
