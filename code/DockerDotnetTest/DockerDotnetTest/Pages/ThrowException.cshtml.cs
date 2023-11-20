using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class ThrowExceptionModel : PageModel
    {
        public string Status { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public void OnPost()
        {
            throw new Exception();
        }

        public void OnPostTryCatch()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
            }

            Status = "Exception gefangen";
        }
    }
}
