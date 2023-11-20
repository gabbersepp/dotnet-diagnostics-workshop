using System.Diagnostics;
using DockerDotnetTest.Scenario.EventCounter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class EventCountersModel : PageModel
    {
        static Random random = new Random();
        public string Status { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public void OnGetRandomRequestDuration()
        {
            var sw = new Stopwatch();
            sw.Start();
            Thread.Sleep(random.Next(2000));
            sw.Stop();
            EventCounterEventSource.NumberOfRequests.Increment();
            EventCounterEventSource.RequestTime.WriteMetric(sw.ElapsedMilliseconds);
            Status = "Request dauerte " + sw.ElapsedMilliseconds + " ms";
        }

        public void OnGetAllocateMemory()
        {
            var sw = new Stopwatch();
            sw.Start();
            var list = new List<byte[]>();

            while (sw.ElapsedMilliseconds < 2 * 60 * 1000)
            {
                list.Add(new byte[random.Next(300)].Select(x => (byte)1).ToArray());
                Thread.Sleep(10);
            }

            Status = list.Sum(x => x.Length) + " bytes reserviert";
        }
    }
}
