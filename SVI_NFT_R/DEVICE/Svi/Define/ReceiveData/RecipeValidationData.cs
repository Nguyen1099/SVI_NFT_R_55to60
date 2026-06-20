using System;
using System.Collections.Concurrent;

namespace SVI_NFT_R.DEVICE.Svi
{
    public partial class ReceiveData
    {
        [Serializable]
        public class RecipeValidationData : KeepData
        {
            public int Count { get { return Data.Count; } }
            public ConcurrentDictionary<string, string> Data { get; set; }

            public RecipeValidationData()
            {
                Data = new ConcurrentDictionary<string, string>();
                ResetRegistTime();
            }
        }
    }
}
