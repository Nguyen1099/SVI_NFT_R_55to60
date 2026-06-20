namespace SVI_NFT_R
{
    public static partial class InterferenceRegion
    {
        public static IInterferenceRegionPermit InspectionStageArea { get; private set; } = new ImplInterferenceRegionPermit(name: nameof(InspectionStageArea));
        public static IInterferenceRegionPermit InShuttleAlignArea { get; private set; } = new ImplInterferenceRegionPermit(name: nameof(InShuttleAlignArea));
        //public static IInterferenceRegionPermit SampleArea { get; private set; } = new ImplInterferenceRegionPermit(name: nameof(SampleArea));
    }
}