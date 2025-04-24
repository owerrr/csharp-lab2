using BitMatrixLib;
using System.Collections;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BitMatrix b1 = new(3, 4, 1);
            Console.WriteLine(b1);
            int[] bits = { 1, 2, 3, 4, 5, 6, 0, 0, 1, 2, 3, 0, 0, 0, 0, 3 };
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
            // operator `==`, `!=`
            // dwa `null`
            BitMatrix m1 = null;
            BitMatrix m2 = null;
            Console.WriteLine(m1 == m2);
            Console.WriteLine(m1 != m2);


            // indekser - indeksy poza zakresem
            int[] arr = new int[] { -1, 1, 3, 4 };
            foreach (var i in arr)
                foreach (var j in arr)
                {
                    var m = new BitMatrix(3, 4);
                    try
                    {
                        m[i, j] = 1;
                        Console.WriteLine($"m[{i}, {j}] = {m[i, j]}");
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine($"m[{i}, {j}] = exception");
                    }
                }

            // sprawdzenie, czy tworzona
            // jest niezależna kopia
            var am = new BitMatrix(2, 3);
            var am1 = (BitMatrix)(am.Clone());
            am[0, 0] = 1;
            Console.WriteLine(am[0, 0] != am1[0, 0]);
            string s = @"1111
0000
1100";
            BitMatrix sm = BitMatrix.Parse(s);
            Console.WriteLine(sm.NumberOfRows);
            Console.WriteLine(sm.NumberOfColumns);
            Console.WriteLine(sm);

            // konwersja jawna z `int[,]` na `BitMatrix`
            // dane poprawne
            string s2 = @"01
10";
            int[,] a = new int[,] { { 0, 1 }, { 1, 0 } };
            var cm = (BitMatrix)a;
            Console.WriteLine(2 == cm.NumberOfRows);
            Console.WriteLine(2 == cm.NumberOfColumns);
            var test = cm.ToString();
            Console.WriteLine(s2 == test);

            int[,] test2 = cm;
            Console.WriteLine(test2);
            bool[,] bool_test = new bool[,] { { false, true }, { true, false } };
            var bool_test_matrix = (BitMatrix)bool_test;
            bool[,] testowe = bool_test_matrix;
            Console.WriteLine(testowe[0, 1] == bool_test[0, 1]);

            // konwersja `BitMatrix` na `BitArray`
            var c1m = new BitMatrix(2, 3, 1, 0, 1, 1, 1, 0);
            BitArray bitArr = (BitArray)c1m;

            Console.WriteLine(c1m.NumberOfRows * c1m.NumberOfColumns == bitArr.Count);
            //Console.WriteLine(c1m);
            //foreach(var bit in bitArr)
            //{
            //    Console.WriteLine(bit.ToString());
            //}
            //Console.WriteLine(bitArr);

            for (int i = 0; i < c1m.NumberOfRows; i++)
                for (int j = 0; j < c1m.NumberOfColumns; j++)
                {
                    Console.WriteLine($"TESTUJE: {c1m[i, j]} == {bitArr[i * c1m.NumberOfColumns + j]}");
                    if (c1m[i, j] != BitMatrix.BoolToBit(bitArr[i * c1m.NumberOfColumns + j]))
                    {
                        Console.WriteLine("Fail");
                        return;
                    }
                }

            // czy niezależna kopia
            c1m[1, 2] = 1;

            Console.WriteLine(c1m[1, 2] != BitMatrix.BoolToBit(bitArr[5]));

            // operator !
            // poprawne dane
            var m4 = new BitMatrix(2, 3, 1, 0, 1, 0, 1, 0);
            var expectedm5 = new BitMatrix(2, 3, 0, 1, 0, 1, 0, 1);
            var m5 = !m4;
            if (expectedm5.Equals(m5))
                Console.WriteLine("Correct data: Pass");

            //czy niezależna kopia
            if (!ReferenceEquals(m4, m5))
                Console.WriteLine("Correct data, ReferenceEquals: Pass");
            m4[1, 2] = 1; // było 0
            Console.WriteLine(m4[1, 2] == m5[1, 2]);

            // argument null
            try
            {
                m4 = !(BitMatrix)null;
                Console.WriteLine(m4);
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
