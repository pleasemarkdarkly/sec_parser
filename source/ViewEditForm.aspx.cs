using System;
using SECCrawler.BLL;
using System.Web.UI.WebControls;
using SECCrawler.DAL;
using System.Web;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class ViewEditForm : System.Web.UI.Page
    {
        #region declares
        private secCrawlerData.tblSEC_FormsRow _form;
        private int _extractDocSeq = 0;
        private bool _treatAsParagraphs = false;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString.Get("formID") == null)
                Response.Redirect("./FormList.aspx", true);
            if (IsPostBack) return;
            this.DropSubDoc.Items.Add(new ListItem("all","-1"));
            var docCount = new SECFormsDocAnalyzer().GetFormSubdocCount(GetForm.FormID);
            for (var i=0;i<=docCount;i++)
            {
                if (i==0)
                    DropSubDoc.Items.Add(new ListItem("header", i.ToString()));
                else
                    DropSubDoc.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        protected secCrawlerData.tblSEC_FormsRow GetForm
        {
            get
            {
                if (_form == null)
                {
                    var formId = new Guid(this.Request.QueryString.Get("formID"));
                    var manager = new DAL.SECFormsManager();
                    var table = manager.GetFormsByFormID(formId);
                    _form = table[0];
                }
                return _form;
            }
        }
        protected string GetUrLforIFrame
        {
            get
            {
                try
                {
                    if (_treatAsParagraphs)
                    {
                        if (checkProximitySearch.Checked)
                            return "./FetchParagraphsFromDB.aspx?FormID=" + GetForm.FormID 
                                + "&seq=" + _extractDocSeq 
                                + "&proximity=on&proximityRange=" + DropProximity.SelectedValue 
                                + "&proximityTerm1=" + HttpUtility.UrlEncode(this.TextProximity1.Text) 
                                + "&proximityTerm2=" + HttpUtility.UrlEncode(this.TextProximity2.Text)
                                + "&additionalRange=" + DropProximityAdd.SelectedValue;
                        if (CheckSearch.Checked)
                            return "./FetchParagraphsFromDB.aspx?FormID=" + GetForm.FormID
                            + "&search=on&seq=" + _extractDocSeq
                                + "&proximityTerm1=" + HttpUtility.UrlEncode(this.TextProximity1.Text)
                                + "&additionalRange=" + DropProximityAdd.SelectedValue;
                        return "./FetchParagraphsFromDB.aspx?FormID=" + GetForm.FormID 
                            + "&seq=" + _extractDocSeq;

                    }
                    return "./FetchDocumentFromDB.aspx?FormID=" + GetForm.FormID 
                        + "&seq=" + _extractDocSeq;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #region UI events
        protected void ButtonExtract_Click(object sender, EventArgs e)
        {
           try
           {
               _treatAsParagraphs = false;
               _extractDocSeq = int.Parse(this.DropSubDoc.SelectedValue);
           }
            catch
            {
                _extractDocSeq = 0;
            }
        }
        protected void btnSplit_Click(object sender, EventArgs e)
        {
            try
            {
                _treatAsParagraphs = true;
                _extractDocSeq = int.Parse(this.DropSubDoc.SelectedValue);
            }
            catch
            {
                _extractDocSeq = 0;
            }
        }
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            TextProximity1.Enabled = (checkProximitySearch.Checked | CheckSearch.Checked);
            TextProximity2.Enabled = checkProximitySearch.Checked;
            DropProximity.Enabled = checkProximitySearch.Checked;
            DropProximityAdd.Enabled = (checkProximitySearch.Checked | CheckSearch.Checked);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _treatAsParagraphs = true;
                _extractDocSeq = int.Parse(this.DropSubDoc.SelectedValue);
            }
            catch
            {
                _extractDocSeq = 0;
            }
        }
        #endregion
    }
}
