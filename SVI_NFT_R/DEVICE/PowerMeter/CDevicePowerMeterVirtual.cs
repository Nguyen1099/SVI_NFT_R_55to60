using HLDeviceDLL.PowerMeter.Accura;

namespace HLDevice.PowerMeter
{
    public class CDevicePowerMeterVirtual : Abstract.CDevicePowerMeterAbstract
    {
        private readonly CPowerMeterError m_objError = new CPowerMeterError();

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

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
        }

        /// <summary>
        /// 해제
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return "1.0.0.1";
        }

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return true;
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
            measurementHeaderOrNull = null;
            bool bReturn = false;
            do
            {
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
                measureData = 1234;
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 단상
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 단상
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 단상
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 단상
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 삼상
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 삼상
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
                measureData = 1234;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2550CM 데이터 조회 - 삼상
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
                measureData = 1234;
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

        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {

        }
    }
}
