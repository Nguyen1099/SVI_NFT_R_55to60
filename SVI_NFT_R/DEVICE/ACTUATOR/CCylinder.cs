using System.IO;

namespace SVI_NFT_R
{
    public class CCylinder : CCylinderAbstract
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="objDocument"></param>
        public CCylinder(CDocument objDocument)
            : base(objDocument)
        {

        }

        /// <summary>
        /// 실린더 초기화 함수
        /// </summary>
        /// <param name="objIO"></param>
        /// <param name="objCylinderInitialize"></param>
        /// <returns></returns>
        public override bool Initialize(HLDevice.CDeviceIO objIO, CCylinderInitializeParameter objCylinderInitialize)
        {
            bool bReturn = false;
            do
            {
                // IO 객체 연결
                m_objIO = objIO;
                // 실린더 데이터 정보 객체 생성
                m_objCylinderDataParameter = new CCylinderDataParameter();
                // 초기화 객체 생성
                m_objCylinderInitializeParameter = new CCylinderInitializeParameter();
                m_objCylinderInitializeParameter = objCylinderInitialize;
                mCylinderType = objCylinderInitialize.eCylinderType;

                // 실린더 데이터 로딩.
                if (false == LoadCylinderData(m_objCylinderInitializeParameter.strFilePath, m_objCylinderInitializeParameter.strFileName))
                {
                    break;
                }

                // 실린더 데이터 저장
                if (false == SaveCylinderData(m_objCylinderInitializeParameter.strFilePath, m_objCylinderInitializeParameter.strFileName))
                {
                    break;
                }

                ECylinderStatus currentState = default(ECylinderStatus);
                GetCylinderStatus(ref currentState);

                m_eCommand = (ECylinderCommand)currentState;
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
        /// 실린더 커맨드 수행
        /// </summary>
        /// <param name="eCommand"></param>
        /// <param name="eSensorIgnore"></param>
        /// <returns></returns>
        public override bool SetCylinderCommand(ECylinderCommand eCommand, ESensorCheck eSensorIgnore = ESensorCheck.CHECK)
        {
            return base.SetCylinderCommand(eCommand, eSensorIgnore);
        }

        /// <summary>
        /// 실린더 상태 확인
        /// </summary>
        /// <param name="eStatusType"></param>
        /// <returns></returns>
        public override bool GetCylinderStatus(ref ECylinderStatus eStatusType)
        {
            return base.GetCylinderStatus(ref eStatusType);
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public override bool LoadCylinderData(string strFilePath, string strFileName)
        {
            bool bReturn = false;

            do
            {
                string strPath = string.Format("{0:S}\\DATA\\{1:S}", strFilePath, strFileName);
                ClassINI objINI = new ClassINI(strPath);

                m_objCylinderDataParameter.iFirstMoveAfterDelayTime = objINI.GetInt32(m_objCylinderInitializeParameter.strCylinderName, "iFirstMoveAfterDelayTime", 0);
                m_objCylinderDataParameter.iSecondMoveAfterDelayTime = objINI.GetInt32(m_objCylinderInitializeParameter.strCylinderName, "iSecondMoveAfterDelayTime", 0);
                m_objCylinderDataParameter.iMiddleMoveAfterDelayTime = objINI.GetInt32(m_objCylinderInitializeParameter.strCylinderName, "iMiddleMoveAfterDelayTime", 0);
                m_objCylinderDataParameter.iCylinderTimeOut = objINI.GetInt32(m_objCylinderInitializeParameter.strCylinderName, "iCylinderTimeOut", 5000);

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileName"></param>
        /// <param name="objCylinderData"></param>
        /// <returns></returns>
        public override bool SaveCylinderData(string strFilePath, string strFileName, CCylinderDataParameter objCylinderData)
        {
            bool bReturn = false;
            do
            {
                if (0 >= strFilePath.Length)
                    strFilePath = m_objCylinderInitializeParameter.strFilePath;
                if (0 >= strFileName.Length)
                    strFileName = m_objCylinderInitializeParameter.strFileName;
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

                if (m_objCylinderDataParameter.iFirstMoveAfterDelayTime != objCylinderData.iFirstMoveAfterDelayTime)
                {
                    objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iFirstMoveAfterDelayTime", objCylinderData.iFirstMoveAfterDelayTime);
                }

                if (m_objCylinderDataParameter.iSecondMoveAfterDelayTime != objCylinderData.iSecondMoveAfterDelayTime)
                {
                    objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iSecondMoveAfterDelayTime", objCylinderData.iSecondMoveAfterDelayTime);
                }

                if (m_objCylinderDataParameter.iMiddleMoveAfterDelayTime != objCylinderData.iMiddleMoveAfterDelayTime)
                {
                    objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iMiddleMoveAfterDelayTime", objCylinderData.iMiddleMoveAfterDelayTime);
                }

                if (m_objCylinderDataParameter.iCylinderTimeOut != objCylinderData.iCylinderTimeOut)
                {
                    objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iCylinderTimeOut", objCylinderData.iCylinderTimeOut);
                }

                m_objCylinderDataParameter = (CCylinderDataParameter)objCylinderData.Clone();
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
        protected override bool SaveCylinderData(string strFilePath, string strFileName)
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
                objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iFirstMoveAfterDelayTime", m_objCylinderDataParameter.iFirstMoveAfterDelayTime);
                objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iSecondMoveAfterDelayTime", m_objCylinderDataParameter.iSecondMoveAfterDelayTime);
                objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iMiddleMoveAfterDelayTime", m_objCylinderDataParameter.iMiddleMoveAfterDelayTime);
                objINI.WriteValue(m_objCylinderInitializeParameter.strCylinderName, "iCylinderTimeOut", m_objCylinderDataParameter.iCylinderTimeOut);

                bReturn = true;
            } while (false);

            return bReturn;
        }
    }
}
