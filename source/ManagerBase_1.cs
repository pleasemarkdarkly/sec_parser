namespace SECCrawler.DAL
{
    public class ManagerBase
    {
        protected System.Configuration.AppSettingsReader ConfigReader = new System.Configuration.AppSettingsReader();
        public string GetConnectionString()
        {
            var cs = (string)ConfigReader.GetValue("ConnectionString", string.Empty.GetType());
            return cs;
        }

        public ManagerBase()
        {
            
        }

    }
}
