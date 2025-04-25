using ChmodConverterLib;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ans = "";
            Console.WriteLine("Type argument to convert");
            Console.WriteLine("To stop program type: end");
            while(ans.ToLower() != "end")
            {
                Console.Write(" > ");
                ans = Console.ReadLine();
                try
                {
                    if (Char.IsDigit(ans[0]))
                        Console.WriteLine($"ans = {ChmodConverter.NumericToSymbolic(Int32.Parse(ans))}");
                    else
                        Console.WriteLine($"ans = {ChmodConverter.SymbolicToNumeric(ans)}");
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return;
        }
    }
}
