using System;

namespace Stos
{
    class Program
    {
        static void Main(string[] args)
        {
            StosWTablicy<string> s = new StosWTablicy<string>(2);
            System.Console.WriteLine(s.Capacity);
            s.Push("km");
            s.Push("aa");
            s.Push("xx");
            System.Console.WriteLine(s.Capacity);
            for(int i = 0 ; i < 15; i++) // 3 + 14 = 17
                s.Push("aa");
            System.Console.WriteLine(s.Capacity);
            s.TrimExcess();
            System.Console.WriteLine(s.Capacity);
            System.Console.WriteLine(s.Count);
            foreach (var x in s.ToArray())
                Console.WriteLine(x);

            Console.WriteLine();

            Console.WriteLine(s[1]);

            Console.WriteLine();
            foreach(string d in s){
                Console.WriteLine($" > {d}");
            }
        }
    }
}
