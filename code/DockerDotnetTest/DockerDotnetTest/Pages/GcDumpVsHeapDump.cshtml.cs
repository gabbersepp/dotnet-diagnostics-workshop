using DockerDotnetTest.Scenario.GcDumpVsHeapDump;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class GcDumpVsHeapDumpModel : PageModel
    {
        private static Car? car;

        public string Status { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public void OnGetCreateObject()
        {
            car ??= new Car();
            Status = "'Car' Objekt wurde erstellt, falls noch nicht vorhanden";
        }
    }
}
