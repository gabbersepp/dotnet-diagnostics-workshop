namespace ClassLibrary1
{
    public class Class1
    {
        public void Bla()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            throw new Exception();
        }
    }
}