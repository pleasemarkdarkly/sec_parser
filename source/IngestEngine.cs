using System;
using COI.BLL.Parsers;
using COI.DAL;
namespace COI.Daemon
{
    public class IngestEngine
    {
        private readonly int _investigationID;
        private readonly bool _useConsole;
        private void Log(string s)
        {
            if (_useConsole)
                Console.WriteLine(s);
        }
        private readonly CompanyManager _dal= new CompanyManager();
        public IngestEngine(int investigationId, bool useConsole)
        {
            _investigationID = investigationId;
            _useConsole = useConsole;
        }
        public DtstCOI.company_idDataTable GetSrc10OwnershipQ()
        {
            return _dal.GetCompanyIdsByQ();
        }
        public void ProcessSrc10OwnershipQ(DtstCOI.company_idDataTable q)
        {
            const string url = "http://sec.gov/cgi-bin/own-disp?action=getissuer&CIK=";
            foreach (var company in q)
            {
                Log("processing company " + company.company_name);

                var parser = new Source10OwnershipParser(url+company.identifier, _investigationID);
                Log(parser.ParseOwnership(false));
            }
        }
    }
}
