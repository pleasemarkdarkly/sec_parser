using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Xml;
using System.IO;
using System.Collections.Generic;
namespace COI.Util
{
    public class ExcelEngine
    {
        private const string Emptycel = "<Cell><Data ss:Type='String'> </Data></Cell>";
        public static void Convert(DataSet ds, Stream outputStream, int emptyRows, int emptyCols, List<string> summary)
        {
            IEnumerable tables = ds.Tables;
            var x = new XmlTextWriter(outputStream, Encoding.UTF8);
            int sheetNumber = 0;
            x.WriteRaw("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?>");
            x.WriteRaw("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
            x.WriteRaw("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
            x.WriteRaw("xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            x.WriteRaw("<Styles><Style ss:ID='sText'><NumberFormat ss:Format='@'/></Style>");
            x.WriteRaw("<Style ss:ID='sDate'><NumberFormat ss:Format='[$-409]m/d/yy\\ h:mm\\ AM/PM;@'/>");
            x.WriteRaw("</Style></Styles>");
            foreach (DataTable dt in tables)
            {
                sheetNumber++;
                var sheetName = !string.IsNullOrEmpty(dt.TableName) ? dt.TableName : "Sheet" + sheetNumber.ToString();
                x.WriteRaw("\r\n<Worksheet ss:Name='" + sheetName + "'>");
                x.WriteRaw("<Table>");
                /*empty cols*/
                for (var j = 0; j < emptyCols; j++) x.WriteRaw("<Column  ss:StyleID='sText'/>");
                var columnTypes = new string[dt.Columns.Count];
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    var colType = dt.Columns[i].DataType.ToString().ToLower();
                    if (colType.Contains("datetime"))
                    {
                        columnTypes[i] = "DateTime";
                        x.WriteRaw("<Column ss:StyleID='sDate'/>");

                    }
                    else if (colType.Contains("string") || colType.Contains("guid"))
                    {
                        columnTypes[i] = "String";
                        x.WriteRaw("<Column ss:StyleID='sText'/>");
                    }
                    else
                    {
                        x.WriteRaw("<Column />");
                        columnTypes[i] = colType.Contains("boolean") ? "Boolean" : "Number";
                    }
                }
                /*empty rows*/
                for (var j = 0; j < emptyRows; j++) x.WriteRaw("\r\n<Row />");
                if (summary != null)
                {
                    foreach (var sum in summary)
                    {
                        x.WriteRaw("\r\n<Row>");
                        for (var j = 0; j < emptyCols; j++) x.WriteRaw(Emptycel);
                        x.WriteRaw("<Cell><Data ss:Type='String'>" + sum + "</Data></Cell></Row>");
                    }
                }
                //column headers
                x.WriteRaw("\r\n<Row>");
                /*empty cols*/
                for (var j = 0; j < emptyCols; j++) x.WriteRaw(Emptycel);
                foreach (DataColumn col in dt.Columns)
                {
                    x.WriteRaw("<Cell ss:StyleID='sText'><Data ss:Type='String'>");
                    x.WriteRaw(col.ColumnName);
                    x.WriteRaw("</Data></Cell>");
                }
                x.WriteRaw("</Row>");
                //data
                foreach (DataRow row in dt.Rows)
                {
                    bool missedNullColumn = false;
                    x.WriteRaw("\r\n<Row>");
                    /*empty cols*/
                    for (var j = 0; j < emptyCols; j++) x.WriteRaw(Emptycel);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            if (missedNullColumn)
                            {
                                int displayIndex = i + 1 + emptyCols;
                                x.WriteRaw("<Cell ss:Index='" + displayIndex.ToString() +
                                           "'><Data ss:Type='" +
                                           columnTypes[i] + "'>");
                                missedNullColumn = false;
                            }
                            else
                            {
                                x.WriteRaw("<Cell><Data ss:Type='" +
                                           columnTypes[i] + "'>");
                            }
                            switch (columnTypes[i])
                            {
                                case "DateTime":
                                    x.WriteRaw(((DateTime)row[i]).ToString("s"));
                                    break;
                                case "Boolean":
                                    x.WriteRaw(((bool)row[i]) ? "1" : "0");
                                    break;
                                case "String":
                                    x.WriteString(row[i].ToString());
                                    break;
                                default:
                                    x.WriteString(row[i].ToString());
                                    break;
                            }

                            x.WriteRaw("</Data></Cell>");
                        }
                        else
                        {
                            missedNullColumn = true;
                        }
                    }
                    x.WriteRaw("</Row>");
                }
                x.WriteRaw("\r\n</Table></Worksheet>");
            }
            x.WriteRaw("</Workbook>");
            x.Flush();

        }
    }
}