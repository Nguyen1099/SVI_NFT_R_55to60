using System.Threading;

namespace SVI_NFT_R
{
    public class CDatabaseAbstract
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
        /// 프로세스 데이터베이스
        /// </summary>
        protected CProcessDatabase m_objProcessDatabase;
    }
}