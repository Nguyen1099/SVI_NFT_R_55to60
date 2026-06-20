using System.Linq;

namespace HLDevice.FFU
{
    public class CDeviceFFUVirtual : HLDevice.Abstract.CDeviceFFUAbstract
    {
        private readonly HLDevice.Abstract.CDeviceFFUAbstract.CFFUError m_objError = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUError();

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CDeviceFFUVirtual()
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
        public override bool HLInitialize(HLDevice.Abstract.CDeviceFFUAbstract.CInitializeParameter objInitializeParameter)
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLGetRequiredSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 50;
            bool bReturn = false;

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
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLSetRequiredSpeed(int iSlave, int iFFUIndex, int iValue)
        {
            bool bReturn = false;

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
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLSetAllRequiredSpeed(int iSlave, int iValue)
        {
            bool bReturn = false;
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
        /// <param name="iFFUIndex"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public override bool HLGetCurrentSpeed(int iSlave, int iFFUIndex, out int iValue)
        {
            iValue = 40;
            bool bReturn = false;
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
        /// <param name="iFFUIndex"></param>
        /// <param name="objStatus"></param>
        /// <returns></returns>
        public override bool HLGetStatus(int iSlave, int iFFUIndex, out HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus objStatus)
        {
            objStatus = new HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus();
            bool bReturn = false;

            do
            {
                objStatus[HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus.EStatus.AC_FAIL] = false;
                objStatus[HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus.EStatus.ERROR_COMMUNICATION] = false;
                objStatus[HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus.EStatus.MOTOR_RUN] = true;
                objStatus[HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus.EStatus.OVER_CURRENT] = true;
                objStatus[HLDevice.Abstract.CDeviceFFUAbstract.CFFUStatus.EStatus.REMOTE_STAUS] = true;

                bReturn = true;
            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSlave"></param>
        /// <param name="iFFUIndex"></param>
        /// <param name="bReset"></param>
        /// <returns></returns>
        public override bool HLSetReset(int iSlave, int iFFUIndex, bool bReset)
        {
            bool bReturn = false;
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
        /// <param name="bReset"></param>
        /// <returns></returns>
        public override bool HLSetAllReset(int iSlave, bool bReset)
        {
            bool bReturn = false;
            do
            {
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 모든 채널에 팬 속도 값
        /// </summary>
        /// <param name="readValues"></param>
        /// <returns></returns>
        public override bool HLGetFanSpeedAll(out int[] readValues)
        {
            readValues = Enumerable.Repeat(50, 32).ToArray();
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
        public override HLDevice.Abstract.CDeviceFFUAbstract.CFFUError HLGetErrorCode()
        {
            return (HLDevice.Abstract.CDeviceFFUAbstract.CFFUError)m_objError.Clone();
        }

    }
}
