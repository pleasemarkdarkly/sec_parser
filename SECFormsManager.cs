namespace SECCrawler.DAL
{
    using System;

    public class SECFormsManager : ManagerBase
    {
        readonly secCrawlerDataTableAdapters.tblSEC_FormsTableAdapter 
            _adapter = new secCrawlerDataTableAdapters.tblSEC_FormsTableAdapter();
        public secCrawlerData GetFormDownloadQueue()
        {
            var dtst = new secCrawlerData();
            _adapter.FillByDownloadQ(dtst.tblSEC_Forms);
            return dtst;
        }
        public secCrawlerData.tblSEC_FormsDataTable GetFormSniperQueue()
        {
            var table = _adapter.GetDataByFormsWithNoSnippets();
            return table;
        }
        public secCrawlerData.tblSEC_FormsDataTable GetFormSniperQueue(string criteria,string fullTextCriteria)
        {
            if (!fullTextCriteria.StartsWith("\"") || !fullTextCriteria.EndsWith("\""))
                fullTextCriteria = string.Format("\"{0}\"", fullTextCriteria.Trim());
            var table = _adapter.GetDataByFormsWithNoSnyppetByType(criteria,fullTextCriteria);
            return table;
        }
        public secCrawlerData GetFormIngestQueue()
        {
            var dtst = new secCrawlerData();
            _adapter.FillByIngestQ(dtst.tblSEC_Forms);
            return dtst;
        }
        public secCrawlerData GetFormBlob(Guid formId)
        {
            var adapter1 = new secCrawlerDataTableAdapters.tblSEC_FormsLocalBLOBTableAdapter();
            var dtst = new secCrawlerData();
            adapter1.Fill(dtst.tblSEC_FormsLocalBLOB, formId);
            return dtst;
        }
        public secCrawlerData.tblSEC_FormsDataTable GetFormsByFormID(Guid formID)
        {
            var table = _adapter.GetDataByFormID(formID);
            return table;
        }
        public secCrawlerData.tblSEC_FormsDataTable GetFormsByFullTextSearchAndFormType(string searchCriteria, string formType, LinkResponseType makeURLClickable, string customURL)
        {
            var table = _adapter.GetDataByFullTextSearchAndFormType(searchCriteria, formType);
            if (makeURLClickable == LinkResponseType.ClickableWithAbsolutePath)
            {
                var basePath = (string)ConfigReader.GetValue("LocalDataStoreWebShare", string.Empty.GetType());
                foreach (var rowF in table)
                {
                    rowF.FormPartialURL = "<A href=\"" + basePath + rowF.FormPartialURL + "\">" + rowF.FormPartialURL + "</A>";
                }
            }
            if (makeURLClickable == LinkResponseType.ClickableWithModifiedLink)
            {
                var basePath = (string)ConfigReader.GetValue("LocalDataStoreWebShare", string.Empty.GetType());
                foreach (var rowF in table)
                {
                    rowF.FormPartialURL = "<A href=\"" + customURL + rowF.FormID.ToString() + "\">" + rowF.FormPartialURL + "</A>";
                }
            }
            if (makeURLClickable == LinkResponseType.NotClickableWithModifiedPath)
            {
                string basePath;
                if (string.IsNullOrEmpty(customURL))
                     basePath = (string)ConfigReader.GetValue("LocalDataStoreWebShare", string.Empty.GetType());
                else
                {
                    basePath = customURL;
                }
                foreach (var rowF in table)
                {
                    rowF.FormPartialURL = basePath + rowF.FormPartialURL ;
                }
            } 
            
            return table;
        }
        public DtstSECCrawlerViews.spCOI_fullTextSearchByFormTypeWithSnippetDataTable GetFormsByFullTextSearchAndFormTypeWithSnippet(string searchCriteria, string formType, LinkResponseType makeURLClickable, string customURL)
        {return GetFormsByFullTextSearchAndFormTypeWithSnippet(searchCriteria, formType, makeURLClickable, customURL,"%"); }
        public DtstSECCrawlerViews.spCOI_fullTextSearchByFormTypeWithSnippetDataTable GetFormsByFullTextSearchAndFormTypeWithSnippet(string searchCriteria, string formType, LinkResponseType makeURLClickable, string customURL, string coName)
        {
            var adapter1 = new DtstSECCrawlerViewsTableAdapters.spCOI_fullTextSearchByFormTypeWithSnippetTableAdapter();
            var table = adapter1.GetData(searchCriteria, formType,coName);
            if (makeURLClickable == LinkResponseType.ClickableWithAbsolutePath)
            {
                var basePath = (string)ConfigReader.GetValue("LocalDataStoreWebShare", string.Empty.GetType());
                foreach (var rowF in table)
                {
                    rowF.FormPartialURL = "<A target=_blank href=\"" + basePath + rowF.FormPartialURL + "\">" + rowF.FormPartialURL + "</A>";
                }
            }
            if (makeURLClickable == LinkResponseType.ClickableWithModifiedLink)
            {
                var basePath = (string)ConfigReader.GetValue("LocalDataStoreWebShare", string.Empty.GetType());
                foreach (var rowF in table)
                {
                    rowF.FormPartialURL = "<A target=_blank href=\"" + customURL + rowF.FormID.ToString() + "\">" + rowF.FormPartialURL + "</A>";
                }
            }
            if (makeURLClickable == LinkResponseType.NotClickableWithModifiedPath)
            {
                string basePath;
                if (string.IsNullOrEmpty(customURL))
                    basePath = (string)ConfigReader.GetValue("LocalDataStoreWebShare", string.Empty.GetType());
                else
                {
                    basePath = customURL;
                }
                foreach (var rowF in table)
                {
                    rowF.FormPartialURL = basePath + rowF.FormPartialURL;
                }
            }

            return table;
        }
        public void Save(secCrawlerData dtst)
        {
            if (dtst.tblSEC_Forms.IsInitialized)
            {
                _adapter.Update(dtst.tblSEC_Forms);
            }
            if (!dtst.tblSEC_FormsLocalBLOB.IsInitialized) return;
            var adapter1 = new secCrawlerDataTableAdapters.tblSEC_FormsLocalBLOBTableAdapter();
            adapter1.Update(dtst.tblSEC_FormsLocalBLOB);
        }
        public secCrawlerData.tblSEC_FormTypeDataTable GetFormTypes()
        {
            var adapter1 = new secCrawlerDataTableAdapters.tblSEC_FormTypeTableAdapter();
            var table = adapter1.GetData();
            return table;
        }
    }
}