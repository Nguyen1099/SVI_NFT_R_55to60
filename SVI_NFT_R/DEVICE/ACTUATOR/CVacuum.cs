using System.Diagnostics;
using System.IO;

namespace SVI_NFT_R
{
    public class CVacuum : CVacuumAbstract
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        /// <returns></returns>
        public CVacuum(CDocument objDocument)
            : base(objDocument)
        {

        }

        /// <summary>
        /// 진공 초기화 함수
        /// </summary>
        /// <param name="objIO"></param>
        /// <param name="objVacuumInitialize"></param>
        /// <returns></returns>
        public override bool Initialize(HLDevice.CDeviceIO objIO, CVacuumInitializeParameter objVacuumInitialize)
        {
            bool bReturn = false;
            do
            {
                // IO 객체 연결
                m_objIO = objIO;
                // 실린더 데이터 정보 객체 생성
                m_objVacuumDataParameter = new CVacuumDataParameter();
                // 초기화 객체 생성
                m_objVacuumInitializeParameter = new CVacuumInitializeParameter();
                m_objVacuumInitializeParameter = objVacuumInitialize;

                switch (m_objVacuumInitializeParameter.eVacuumSolenoidType)
                {
                    case EVacuumSolenoidType.SINGLE_SOLENOID:
                        if (m_objVacuumInitializeParameter.iSuccessVacuumDetectCount > 1)
                        {
                            m_objVacuumInitializeParameter.iSuccessVacuumDetectCount = 1;
                        }
                        break;
                    case EVacuumSolenoidType.DOUBLE_SOLENOID:
                        if (m_objVacuumInitializeParameter.iSuccessVacuumDetectCount > 2)
                        {
                            m_objVacuumInitializeParameter.iSuccessVacuumDetectCount = 2;
                        }
                        break;
                    case EVacuumSolenoidType.TRIPLE_SOLENOID:
                        if (m_objVacuumInitializeParameter.iSuccessVacuumDetectCount > 3)
                        {
                            m_objVacuumInitializeParameter.iSuccessVacuumDetectCount = 3;
                        }
                        break;
                    case EVacuumSolenoidType.QUARD_SOLENOID:
                        if (m_objVacuumInitializeParameter.iSuccessVacuumDetectCount > 4)
                        {
                            m_objVacuumInitializeParameter.iSuccessVacuumDetectCount = 4;
                        }
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                // 버큠 데이터 로딩.
                if (false == LoadVacuumData(m_objVacuumInitializeParameter.strFilePath, m_objVacuumInitializeParameter.strFileName))
                {
                    break;
                }

                // 버큠 데이터 저장
                if (false == SaveVacuumData(m_objVacuumInitializeParameter.strFilePath, m_objVacuumInitializeParameter.strFileName))
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void DeInitialize()
        {

        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public override bool LoadVacuumData(string strFilePath, string strFileName)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format("{0:S}\\DATA\\{1:S}", strFilePath, strFileName);
                ClassINI objINI = new ClassINI(strPath);

                m_objVacuumDataParameter.iVacuumOffDelayTime = objINI.GetInt32(m_objVacuumInitializeParameter.strVacuumName, "iVacuumOffDelayTime", 500);
                m_objVacuumDataParameter.iVacuumTimeOut = objINI.GetInt32(m_objVacuumInitializeParameter.strVacuumName, "iVacuumTimeOut", 5000);
                m_objVacuumDataParameter.SolenoidDelayTime = objINI.GetInt32(m_objVacuumInitializeParameter.strVacuumName, "SolenoidDelayTime", 0);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <param name="objVacuumData"></param>
        /// <returns></returns>
        public override bool SaveVacuumData(string strFilePath, string strFileName, CVacuumDataParameter objVacuumData)
        {
            bool bReturn = false;
            do
            {
                if (0 >= strFilePath.Length)
                    strFilePath = m_objVacuumInitializeParameter.strFilePath;
                if (0 >= strFileName.Length)
                    strFileName = m_objVacuumInitializeParameter.strFileName;
                // 파일 경로에 파일 유무를 판단하고 없을 경우 생성 후 저장.
                string strPath = string.Format("{0:S}\\DATA", strFilePath);
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strFilePath);
                }

                strPath = string.Format("{0:S}\\DATA\\{1:S}", strFilePath, strFileName);
                Constants.SaveBackupFile(strPath);
                ClassINI objINI = new ClassINI(strPath);

                if (m_objVacuumDataParameter.iVacuumOffDelayTime != objVacuumData.iVacuumOffDelayTime)
                {
                    objINI.WriteValue(m_objVacuumInitializeParameter.strVacuumName, "iVacuumOffDelayTime", objVacuumData.iVacuumOffDelayTime);
                }

                if (m_objVacuumDataParameter.iVacuumTimeOut != objVacuumData.iVacuumTimeOut)
                {
                    objINI.WriteValue(m_objVacuumInitializeParameter.strVacuumName, "iVacuumTimeOut", objVacuumData.iVacuumTimeOut);
                }

                if (m_objVacuumDataParameter.SolenoidDelayTime != objVacuumData.SolenoidDelayTime)
                {
                    objINI.WriteValue(m_objVacuumInitializeParameter.strVacuumName, "SolenoidDelayTime", objVacuumData.SolenoidDelayTime);
                }

                m_objVacuumDataParameter = objVacuumData.DeepClone();

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        protected override bool SaveVacuumData(string strFilePath, string strFileName)
        {
            bool bReturn = false;

            do
            {
                // 파일 경로에 파일 유무를 판단하고 없을 경우 생성 후 저장.
                string strPath = string.Format("{0:S}\\DATA", strFilePath);
                if (false == Directory.Exists(strPath))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(strFilePath);
                }

                strPath = string.Format("{0:S}\\DATA\\{1:S}", strFilePath, strFileName);

                ClassINI objINI = new ClassINI(strPath);
                objINI.WriteValue(m_objVacuumInitializeParameter.strVacuumName, "iVacuumOffDelayTime", m_objVacuumDataParameter.iVacuumOffDelayTime);
                objINI.WriteValue(m_objVacuumInitializeParameter.strVacuumName, "iVacuumTimeOut", m_objVacuumDataParameter.iVacuumTimeOut);
                objINI.WriteValue(m_objVacuumInitializeParameter.strVacuumName, "SolenoidDelayTime", m_objVacuumDataParameter.SolenoidDelayTime);

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
