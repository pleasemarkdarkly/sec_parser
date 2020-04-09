using System;
namespace COI.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new IngestEngine(90,true);
            var sleepSecs = 30;
            while (true)
            {
                try
                {
                    var companies = engine.GetSrc10OwnershipQ();
                    var rowcount = companies.Rows.Count;
                    if (rowcount == 0)
                    {
                        if (sleepSecs < 300) sleepSecs += 10;
                        Console.WriteLine("Q empty, will sleep for " + sleepSecs + " seconds.");
                        for (var i = 0; i < sleepSecs; i += 5)
                        {
                            Console.Write(i.ToString() + "..");
                            System.Threading.Thread.Sleep(5000);
                            if (ChekConsoleExitCondition()) return;
                        }
                        Console.WriteLine();
                    }
                    else {sleepSecs = 10;
                        engine.ProcessSrc10OwnershipQ(companies);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                if (ChekConsoleExitCondition()) return;
                System.Threading.Thread.Sleep(100);
            }
        }
        static bool ChekConsoleExitCondition()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey().Key == System.ConsoleKey.Escape)
                {
                    Console.WriteLine("...Thanks for using the Corner Office Investigations engine ... press enter to close this window");
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }
    }
}

