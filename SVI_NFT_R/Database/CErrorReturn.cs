namespace SVI_NFT_R
{
    public class CErrorReturn
    {
        /// <summary>
        /// 에러 유무
        /// </summary>
        private bool _bError;
        public bool m_bError
        {
            get { return _bError; }
            set { _bError = value; }
        }
        /// <summary>
        /// 에러 메세지
        /// </summary>
        private string _strErrorMessage;
        public string m_strErrorMessage
        {
            get { return _strErrorMessage; }
            set { _strErrorMessage = value; }
        }
        /// <summary>
        /// 에러 발생 클래스 이름
        /// </summary>
        private string _strClassName;
        public string m_strClassName
        {
            get { return _strClassName; }
            set { _strClassName = value; }
        }
        /// <summary>
        /// 에러 발생 함수 이름
        /// </summary>
        private string _strFunctionName;
        public string m_strFunctionName
        {
            get { return _strFunctionName; }
            set { _strFunctionName = value; }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public CErrorReturn()
        {
            _bError = true;
            _strErrorMessage = "";
            _strClassName = "";
            _strFunctionName = "";
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="strClassName"></param>
        /// <param name="strFunctionName"></param>
        /// <returns></returns>
        public CErrorReturn(string strClassName, string strFunctionName)
        {
            _bError = true;
            _strErrorMessage = "";
            _strClassName = strClassName;
            _strFunctionName = strFunctionName;
        }
    }
}