using System.Collections.Generic;

namespace HLDevice.FileInterface
{
    public class CDeviceFileInterfaceFTP : Abstract.CDeviceFileInterfaceAbstract
    {
        CFileInterfaceError m_objError = new CFileInterfaceError();
        HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTP m_objFileInterface = new HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTP();

        /// <summary>
        /// 해제
        /// </summary>
        /// <returns></returns>
        public override string HLGetVersion()
        {
            return m_objFileInterface.GetVersion();
        }

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
                m_objFileInterface.HLEventDownLoadCompleted += m_objFileInterface_HLEventDownLoadCompleted;
                m_objFileInterface.HLEventDownLoadProgressPercentage += m_objFileInterface_HLEventDownLoadProgressPercentage;
                m_objFileInterface.HLEventUpLoadCompleted += m_objFileInterface_HLEventUpLoadCompleted;
                m_objFileInterface.HLEventUpLoadProgressPercentage += m_objFileInterface_HLEventUpLoadProgressPercentage;
                m_objFileInterface.HLEventUploadFileListChanged += objFileInterface_HLEventUploadFileListChanged;

                HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CInitializeParameter objParameter = new HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CInitializeParameter();
                objParameter.objConnectInformation.strHostName = objInitializeParameter.objConnectInformation.strHostName;
                objParameter.objConnectInformation.strHostPort = objInitializeParameter.objConnectInformation.strHostPort;
                objParameter.objConnectInformation.strUserName = objInitializeParameter.objConnectInformation.strUserName;
                objParameter.objConnectInformation.strUserPassword = objInitializeParameter.objConnectInformation.strUserPassword;

                if (false == m_objFileInterface.HLInitialize(objParameter))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 이벤트 - 업로드 퍼센테이지
        /// </summary>
        /// <param name="iProgressPercentage"></param>
        void m_objFileInterface_HLEventUpLoadProgressPercentage(int iProgressPercentage)
        {
            HLEventUpLoadPercentage(iProgressPercentage);
        }

        /// <summary>
        /// FTP 이벤트 - 업로드 완료 상태
        /// </summary>
        /// <param name="bComplete"></param>
        void m_objFileInterface_HLEventUpLoadCompleted(bool bComplete)
        {
            HLEventUpLoadComplete(bComplete);
        }

        /// <summary>
        /// FTP 이벤트 - 다운로드 퍼센테이지
        /// </summary>
        /// <param name="iProgressPercentage"></param>
        void m_objFileInterface_HLEventDownLoadProgressPercentage(int iProgressPercentage)
        {
            HLEventDownLoadPercentage(iProgressPercentage);
        }

        /// <summary>
        /// FTP 이벤트 - 다운로드 완료 상태
        /// </summary>
        /// <param name="bComplete"></param>
        void m_objFileInterface_HLEventDownLoadCompleted(bool bComplete)
        {
            HLEventDownLoadComplete(bComplete);
        }

        private void objFileInterface_HLEventUploadFileListChanged(int uploadTotalCount, int uploadCount, string uploadFileName, string uploadPath)
        {
            RaiseUploadFileListChanged(uploadTotalCount, uploadCount, uploadFileName, uploadPath);
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void HLDeInitialize()
        {
            m_objFileInterface.HLDeInitialize();
        }

        /// <summary>
        /// 접속 정보 변경
        /// </summary>
        /// <param name="objConnectInformation"></param>
        /// <returns></returns>
        public override bool HLSetConnectInformation(HLDevice.Abstract.CDeviceFileInterfaceAbstract.CInitializeParameter.structureConnectInformation objConnectInformation)
        {
            bool bReturn = false;
            do
            {
                HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CInitializeParameter.structureConnectInformation objParameter = new HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CInitializeParameter.structureConnectInformation();
                objParameter.strHostName = objConnectInformation.strHostName;
                objParameter.strHostPort = objConnectInformation.strHostPort;
                objParameter.strUserName = objConnectInformation.strUserName;
                objParameter.strUserPassword = objConnectInformation.strUserPassword;

                if (false == m_objFileInterface.HLSetConnectInformation(objParameter))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 파일 업로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="strLocalFilePathName"></param>
        /// <param name="strUpLoadFilePathName"></param>
        /// <returns></returns>
        public override bool HLUploadFile(string strLocalFilePathName, string strUpLoadFilePathName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFileInterface.HLUploadFile(strLocalFilePathName, strUpLoadFilePathName))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLUploadFiles(string localDirectoryPath, string uploadDirectoryPath, List<string> fileNames)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFileInterface.HLUploadFiles(localDirectoryPath, uploadDirectoryPath, fileNames))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 파일 다운로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="strLocalFilePathName"></param>
        /// <param name="strDownLoadFilePathName"></param>
        /// <returns></returns>
        public override bool HLDownloadFile(string strLocalFilePathName, string strDownLoadFilePathName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFileInterface.HLDownloadFile(strLocalFilePathName, strDownLoadFilePathName))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 파일 이름 바꾸기
        /// </summary>
        /// <param name="strRenameFilePathName"></param>
        /// <param name="strRename"></param>
        /// <returns></returns>
        public override bool HLRenameFile(string strRenameFilePathName, string strRename)
        {
            bool bReturn = false;
            do
            {

                if (false == m_objFileInterface.HLRenameFile(strRenameFilePathName, strRename))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 파일 삭제
        /// </summary>
        /// <param name="strDeleteFilePathName"></param>
        /// <returns></returns>
        public override bool HLDeleteFile(string strDeleteFilePathName)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFileInterface.HLDeleteFile(strDeleteFilePathName))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 파일 리스트
        /// </summary>
        /// <param name="strDirectoryPath"></param>
        /// <returns></returns>
        public override List<string> HLGetAllFiles(string strDirectoryPath)
        {
            List<string> strReturn = new List<string>();

            strReturn = m_objFileInterface.HLGetAllFiles(strDirectoryPath);

            return strReturn;
        }

        /// <summary>
        /// FTP 파일 사이즈
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public override long HLGetFileSize(string strFilePath)
        {
            long lFileSize = 0;

            lFileSize = m_objFileInterface.HLGetFileSize(strFilePath);

            return lFileSize;
        }

        /// <summary>
        /// FTP 폴더 생성
        /// </summary>
        /// <param name="strCreateDirectoryPath"></param>
        /// <returns></returns>
        public override bool HLCreateDirectory(string strCreateDirectoryPath)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFileInterface.HLCreateDirectory(strCreateDirectoryPath))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 폴더 삭제
        /// </summary>
        /// <param name="strDeleteDirectoryPath"></param>
        /// <returns></returns>
        public override bool HLDeleteDirectory(string strDeleteDirectoryPath)
        {
            bool bReturn = false;
            do
            {
                if (false == m_objFileInterface.HLDeleteFile(strDeleteDirectoryPath))
                {
                    MakeError();
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// DLL에서 나온 에러를 현재 클래스에 맞게 변환한다
        /// </summary>
        private void MakeError()
        {
            HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CFTPError objError = m_objFileInterface.HLGetErrorCode();
            m_objError.iReturnCode = objError.iReturnCode;
            m_objError.strEventTime = objError.strEventTime;
            m_objError.strFunctionName = objError.strFunctionName;
            m_objError.strMessage = objError.strMessage;
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public override CFileInterfaceError HLGetErrorCode()
        {
            return (CFileInterfaceError)m_objError.Clone();
        }

    }
}
