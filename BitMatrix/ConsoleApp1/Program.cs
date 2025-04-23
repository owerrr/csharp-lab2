using BitMatrixLib;
using System.Collections;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // operator !
            // poprawne dane
            var m = new BitMatrix(2, 3, 1, 0, 1, 0, 1, 0);
            var expected = new BitMatrix(2, 3, 0, 1, 0, 1, 0, 1);
            var m3 = !m;
            if (expected.Equals(m3))
                Console.WriteLine("Correct data: Pass");

            //czy niezależna kopia
            if (!ReferenceEquals(m, m3))
                Console.WriteLine("Correct data, ReferenceEquals: Pass");
            m[1, 2] = 1; // było 0
            Console.WriteLine(m[1, 2] == m3[1, 2]);

            // argument null
            try
            {
                m = !(BitMatrix)null;
                Console.WriteLine(m);
                Console.WriteLine("Argument null: Fail");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Argument null: Pass");
            }
            Console.ReadLine();
        }
    }
}
