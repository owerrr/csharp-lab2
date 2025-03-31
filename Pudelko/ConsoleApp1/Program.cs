using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PudelkoLibrary;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("TEST");
        Pudelko p1 = new(2.5d, 9.321d, 0.1d, UnitOfMeasure.meter);

        Console.WriteLine(p1.ToString());
        Console.WriteLine(p1.ToString("cm"));
        Console.WriteLine(p1.ToString("mm"));
        Console.WriteLine(p1.ToString("m"));
        Console.WriteLine($"{p1.Objetosc} m3");
        Console.WriteLine($"{p1.Pole} m2");

        Pudelko p2 = new(1d, 2.1d, 3.05d, UnitOfMeasure.meter);
        Pudelko p3 = new(1d, 3.05d, 2.1d, UnitOfMeasure.meter);

        Console.WriteLine(p2 == p3);
        Console.WriteLine(p2+p3);
        Console.WriteLine(p2[0]);
        Console.WriteLine(p2[1]);
        Console.WriteLine(p2[2]);
        //Console.WriteLine(p2[3]);
        Console.WriteLine("FOREACH =");
        foreach(var item in p2)
        {
            Console.WriteLine($"{item}"); 
        }

        Pudelko p4 = Pudelko.Parse("2.500 m × 9321 mm × 0.100 m");
    }
}