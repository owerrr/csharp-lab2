namespace Wprowadzenie_LINQ
{
    public class Program
    {
        private const string s = "Krzysztof Molenda,  Jan Kowalski, Andrzej Kowalski, Anna Abacka , Józef Kabacki, Kazimierz Moksa";

        static void Main(string[] args)
        {

            //WithoutLINQ();
            //WithLINQ();
            Console.WriteLine("\nZadanie 1:\n");
            Zadanie1();
            Console.WriteLine("\nZadanie 2:\n");
            Zadanie2();
        }

        private static void WithoutLINQ()
        {
            string[] persons = s.Split(',', StringSplitOptions.RemoveEmptyEntries);

            string[] persons_trimmed = new string[persons.Length];

            Console.WriteLine("PERSONS TRIMMED\n========================\n");

            for (int i = 0; i < persons.Length; i++)
            {
                persons_trimmed[i] = persons[i].Trim();
            }

            foreach (string p in persons_trimmed)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("\nPERSONS {LastName FirstName}\n========================\n");

            string[] persons_trimmed_firstlastName = new string[persons_trimmed.Length];

            for (int i = 0; i < persons_trimmed.Length; i++)
            {
                string[] temp = persons_trimmed[i].Split(' ');
                string firstName = temp[0].Trim();
                string lastName = temp[1].Trim();
                persons_trimmed_firstlastName[i] = $"{lastName} {firstName}";
            }

            foreach (string p in persons_trimmed_firstlastName)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("\nPERSONS {LastName FirstName}\nSTRING JOIN\n========================\n");

            string s1 = string.Join(", ", persons_trimmed_firstlastName);
            Console.WriteLine(s1);
        }

        private static void WithLINQ()
        {
            Console.WriteLine("\nUSING LINQ\n========================\n");

            var q1 = s.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => (nazwisko: x[1], imie: x[0]))
                .OrderBy(x => x.nazwisko).ThenBy(x => x.imie)
                .Select((x, i) => $"{i+1}. {x.imie} {x.nazwisko}")
                .ToList();

            foreach (var p in q1)
            {
                Console.WriteLine(p);
            }
        }

        private static void Zadanie1()
        {
            string s_input = "Krzysztof Molenda, 1965-11-20; Jan Kowalski, 1987-01-01; Anna Abacka, 1972-05-20; Józef Kabacki, 2000-01-02; Kazimierz Moksa, 2001-01-02";

            var q1 = s_input.Split(';', StringSplitOptions.RemoveEmptyEntries)
                     .Select(x => x.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries))
                     .Select(x => (fullName: x[0].Split(' '), dateOfBirth: x[1]))
                     .OrderBy(x => DateTime.Parse(x.dateOfBirth))
                     .ThenBy(x => x.fullName[0])
                     .Select(x => $"{x.fullName[1]} {x.fullName[0]} {x.dateOfBirth}")
                     .ToList();

            foreach(var p in q1)
            {
                Console.WriteLine(p);
            }
            //var s_output = String.Join(", ", q1);
            //Console.WriteLine(s_output);

            var q2 = (
                            from p in s_input.Split(';', StringSplitOptions.RemoveEmptyEntries)
                            let nameAndDate = p.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries)
                            let fullName = nameAndDate[0].Split(' ')
                            let dateOfBirth = DateTime.Parse(nameAndDate[1])
                            orderby dateOfBirth, fullName[0]
                            select $"{fullName[1]} {fullName[0]} {dateOfBirth:yyyy-MM-dd}"
                        ).ToList();

            Console.WriteLine("\nw notacji query");
            foreach (var p in q2)
            {
                Console.WriteLine(p);
            }
        }

        private static void Zadanie2()
        {
            string s_input = "Krzysztof Molenda, Jan Kowalski,  Anna    Abacka, Józef Kabacki, Kazimierz Moksa,  Alfred Alacki, Test Test";

            var q1 = s_input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
                            .Select(x => (firstNameLetter: x[0][0], lastNameLetter: x[1][0]))
                            .GroupBy(x => $"{x.firstNameLetter}{x.lastNameLetter}")
                            .Where(x => x.Count() > 1)
                            .Select(x => $"{x.Key} x{x.Count()}")
                            .ToList();

            foreach (var p in q1)
            {
                Console.WriteLine(p);
            }

            var q2 = (
                            from p in s_input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            let name = p.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            let letters = $"{name[0][0]}{name[1][0]}"
                            group letters by letters into x
                            where x.Count() > 1
                            select $"{x.Key} x{x.Count()}"
                        ).ToList();

            Console.WriteLine("\nw notacji query");
            foreach (var p in q2)
            {
                Console.WriteLine(p);
            }
        }
    }
}
