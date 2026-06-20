using System.Collections.Generic;

namespace HLDevice.FileInterface
{
    public class CDeviceFileInterfaceVirtual : Abstract.CDeviceFileInterfaceAbstract
    {
        CFileInterfaceError m_objError = new CFileInterfaceError();

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
        public override bool HLInitialize(CInitializeParameter objInitializeParameter)
        {
            bool bReturn = false;

            do
            {
                HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CInitializeParameter objParameter = new HLDeviceDLL.FileInterface.FTP.CDeviceFileInterfaceFTPDefine.CInitializeParameter();
                objParameter.objConnectInformation.strHostName = objInitializeParameter.objConnectInformation.strHostName;
                objParameter.objConnectInformation.strHostPort = objInitializeParameter.objConnectInformation.strHostPort;
                objParameter.objConnectInformation.strUserName = objInitializeParameter.objConnectInformation.strUserName;
                objParameter.objConnectInformation.strUserPassword = objInitializeParameter.objConnectInformation.strUserPassword;

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
        /// 접속 정보 변경
        /// </summary>
        /// <param name="objConnectInformation"></param>
        /// <returns></returns>
        public override bool HLSetConnectInformation(HLDevice.Abstract.CDeviceFileInterfaceAbstract.CInitializeParameter.structureConnectInformation objConnectInformation)
        {
            bool bReturn = false;
            do
            {

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
                base.HLEventUpLoadComplete(bReturn);
                bReturn = true;
            } while (false);

            return bReturn;
        }

        public override bool HLUploadFiles(string localDirectoryPath, string uploadDirectoryPath, List<string> fileNames)
        {
            bool bReturn = false;
            do
            {
                base.RaiseUploadFileListChanged(fileNames.Count, fileNames.Count, string.Empty, uploadDirectoryPath);
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
                bReturn = true;
                base.HLEventDownLoadComplete(bReturn);
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

                bReturn = true;
            } while (false);

            return bReturn;
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

            return lFileSize;
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
        public override CFileInterfaceError HLGetErrorCode()
        {
            return (CFileInterfaceError)m_objError.Clone();
        }

    }
}
