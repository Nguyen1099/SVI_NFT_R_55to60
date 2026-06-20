using HLDevice.Abstract;
using HLDeviceDLL.PowerMeter.Accura;
using SVI_NFT_R;

namespace HLDevice
{
    public class CDevicePowerMeter
    {
        private CDevicePowerMeterAbstract m_objPowerMeter;
        private CDocument m_objDocument;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CPowerMeterParameter objPowerMeterParameter)
        {
            m_objDocument = document;

            if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            {
                m_objPowerMeter = new PowerMeter.CDevicePowerMeterVirtual();
            }
            else
            {
                m_objPowerMeter = new PowerMeter.CDevicePowerMeterAccura2300();
            }

            CDevicePowerMeterAbstract.CInitializeParameter objInitializeParameter = new CDevicePowerMeterAbstract.CInitializeParameter
            {
                eType = (CDevicePowerMeterAbstract.CInitializeParameter.EType)objPowerMeterParameter.eType,
                strSocketIPAddress = objPowerMeterParameter.strSocketIPAddress,
                iSocketPortNumber = objPowerMeterParameter.iSocketPortNumber,
                strSerialPortName = objPowerMeterParameter.strSerialPortName,
                iSerialPortBaudrate = objPowerMeterParameter.iSerialPortBaudrate,
                iSerialPortDataBits = objPowerMeterParameter.iSerialPortDataBits,
                eParity = (CDevicePowerMeterAbstract.CInitializeParameter.ESerialPortParity)objPowerMeterParameter.eParity,
                eStopBits = (CDevicePowerMeterAbstract.CInitializeParameter.ESerialPortStopBits)objPowerMeterParameter.eStopBits
            };
            m_objPowerMeter.UnitType = objPowerMeterParameter.iUnitType;

            return m_objPowerMeter.HLInitialize(objInitializeParameter);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objPowerMeter.HLDeInitialize();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objPowerMeter.HLGetVersion();
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public bool HLIsConnected()
        {
            return m_objPowerMeter.HLIsConnected();
        }

        /// <summary>
        /// 통신 원격 설정 잠금 (Setup)
        /// </summary>
        /// <returns></returns>
        public bool HLRemoteSetupLock()
        {
            return m_objPowerMeter.HLRemoteSetupLock();
        }

        /// <summary>
        /// 통신 원격 설정 잠금 해제 (Setup)
        /// </summary>
        /// <returns></returns>
        public bool HLRemoteSetupUnlock()
        {
            return m_objPowerMeter.HLRemoteSetupUnlock();
        }

        /// <summary>
        /// 통신 원격 설정 잠금 (Control)
        /// </summary>
        /// <returns></returns>
        public bool HLRemoteControlLock()
        {
            return m_objPowerMeter.HLRemoteControlLock();
        }

        /// <summary>
        /// 통신 원격 설정 잠금 해제 (Control)
        /// </summary>
        /// <returns></returns>
        public bool HLRemoteControlUnlock()
        {
            return m_objPowerMeter.HLRemoteControlUnlock();
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
        public bool HLDataFetch()
        {
            return m_objPowerMeter.HLDataFetch();
        }

        /// <summary>
        /// 계측 데이터의 aggregation 구간의 시작 및 종료 지점에 time-stamp와 Accura 2500/2550 AC 모듈의 각 ID 별 계측 데이터의 유형 및 유효성을 확인 할 수 있다
        /// </summary>
        /// <param name="measurementHeaderOrNull"></param>
        /// <returns></returns>
        public bool HLGetMeasurementHeader(out MeasurementHeaderSet measurementHeaderOrNull)
        {
            return m_objPowerMeter.HLGetMeasurementHeader(out measurementHeaderOrNull);
        }

        /// <summary>
        /// ACCURA 2500M 데이터 조회
        /// </summary>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(IAccura2500_MeasureItem<ushort> item, out ushort measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(item, out measureData);
        }

        /// <summary>
        /// ACCURA 2500M 데이터 조회
        /// </summary>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(IAccura2500_MeasureItem<float> item, out float measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<ushort> item, out ushort measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<float> item, out float measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<int> item, out int measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 1P (단상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<uint> item, out uint measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 3P (단상 or 삼상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<ushort> item, out ushort measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 3P (단상 or 삼상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<float> item, out float measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 3P (단상 or 삼상)
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<int> item, out int measureData)
        {
            return m_objPowerMeter.HLGetMeasureData(unitId, item, out measureData);
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public CDevicePowerMeterAbstract.CPowerMeterError HLGetErrorCode()
        {
            return m_objPowerMeter.HLGetErrorCode();
        }

        public bool TryReadFdcReportData(int unitID, ref FdcReportDataSet data)
        {
            return m_objPowerMeter.TryReadFdcReportData(unitID, ref data);
        }

        public bool TryReadLeakageCurrent(int unitID, ref float leakageCurrent)
        {
            return m_objPowerMeter.TryReadLeakageCurrent(unitID, ref leakageCurrent);
        }
    }
}
