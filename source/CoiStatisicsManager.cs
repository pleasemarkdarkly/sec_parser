namespace SECCrawler.DAL
{
    public class CoiStatisicsManager
    {
        public DtstSECCrawlerViews.spCOI_IngestMonitorDataTable GetFormIngestQueue()
        {
            var adapter = new DtstSECCrawlerViewsTableAdapters.spCOI_IngestMonitorTableAdapter();
            var table=adapter.GetData();
            return table;
        }
        public DtstSECCrawlerViews.spCOI_StatisticsDataTable GetCrawlerStatistics()
        {
            var adapter = new DtstSECCrawlerViewsTableAdapters.spCOI_StatisticsTableAdapter();
            return adapter.GetData();
        }
    }
}
