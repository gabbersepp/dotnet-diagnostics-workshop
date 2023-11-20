using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDotnetTest.Pages
{
    public class MonitorModel : PageModel
    {
        private static TestObject Obj;

        public string Status { get; set; } = string.Empty;

        public string HashCode { get; set; } = string.Empty;
        public string ThreadId { get; set; } = string.Empty;

        static MonitorModel()
        {
            Obj = new TestObject();
        }

        public void OnGet()
        {
        }

        public void OnGetHashAndLock()
        {
            OnGetResetObject();
            OnGetLockObject();
            OnGetRequestHashcode();
            Status = "Objekt resettet + Hash erzeugt + Lock erzeugt";
        }

        public void OnGetResetObject()
        {
            Obj = new TestObject();
            ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            Status = "Objekt resettet";
        }

        public void OnGetLockObject()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            Monitor.Enter(Obj);
            Status = "Lock erzeugt";
        }

        public void OnGetRequestHashcode()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            HashCode = $"0x{Obj.GetHashCode().ToString("X")}";
            Status = "Hashcode erzeugt";
        }
    }

    public class TestObject
    {
        public int Dummy = 1234;
    }
}
