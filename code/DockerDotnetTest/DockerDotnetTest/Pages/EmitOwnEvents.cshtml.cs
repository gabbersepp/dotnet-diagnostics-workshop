using DockerDotnetTest.Scenario.EmitOwnEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class EmitOwnEventsModel : PageModel
    {
        [BindProperty]
        public string PostParameter { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        private static bool requestStop = false;

        public void OnGetSendRandomNumbers()
        {
            requestStop = false;

            var t = new Thread(() =>
            {
                var random = new Random();

                while (!requestStop)
                {
                    EmitOwnEventsEventSource.RandomEventSource.Info(EventSourceKeywords.Int, "Random.Int.Periodical",
                        random.Next().ToString());
                    EmitOwnEventsEventSource.RandomEventSource.Info(EventSourceKeywords.Double,
                        "Random.Double.Periodical", random.NextDouble().ToString());

                    Thread.Sleep(500);
                }
            });

            Status = "Timer erfolgreich erstellt";
        }

        public void OnGetStopSendRandomNumbers()
        {
            requestStop = true;
            Status = "Timer beendet";
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            EmitOwnEventsEventSource.RequestContentEventSource.Info(EventSourceKeywords.PostParameter, "OnPost", PostParameter);
        }
    }
}
