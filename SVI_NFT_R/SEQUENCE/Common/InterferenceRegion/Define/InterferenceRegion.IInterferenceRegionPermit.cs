namespace SVI_NFT_R
{
    public static partial class InterferenceRegion
    {
        public interface IInterferenceRegionPermit
        {
            string Name { get; }
            bool IsEnter { get; }

            bool TryEnter();
            void Exit();
        }
    }
}