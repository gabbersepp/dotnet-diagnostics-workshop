using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class CompareUserModeKernelModeCpuSamplingModel : PageModel
    {
        public string Status { get; set; }

        public void OnGet()
        {
        }

        private static void LongRunningWhile()
        {
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
            Thread.Sleep(60 * 1000);
        }

        private static async Task<bool> Delay()
        {
            await Task.Delay(60 * 1000);

            return true;
        }

        public void OnPost()
        {
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

            Status = "Fertig";
        }
    }
}
