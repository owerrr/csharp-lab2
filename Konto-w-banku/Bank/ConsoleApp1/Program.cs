using Bank;

namespace ConsoleApp1
{
    public class Program
    {
        public delegate void test(string mess);
        public static void msgMt(string sd)
        {
            Console.WriteLine(sd);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========TESTOWANIE RZUTOWANIA");
            var molenda = new Konto("Molenda", 100);
            Console.WriteLine(molenda + "\n");
            molenda = molenda.ConvertToPlus();
            Console.WriteLine("po rzutowaniu Konto=>KontoPlus:\n" + molenda + "\n");
            molenda.Wyplata(100);
            Console.WriteLine(molenda + "\n");
            molenda = molenda.ConvertToKonto();
            Console.WriteLine("po rzutowaniu KontoPlus=>Konto:\n" + molenda+"\n");

            KontoLimit testLimit = new("klient1", 200, 500);
            Console.WriteLine(testLimit + "\n");
            testLimit.Wplata(200);
            Console.WriteLine(testLimit + "\n");
            testLimit.Wyplata(500);
            Console.WriteLine(testLimit + "\n");
            testLimit.Wplata(500);
            Console.WriteLine(testLimit + "\n");
            KontoPlus testLimitToPlus = (KontoPlus)testLimit;
            Console.WriteLine(testLimitToPlus + "\n");
            Konto testLimitToDefault = (Konto)testLimit;
            Console.WriteLine(testLimitToDefault);
            Console.WriteLine("=============KONIEC TESTOWANIA\n");

            Bank.Bank b1 = new();

            // TODO:
            //Console.WriteLine("Do you want to save logs? (Y/n)");
            //string ans = Console.ReadLine();
            //if (ans == "" || ans.ToLower() == "y")
            //{
            //    Console.WriteLine("saving logs...");
            //}


        }
    }
}
