using DockerDotnetTest.Scenario.ThreadTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class ThreadTimeModel : PageModel
    {
        public string Status { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public void OnPostNonTask()
        {
            new Requests().GetFeeds();
            Status = "Requests nicht asynchron gesendet";
        }

        public async void OnPostTask()
        {
            await new Requests().GetFeedsAsync();
            Status = "Requests asynchron gesendet";
        }
    }
}
