namespace SECCrawler.DAL
{
    public enum LinkResponseType
    {
        NotChanged=0,
        ClickableWithRelativePath=1,
        ClickableWithAbsolutePath=2,
        ClickableWithModifiedLink=3,
        NotClickableWithModifiedPath=4
    }
}
