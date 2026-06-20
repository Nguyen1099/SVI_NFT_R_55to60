namespace SVI_NFT_R.CIM
{
    public class LoginAcknowledge
    {
        public bool ReceivedMessage { get; set; } = false;
        public bool Suceess { get; set; } = false;
        public string Message { get; set; } = string.Empty;

        public void Reset()
        {
            ReceivedMessage = false;
            Suceess = false;
            Message = string.Empty;
        }
    }
}