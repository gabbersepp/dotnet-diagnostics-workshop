using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class SimpleSocketTracesModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string EchoMessage => new string(Message.Reverse().ToArray());

        public string JsonResponse { get; set; } = string.Empty;

        public string HostNameResult { get; set; } = string.Empty;
        public string PortResult { get; set; } = string.Empty;
        public string CertResult { get; set; } = string.Empty;


        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };

        public void OnGet()
        {
        }

        public IActionResult OnPostDoAjaxEcho(Data data)
        {
            return new JsonResult(new string(data.Message.Reverse().ToArray()));
        }

        public async Task<IActionResult> OnGetDoSomethingAsync()
        {
            using HttpResponseMessage response = await httpClient.GetAsync("todos/3");

            JsonResponse = await response.Content.ReadAsStringAsync();

            Status = "Todo Liste abgefragt";

            return Page();
        }

        public async Task<IActionResult> OnGetHostnameNotResolveable()
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://google123456.de");
            var result = client.GetAsync("/").Result;
            HostNameResult = await result.Content.ReadAsStringAsync();

            Status = "ungültigen hostname angefragt";

            return Page();
        }        
        
        public async Task<IActionResult> OnGetPortNotReachable()
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://biehler-josef.de:12345");
            var result = client.GetAsync("/").Result;
            PortResult = await result.Content.ReadAsStringAsync();

            Status = "Nicht erreichbaren Port abgefragt";

            return Page();
        }
        
        public async Task<IActionResult> OnGetCertInvalid()
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://untrusted-root.badssl.com/");
            var result = client.GetAsync("/").Result;
            CertResult = await result.Content.ReadAsStringAsync();

            Status = "Seite mit ungültigem Zertifikat abgefragt";

            return Page();
        }
    }

    public class Data
    {
        public string Message { get; set; }
    }
}
