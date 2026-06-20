using HLDeviceDLL.PowerMeter.Accura;

namespace HLDevice.Abstract
{
    public abstract class CDevicePowerMeterAbstract
    {
        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CInitializeParameter
        {
            /// <summary>
            /// 통신 타입
            /// </summary>
            public enum EType
            {
                TYPE_SOCKET_CLIENT = 0,
                TYPE_SOCKET_SERVER,
                TYPE_SERIAL_PORT,
                TYPE_FINAL
            };
            /// <summary>
            /// 시리얼포트 Parity
            /// </summary>
            public enum ESerialPortParity
            {
                PARITY_NONE = 0,
                PARITY_ODD,
                PARITY_EVEN,
                PARITY_MARK,
                PARITY_SPACE
            };
            /// <summary>
            /// 시리얼포트 StopBits
            /// </summary>
            public enum ESerialPortStopBits
            {
                STOP_BITS_NONE = 0,
                STOP_BITS_ONE,
                STOP_BITS_TWO,
                STOP_BITS_ONE_POINT_FIVE
            };

            public EType eType;
            public string strSocketIPAddress;
            public int iSocketPortNumber;

            public string strSerialPortName;
            public int iSerialPortBaudrate;
            public int iSerialPortDataBits;
            public ESerialPortParity eParity;
            public ESerialPortStopBits eStopBits;

            public object Clone()
            {
                CInitializeParameter objInitializeParameter = new CInitializeParameter();
                objInitializeParameter.eType = eType;
                objInitializeParameter.strSocketIPAddress = strSocketIPAddress;
                objInitializeParameter.iSocketPortNumber = iSocketPortNumber;

                objInitializeParameter.strSerialPortName = strSerialPortName;
                objInitializeParameter.iSerialPortBaudrate = iSerialPortBaudrate;
                objInitializeParameter.iSerialPortDataBits = iSerialPortDataBits;
                objInitializeParameter.eParity = eParity;
                objInitializeParameter.eStopBits = eStopBits;
                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CPowerMeterError
        {
            /// <summary>
            /// 이벤트 발생 시간
            /// </summary>
            public string strEventTime;
            /// <summary>
            /// 수행된 함수 이름
            /// </summary>
            public string strFunctionName;
            /// <summary>
            /// 알람 리턴 결과
            /// </summary>
            public int iReturnCode;
            /// <summary>
            /// 알람 메세지
            /// </summary>
            public string strMessage;

            public object Clone()
            {
                CPowerMeterError objError = new CPowerMeterError();
                objError.strEventTime = strEventTime;
                objError.strFunctionName = strFunctionName;
                objError.iReturnCode = iReturnCode;
                objError.strMessage = strMessage;

                return objError;
            }
        }
        public int UnitType { get; set; }

        /// <summary>
        /// 초기화 추상객체
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public abstract bool HLInitialize(CInitializeParameter objInitializeParameter);

        /// <summary>
        /// 해제 추상객체
        /// </summary>
        public abstract void HLDeInitialize();

        /// <summary>
        /// 버전 추상객체
        /// </summary>
        /// <returns></returns>
        public abstract string HLGetVersion();

        /// <summary>
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public virtual bool HLIsConnected()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 (Setup)
        /// </summary>
        /// <returns></returns>
        public virtual bool HLRemoteSetupLock()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 해제 (Setup)
        /// </summary>
        /// <returns></returns>
        public virtual bool HLRemoteSetupUnlock()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 (Control)
        /// </summary>
        /// <returns></returns>
        public virtual bool HLRemoteControlLock()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 통신 원격 설정 잠금 해제 (Control)
        /// </summary>
        /// <returns></returns>
        public virtual bool HLRemoteControlUnlock()
        {
            bool bReturn = false;

            do
            {
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
        public virtual bool HLDataFetch()
        {
            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 계측 데이터의 aggregation 구간의 시작 및 종료 지점에 time-stamp와 Accura 2500/2550 AC 모듈의 각 ID 별 계측 데이터의 유형 및 유효성을 확인 할 수 있다
        /// </summary>
        /// <param name="measurementHeaderOrNull"></param>
        /// <returns></returns>
        public virtual bool HLGetMeasurementHeader(out MeasurementHeaderSet measurementHeaderOrNull)
        {
            measurementHeaderOrNull = null;

            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2500M 데이터 조회
        /// </summary>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public virtual bool HLGetMeasureData(IAccura2500_MeasureItem<ushort> item, out ushort measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// ACCURA 2500M 데이터 조회
        /// </summary>
        /// <param name="item"></param>
        /// <param name="measureData"></param>
        /// <returns></returns>
        public virtual bool HLGetMeasureData(IAccura2500_MeasureItem<float> item, out float measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<ushort> item, out ushort measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<float> item, out float measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<int> item, out int measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM1P_MeasureItem<uint> item, out uint measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<ushort> item, out ushort measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<float> item, out float measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
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
        public virtual bool HLGetMeasureData(int unitId, IAccura2550CM3P_MeasureItem<int> item, out int measureData)
        {
            measureData = 0;

            bool bReturn = false;

            do
            {
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public virtual CPowerMeterError HLGetErrorCode()
        {
            CPowerMeterError objError = new CPowerMeterError();
            return objError;
        }

        public virtual bool TryReadFdcReportData(int unitID, ref FdcReportDataSet data)
        {
            switch (UnitType)
            {
                case 0: // 단상
                    {
                        data.PHASE_POWER = 1;
                        data.INSTANTANEOUS_POWER = 2.2f;
                        data.LEAKAGE_CURRENT = 3.3f;
                        data.R_PHASE_VOLTAGE = 4.4f;
                        data.R_PHASE_CURRENT = 5.5f;
                        data.R_PHASE_VOLTAGE_VTHD = 6.6f;
                        data.R_PHASE_CURRENT_ITDD = 7.7f;
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
                        data.PHASE_POWER = 1;
                        data.INSTANTANEOUS_POWER = 2.2f;
                        data.LEAKAGE_CURRENT = 3.3f;
                        data.R_PHASE_VOLTAGE = 4.4f;
                        data.R_PHASE_CURRENT = 5.5f;
                        data.R_PHASE_VOLTAGE_VTHD = 6.6f;
                        data.R_PHASE_CURRENT_ITDD = 7.7f;
                        data.S_PHASE_VOLTAGE = 8.8f;
                        data.S_PHASE_CURRENT = 9.9f;
                        data.S_PHASE_VOLTAGE_VTHD = 10.10f;
                        data.S_PHASE_CURRENT_ITDD = 11.11f;
                        data.T_PHASE_VOLTAGE = 12.12f;
                        data.T_PHASE_CURRENT = 13.13f;
                        data.T_PHASE_VOLTAGE_VTHD = 14.14f;
                        data.T_PHASE_CURRENT_ITDD = 15.15f;
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        public virtual bool TryReadLeakageCurrent(int unitID, ref float leakageCurrent)
        {
            leakageCurrent = 3.3f;
            return true;
        }
    }
}
