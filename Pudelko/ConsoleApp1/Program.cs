using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PudelkoLibrary;

public static class PudelkoExtended
{
    public static Pudelko Kompresuj(this Pudelko p)
    {
        double obj = p.Objetosc;
        double sideLen = Math.Pow(obj, 1d / 3d);
        return new Pudelko(sideLen, sideLen, sideLen);
    }

    public static string WypiszPudelka(this List<Pudelko> p)
    {
        string res = "";
        for (int i = 0; i < p.Count; i++)
        {
            res += $"{i + 1}. {p[i]}\n";
        }
        return res;
    }
}

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Tworzenie pudełka prawidłowo:");
        Pudelko p1 = new(4.2314d, 1.2344d, 1.2134d);
        Console.WriteLine(p1);
        try
        {
            Console.WriteLine("\nTworzenie pudełka, wartości ujemne");
            Pudelko p2 = new(-2, 5, 5);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        try
        {
            Console.WriteLine("\nTworzenie pudełka, wartości powyżej 10m:");
            Pudelko p3 = new(11, 5, 5);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine("\nTworzenie domyślnego pudełka [cm]:");
        Pudelko p4 = new();
        Console.WriteLine(p4.ToString("cm"));

        Console.WriteLine($"\nZwracanie wartości A, B, C pudełka:" +
                        $"\n{p1.A}, {p1.B}, {p1.C}");

        Console.WriteLine($"\nMetoda ToString Pudelka na 4 sposoby:" +
                        $"\nBRAK\t=\t{p1}" +
                        $"\nM\t=\t{p1.ToString("m")}" +
                        $"\nCM\t=\t{p1.ToString("cm")}" +
                        $"\nMM\t=\t{p1.ToString("mm")}");

        Console.WriteLine($"\nObjetosc pudelka p1:\n{p1.Objetosc}" +
                        $"\nPole pudelka p1:\n{p1.Pole}");

        Pudelko p5 = new();
        Console.WriteLine($"\nPorównanie pudełek:\n{p4} == {p5} ? {p4 == p5}");
        Console.WriteLine($"\nPorównanie pudełek:\n{p4} != {p5} ? {p4 != p5}");

        Console.WriteLine($"\nDodawanie pudełek:\n{p4} + {p5} = {p4 + p5}");

        Console.WriteLine($"\nKonwersja pudełka {p4}:");
        double[] p4_converted = (double[])p4;
        foreach(var x in p4_converted)
        {
            Console.Write($"{x} ");
        }

        Console.WriteLine($"\n\nKonwersja ValueTuple<int, int, int> na Pudelko:");
        ValueTuple<int, int, int> p6_tuple = (1000, 1000, 1000);
        Pudelko p6 = p6_tuple;
        Console.WriteLine(p6);

        Console.WriteLine($"\nIndex 0 w pudełku {p6} to:\n{p6[0]}");

        Console.WriteLine($"\nTest metody foreach na pudelku {p6}:");
        foreach(var x in p6)
        {
            Console.Write($"{x} ");
        }

        Console.WriteLine("\n\nTest metody Parse ze stringa:");
        Pudelko p7 = Pudelko.Parse("2.500 m × 9.321 m × 0.100 m");
        Console.WriteLine(p7);

        List<Pudelko> pudelka = new List<Pudelko>
        {
            new(4.1d, 5d, 2.7d, UnitOfMeasure.meter),
            new(500d, 237d, 189d, UnitOfMeasure.centimeter),
            new(2370d, 5000d, 1890d, UnitOfMeasure.milimeter),
            new(5.7d, 2d),
            new(4d),
            new(7d, 6.1d, 2d)
        };  

        Console.WriteLine("\nAKTUALNE PUDELKA:");
        Console.WriteLine(pudelka.WypiszPudelka());
        

        Console.WriteLine("\nTrwa sortowanie pudełek...");
        Comparison<Pudelko> comparePudelka = (p1, p2) =>
        {
            int CompareObj = p1.Objetosc.CompareTo(p2.Objetosc);
            if (CompareObj != 0) return CompareObj;

            int ComparePole = p1.Pole.CompareTo(p2.Pole);
            if (ComparePole != 0) return ComparePole;

            return (p1.A+p1.B+p1.C).CompareTo(p2.A+p2.B+p2.C);
        };
        pudelka.Sort(comparePudelka);
        Console.WriteLine(pudelka.WypiszPudelka());
    }
}