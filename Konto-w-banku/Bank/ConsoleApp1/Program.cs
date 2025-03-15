using Bank;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Bank.Bank b1 = new();

            Console.WriteLine("Do you want to save logs? (Y/n)");
            string ans = Console.ReadLine();
            if(ans == "" || ans.ToLower() == "y")
            {
                Console.WriteLine("saving logs...");
            }
        }
    }
}
