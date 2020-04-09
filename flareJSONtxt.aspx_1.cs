using System;
using System.Collections.Generic;
using System.Data;
using COI.DAL;
namespace COI.WebUI.Diagrams
{
    public partial class flareJSONtxt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReadArguments();
            PopulateLists();
            Response.Output.WriteLine("[");
            CreatePeopleNodes();
            CreateCompanyNodes();
            Response.Output.WriteLine("]");
        }
        #region declares
        private const int MaxNodes = 250;
        private readonly List<string> _ppl = new List<string>();
        private readonly List<string> _cos = new List<string>();
        private int _investigationID;
        private readonly LinkManager _dal = new LinkManager();
        private DtstCOI.individual_company_linkDataTable _icl;
        private DtstCOI.company_company_linkDataTable _ccl;
        private DtstCOI.individual_individual_linkDataTable _iil;
        private const string Company="company";
        private const string Individual = "individual";
        #endregion
        private void PopulateLists()
        {
            _icl = _dal.GetIndividualCompanyLink(_investigationID);
            foreach (var row in _icl)
            {
                row.individual_name = ReplaceAllSpecialCharectersWith_(row.individual_name);
                if (_ppl.IndexOf(row.individual_name) == -1 && _ppl.Count < MaxNodes)
                    _ppl.Add(row.individual_name);
                row.company_name = ReplaceAllSpecialCharectersWith_(row.company_name);
                if (_cos.IndexOf(row.company_name) == -1 && _cos.Count < MaxNodes)
                    _cos.Add(row.company_name);
            }
            _ccl = _dal.GetCoCoLinks(_investigationID);
            foreach (var ccl in _ccl)
            {
                ccl.company_name = ReplaceAllSpecialCharectersWith_(ccl.company_name);
                ccl.company_name2 = ReplaceAllSpecialCharectersWith_(ccl.company_name2);
            }
        }
        private void ReadArguments()
        {
            try { _investigationID = int.Parse(Request.QueryString["investigationID"]); }
            catch { _investigationID = 0; }
        }
        private void CreatePeopleNodes()
        {
            foreach (var person in _ppl)
            {
                var line =GenJSON(person,Individual,string.Empty);
                if (_cos.IndexOf(person) != _ppl.Count - 1) line += ",";
                Response.Output.WriteLine(line);
            }
        }
        private void CreateCompanyNodes()
        {
            var i = 0;
            foreach (var co in _cos)
            {
                var imports = GetLinkedIndividualsByCo(co);
                var importsC = GetCompaniesLikedByCo(co);
                if (imports != string.Empty & importsC != string.Empty) imports += ",";
                imports += importsC;
                var line = GenJSON(co,Company,imports);
                if (_cos.IndexOf(co) != _cos.Count - 1) line += ",";
                Response.Output.WriteLine(line);
                i = i > 3 ? 0 : i + 1;
            }
        }
        private string GetLinkedIndividualsByCo(string co)
        {
            var icl1 = new DataView(_icl, _icl.company_nameColumn.ColumnName + "='" + co.Replace("'", "''") + "'", string.Empty, DataViewRowState.CurrentRows);
            var imports = string.Empty;
            foreach (DataRowView drv in icl1)
            {
                var iclRow = (DtstCOI.individual_company_linkRow)drv.Row;
                if (imports.Contains(iclRow.individual_name)) continue;
                if (imports.Length > 0) imports += ",";
                imports += GenJSONName(iclRow.individual_name,Individual);
            }
            return imports;
        }
        private string GetCompaniesLikedByCo(string co)
        {
            var ccl1=new DataView(_ccl,_ccl.company_nameColumn.ColumnName + "='" + co.Replace("'", "''") + "'", string.Empty, DataViewRowState.CurrentRows);
            var imports = string.Empty;
            foreach (DataRowView drv in ccl1)
            {
                var iclRow = (DtstCOI.company_company_linkRow)drv.Row;
                if (imports.Contains(iclRow.company_name2)) continue;
                if (imports.Length > 0) imports += ",";
                imports += GenJSONName(iclRow.company_name2,Company);
            }
            return imports;
        }
        private static string GenJSON(string name,string type,string imports)
        {
            var line = "{\"name\":" + GenJSONName(name,type) + ",\"imports\":[" + imports + "]}";;
            return line;
        }
        private static string GenJSONName(string name,string type)
        {

            var line = "\"flare." + name.Substring(0, 1).ToLower() + "." + type + "." + name + "\"";
            return line;
        }
        private static string ReplaceAllSpecialCharectersWith_(string s)
        {
            const string cs = @"/-\ #$%^.";
            var chars = cs.ToCharArray();
            s = s.Trim();
            foreach( var c in chars)
            {
                s = s.Replace(c, '_');
            }
            while (s.Contains("__"))
            {
                s = s.Replace("__", "_");
            }
            if (s.EndsWith("_")) s = s.Substring(0, s.Length - 1);
            return s;
        }

    }
}
