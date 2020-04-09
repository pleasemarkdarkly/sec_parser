using System;
using System.Data;
using COI.Util;
using COI.DAL;
using Dundas.Charting.WebControl;
using System.Collections.Generic;
using System.Drawing;
namespace COI.WebUI.Charts
{
    public partial class TimeLine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var tableInv = new InvestigationManager().GetInvestigations();
            DropDownList1.DataSource = tableInv;
            DropDownList1.DataTextField = tableInv.investigation_titleColumn.ColumnName;
            DropDownList1.DataValueField = tableInv.investigation_idColumn.ColumnName;
            DropDownList1.DataBind();
            DropY1.Items.Clear(); DropY2.Items.Clear();
            for (int i = DateTime.Now.Year - 35; i <= DateTime.Now.Year; i++)
            {
                DropY1.Items.Add(i.ToString());
                DropY2.Items.Add(i.ToString());
            }
        }
        protected void ButtonDraw_Click(object sender, EventArgs e)
        {
            Chart2.Visible = false;
            Chart1.Visible = false;
            var chart = Chart1;
            if (DropChart.SelectedValue == "2" || DropChart.SelectedValue=="3")
                chart = Chart2;
            if (DropImageType.SelectedValue == "Jpeg")
                chart.ImageType = ChartImageType.Jpeg;
            else if (DropImageType.SelectedValue == ChartImageType.Png.ToString())
                chart.ImageType = ChartImageType.Png;
            else
                chart.ImageType = ChartImageType.Flash;
            chart.Titles[0].Text = TextTile.Text;
            chart.Titles[1].Text = TextTile2.Text;
            if (DropChart.SelectedValue == "1")
                DrawChart1();
            if (DropChart.SelectedValue == "2")
                DrawChart2();
            if (DropChart.SelectedValue == "3")
                DrawChart3();
        }
        void AddExcelAttachment(DataTable table)
        {
            var dtst = new DataSet();
            dtst.Tables.Add(table);
            table.TableName = "Search_Results";
            Response.ClearContent();
            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-disposition", "Attachment; filename=SECfilingsSearchResult.xml");
            //attachments
            var summary = new List<string>
              {
                  "Exported data from timeline Chart"
              };
            ExcelEngine.Convert(dtst, Response.OutputStream, 2, 2, summary);
            Response.End();
        }
        void DrawChart1(){
            Chart1.Visible = (CheckChart.Checked);
            Chart1.Width = int.Parse(DropSize.SelectedValue);
            Chart1.Height = (int)((Chart1.Width.Value) *3/4);
            var investigationId = int.Parse(DropDownList1.SelectedValue);
            var table = new LinkManager().GetIndividualCompanyLink(investigationId);
            if (CheckExcel.Checked)
            {
                AddExcelAttachment(table);
                return;
            }
            var invManager = new InvestigationManager();
            var tableI = invManager.GetInvestigatedIndividuals(investigationId);
            Chart1.Series.Clear();
            var companies = new List<string>();
            foreach (var individual in tableI)
            {
                var s = Chart1.Series.Add(individual.individual_name,1);
                s.SmartLabels.Enabled = true;
                s.ChartType = SeriesChartType.Point.ToString();
                s.YValueType =ChartValueTypes.Date;
                Chart1.ChartAreas[0].AxisY.LabelStyle.Format = "yyyy";
                var v = new DataView(table, "individual_name='" + individual.individual_name.Replace("'","''") + "'",
                                     table.company_nameColumn.ColumnName + " asc", DataViewRowState.CurrentRows);
                foreach (DataRowView viewRow in v)
                {
                    var link = (DtstCOI.individual_company_linkRow) viewRow.Row;
                    if (link.Islink_dateNull()) continue;
                    if (companies.IndexOf(link.company_name)==-1)
                    {
                        companies.Add(link.company_name);
                    }
                    var xVal = companies.IndexOf(link.company_name);
                    var point = new DataPoint();
                    point.SetValueXY( xVal +1,link.link_date);
                    if (!link.IspositionNull())
                        point.Label = link.position;
                    point.AxisLabel = link.company_name;
                    s.Points.Add(point);
                }
            }
            if (!CheckData.Checked) return;
            GridView1.DataSource = table;
            GridView1.DataBind();
        }
        void DrawChart2()
        {
            var investigationId = int.Parse(DropDownList1.SelectedValue);
            var beginDate = new DateTime(int.Parse(DropY1.SelectedValue), 1, 1);
            var endDate = new DateTime(int.Parse(DropY2.SelectedValue), 12, 31);
            var  ct= CheckAutoYears.Checked ? new ChartTools(investigationId,Chart2,0,0) : new ChartTools(investigationId,beginDate,endDate,Chart2,0,0);

            var positions = new List<string>{ "mentioned in filing" };
            Chart2.Visible = CheckChart.Checked;
            Chart2.Width = int.Parse(DropSize.SelectedValue);
            Chart2.Height = (int)((Chart2.Width.Value) * 3 / 4);
            //series are for positions
            Chart2.Series.Clear();
            var s = Chart2.Series.Add(positions[0]);
            s.ChartType = SeriesChartType.Point.ToString();
            s.XValueType = ChartValueTypes.Date;
            Chart2.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
            Chart2.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
            Chart2.ChartAreas[0].AxisX.Interval = 1;
            //
            var table = new LinkManager().GetIndividualCompanyLink(investigationId);
            if (CheckExcel.Checked)
            {
                AddExcelAttachment(table);
                return;
            }
            var invManager = new InvestigationManager();
            var tableI = invManager.GetInvestigatedIndividuals(investigationId);
            var tableC = invManager.GetInvestigatedCompanies(investigationId);
            var yIndex = 0;
            foreach (var individual in tableI)
            {
                foreach (var company in tableC)
                {
                    var links = new DataView(table, table.company_nameColumn.ColumnName +"='" 
                        + company.company_name + "' and " 
                        + table.individual_nameColumn.ColumnName +"='" 
                        + individual.individual_name+"'" , "link_date asc",DataViewRowState.CurrentRows);
                    yIndex++;
                    Chart2.ChartAreas[0].AxisY.CustomLabels.Add(yIndex-0.5, yIndex+0.5, company.company_name);
                    foreach (DataRowView rv in links)
                    {
                        var link = (DtstCOI.individual_company_linkRow) rv.Row;
                        if (!ct.IsPointUsable(link)) continue;
                        string position;
                        int positionSeriesIndex;
                        if (link.IspositionNull() || link.position.Trim() == string.Empty)
                        {
                            position = positions[0];
                            positionSeriesIndex = 0;
                        }
                        else
                        {
                            position = link.position.Trim().ToLower();
                            positionSeriesIndex = positions.IndexOf(position);
                            if (positionSeriesIndex==-1)
                            {
                                s=Chart2.Series.Add(position);
                                s.ChartType = SeriesChartType.Point.ToString();
                                s.XValueType = ChartValueTypes.Date;
                                s.MarkerSize =(int) Chart2.Height.Value/100;
                                positions.Add(position);
                                positionSeriesIndex = positions.Count - 1;
                            }
                        }
                        var point = new Dundas.Charting.WebControl.DataPoint();
                        point.SetValueXY(link.link_date,yIndex);
                        Chart2.Series[positionSeriesIndex].Points.Add(point);
                    }
                }
                yIndex++;
                var l1=Chart2.ChartAreas[0].AxisY.CustomLabels.Add(yIndex - 0.5, yIndex + 0.5, "--- "+individual.individual_name.ToUpper()+" ---");
                Chart2.ChartAreas[0].AxisY.CustomLabels[l1].GridTick=GridTick.GridLine;
                Chart2.ChartAreas[0].AxisY.StripLines.Add(new StripLine {IntervalOffset = yIndex,
                    BackColor = Color.LightGray, StripWidth = 0.2});
                yIndex++;
            }
            Chart2.ChartAreas[0].AxisY.Maximum = yIndex;
            Chart2.ChartAreas[0].AxisY.LabelsAutoFitStyle = LabelsAutoFitStyle.DecreaseFont;
            if (!CheckData.Checked) return;
            GridView1.DataSource = table;
            GridView1.DataBind();
        }
        void DrawChart3()
        {
            var investigationId = int.Parse(DropDownList1.SelectedValue);
            var beginDate = new DateTime(int.Parse(DropY1.SelectedValue), 1, 1);
            var endDate = new DateTime(int.Parse(DropY2.SelectedValue), 12, 31);
            var ct = CheckAutoYears.Checked ? new ChartTools(investigationId,Chart2,0,0) : new ChartTools(investigationId, beginDate, endDate,Chart2,0,0);
            var positions = new List<string>() { "mentioned in filing" };
            Chart2.Visible = CheckChart.Checked;
            Chart2.Width = int.Parse(DropSize.SelectedValue);
            Chart2.Height = (int)((Chart2.Width.Value) * 3D / 4D);
            //series are for positions
            Chart2.Series.Clear();
            var s = Chart2.Series.Add(positions[0]);
            s.ChartType = SeriesChartType.Point.ToString();
            s.XValueType = ChartValueTypes.Date;
            Chart2.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
            Chart2.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
            Chart2.ChartAreas[0].AxisX.Interval = 1;
            //
            var table = new LinkManager().GetIndividualCompanyLink(investigationId);
            if (CheckExcel.Checked)
            {
                AddExcelAttachment(table);
                return;
            }
            var invManager = new InvestigationManager();
            var tableI = invManager.GetInvestigatedIndividuals(investigationId);
            var tableC = invManager.GetInvestigatedCompanies(investigationId);
            var yIndex = 0;
            var count = 0;
            foreach (var company in tableC)
            {
                var inds = 0;
                var indsList = new List<DtstCOI.investigated_individualRow>();
                foreach (var individual in tableI)
                {
                    var sort = table.company_nameColumn.ColumnName + "='"
                                  + company.company_name.Replace("'", "''") + "' and "
                                  + table.individual_nameColumn.ColumnName + "='"
                                  + individual.individual_name.Replace("'", "''") + "'";
                    var useIndividual = false;
                    var links = new DataView(table, sort, "link_date asc",
                                             DataViewRowState.CurrentRows);
                    if (links.Count == 0) continue;
                    foreach (DataRowView rv in links)
                    {
                        var link = (DtstCOI.individual_company_linkRow) rv.Row;
                        if (!ct.IsPointUsable(link)) continue;
                        useIndividual = true;
                    }
                    if (useIndividual) {inds++;indsList.Add(individual);}
                }
                if (inds<=1) continue;
                foreach (var individual in indsList)
                {
                    var sort = table.company_nameColumn.ColumnName + "='"
                                  + company.company_name.Replace("'", "''") + "' and "
                                  + table.individual_nameColumn.ColumnName + "='"
                                  + individual.individual_name.Replace("'", "''") + "'";
                    var links = new DataView(table, sort, "link_date asc", DataViewRowState.CurrentRows);
                    if (links.Count==0) continue;
                    yIndex++;
                    Chart2.ChartAreas[0].AxisY.CustomLabels.Add(yIndex - 0.5, yIndex + 0.5, 
                        individual.individual_name );
                    foreach (DataRowView rv in links)
                    {
                        var link = (DtstCOI.individual_company_linkRow)rv.Row;
                        if (!ct.IsPointUsable(link)) continue;
                        string position;
                        int positionSeriesIndex;
                        if (link.IspositionNull() || link.position.Trim() == string.Empty)
                        {
                            position = positions[0];
                            positionSeriesIndex = 0;
                        }
                        else
                        {
                            position = link.position.Trim().ToLower();
                            positionSeriesIndex = positions.IndexOf(position);
                            if (positionSeriesIndex == -1)
                            {
                                s = Chart2.Series.Add(position);
                                s.ChartType = SeriesChartType.Point.ToString();
                                s.XValueType = ChartValueTypes.Date;
                                s.MarkerSize = (int)Chart2.Height.Value / 100;
                                positions.Add(position);
                                positionSeriesIndex = positions.Count - 1;
                            }
                        }
                        var point = new Dundas.Charting.WebControl.DataPoint();
                        point.SetValueXY(link.link_date, yIndex);
                        Chart2.Series[positionSeriesIndex].Points.Add(point);
                        count++;
                    }
                }
                yIndex++;
                Chart2.ChartAreas[0].AxisY.CustomLabels.Add(yIndex - 0.5, yIndex + 0.5, company.company_name);
                Chart2.ChartAreas[0].AxisY.StripLines.Add(new StripLine
                {
                    IntervalOffset = yIndex -0.1,
                    BackColor = Color.LightGray,
                    StripWidth = 0.2
                });
            }
            yIndex++;
            Chart2.ChartAreas[0].AxisY.Maximum = yIndex;
            Chart2.Titles[1].Text += " " + count + " matches found";
            if (!CheckData.Checked) return;
            GridView1.DataSource = table;
            GridView1.DataBind();
        }
    }
}
