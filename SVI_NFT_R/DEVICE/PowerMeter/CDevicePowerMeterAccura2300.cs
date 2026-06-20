using HLDevice.Abstract;
using HLDeviceDLL.PowerMeter.Accura;
using System;
using static HLDeviceDLL.PowerMeter.Accura.CDevicePowerMeterAccuraDefine;

namespace HLDevice.PowerMeter
{
    public class CDevicePowerMeterAccura2300 : CDevicePowerMeterAbstract
    {
        private readonly CPowerMeterError m_objError = new CPowerMeterError();
        private CDevicePowerMeterAccura m_objPowerMeter = new CDevicePowerMeterAccura();

        public override bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                CDevicePowerMeterAccuraDefine.CInitializeParameter objParameter = new CDevicePowerMeterAccuraDefine.CInitializeParameter();
                objParameter.eType = (CDevicePowerMeterAccuraDefine.CInitializeParameter.enumType)objInitializeParameter.eType;
                objParameter.strSerialPortName = objInitializeParameter.strSerialPortName;
                objParameter.iSerialPortBaudrate = objInitializeParameter.iSerialPortBaudrate;
                objParameter.iSerialPortDataBits = objInitializeParameter.iSerialPortDataBits;
                objParameter.eParity = (CDevicePowerMeterAccuraDefine.CInitializeParameter.enumSerialPortParity)objInitializeParameter.eParity;
                objParameter.eStopBits = (CDevicePowerMeterAccuraDefine.CInitializeParameter.enumSerialPortStopBits)objInitializeParameter.eStopBits;

                objParameter.strSocketIPAddress = objInitializeParameter.strSocketIPAddress;
                objParameter.iSocketPortNumber = objInitializeParameter.iSocketPortNumber;
                objParameter.iUnitType = UnitType;

                if (m_objPowerMeter.HLInitialize(objParameter) == false)
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override void HLDeInitialize()
        {
            m_objPowerMeter.HLDeInitialize();
        }

        public override string HLGetVersion()
        {
            return m_objPowerMeter.GetVersion();
        }

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
        /// 'Accura_2300_2350_Communication_UserGuide_Rev1_60_Korean_181005.pdf' Page139 참조
        /// 
        ///  Register 11044을 읽으면, aggregation과 index selection으로 지정된「Measurement Header」, 「Measurement Data」,
        /// 「Measurement Max/ Min Data」 의 계측 데이터가 register 11045 - 32300 으로 fetch 되고 이에 따라 index selection이
        /// 갱신된다.
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

        public override bool TryReadFdcReportData(int unitID, ref FdcReportDataSet data)
        {
            double readValue;
            switch (UnitType)
            {
                case 0: // 단상
                    {
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_1P.k_Net_kWh, out readValue) == false)
                        {
                            return false;
                        }
                        data.PHASE_POWER = Convert.ToInt32(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_1P.k_Demand_kW, out readValue) == false)
                        {
                            return false;
                        }
                        data.INSTANTANEOUS_POWER = Convert.ToSingle(readValue);
                        // ! H/W에서 지원 안함
                        //if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM1P.Leakage_current, out readFloatValue) == false)
                        //{
                        //    return false;
                        //}
                        //data.LEAKAGE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_1P.k_Voltage_Van, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_1P.k_Current_I, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_1P.k_Voltage_THD_A, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE_VTHD = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_1P.k_Current_TDD, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT_ITDD = Convert.ToSingle(readValue);
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
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Net_kWh, out readValue) == false)
                        {
                            return false;
                        }
                        data.PHASE_POWER = Convert.ToInt32(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Active_power_Ptot, out readValue) == false)
                        {
                            return false;
                        }
                        data.INSTANTANEOUS_POWER = Convert.ToSingle(readValue);
                        // ! H/W에서 지원 안함
                        //if (HLGetMeasureData(unitID, MeasureItems.Accura2550CM3P_3Phase.Leakage_current, out readFloatValue) == false)
                        //{
                        //    return false;
                        //}
                        //data.LEAKAGE_CURRENT = readFloatValue;
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Voltage_Vab, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Current_Ia, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Voltage_THD_A, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_VOLTAGE_VTHD = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Current_TDD_A, out readValue) == false)
                        {
                            return false;
                        }
                        data.R_PHASE_CURRENT_ITDD = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Voltage_Vbc, out readValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_VOLTAGE = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Current_Ib, out readValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_CURRENT = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Voltage_THD_B, out readValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_VOLTAGE_VTHD = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Current_TDD_B, out readValue) == false)
                        {
                            return false;
                        }
                        data.S_PHASE_CURRENT_ITDD = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Voltage_Vca, out readValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_VOLTAGE = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Current_Ic, out readValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_CURRENT = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Voltage_THD_C, out readValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_VOLTAGE_VTHD = Convert.ToSingle(readValue);
                        if (HLGetMeasureData(unitID, CAccuraMap.enum_K_Power_3P.k_Current_TDD_C, out readValue) == false)
                        {
                            return false;
                        }
                        data.T_PHASE_CURRENT_ITDD = Convert.ToSingle(readValue);
                    }
                    break;

                default:
                    return false;
            }

            return true;
        }

        public override bool TryReadLeakageCurrent(int unitID, ref float leakageCurrent)
        {
            // ! H/W에서 지원 안함
            return false;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override CPowerMeterError HLGetErrorCode()
        {
            return (CPowerMeterError)m_objError.Clone();
        }

        /// <summary>
        /// 적산량계 데이터 리딩 - 단상
        /// </summary>
        /// <param name="functionIndex">enum_K_Power_1P 인자값에 따른 데이터 반환</param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitID, CAccuraMap.enum_K_Power_1P functionIndex, out double measureData)
        {
            bool bReturn = false;
            do
            {

                if (false == m_objPowerMeter.HLGetMeasureDataRevise(unitID, functionIndex, out measureData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// 적산량계 데이터 리딩 - 3상
        /// </summary>
        /// <param name="functionIndex">enum_K_Power_3P 인자값에 따른 데이터 반환</param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitID, CAccuraMap.enum_K_Power_3P functionIndex, out double measureData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLGetMeasureDataRevise(unitID, functionIndex, out measureData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Modbus ReadCoils
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="bReadData"></param>
        /// <returns></returns>
        public bool HLReadCoils(int iSlave, int iAddress, int iReadCount, out bool[] bReadData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLReadCoils(iSlave, iAddress, iReadCount, out bReadData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Modbus WriteCoils
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public bool HLWriteSingleCoil(int iSlave, int iAddress, bool bData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLWriteSingleCoil(iSlave, iAddress, bData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Modbus ReadInputRegisters - double형 데이터 읽기
        /// 설명 : double형 데이터를 읽을려면 4를 읽어야 가능하기때문에 크기는 고정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="dReadData"></param>
        /// <returns></returns>
        public bool HLReadInputRegisters(int iSlave, int iAddress, out double dReadData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLReadInputRegisters(iSlave, iAddress, out dReadData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }


        /// <summary>
        /// Modbus ReadInputRegisters - float형 데이터 읽기
        /// 설명 : float형 데이터를 읽을려면 2를 읽어야 가능하기때문에 크기는 고정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="fReadData"></param>
        /// <returns></returns>
        public bool HLReadInputRegisters(int iSlave, int iAddress, out float fReadData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLReadInputRegisters(iSlave, iAddress, out fReadData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Modbus ReadInputRegisters - int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public bool HLReadInputRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLReadInputRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Modbus ReadInputRegisters - int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLReadHoldingRegisters(iSlave, iAddress, iReadCount, out iReadData))
                {
                    MakeError();
                    break;
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        /// <summary>
        /// Modbus WriteHoldingRegisters
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iData"></param>
        /// <returns></returns>
        public bool HLWriteHoldingRegisters(int iSlave, int iAddress, int iData)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objPowerMeter.HLWriteHoldingRegisters(iSlave, iAddress, iData))
                {
                    MakeError();
                }
                bReturn = true;
            } while (false);
            return bReturn;
        }

        private void MakeError()
        {
            CAccuraError objError = m_objPowerMeter.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }
    }
}
