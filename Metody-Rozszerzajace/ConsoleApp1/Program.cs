namespace ConsoleApp1
{
    public static class Testowa
    {
        public static int? WordCount(this string word, params char[] parameters)
        {
            if (word == null || word.Trim().Length <= 0)
                return null;
            return word.Split(parameters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        static char[] samogloski = { 'a', 'A', 'e', 'E', 'i', 'I', 'o', 'O', 'u', 'U', 'y', 'Y', 'ę', 'Ę', 'ą', 'Ą' };
        public static string BezSamoglosek(this string word)
        {
            string res = "";
            foreach (char c in word)
            {
                if (samogloski.Contains(c))
                    continue;
                res += c;
            }
            return res;
        }

        static char[] allowedChars = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ',', '.' };
        public static bool IsNumeric(this string word)
        {
            bool usedDot = false;
            for (int i = 0; i < word.Length; i++)
            {
                if (i == 0 && word[i] == '-')
                    continue;

                if (word[i] == '.')
                {
                    if (usedDot)
                    {
                        return false;
                    }
                    usedDot = true;
                }

                if (!allowedChars.Contains(word[i]))
                    return false;
            }

            return true;
        }

        public static decimal? Mediana(this IEnumerable<int> wartosc)
        {
            if (wartosc == null || wartosc.Count() == 0) return null;

            wartosc = wartosc.OrderBy(x => x);


            if (wartosc.Count() % 2 == 1)
            {
                int idx_mid = wartosc.Count() / 2;
                return wartosc.ElementAt(idx_mid);
            }
            else
            {
                int idx_mid = wartosc.Count() / 2;
                decimal a = wartosc.ElementAt(idx_mid);
                decimal b = wartosc.ElementAt(idx_mid - 1);

                return (a + b) / 2;
            }
        }

        public static string Dump<T>(this IList<T> lista)
        {
            string tekst = "[";
            foreach (var n in lista)
            {
                if (!Equals(n, lista[lista.Count - 1]))
                    tekst += $"{n}, ";
                else tekst += n;
            }
            tekst += "]";

            return tekst;
        }

        public static string Dump<T>(this IEnumerable<T> lista)
        {
            string tekst = "{";
            bool isFirst = true;
            foreach (var n in lista)
            {
                if (isFirst)
                {
                    isFirst = false;
                    tekst += n;
                }
                else
                {
                    tekst += $", {n}";
                }
            }
            tekst += "}";

            return tekst;
        }

        public static bool Between<T>(this T val, T lower, T upper, Comparison<T>? comp = null) where T : IComparable<T>
        {
            if (val is null || lower is null || upper is null) return false;

            if (comp == null)
            {
                //Console.WriteLine($"{val.CompareTo(lower)} >= 0 && {val.CompareTo(upper)}");
                return val.CompareTo(lower) >= 0 && val.CompareTo(upper) <= 0;
            }
            else
            {
                //Console.WriteLine($"{comp(val, lower)} >= 0 && {comp(val, upper)}");
                return comp(val, lower) >= 0 && comp(val, upper) <= 0;
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WORDCOUNT: ");
            Console.WriteLine($"Ala ma kota = {"Ala ma kota".WordCount()}");
            Console.WriteLine($"Ala ma kota, ala nie ma psa = {"Ala ma kota, ala nie ma psa".WordCount()}");


            Console.WriteLine("\nBEZSAMOGLOSEK: ");
            Console.WriteLine($"Ala ma kota = {"Ala ma kota".BezSamoglosek()}");
            Console.WriteLine($"ala nie ma psa = {"ala nie ma psa".BezSamoglosek()}");


            Console.WriteLine("\nISNUMERIC: ");
            Console.WriteLine($"asda = {"asda".IsNumeric()}");
            Console.WriteLine($"123 = {"123".IsNumeric()}");
            Console.WriteLine($"-512 = {"-512".IsNumeric()}");

            Console.WriteLine("\nMEDIANA: ");
            int[] tab = { 2, 3, 1 };
            Console.WriteLine(tab.Mediana());
            int[] tab2 = { 2, 3, 1, 4 };
            Console.WriteLine(tab2.Mediana());

            Console.WriteLine("\nDUMP1: ");
            var lista = new List<int> { 0, 1, 2, 3, 4 };
            Console.WriteLine(lista.Dump());

            Console.WriteLine("\nDUMP2: ");
            Console.WriteLine("TESTOWY".Dump());
            var set = new HashSet<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine(set.Dump());


            Console.WriteLine("\nBETWEEN: ");
            Console.WriteLine(5.Between(0, 9));
            Console.WriteLine(10.Between(0, 9));
            Console.WriteLine("ala".Between("ala", "baba"));
            Console.WriteLine("baba".Between("ala", "baba"));
            Console.WriteLine("Ala".Between("ala", "baba"));

        }
    }
}
