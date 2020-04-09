using COI.DAL.DtstCOITableAdapters;
namespace COI.DAL
{
    public class IndividualManager:ManagerBase
    {
        private readonly DtstCOITableAdapters.individualTableAdapter
            _adapterInd=new individualTableAdapter();
        public DtstCOI.individualDataTable GetIndividuals()
        {var table = _adapterInd.GetData();
         return table;}
        public DtstCOI.individualDataTable SearchByName(string searchPattern)
        {return _adapterInd.GetDataBySearch('%' + searchPattern.Trim() + '%');}
        public void Save(DtstCOI.individualDataTable table)
        {_adapterInd.Update(table);}
        public void ConvertIndividualToAlias(string targetIndividual,string alias)
        {var adap = new DtstCOIsprocsTableAdapters.QueriesTableAdapter();
         adap.SP_convert2individuals_into_1individual_and_alias(targetIndividual, alias);}
        public void ImportIndividualFromSrc10(string individual,string alias)
        {var adap = new DtstCOIsprocsTableAdapters.QueriesTableAdapter();
         adap.SP_process_imports_10_mentioned(individual, alias);}
        }
    }

