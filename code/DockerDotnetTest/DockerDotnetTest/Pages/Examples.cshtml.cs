using System.Diagnostics;
using DockerDotnetTest.Scenario.Exceptions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using DockerDotnetTest.Scenario.DynamicClasses;
using DockerDotnetTest.Scenario.Memory;
using Microsoft.IdentityModel.Tokens;

namespace DockerDotnetTest.Pages
{
    public class ExamplesModel : PageModel
    {
        private readonly IMemoryConsumption _memoryConsumption;
        public string Status { get; set; } = string.Empty;

        public ExamplesModel(IMemoryConsumption memoryConsumption)
        {
            _memoryConsumption = memoryConsumption;
        }

        public string Message { get; set; } = "";
        public void OnGet()
        {
        }

        public void OnGetRestrictiveGetter()
        {
            var obj = new TestClass();
            var proxy = RestrictiveGetterClassBuilder.Build(obj) as TestClass;

            try
            {
                Console.WriteLine(proxy.Field);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            proxy.Field = "1234";
            Console.WriteLine(proxy.Field);
            Status = "RestrictiveGetter abgeschlossen";
        }

        public void OnGetThreadPoolStarvationFull()
        {
            //ThreadPool.SetMinThreads(2000, 10);
            Task.Run(() =>
            {
                for (var i = 0; i < 2000; i++)
                {
                    Task.Run(async () =>
                    {
                        // force the creation of an async task
                        await Task.Delay(10);

                        // now do evil stuff
                        Thread.Sleep(10 * 1000);
                    });
                }

                Console.WriteLine("tasks erstellt");
            });

            Status = "ThreadPoolStarvation abgeschlossen. Starte 'Say Hello' Request";
        }

        public void OnGetSayHelloAfterThreadPoolStarvation()
        {
            Message = "Hello";
        }

        private static object LockObject = new object();

        public void OnGetSetupSql()
        {
            string connectionString =
                $"Data Source={Environment.GetEnvironmentVariable("SQLIP")};Initial Catalog=master;User Id=sa;Password=Test1234!;TrustServerCertificate=True";

            string queryString = @"
CREATE TABLE Test (
    Field1 nvarchar(max)
);
INSERT INTO test(Field1) VALUES('Test');
";

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection = connection;
                    command.CommandTimeout = 60 * 10;
                    
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Status = "SQL Setup abgeschlossen";
        }

        public void OnGetLockAndQuery()
        {
            lock (LockObject)
            {
                string connectionString =
                    $"Data Source={Environment.GetEnvironmentVariable("SQLIP")};Initial Catalog=master;User Id=sa;Password=Test1234!;TrustServerCertificate=True";

                string queryString = @"
SELECT * FROM Test WITH(HOLDLOCK)
";

                using (SqlConnection connection =
                       new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlTransaction transaction;
                        transaction = connection.BeginTransaction();
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Connection = connection;
                        command.CommandTimeout = 60 * 10;
                        command.Transaction = transaction;

                        Console.WriteLine("before reading data");
                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("after reading data");
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            Status = "LockAndQuery beendet";
        }
        
        public void OnGetQueryAndLock()
        {
            string connectionString =
                $"Data Source={Environment.GetEnvironmentVariable("SQLIP")};Initial Catalog=master;User Id=sa;Password=Test1234!;TrustServerCertificate=True";

            string queryString = @"
UPDATE Test SET Field1 = Field1
";

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlTransaction transaction;
                    transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection = connection;
                    command.CommandTimeout = 60 * 10;
                    command.Transaction = transaction;

                    Console.WriteLine("before updating data");
                    command.ExecuteNonQuery();
                    Console.WriteLine("after updating data");
                    Console.WriteLine("try to obtain lock in 10 seconds");

                    Thread.Sleep(10 * 1000);

                    lock (LockObject)
                    {
                        Console.WriteLine("Obtained lock");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Status = "QueryAndLock beendet";
        }

        public void OnGetGcCollect()
        {
            GC.Collect();

            Status = "GC manuell ausgeführt";
        }

        public void OnGetCreateSameDynamicClassSeveralTimes()
        {
            var memberNames = new List<string>();
            for (var i = 0; i < 500; i++)
            {
                memberNames.Add($"Property{i}");
            }
            for (var i = 0; i < 10000; i++)
            {
                DynamicClassHelper.CreateAnonymousType(memberNames);
            }

            Status = "Speicher vollgemüllt mit dynamischen Klassen";
        }

        public void OnGetProduceManagedLeak()
        {
            _memoryConsumption.AddCache();

            Status = "Managed leak produziert am: " + DateTimeOffset.Now.ToString();
        }

        public void OnGetFatalException()
        {
            int test = 0;

            unsafe
            {
                int* ptr = &test;
                *(ptr + 100000) = 1;
            }

            Status = "FatalException erzeugt";
        }

        public void OnGetHighCpu()
        {
            var sw = new Stopwatch();
            sw.Start();

            var idleThread = new Thread(() =>
            {
                while (sw.ElapsedMilliseconds < 120000)
                {
                    Console.WriteLine("Hello from idle thread");
                    // just do nearly nothing
                    Thread.Sleep(5);
                }
            });
            idleThread.Start();

            var cpuThread = new Thread(() =>
            {
                while (sw.ElapsedMilliseconds < 120000)
                {
                    if (sw.ElapsedMilliseconds % 1000 == 0)
                    {
                        // just give the remaining stuff time to do something
                        // otherwise I was not able to complete the trace
                        Thread.Sleep(1);
                    }
                }
            });

            cpuThread.Priority = ThreadPriority.Lowest;
            cpuThread.Start();

            Status = "CPU wird ausgelastet";
        }

        public void OnGetMemoryConsumptionSource()
        {
            var sw = new Stopwatch();
            sw.Start();
            var list = new List<object>();

            while (sw.ElapsedMilliseconds < 120000)
            {
                list.Add(new ExamplesModel(new MemoryConsumption()));

                if (sw.ElapsedMilliseconds % 1000 == 0)
                {
                    // just give the remaining stuff time to do something
                    // otherwise I was not able to complete the trace
                    Thread.Sleep(1);
                }
            }

            Status = "Speicher wurde vollgemüllt";
        }
    }
}
