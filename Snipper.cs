using System;
using SECCrawler.DAL;
using SECCrawler.BLL;
namespace SECCrawler.SnipperDude
{
    class Snipper
    {
        private readonly bool _useConsole;
        public Snipper(bool useConsole)
        {
            _useConsole = useConsole;
        }
        public int Snipe()
        {
            var generatedCount = 0;
            var adapter = new SECFormsManager();
            var manager = new SECFormsSnippetManager();
            var analyzer = new SECFormsDocAnalyzer();
            var formsTable = adapter.GetFormSniperQueue();
            var snippertsTable = new secCrawlerData.tblSEC_Forms_snippetDataTable();
            foreach (var form1 in formsTable)
            {

                var newSnippet = analyzer.GetFormSnippet(form1.FormID, 1);
                snippertsTable.AddtblSEC_Forms_snippetRow(
                    form1.FormID,1,newSnippet,"default snippet"
                    );
                Log(newSnippet);
                generatedCount++;
            }
            var snippetTypes = new[]{"former name"};//,"market information"};
            var snippetCriteria = new[] { "FORMER CONFORMED NAME", "Market Information" };
            var documentIndex = new[] {0, 1};
            for (var i = 0; i < snippetTypes.Length; i++)
            {
                formsTable = adapter.GetFormSniperQueue(snippetTypes[i], snippetCriteria[i]);
                foreach (var form1 in formsTable)
                {
                    var newSnippet =analyzer.GetFormSnippet(form1.FormID, documentIndex[i], snippetCriteria[i]);
                    if (newSnippet.Length == 0) continue;
                    snippertsTable.AddtblSEC_Forms_snippetRow(
                        form1.FormID,documentIndex[i], newSnippet, snippetTypes[i]);
                    Log(newSnippet);
                    generatedCount++;
                }
                manager.Save(snippertsTable);
            }
            return generatedCount;
        }
        protected void Log(string note)
        {
            if (_useConsole)
                Console.WriteLine(note);
        }
    }
}
