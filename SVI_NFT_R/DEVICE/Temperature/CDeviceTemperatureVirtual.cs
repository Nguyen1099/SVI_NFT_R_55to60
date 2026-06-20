namespace HLDevice.Temperature
{
    public class CDeviceTemperatureVirtual : HLDevice.Abstract.CDeviceTemperatureAbstract
    {
        private readonly HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError m_objError = new HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceTemperatureVirtual()
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
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public override bool HLInitialize(HLDevice.Abstract.CDeviceTemperatureAbstract.CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {



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
        /// 접속 상태
        /// </summary>
        /// <returns></returns>
        public override bool HLIsConnected()
        {
            return true;
        }

        public override bool HLGetPVData(int iSlave, out double dPVData)
        {
            dPVData = 10d + iSlave;
            return true;
        }

        public override bool HLReadTemp(byte byteID, out double dTempData)
        {
            dTempData = 11d + byteID;
            return true;
        }

        public override bool HLGetInputData(int iSlave, out double dInputData)
        {
            dInputData = iSlave + 0.001d;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="bReadData"></param>
        /// <returns></returns>
        public override bool HLReadCoils(int iSlave, int iAddress, int iReadCount, out bool[] bReadData)
        {
            bool bReturn = false;
            bReadData = null;

            do
            {
                bReadData = new bool[iReadCount];
                bReadData[3] = true;
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public override bool HLWriteSingleCoil(int iSlave, int iAddress, bool bData)
        {
            bool bReturn = false;

            do
            {


                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// double형 데이터 읽기
        /// 설명 : double형 데이터를 읽을려면 4를 읽어야 가능하기때문에 크기는 고정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="dReadData"></param>
        /// <returns></returns>
        public override bool HLReadInputRegisters(int iSlave, int iAddress, out double dReadData)
        {
            bool bReturn = false;
            dReadData = 9;

            do
            {


                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// float형 데이터 읽기
        /// 설명 : float형 데이터를 읽을려면 2를 읽어야 가능하기때문에 크기는 고정
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="fReadData"></param>
        /// <returns></returns>
        public override bool HLReadInputRegisters(int iSlave, int iAddress, out float fReadData)
        {
            bool bReturn = false;
            fReadData = 9f;

            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public override bool HLReadInputRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            iReadData = 9;

            do
            {


                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// int형 데이터 읽기
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iReadCount"></param>
        /// <param name="iReadData"></param>
        /// <returns></returns>
        public override bool HLReadHoldingRegisters(int iSlave, int iAddress, int iReadCount, out int iReadData)
        {
            bool bReturn = false;
            iReadData = 9;

            do
            {

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iAddress"></param>
        /// <param name="iData"></param>
        /// <returns></returns>
        public override bool HLWriteHoldingRegisters(int iSlave, int iAddress, int iData)
        {
            bool bReturn = false;

            do
            {


                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {

        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceTemperatureAbstract.CTemperatureError)m_objError.Clone();
        }

    }
}
