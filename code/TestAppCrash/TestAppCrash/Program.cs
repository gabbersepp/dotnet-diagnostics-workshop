using System.Runtime.CompilerServices;
using ClassLibrary1;

namespace TestAppCrash
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Start new Thread");
            new Class1().Bla();
        }
    }
}