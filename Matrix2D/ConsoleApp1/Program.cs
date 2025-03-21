using MatrixClass;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix2D m2d_1 = new(2, 1, 3, 2);
            int[] test = [5, 1, 2, 6];
            Matrix2D m2d_2 = new();
            Matrix2D m2d_3 = Matrix2D.Parse("[32, 41], [15, 27]");

            Matrix2D m2d_4 = new();

            Console.WriteLine("Obecne macierze:");
            Console.WriteLine($"A1 = {m2d_1}");
            Console.WriteLine($"A1 = {m2d_2}");
            Console.WriteLine($"A1 = {m2d_3}");

            Console.WriteLine("Dodawanie:");
            Console.WriteLine($"A1 + A2 = {m2d_1 + m2d_2}\n");

            Console.WriteLine("Odejmowanie:");
            Console.WriteLine($"A1 - A2 = {m2d_1 - m2d_2}\n");

            Console.WriteLine("Mnozenie:");
            Console.WriteLine($"A1 * A2 = {m2d_1 * m2d_2}\n");

            Console.WriteLine("Mnozenie przez liczbe calkowita:");
            Console.WriteLine($"A1 * 3 = {m2d_1 * 3}\n");

            Console.WriteLine("Mnozenie przez liczbe calkowita (na odwrot):");
            Console.WriteLine($"3 * A1 = {3 * m2d_1}\n");

            Console.WriteLine("Zmiana znaków:");
            Console.WriteLine($"-A1 = {-m2d_1}\n");

            Console.WriteLine("Transpozycja");
            Matrix2D.Transpose(m2d_1);
            Console.WriteLine($"A1 transponowane = {m2d_1}\n");
            Matrix2D.Transpose(m2d_3);
            Console.WriteLine($"A3 transponowane = {m2d_3}\n");

            Console.WriteLine("wyznacznik obecnych macierzy:");
            Console.WriteLine($"A1 = {m2d_1.Det()}");
            Console.WriteLine($"A2 = {m2d_2.Det()}");
            Console.WriteLine($"A3 = {m2d_3.Det()}");

            Console.WriteLine("wyznacznik obecnych macierzy (ale metoda statyczna):");
            Console.WriteLine($"A1 = {Matrix2D.Determinant(m2d_1)}");
            Console.WriteLine($"A2 = {Matrix2D.Determinant(m2d_2)}");
            Console.WriteLine($"A3 = {Matrix2D.Determinant(m2d_3)}");

            Console.WriteLine("Jawne rzutowanie z Matrix2D na tablice 2D int: ");
            int[,] converted_m2d_1 = (int[,])m2d_1;
            foreach (int n in converted_m2d_1)
                Console.Write($"{n} ");

            Console.WriteLine("\nCzy macierz X rowna Y: ");
            Console.WriteLine($"{m2d_2} == {m2d_4} ?");
            Console.Write(m2d_2 == m2d_4 ? "TAK" : "NIE");
            Console.WriteLine($"\n{m2d_1} == {m2d_4} ?");
            Console.Write(m2d_1 == m2d_4 ? "TAK" : "NIE");

            //Console.WriteLine("Hello, World!");
        }
    }
}
