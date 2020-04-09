using System;
using System.Collections.Generic;
using System.Data;
using COI.DAL;
using Dundas.Charting.WebControl;
using COI.Util;
using System.Drawing;
namespace COI.WebUI.Charts
{
    public class ChartTools
    {
        #region declares
        private readonly bool _autoTimeSpan;
        private readonly DateTime _timeSpanEnds;
        private readonly DateTime _timeSpanBegins;
        private readonly int _investigationId;
        private readonly Chart _chart;
        private readonly int _sourceID;
        private readonly int _intervalDays;
        private readonly string _companyName;
        private List<string> _series=new List<string>();
        private DtstCOI.company_shares_tradedDataTable _sharesTraded;
        public DtstCOI.company_shares_tradedDataTable SharesTradedTable
            {get{
                if (_sharesTraded == null)
                    _sharesTraded = new SharesManager().GetSharesTraded(_companyName, _sourceID,_intervalDays);
                return _sharesTraded;
            }}
        #endregion
        #region constructs
        public ChartTools(int investigationID, Chart chart,int sourceID, int intervalDays)
        {
            _autoTimeSpan = true;
            _investigationId = investigationID;
            _chart = chart;
            _companyName = string.Empty;
            _sourceID = sourceID;
            _intervalDays = intervalDays;
        }
        public ChartTools(string companyName, Chart chart, int sourceID, int intervalDays)
        {
            _autoTimeSpan = true;
            _companyName = companyName;
            _investigationId = 0;
            _chart = chart;
            _sourceID = sourceID;
            _intervalDays = intervalDays;
        }
        public ChartTools(int investigationID, DateTime timeSpanBegins, DateTime timeSpanEnds, Chart chart, int sourceID, int intervalDays)
        {
            _autoTimeSpan = false;
            _timeSpanEnds = timeSpanEnds;
            _timeSpanBegins = timeSpanBegins;
            _investigationId = investigationID;
            _chart = chart;
            _companyName = string.Empty;
            _sourceID = sourceID;
            _intervalDays = intervalDays;
        }
        public ChartTools(string companyName, DateTime timeSpanBegins, DateTime timeSpanEnds, Chart chart, int sourceID, int intervalDays)
        {
            _companyName = companyName;
            _autoTimeSpan = false;
            _timeSpanEnds = timeSpanEnds;
            _timeSpanBegins = timeSpanBegins;
            _investigationId = 0;
            _chart = chart;
            _sourceID = sourceID;
            _intervalDays = intervalDays;
        }
        #endregion
        #region generalUseSubs
        public bool IsPointUsable(DtstCOI.individual_company_linkRow r)
        {
            if (r.Islink_dateNull()) return false;
            if (!_autoTimeSpan)
            {
                if (r.link_date < _timeSpanBegins) return false;
                if (r.link_date > _timeSpanEnds) return false;
            }
            return true;
        }
        public bool IsPointUsable(DtstCOI.company_eventRow r)
        {
            if (!_autoTimeSpan)
            {
                if (r.event_date < _timeSpanBegins) return false;
                if (r.event_date > _timeSpanEnds) return false;
            }
            return true;
        }
        public bool IsPointUsable(DtstCOI.company_shares_tradedRow r)
        {
            if (!_autoTimeSpan)
            {
                if (r.date < _timeSpanBegins) return false;
                if (r.date > _timeSpanEnds) return false;
            }
            return true;
        }
        public void Resize(string swidth, double ratio)
        {
            int width;
            try
            {
                width = int.Parse(swidth);
            }
            catch (Exception)
            {
                width = (int)_chart.Width.Value;
            }
            var height = (int)(width * ratio);
            _chart.Width = width;
            _chart.Height = height;
        }
        public void Reset(string newTitle)
        {
            _chart.Series.Clear();
            _chart.Title = NameAnalyzer.NameCapitalizer( newTitle);
            _series=new List<string>();
            _chart.ChartAreas[0].AxisX.StripLines.Clear();
           // _chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
        }

        #endregion
        #region companyTimeLine2
        public void DoEvents()
        {
            var eventManager = new CompanyManager();
            var table = eventManager.GetEventsByCo(_companyName);
            var i = 1;
            foreach (var row in table)
            {
                if (!IsPointUsable(row)) continue;
                /*<DCWC:StripLine BackColor="Gray" BorderColor="Black" Interval="2" 
                  IntervalOffsetType="Days" IntervalType="Days" StripWidth="1" />*/
                var minDate = _timeSpanBegins;
                var l = new StripLine
                {
                    StripWidth = 0,
                    StripWidthType=DateTimeIntervalType.Days,
                    BackColor=Color.Blue,
                    BorderColor=Color.Blue,
                    Interval =0,
                    IntervalOffset =  (row.event_date - minDate).TotalDays,// row.event_date.ToOADate(),
                   // Title = row.event_type
                };
                i++;
                _chart.ChartAreas[0].AxisX.StripLines.Add(l);

            }
        }
        public void DoSharesVolume()
        {
            if (_companyName == string.Empty) return;
            var s = _chart.Series.Add("Shares Volume");
            _series.Add(s.Name);
            s.ChartType = "Line";
            //s.YAxisType =AxisType.Secondary;
            var dal = new SharesManager();
            var table = dal.GetSharesTraded(_companyName, _sourceID);
            var vw = new DataView(table, "", table.dateColumn.ColumnName + " asc", DataViewRowState.CurrentRows);
            foreach (DataRowView rowView in vw)
            {
                var row = (DtstCOI.company_shares_tradedRow)rowView.Row;
                if (IsPointUsable(row))
                    s.Points.AddXY(row.date, row.vloume);
            }
        }
        #endregion
        #region AreaChart
        private DateTime GetVolumePeak()
        {
            var table = SharesTradedTable;
            var peakDate = DateTime.Today;
            var peak=0;
            foreach (var trade in table)
            {
                if (trade.vloume <= peak) continue;
                peakDate = trade.date;
                peak = trade.vloume;
            }
            return peakDate;
        }
        public DataView GetAreaPoints(int goalVolume)
        {
            var peakDate = GetVolumePeak();
            var minDate = peakDate;
            var maxDate = peakDate;
            var foundLowerBoundary = false;
            var foundUpperBoundary = false;
            var table = SharesTradedTable;
            var centerpoint = table.FindBycompany_namedatesource_idintervalDays(_companyName, peakDate, _sourceID,
                                                                                _intervalDays);
            var volume = centerpoint.vloume;
            while (volume<goalVolume)
            {
                if (!foundLowerBoundary)
                {
                    var prevPoint = GetPrevPoint(minDate);
                    if (prevPoint == null)
                        foundLowerBoundary = true;
                    else
                    {
                        volume += prevPoint.vloume;
                        minDate = prevPoint.date;
                    }
                }
                if(!foundUpperBoundary)
                {
                    var nextPoint = GetNextPoint(maxDate);
                    if (nextPoint == null)
                        foundUpperBoundary = true;
                    else
                    {
                        volume += nextPoint.vloume;
                        maxDate = nextPoint.date;
                    }
                }
                if (foundUpperBoundary && foundLowerBoundary) break;
            }
            var vw = new DataView(table,
                                  string.Format("{0} >= '{1:yyyy-MM-dd}' and {0} <='{2:yyyy-MM-dd}'",
                                                table.dateColumn.ColumnName,
                                                minDate, maxDate), string.Format("{0} asc", table.dateColumn.ColumnName),
                                  DataViewRowState.CurrentRows);
            return vw;
        }
        private DtstCOI.company_shares_tradedRow GetPrevPoint(DateTime date)
        {
            try
            {
                var nextDate=(DateTime)SharesTradedTable.Compute(
                    string.Format("max ({0})", SharesTradedTable.dateColumn.ColumnName)
                    , string.Format("{0} < '{1:yyyy-MM-dd}'"
                    , SharesTradedTable.dateColumn.ColumnName
                    , date));
                var row = SharesTradedTable.FindBycompany_namedatesource_idintervalDays(_companyName, nextDate, _sourceID, _intervalDays);
                return row;
            }
            catch
            {
                return null;
            }
        }
        private DtstCOI.company_shares_tradedRow GetNextPoint(DateTime date)
        {
            try
            {
                var nextDate = (DateTime)SharesTradedTable.Compute(
                    string.Format("min ({0})", SharesTradedTable.dateColumn.ColumnName)
                    , string.Format("{0} > '{1:yyyy-MM-dd}'"
                    , SharesTradedTable.dateColumn.ColumnName
                    , date));
                var row = SharesTradedTable.FindBycompany_namedatesource_idintervalDays(_companyName, nextDate, _sourceID, _intervalDays);
                return row;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}