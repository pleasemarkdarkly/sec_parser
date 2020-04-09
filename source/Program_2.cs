using System;
using SECCrawler.IngestLib;

namespace SECCrawler.Ingestor
{
    class Program
    {
        static void Main(string[] args)
        {
            var ingestor = new IngestSecFormBlob(true);
            int sleepSecs = 30;
            while (true)
            {
                try
                {
                    var rowcount=ingestor.DoIngest();
                    if (rowcount==0)
                    {
                        if(sleepSecs<300)sleepSecs+=10;
                        Console.WriteLine("Q empty, will sleep for " + sleepSecs +" seconds.");
                        for(int i=0;i<sleepSecs;i+=5)
                        {
                            Console.Write(i.ToString() + "..");
                            System.Threading.Thread.Sleep(5000);
                            if (ChekConsoleExitCondition()) return;
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                       sleepSecs= 10;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                if (ChekConsoleExitCondition()) return;
                System.Threading.Thread.Sleep(500);
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
