using ChmodConverterLib;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string test = "rwxrwxrwx";
            int tmp = ChmodConverter.SymbolicToNumeric(test);
            Console.WriteLine(Convert.ToString(tmp));

            int tmp2 = 777;
            Console.WriteLine(ChmodConverter.NumericToSymbolic(tmp2));
        }
    }
}
