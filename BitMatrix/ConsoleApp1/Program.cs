using BitMatrixLib;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BitMatrix b1 = new(3,4, 1);
            Console.WriteLine(b1);
            int[] bits = { 1, 2, 3, 4, 5, 6, 0, 0, 1, 2, 3, 0, 0, 0, 0, 3};
            BitMatrix b2 = new(5, 4, bits);
            Console.WriteLine(b2);
            int[,] bits2 = { { 1, 0, 1 }, { 0, 1, 1 } };

            BitMatrix b3 = new(bits2);
            Console.WriteLine(b3);
            bool[,] bits3 = {   { true, true, false, false },
                                { false, true, true, false },
                                { true, true, false, false },
                                { true, false, true, false }
                            };
            BitMatrix b4 = new(bits2);
            BitMatrix b5 = new(bits2);
            Console.WriteLine(b3);
            Console.WriteLine(b4 == b5);



            Console.ReadLine();
        }
    }
}
