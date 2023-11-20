using System.Diagnostics;

namespace CompareUserKernelModeSampling;

public class Program
{
    private static void LongRunningWhile()
    {
        Thread.CurrentThread.Name = "Long running WHILE";

        var sw = new Stopwatch();
        sw.Start();
        while (sw.ElapsedMilliseconds < 60 * 1000)
        {
            if (sw.ElapsedMilliseconds % 1000 == 0)
            {
                // just give the remaining stuff time to do something
                // otherwise I was not able to complete the trace
                Thread.Sleep(1);
            }
        }
    }

    private static void Sleep()
    {
        // this is just to ensure that our function appears in the trace
        /*            var sw = new Stopwatch();
                    sw.Start();
                    while (sw.ElapsedMilliseconds < 10 * 1000)
                    {
                        if (sw.ElapsedMilliseconds % 10 == 0)
                            Thread.Sleep(1);
                    }
        */
        Thread.CurrentThread.Name = "Thread.Sleep";
        Thread.Sleep(60 * 1000);
    }

    private static async Task<bool> Delay()
    {
        // this is just to ensure that our function appears in the trace
        /*            var sw = new Stopwatch();
                    sw.Start();
                    while (sw.ElapsedMilliseconds < 10 * 1000)
                    {
                        if (sw.ElapsedMilliseconds % 10 == 0)
                            Thread.Sleep(1);
                    }
        */
        await Task.Delay(60 * 1000);


        return true;
    }

    public static void Main()
    {
        Console.ReadKey();
        Console.WriteLine("Start");

        var t1 = new Thread(LongRunningWhile);
        t1.Start();

        var t2 = new Thread(Sleep);
        t2.Start();

        var task = Task.Run(async () => await Delay());

        t1.Join();
        t2.Join();
        var taskResult = task.Result;

        Console.WriteLine("Finished");
    }
}