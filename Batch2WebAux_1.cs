using System;
using System.IO;
using System.Text;
using System.Web;
using COI.DAL;
namespace COI.WebUI.Investigator.Batch
{
    public class Batch2WebAux:TextWriter
    {
        public override Encoding Encoding
        {
            get { return _log.Encoding; }
            //set { _log.Encoding = value; }
        }
        private readonly StringWriter _log=new StringWriter();
        private readonly HttpResponse _response;
        private readonly BatchManager.BatchNamesEnum _batchType;
        private readonly string _batchName;
        private double _progress = 0D;
        private int _bufferSize = 0;
        private DateTime _lastFlush = DateTime.Now;
        public Batch2WebAux(HttpResponse response,BatchManager.BatchNamesEnum batchType)
        {   _response = response;
            _batchType = batchType;
            _batchName=BatchManager.BatchNames[(int)batchType];}
        public void AddHead()
        {
            _response.Output.WriteLine(Head);
        }
        public void AddFoot()
        { _response.Output.WriteLine(Foot); }
        //do batch
        public override void WriteLine(string line)
        {
            _log.WriteLine(line);
            _response.Output.WriteLine(line.Replace("\r\n", "<br />\r\n") + "<br />\r\n");
            _bufferSize += line.Length;
            if(_bufferSize>2000) Flush();
            if((DateTime.Now-_lastFlush).TotalSeconds>10) Flush();
        }
        public void SetProgress(double progress)
        {
            if (progress - _progress <= 0.01) return;
            _progress = progress;
            _response.Output.WriteLine("<hr />Progress:{0:F3}%<hr />\r\n", progress*100D);
            Flush();
        }
        public override void Flush()
        {
            _response.Flush();
            _bufferSize = 0;
            _lastFlush = DateTime.Now;
        }
        //finish
        public string GetOutput(){return _log.ToString(); }
        public void Save()
        {
            var dal = new BatchManager();
            var table = new DtstCOI.batch_runDataTable();
            table.Addbatch_runRow(_batchName, DateTime.Now, GetOutput());
            dal.Save(table);
        }
        public static string Head
        {
            get
            {
                var s = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                s += "<html xmlns=\"http://www.w3.org/1999/xhtml\" ><head runat=server><title>OTCBB System Changes</title></head><body>";
                s += " <span style=\"font-family: Arial; font-size: small\">\r\n";
                return s;
            }
        }
        public static string Foot
        {get { return "<hr />END OF BATCH JOB</span></body></html>"; }}
    }
}
