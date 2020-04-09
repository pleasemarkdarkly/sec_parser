using System;
using SECCrawler.FTPClient;
namespace SECCrawler.Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadInitiator = new DownloadInitiator();
            var sleep = 10;
            while (true)
            {
                var items=downloadInitiator.ProcessTickConsoleVersion();
                    Console.WriteLine("Process " + items.ToString() 
                        + " with no errors" + DateTime.Now.ToString());
                if (items==0)
                {
                    Console.WriteLine("sleeping a few seconds " + sleep.ToString());
                    for (var i=0;i<sleep;i++)
                    {
                        System.Threading.Thread.Sleep(1000);
                        if (ChekConsoleExitCondition()) return;
                    }
                    if (sleep<300)sleep+=5;
                }
                else
                {
                    if (ChekConsoleExitCondition()) return;
                    sleep = 5;
                }
            }
        }
        static bool ChekConsoleExitCondition()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey().Key == System.ConsoleKey.Escape)
                {
                    Console.WriteLine("...Thanks for using the SEC Crawler downloader ... press enter to close this window");
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }
    }
}
