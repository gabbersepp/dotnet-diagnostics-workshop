using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class DeadlockMonitorEnterModel : PageModel
    {
        public int MethodRuntime { get; set; } = 0;
        public string Status { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public void OnPostTwoMonitor()
        {
            var sw = new Stopwatch();
            sw.Start();

            var locker = new object();
            var locker2 = new object();

            var t1 = new Thread(() =>
            {
                lock (locker)
                {
                    Console.WriteLine("Lock 1 in Thread 1 erhalten");
                    Thread.Sleep(100);

                    lock (locker2)
                    {
                        Console.WriteLine("Lock 2 in Thread 1 erhalten");
                        Thread.Sleep(100);
                    }
                }

                Console.WriteLine("Thread 1 Ende");
            });

            var t2 = new Thread(() =>
            {
                lock (locker2)
                {
                    Console.WriteLine("Lock 2 in Thread 2 erhalten");
                    Thread.Sleep(100);

                    lock (locker)
                    {
                        Console.WriteLine("Lock 1 in Thread 2 erhalten");
                        Thread.Sleep(100);
                    }
                }

                Console.WriteLine("Thread 2 Ende");
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            MethodRuntime = (int)sw.ElapsedMilliseconds;

            Status = "Beide Threads beendet";
        }
    }
}
