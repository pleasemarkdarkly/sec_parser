using System.Collections.Generic;
using System.Data;
using System.Web;

namespace COI.Util.Web
{
    public class ExcelAttachment
    {
        public static void ReplaceResponseWithXLS(DataSet d,HttpResponse response, List<string> summary,string fileName)
        {
            response.ClearContent();
            response.ContentType = "application/ms-excel";
            response.AppendHeader("Content-disposition", "Attachment; filename="+fileName+".xml");
            ExcelEngine.Convert(d, response.OutputStream, 2, 2, summary);
            response.End();
        }
    }
}
