using System;
using System.Drawing;
using System.Web;
using COI.DAL;
using System.Collections.Generic;
using Dundas.Charting.WebControl;
using System.Data;
namespace COI.WebUI.Charts
{
    public partial class CompanyTimeLine : System.Web.UI.Page
    {
        private List<string> _series;
        private DateTime _minDate;
        private DateTime _maxDate;
        private string _coName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _coName = Request.QueryString["coName"];
            LabelCoName.Text = _coName;
            if (_coName!=string.Empty)
            {DoChart();DoLinks();}
                
            if (!IsPostBack) FillDropDowns();
        }
        protected void FillDropDowns()
        {   DropFromDate.Items.Clear();
            DropToDate.Items.Clear();
            for (var i=DateTime.Now.Year-12;i<=DateTime.Now.Year;i++)
            {   DropFromDate.Items.Add(i.ToString());
                DropToDate.Items.Add(i.ToString());}
            }
        protected void ButtonData_Click(object sender, EventArgs e)
        {
            Response.Redirect("./CompanyView.aspx?coName="+HttpUtility.UrlEncode(_coName),true);
        }
        protected void DoChart()
        {
            Chart1.Series.Clear();
            _series=new List<string>();
            if (CheckDates.Checked)
            {
                _maxDate = (new DateTime(int.Parse(DropToDate.SelectedValue), 12, 31));
                _minDate = (new DateTime(int.Parse(DropFromDate.SelectedValue), 1, 1));
            }
            else
            {
                _minDate = DateTime.Now;
                _maxDate = DateTime.Now.AddYears(-30);
            }
            Chart1.ImageType = DropRenderFormat.SelectedValue == "Flash" ? ChartImageType.Flash : ChartImageType.Png;
            Chart1.Title = _coName.ToUpper();
            DoEvents();
            DoPeople();
            DoShares();
            Chart1.ChartAreas[0].AxisX.Minimum = _minDate.ToOADate();
            Chart1.ChartAreas[0].AxisX.Maximum = _maxDate.ToOADate();
            Chart1.ChartAreas[1].AxisX.Minimum = _minDate.ToOADate();
            Chart1.ChartAreas[1].AxisX.Maximum = _maxDate.ToOADate();
            Chart1.ChartAreas[2].AxisX.Minimum = _minDate.ToOADate();
            Chart1.ChartAreas[2].AxisX.Maximum = _maxDate.ToOADate();
        }
        protected void SetMaxMinDates(DateTime minDate, DateTime maxDate)
        {
            if (CheckDates.Checked) return;
            if (minDate < _minDate) _minDate = minDate;
            if (maxDate > _maxDate) _maxDate = maxDate;
        }
        protected void DoPeople()
        {
            var linMan = new LinkManager();
            var table = linMan.GetIndividualCompanyLink(_coName);
            var people = new List<string>();
            const int i = 1;
            foreach (var link in table)
            {
                if (link.Islink_dateNull()) continue;
                //if (!IsPointUsable(link.link_date)) continue;
                var position = link.link_type.ToLower();
                if (!link.IspositionNull())
                    position = link.position.ToLower();
                Series s;
                if (_series.IndexOf(position) >= 0)
                    s = Chart1.Series[_series.IndexOf(position)];
                else
                {
                    _series.Add(position);
                    s = Chart1.Series.Add(position);
                    s.ChartType = "Point";
                    s.ChartArea = "People";
                    s.Legend = "People";
                    s.MarkerSize = 7;
                }
                var person = link.individual_name.Trim();
                if (people.IndexOf(person) == -1)
                {
                    people.Add(person);
                    Chart1.ChartAreas[1].AxisY.CustomLabels.Add(people.IndexOf(person) - 0.5 + i,
                                                                people.IndexOf(person) + 0.5 + i,
                                                                person);
                }
                SetMaxMinDates(link.link_date, link.link_date);
                var p = s.Points.AddXY(link.link_date, i + people.IndexOf(person));
                s.Points[p].ToolTip = string.Format("{0} | {1} | {2:yyyy-MM-dd}", person, position, link.link_date);
                var docDal = new DocumentManager();
                var dbDoc = docDal.GetDocument(link.supporting_document);
                if (dbDoc.Rows.Count != 1) return;
                if ((!dbDoc[0].Isinternal_linkNull()) && dbDoc[0].internal_link != string.Empty)
                    s.Points[p].Href = dbDoc[0].internal_link;
                else
                    s.Points[p].Href = dbDoc[0].external_link;
            }
        }
        protected bool IsHighligtedEvent(DtstCOI.company_eventRow evnt)
        {
            return (evnt.event_type.ToLower() == "registered agent change"
                    || evnt.event_type.ToLower() == "registered agent address change"
                    || evnt.event_type.ToLower() == "registered agent name change"
                    || evnt.event_type.ToLower().Contains("promissory note")
                    || evnt.event_type.ToLower() == "registered agent resignation");
        }
        protected void DoEvents()
        {
            var countP = 0;
            var eventManager = new CompanyManager();
            var docDal = new DocumentManager();
            var table = eventManager.GetEventsByCo(_coName);
            var eventTypes = new List<string>();
            Chart1.ChartAreas[0].AxisY.CustomLabels.Add(  0,1,"User defined");
            Chart1.ChartAreas[0].AxisY.CustomLabels.Add(1,2, "A.I. found");
            Chart1.ChartAreas[0].AxisY.StripLines.Add(new StripLine
            {
                IntervalOffset = 1D,
                BorderColor = Color.Black,
                StripWidth = 0D,
                BorderStyle = ChartDashStyle.DashDot
            });
            foreach (var evnt in table)
            {
                var evType = evnt.event_type.ToLower();
                //if (!IsPointUsable(evnt.event_date)) continue;
                if (eventTypes.IndexOf(evType) == -1) eventTypes.Add(evType);
            }
            foreach (var evnt in table)
            {
                //if (!IsPointUsable(evnt.event_date)) continue;
                Series s;
                var evType = evnt.event_type.ToLower();
                if (_series.IndexOf(evType) >= 0)
                    s = Chart1.Series[_series.IndexOf(evType)];
                else
                {
                    _series.Add(evType);
                    s = Chart1.Series.Add(evType);
                    s.ChartType = "Point";
                    s.MarkerSize = 7;
                    if (IsHighligtedEvent(evnt))
                    {
                        s.MarkerStyle = MarkerStyle.Cross;
                        s.MarkerSize = 15;
                    }
                }
                if (IsHighligtedEvent(evnt))
                {
                    for (var i = 0; i < 3;i++ )
                        Chart1.ChartAreas[i].AxisX.StripLines.Add(new StripLine
                        {
                            IntervalOffset = evnt.event_date.ToOADate(),
                            BorderColor = Color.DarkGray,
                            BorderStyle = ChartDashStyle.DashDot,
                            StripWidth = 0
                        });
                }
                var index = evnt.user_defined ? 0.5D : 1.5D;
                index += -0.25D + (0.5D * (((double)eventTypes.IndexOf(evType) + 1) / ((double)eventTypes.Count)));
                var p = s.Points.AddXY(evnt.event_date, index);
                countP++;
                s.Points[p].ToolTip =
                    evnt.event_description.ToLower() != evType
                    ? string.Format("{0} | {1} | {2:yyyy-MM-dd}", evType, evnt.event_description, evnt.event_date)
                    : string.Format("{0} | {1:yyyy-MM-dd}", evType, evnt.event_date); 
                SetMaxMinDates(evnt.event_date, evnt.event_date);
                var dbDoc = docDal.GetDocument(evnt.supporting_doc_id);
                if (dbDoc.Rows.Count!=1) return;
                if ((!dbDoc[0].Isinternal_linkNull()) && dbDoc[0].internal_link!=string.Empty)
                    s.Points[p].Href = dbDoc[0].internal_link;
                else
                s.Points[p].Href = dbDoc[0].external_link;
            }
            if (countP==0)
            {
                _series.Add("No known company events");
                var s = Chart1.Series.Add("No known company events");
                s.ChartType = "Point";
                s.MarkerSize = 7;
                s.Points.AddXY(_maxDate, 1);
            }
        }
        protected void DoShares()
        {   Series s;
            const string area="SharesTraded";
            const string volume = "Shares volume";
            if (_series.IndexOf(volume) >= 0)
                s = Chart1.Series[volume];
            else
            {   _series.Add(volume);
                s = Chart1.Series.Add(volume);
                s.ChartArea = area;
                s.ChartType = "Line";
                s.Legend = area;
            }
            Series sp;
            const string price = "Shares price";
            if (_series.IndexOf(price) >= 0)
                sp = Chart1.Series[price];
            else
            {
                _series.Add(price);
                sp = Chart1.Series.Add(price);
                sp.ChartArea = area;
                sp.ChartType = "StepLine";
                sp.Legend = area;
                sp.YAxisType = AxisType.Secondary;
            }
            var dal = new SharesManager();
            var table=dal.GetSharesTraded(_coName,70);
            var vw = new DataView(table, "", table.dateColumn.ColumnName + " asc", DataViewRowState.CurrentRows);
            foreach (DataRowView rowView in vw)
            {
                var row =(DtstCOI.company_shares_tradedRow) rowView.Row;
                if (!IsPointUsable(row.date)) continue;
                var p = s.Points.AddXY(row.date, row.vloume);
                var pp = sp.Points.AddXY(row.date, row.price_close);
                SetMaxMinDates(row.date,row.date);
            }
            //-------------------------------------------------------------------------------
            if (!checkArea.Checked) return;
            //-------------------------------------------------------------------------------
            double ratio ;
            try
            {
                ratio = double.Parse(TextRatio.Text);
            }
            catch {
                ratio = 0.5D; }
            const string vol = "Highlighted Volume";
            if (_series.IndexOf(vol) >= 0)
                s = Chart1.Series[vol];
            else
            {
                _series.Add(vol);
                s = Chart1.Series.Add(vol);
                s.ChartArea = area;
                s.ChartType = "Area";
                s.Legend = area;
            }
            if (TextAreaDescription.Text.Trim() != string.Empty) s.LegendText = TextAreaDescription.Text;
            var ct = new ChartTools(_coName, Chart1, 70, 1);
                var vwA = ct.GetAreaPoints(int.Parse(this.TextVolume.Text));
                foreach (DataRowView rowView in vwA)
                {
                    var row = (DtstCOI.company_shares_tradedRow)rowView.Row;
                    var p = s.Points.AddXY(row.date, row.vloume*ratio);
                }
        }
        protected bool IsPointUsable(DateTime d)
        {
            if (CheckDates.Checked)
            {
                if (d < _minDate || d > _maxDate) return false;
            }
            return true;
        }
        protected void DoLinks()
        {
            const string linkBase="./CompanyTimeLine.aspx?CoName=";
            var dal = new LinkManager();
            Literal1.Text = dal.GetPreviousLinks(_coName,true,linkBase);
            Literal2.Text = dal.GetPreviousLinks(_coName, false, linkBase);
        }
    }
}
