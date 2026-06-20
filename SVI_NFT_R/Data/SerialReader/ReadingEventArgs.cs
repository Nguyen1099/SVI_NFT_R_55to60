using System;

namespace SVI_NFT_R.Data
{
    public class ReadingEventArgs<TReadValue> : EventArgs
    {
        public object Index { get; }
        public string Name { get; }
        public bool Result { get; set; }
        public TReadValue Value { get; set; }

        public ReadingEventArgs(object index, string name)
        {
            Index = index;
            Name = name;
        }
    }
}