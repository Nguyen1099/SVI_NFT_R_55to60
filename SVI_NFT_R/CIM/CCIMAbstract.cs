using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using urLinkDll;

namespace SVI_NFT_R
{
    public abstract class CCIMAbstract
    {
        /// <summary>
        /// 프로세스 종료
        /// </summary>
        protected bool m_bThreadExit;
        /// <summary>
        /// 쓰레드
        /// </summary>
        protected Thread m_ThreadProcess;
        /// <summary>
        /// 쓰레드 상태
        /// </summary>
        protected CCIMDefine.EProcessState m_eStatus;
        /// <summary>
        /// 통신 객체
        /// </summary>
        protected urLinkDllCommunicator m_objCommunicator;
        /// <summary>
        /// 도튜먼트
        /// </summary>
        protected CDocument m_objDocument;
        /// <summary>
        /// XML 직렬화시 네임스페이스
        /// </summary>
        public XmlSerializerNamespaces ns;
        /// <summary>
        /// 리시브된 데이터가 있는지 확인하는 Flag
        /// </summary>
        protected bool m_bReceiveData;
        /// <summary>
        /// 장비 이름
        /// </summary>
        protected string m_strEquipmentID;
        /// <summary>
        /// string UTF8 변환
        /// </summary>
        public sealed class ExtentendStringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get
                {
                    return new UTF8Encoding(false);
                }
            }
        }

        public bool WaitForReceive(TimeSpan timeout)
        {
            bool bResult = false;
            Stopwatch sw = Stopwatch.StartNew();

            do
            {
                while (
                    sw.Elapsed < timeout
                    && CCIMDefine.EProcessState.STS_RECEIVED != m_eStatus
                    )
                {
                    Thread.Sleep(1);
                }

                if (sw.Elapsed > timeout)
                {
                    break;
                }

                bResult = true;
            } while (false);

            return bResult;
        }

        /// <summary>
        /// 쓰레드 진입 여부
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsStatus();
        /// <summary>
        /// 해당 프로세스 수행
        /// </summary>
        /// <returns></returns>
        protected abstract bool DoProcess();
        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        protected abstract bool Initialize();
        /// <summary>
        /// 해제
        /// </summary>
        public abstract void DeInitialize();
        /// <summary>
        /// 상태 가져오기
        /// </summary>
        /// <returns></returns>
        public abstract CCIMDefine.EProcessState GetStatus();
    }
}
