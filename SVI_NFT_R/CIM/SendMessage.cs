using System.Threading;

namespace SVI_NFT_R
{
    class SendMessage
    {
        public WaitCallback SendCallback { get { return mSendCallback; } }
        public object SendParameter { get { return mSendParameter; } }
        public ManualResetEvent WaitHandle { get { return mWaitHandle; } }
        private readonly WaitCallback mSendCallback;
        private readonly object mSendParameter;
        private ManualResetEvent mWaitHandle;

        public SendMessage(WaitCallback sendCallback, object sendParameter)
        {
            mSendCallback = sendCallback;
            mSendParameter = sendParameter;
        }

        public void SetSendFinish()
        {
            if (null != mWaitHandle)
            {
                mWaitHandle.Set();
            }
        }

        public WaitHandle GetWaitHandle()
        {
            if (null == mWaitHandle)
            {
                mWaitHandle = new ManualResetEvent(false);
            }
            return mWaitHandle;
        }
    }
}
