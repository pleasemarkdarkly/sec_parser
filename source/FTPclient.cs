using System;
using System.IO;
using System.Net;
namespace SECCrawler.FTPClient
{
    public class FtpClient
    {
        private readonly string _remotePath;
        private readonly string _localPath;

        public FtpClient(string remotePath, string localPath)
        {
            _remotePath = remotePath;
            _localPath = localPath;
        }
        public void Download(string downloadPartialUrl, bool useConsole,out string errorLog)
        {
            errorLog = string.Empty;
            Stream responseStream = null;
            FileStream fileStream = null;
            string downloadFullURL;
            if (_remotePath.EndsWith("/") ^ downloadPartialUrl.StartsWith("/"))
                downloadFullURL = _remotePath + downloadPartialUrl;
            else if (_remotePath.EndsWith("/") & downloadPartialUrl.StartsWith("/"))
                downloadFullURL = _remotePath + downloadPartialUrl.Substring(01);
            else
                downloadFullURL = _remotePath + "/" + downloadPartialUrl;
            try
            {if (useConsole)
                Console.WriteLine("downloading: " + downloadFullURL);
                var downloadRequest =
                    (FtpWebRequest)WebRequest.Create(downloadFullURL);
                var downloadResponse =
                    (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();

                string fileName =
                    Path.Combine(_localPath, downloadPartialUrl.Replace("/", @"\"));
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                fileStream = File.Create(fileName);
                var buffer = new byte[1024];
                var totalBytes = 0;
                var bufferTimes = 0;
                while (true)
                {
                    int bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    totalBytes += bytesRead;
                    bufferTimes++;
                    if (bufferTimes >= 25 & useConsole)
                    {
                        Console.WriteLine("Bytes:" + totalBytes.ToString());
                        bufferTimes = 0;
                    }
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
                if (useConsole)
                    Console.WriteLine("Bytes:" + totalBytes.ToString());

            }
            catch (Exception ex)
            {
                if (useConsole)
                    Console.WriteLine("Error downloading file.");
                errorLog = ex.ToString();
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
        }

    }

}
