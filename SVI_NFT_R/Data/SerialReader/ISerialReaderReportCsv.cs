namespace SVI_NFT_R.Data
{
    public interface ISerialReaderReportCsv
    {
        object Index { get; }
        string Name { get; }

        string GetOneLineForCsvReport();
    }
}