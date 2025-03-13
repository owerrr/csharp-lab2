using ConsoleApp1.Classes;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Pracownik p1 = new Pracownik("Andrzej", new DateTime(2024, 03, 13), 4000);
            Pracownik p2 = new Pracownik("Andrzej", new DateTime(2024, 03, 13), 4000);
            Pracownik p3 = new Pracownik("Robert", new DateTime(2023, 12, 11), 4100);
            Pracownik p4 = new Pracownik("Kacper", new DateTime(2016, 11, 12), 4200);
            Pracownik p5 = new Pracownik("Michal", new DateTime(2019, 06, 04), 4300);

            List<Pracownik> pracownicy = [p1, p2, p3, p4 ,p5]; 

            foreach(Pracownik p in pracownicy)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine();

            pracownicy.Sort();

            foreach (Pracownik p in pracownicy)
            {
                Console.WriteLine(p);
            }

            if (p1 == p2)
            {
                Console.WriteLine($"{p1} == {p2}");
            }
            if(p1 != p3)
            {
                Console.WriteLine($"{p1} != {p3}");
            }

            //Console.ReadLine();
        }
    }
}
