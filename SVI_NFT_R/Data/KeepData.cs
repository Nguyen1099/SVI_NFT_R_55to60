using System;

namespace SVI_NFT_R
{
    [Serializable]
    public class KeepData
    {
        public TimeSpan RegistElapsed { get { return (DateTime.Now - mRegistTime) - EquipmentStopTime; } }
        public TimeSpan EquipmentStopTime { get; set; } = TimeSpan.Zero;
        private DateTime mRegistTime = DateTime.Now;

        public void ResetRegistTime()
        {
            mRegistTime = DateTime.Now;
        }
    }

}
