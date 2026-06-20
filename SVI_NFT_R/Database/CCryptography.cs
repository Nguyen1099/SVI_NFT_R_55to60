using System;

namespace SVI_NFT_R
{
    public class CCryptography
    {
        /// <summary>
        /// 암호로 사용한 것은 Base64 인코딩
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public string SetEncoding(string strText)
        {
            string strReturn = null;

            do
            {
                try
                {
                    byte[] bData = new byte[strText.Length];
                    bData = System.Text.Encoding.UTF8.GetBytes(strText);
                    string strEncodeData = Convert.ToBase64String(bData);
                    strReturn = strEncodeData;
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return strReturn;
        }

        /// <summary>
        /// 암호로 사용한 것은 Base64 디코딩
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public string SetDecoding(string strText)
        {
            string strReturn = null;

            do
            {
                try
                {
                    System.Text.UTF8Encoding objEncoding = new System.Text.UTF8Encoding();
                    System.Text.Decoder objDecoder = objEncoding.GetDecoder();

                    byte[] bDecode = Convert.FromBase64String(strText);
                    int iCount = objDecoder.GetCharCount(bDecode, 0, bDecode.Length);
                    char[] charDecode = new char[iCount];
                    objDecoder.GetChars(bDecode, 0, bDecode.Length, charDecode, 0);
                    strReturn = new string(charDecode);
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                }
            } while (false);

            return strReturn;
        }
    }
}