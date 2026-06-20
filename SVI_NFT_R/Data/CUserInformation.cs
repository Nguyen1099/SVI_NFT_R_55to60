namespace SVI_NFT_R
{
    public class CUserInformation
    {
        /// <summary>
        /// 유저 아이디
        /// </summary>
        public string m_strID;
        /// <summary>
        /// 유저 이름
        /// </summary>
        public string m_strName;
        /// <summary>
        /// 패스워드 (인코딩)
        /// </summary>
        public string m_strPassword;
        /// <summary>
        /// 권한 레벨
        /// </summary>
        public CDefine.EUserAuthorityLevel m_eAuthorityLevel;
        /// <summary>
        /// CIM 정보로 로그인 여부
        /// </summary>
        public bool IsLoginFromCimInformation { get; set; } = false;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        public CUserInformation()
        {
            m_strID = "Default";
            m_strName = "Default";
            m_strPassword = "";
            m_eAuthorityLevel = CDefine.EUserAuthorityLevel.USER_AUTHORITY_LEVEL_FINAL;
            IsLoginFromCimInformation = false;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strName"></param>
        /// <param name="strPassword"></param>
        /// <param name="eAuthorityLevel"></param>
        /// <returns></returns>
        public CUserInformation(string strID, string strName, string strPassword, CDefine.EUserAuthorityLevel eAuthorityLevel)
        {
            m_strID = strID;
            m_strName = strName;
            m_strPassword = strPassword;
            m_eAuthorityLevel = eAuthorityLevel;
            IsLoginFromCimInformation = false;
        }

        /// <summary>
        /// 복사
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            CUserInformation objUserInformation = new CUserInformation();
            objUserInformation.m_strID = this.m_strID;
            objUserInformation.m_strName = this.m_strName;
            objUserInformation.m_strPassword = this.m_strPassword;
            objUserInformation.m_eAuthorityLevel = this.m_eAuthorityLevel;
            objUserInformation.IsLoginFromCimInformation = this.IsLoginFromCimInformation;
            return objUserInformation;
        }
    }
}