using System.Collections.Generic;
using COI.DAL.DtstCOITableAdapters;

namespace COI.DAL
{
    public class BatchManager:ManagerBase
    {
        private DtstCOITableAdapters.batch_runTableAdapter _adap;
        public DtstCOITableAdapters.batch_runTableAdapter Adap
            {get { if (_adap==null) _adap=new batch_runTableAdapter();return _adap;}}
        public DtstCOI.batch_runDataTable GetData(){return Adap.GetData();}
        public DtstCOI.batch_runDataTable GetLatestData(){return Adap.GetDataByLastRun();}
        public DtstCOI.batch_runDataTable GetLatestData(string name) { return Adap.GetDataByLastRunAndBatchName(name); }
        public void Save(DtstCOI.batch_runDataTable table){Adap.Update(table);}
        public static List<string> BatchNames = new List<string>{"OTCBB system change (all companies by name)",
            "SEC Ownership (all companies by CIK)",
            "All historic NASDAQ data for all known symbols",
            "Name changes from SEC-Crawler snippets",
            "Import all CIKs forn companies already known by name",
            "Search for mentions aff all known individuals in all SEC fillings",
            "Parse quarterly high and low bids from 10Ks for all known SEC companies",
            "Search all SEC litigations for companies and names",
            "Get beneficiary ownership of shares information from all SC 13(a,b,g,etc) filings"};
        public enum BatchNamesEnum
        {   OTCBBSystemChange=0,
            SECOwnership=1,
            NasdaqHistoricAllSymbols=2,
            NameChangesFromSECCrawler=3,
            SECImportCIK=4,
            SECLookupIndividuals=5,
            ParseHistoricShareValuesFrom10K=6,
            ParseLitigations=7,
            ParseSc13S=8
        }
        //region actual batch execution adapters
        public int DoCIKImports(){return new DtstCOIsprocsTableAdapters.QueriesTableAdapter().SP_IMPORT_src10_CIKs();}
        public DtstCOIsprocs.sp_get_forms_for_10kparsingDataTable GetFormsFor10KParsing()
        {return new DtstCOIsprocsTableAdapters.sp_get_forms_for_10kparsingTableAdapter().GetData();}
    }
}
