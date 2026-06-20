using System;
using System.Collections.Generic;

namespace HLDevice.Abstract
{
    public abstract class CDeviceFileInterfaceAbstract
    {
        /// <summary>
        /// 초기화 파라미터
        /// </summary>
        public class CInitializeParameter : ICloneable
        {
            public struct structureConnectInformation
            {
                public string strHostName;
                public int strHostPort;
                public string strUserName;
                public string strUserPassword;

                public structureConnectInformation(structureConnectInformation objConnectInformation)
                {
                    strHostName = objConnectInformation.strHostName;
                    strHostPort = objConnectInformation.strHostPort;
                    strUserName = objConnectInformation.strUserName;
                    strUserPassword = objConnectInformation.strUserPassword;
                }

            }
            public structureConnectInformation objConnectInformation = new structureConnectInformation();

            public object Clone()
            {
                CInitializeParameter objInitializeParameter = new CInitializeParameter();
                objInitializeParameter.objConnectInformation = new structureConnectInformation(this.objConnectInformation);
                return objInitializeParameter;
            }
        }

        /// <summary>
        /// 알람 발생 확인 클래스
        /// </summary>
        public class CFileInterfaceError : ICloneable
        {
            /// <summary>이벤트 발생 시간</summary>
            public string strEventTime;
            /// <summary>수행된 함수 이름</summary>
            public string strFunctionName;
            /// <summary>알람 리턴 결과</summary>
            public int iReturnCode;
            /// <summary>알람 메세지</summary>
            public string strMessage;

            public object Clone()
            {
                CFileInterfaceError objError = new CFileInterfaceError();
                objError.strEventTime = this.strEventTime;
                objError.strFunctionName = this.strFunctionName;
                objError.iReturnCode = this.iReturnCode;
                objError.strMessage = this.strMessage;

                return objError;
            }
        }

        /// <summary>
        /// 다운로드 성공 하였거나 실패 하였을 경우 발생 이벤트
        /// </summary>
        public delegate void HLDownLoadCompleted(bool bComplete);
        public event HLDownLoadCompleted HLEventDownLoadCompleted;

        /// <summary>
        /// 다운로드 되고있는 파일 퍼센트 이벤트
        /// </summary>
        public delegate void HLDownLoadProgressPercentage(int iProgressPercentage);
        public event HLDownLoadProgressPercentage HLEventDownLoadProgressPercentage;

        /// <summary>
        /// 업로드 성공 하였거나 실패 하였을 경우 발생 이벤트
        /// </summary>
        public delegate void HLUpLoadCompleted(bool bComplete);
        public event HLUpLoadCompleted HLEventUpLoadCompleted;

        /// <summary>
        /// 업로드 되고있는 파일 퍼센트 이벤트
        /// </summary>
        public delegate void HLUpLoadProgressPercentage(int iProgressPercentage);
        public event HLUpLoadProgressPercentage HLEventUpLoadProgressPercentage;

        public delegate void HLUploadFileListChanged(int uploadTotalCount, int uploadCount, string uploadFileName, string uploadPath);
        public event HLUploadFileListChanged HLEventUploadFileListChanged;

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
        /// 접속 정보 변경
        /// </summary>
        /// <param name="objConnectInformation"></param>
        /// <returns></returns>
        public virtual bool HLSetConnectInformation(CInitializeParameter.structureConnectInformation objConnectInformation)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }


        /// <summary>
        /// 파일 업로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="strLocalFilePathName"></param>
        /// <param name="strUpLoadFilePathName"></param>
        /// <returns></returns>
        public virtual bool HLUploadFile(string strLocalFilePathName, string strUpLoadFilePathName)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 파일들을 업로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="localDirectoryPath"></param>
        /// <param name="uploadDirectoryPath"></param>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public virtual bool HLUploadFiles(string localDirectoryPath, string uploadDirectoryPath, List<string> fileNames)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 파일 다운로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="strLocalFilePathName"></param>
        /// <param name="strDownLoadFilePathName"></param>
        /// <returns></returns>
        public virtual bool HLDownloadFile(string strLocalFilePathName, string strDownLoadFilePathName)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 파일 이름 바꾸기
        /// </summary>
        /// <param name="strRenameFilePathName"></param>
        /// <param name="strRename"></param>
        /// <returns></returns>
        public virtual bool HLRenameFile(string strRenameFilePathName, string strRename)
        {
            bool bReturn = false;
            do
            {


            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 파일 삭제
        /// </summary>
        /// <param name="strDeleteFilePathName"></param>
        /// <returns></returns>
        public virtual bool HLDeleteFile(string strDeleteFilePathName)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 폴더 생성
        /// </summary>
        /// <param name="strCreateDirectoryPath"></param>
        /// <returns></returns>
        public virtual bool HLCreateDirectory(string strCreateDirectoryPath)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 폴더 삭제
        /// </summary>
        /// <param name="strDeleteDirectoryPath"></param>
        /// <returns></returns>
        public virtual bool HLDeleteDirectory(string strDeleteDirectoryPath)
        {
            bool bReturn = false;
            do
            {

            } while (false);

            return bReturn;
        }

        /// <summary>
        /// FTP 파일 리스트
        /// </summary>
        /// <param name="strDirectoryPath"></param>
        /// <returns></returns>
        public virtual List<string> HLGetAllFiles(string strDirectoryPath)
        {
            List<string> strReturn = new List<string>();

            return strReturn;
        }

        /// <summary>
        /// FTP 파일 사이즈
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public virtual long HLGetFileSize(string strFilePath)
        {
            long lFileSize = 0;

            return lFileSize;
        }

        /// <summary>
        /// 결과를 받아 이벤트를 발생시킴
        /// </summary>
        /// <param name="bComplete"></param>
        protected void HLEventDownLoadComplete(bool bComplete)
        {
            if (null != HLEventDownLoadCompleted)
                HLEventDownLoadCompleted(bComplete);
        }

        /// <summary>
        /// 결과를 받아 이벤트를 발생시킴
        /// </summary>
        /// <param name="iProgressPercentage"></param>
        protected void HLEventDownLoadPercentage(int iProgressPercentage)
        {
            HLEventDownLoadProgressPercentage?.Invoke(iProgressPercentage);
        }

        /// <summary>
        /// 결과를 받아 이벤트를 발생시킴
        /// </summary>
        /// <param name="bComplete"></param>
        protected void HLEventUpLoadComplete(bool bComplete)
        {
            HLEventUpLoadCompleted?.Invoke(bComplete);
        }

        /// <summary>
        /// 결과를 받아 이벤트를 발생시킴
        /// </summary>
        /// <param name="iProgressPercentage"></param>
        protected void HLEventUpLoadPercentage(int iProgressPercentage)
        {
            HLEventUpLoadProgressPercentage?.Invoke(iProgressPercentage);
        }

        protected void RaiseUploadFileListChanged(int uploadTotalCount, int uploadCount, string uploadFileName, string uploadPath)
        {
            HLEventUploadFileListChanged?.Invoke(uploadTotalCount, uploadCount, uploadFileName, uploadPath);
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public virtual CFileInterfaceError HLGetErrorCode()
        {
            CFileInterfaceError objError = new CFileInterfaceError();
            return objError;
        }
    }
}
