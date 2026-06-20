using HLDevice.Abstract;
using SVI_NFT_R;
using System.Collections.Generic;
using System.IO;

namespace HLDevice
{
    public class CDeviceFileInterface
    {
        CDeviceFileInterfaceAbstract m_objFileInterface;
        private CDocument m_objDocument;

        public int UploadListTotalCount { get; private set; } = 0;
        public int UploadListCount { get; private set; } = 0;
        public string UploadListFileName { get; private set; } = string.Empty;
        public string UploadListPath { get; private set; } = string.Empty;
        public bool m_bDownLoadComplete = true;
        public bool m_bUploadComplete = true;
        public int m_iDownProgressPercentage = 100;
        public int m_iUpProgressPercentage = 100;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="objInitializeParameter"></param>
        /// <returns></returns>
        public bool HLInitialize(CDocument document, CConfig.CFileInterfaceParameter objInitializeParameter)
        {
            m_objDocument = document;

            //if (CDefine.ESimulationMode.SIMULATION_MODE_ON == m_objDocument.m_objConfig.GetSystemParameter().eSimulationMode)
            //{
            //    m_objFileInterface = new FileInterface.CDeviceFileInterfaceVirtual();
            //}
            //else
            {
                m_objFileInterface = new FileInterface.CDeviceFileInterfaceFTP();
            }

            m_objFileInterface.HLEventDownLoadCompleted += m_objFileInterface_HLEventDownLoadCompleted;
            m_objFileInterface.HLEventUpLoadCompleted += m_objFileInterface_HLEventUpLoadCompleted;
            m_objFileInterface.HLEventUpLoadProgressPercentage += m_objFileInterface_HLEventUpLoadProgressPercentage;
            m_objFileInterface.HLEventDownLoadProgressPercentage += m_objFileInterface_HLEventDownLoadProgressPercentage;
            m_objFileInterface.HLEventUploadFileListChanged += objFileInterface_HLEventUploadFileListChanged;

            CDeviceFileInterfaceAbstract.CInitializeParameter objFileInterfaceParameter = new CDeviceFileInterfaceAbstract.CInitializeParameter();
            objFileInterfaceParameter.objConnectInformation.strHostName = objInitializeParameter.objConnectInformation.strHostName;
            objFileInterfaceParameter.objConnectInformation.strHostPort = objInitializeParameter.objConnectInformation.strHostPort;
            objFileInterfaceParameter.objConnectInformation.strUserName = objInitializeParameter.objConnectInformation.strUserName;
            objFileInterfaceParameter.objConnectInformation.strUserPassword = objInitializeParameter.objConnectInformation.strUserPassword;

            return m_objFileInterface.HLInitialize(objFileInterfaceParameter);
        }

        /// <summary>
        /// FTP 이벤트 - 다운로드 퍼센테이지
        /// </summary>
        /// <param name="iProgressPercentage"></param>
        void m_objFileInterface_HLEventDownLoadProgressPercentage(int iProgressPercentage)
        {
            m_iDownProgressPercentage = iProgressPercentage;
        }

        /// <summary>
        /// FTP 이벤트 - 업로드 완료 상태
        /// </summary>
        /// <param name="bComplete"></param>
        void m_objFileInterface_HLEventUpLoadCompleted(bool bComplete)
        {
            m_bUploadComplete = bComplete;
        }

        /// <summary>
        /// FTP 이벤트 - 업로드 퍼센테이지
        /// </summary>
        /// <param name="iProgressPercentage"></param>
        void m_objFileInterface_HLEventUpLoadProgressPercentage(int iProgressPercentage)
        {
            m_iUpProgressPercentage = iProgressPercentage;
        }

        /// <summary>
        /// FTP 이벤트 - 다운로드 완료 상태
        /// </summary>
        /// <param name="bComplete"></param>
        void m_objFileInterface_HLEventDownLoadCompleted(bool bComplete)
        {
            m_bDownLoadComplete = bComplete;
        }

        private void objFileInterface_HLEventUploadFileListChanged(int uploadTotalCount, int uploadCount, string uploadFileName, string uploadPath)
        {
            UploadListTotalCount = uploadTotalCount;
            UploadListCount = uploadCount;
            UploadListFileName = uploadFileName;
            UploadListPath = uploadPath;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
            m_objFileInterface.HLDeInitialize();
        }

        /// <summary>
        /// 버전
        /// </summary>
        /// <returns></returns>
        public string HLGetVersion()
        {
            return m_objFileInterface.HLGetVersion();
        }

        /// <summary>
        /// 접속 정보 변경
        /// </summary>
        /// <param name="objConnectInformation"></param>
        /// <returns></returns>
        public bool HLSetConnectInformation(Abstract.CDeviceFileInterfaceAbstract.CInitializeParameter.structureConnectInformation objConnectInformation)
        {
            return m_objFileInterface.HLSetConnectInformation(objConnectInformation);
        }


        /// <summary>
        /// 파일 업로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="strLocalFilePathName"></param>
        /// <param name="strUpLoadFilePathName"></param>
        /// <returns></returns>
        public bool HLUploadFile(string strLocalFilePathName, string strUpLoadFilePathName)
        {
            // 완료 플래그 클리어 시점
            UploadListPath = Path.GetDirectoryName(strUpLoadFilePathName);
            UploadListFileName = Path.GetFileName(strUpLoadFilePathName);
            UploadListTotalCount = 1;
            UploadListCount = 1;
            m_bUploadComplete = false;
            return m_objFileInterface.HLUploadFile(strLocalFilePathName, strUpLoadFilePathName);
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
            // 완료 플래그 클리어 시점
            UploadListTotalCount = fileNames.Count;
            UploadListCount = 0;
            m_bUploadComplete = false;
            return m_objFileInterface.HLUploadFiles(localDirectoryPath, uploadDirectoryPath, fileNames);
        }

        /// <summary>
        /// 파일 다운로드 폴더는 자동생성됨
        /// </summary>
        /// <param name="strLocalFilePathName"></param>
        /// <param name="strDownLoadFilePathName"></param>
        /// <returns></returns>
        public bool HLDownloadFile(string strLocalFilePathName, string strDownLoadFilePathName)
        {
            // 완료 플래그 클리어 시점
            m_bDownLoadComplete = false;
            return m_objFileInterface.HLDownloadFile(strLocalFilePathName, strDownLoadFilePathName);
        }

        /// <summary>
        /// FTP 파일 이름 바꾸기
        /// </summary>
        /// <param name="strRenameFilePathName"></param>
        /// <param name="strRename"></param>
        /// <returns></returns>
        public bool HLRenameFile(string strRenameFilePathName, string strRename)
        {
            return m_objFileInterface.HLRenameFile(strRenameFilePathName, strRename);
        }

        /// <summary>
        /// FTP 파일 삭제
        /// </summary>
        /// <param name="strDeleteFilePathName"></param>
        /// <returns></returns>
        public bool HLDeleteFile(string strDeleteFilePathName)
        {
            return m_objFileInterface.HLDeleteFile(strDeleteFilePathName);
        }

        /// <summary>
        /// FTP 폴더 생성
        /// </summary>
        /// <param name="strDeleteDirectoryPath"></param>
        /// <returns></returns>
        public bool HLCreateDirectory(string strCreateDirectoryPath)
        {
            return m_objFileInterface.HLCreateDirectory(strCreateDirectoryPath);
        }

        /// <summary>
        /// FTP 폴더 삭제
        /// </summary>
        /// <param name="strDeleteDirectoryPath"></param>
        /// <returns></returns>
        public bool HLDeleteDirectory(string strDeleteDirectoryPath)
        {
            return m_objFileInterface.HLDeleteFile(strDeleteDirectoryPath);
        }

        /// <summary>
        /// FTP 파일 리스트
        /// </summary>
        /// <param name="strDirectoryPath"></param>
        /// <returns></returns>
        public List<string> HLGetAllFiles(string strDirectoryPath)
        {
            return m_objFileInterface.HLGetAllFiles(strDirectoryPath);
        }

        /// <summary>
        /// FTP 파일 사이즈
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public long HLGetFileSize(string strFilePath)
        {
            return m_objFileInterface.HLGetFileSize(strFilePath);
        }

        /// <summary>
        /// 현재 알람 상태 정보를 리턴한다.
        /// </summary>
        /// <returns></returns>
        public Abstract.CDeviceFileInterfaceAbstract.CFileInterfaceError HLGetErrorCode()
        {
            return m_objFileInterface.HLGetErrorCode();
        }
    }
}
