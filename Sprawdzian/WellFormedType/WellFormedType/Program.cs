using PolylineLib;

namespace WellFormedType
{

    public static class PolylineExtensions
    {
        public static bool IsClosed(this Polyline polyline)
        {
            if (polyline == null) throw new ArgumentNullException(nameof(polyline));
            return polyline[0] == polyline[polyline.Count - 1];
        }
        public static Polyline AddFirst(this Polyline polyline, P point)
        {
            if (polyline == null) throw new ArgumentNullException(nameof(polyline));
            if (point == polyline[0])
                return polyline;
            var newPoints = new P[polyline.Count + 1];
            newPoints[0] = point;
            for (int i = 0; i < polyline.Count; i++)
                newPoints[i + 1] = polyline[i];

            return new Polyline(newPoints);
        }
        public static Polyline AddLast(this Polyline polyline, P point)
        {
            if (polyline == null) throw new ArgumentNullException(nameof(polyline));
            if (point == polyline[polyline.Count - 1])
                return polyline;

            var newPoints = new P[polyline.Count + 1];
            for (int i = 0; i < polyline.Count; i++)
                newPoints[i] = polyline[i];
            newPoints[^1] = point;

            return new Polyline(newPoints);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var defaultPolyline = new Polyline();
            Console.WriteLine(defaultPolyline); // (0,0)--(1,1)
            Console.WriteLine($"Count: {defaultPolyline.Count}, Length: {defaultPolyline.Length}");

            var polyline2 = new Polyline(new P(0, 0), new P(1, 2), new P(3, 1));
            Console.WriteLine(polyline2);

            var combined = polyline2 + new Polyline(new P(3, 1), new P(4, 4));
            Console.WriteLine(combined);

            Console.WriteLine(combined == polyline2); // false
            Console.WriteLine(combined != polyline2); // true

            P[] pointsArray = (P[])combined;
            foreach (var p in pointsArray)
                Console.WriteLine(p);

            Console.WriteLine(combined[2]+"\n\nFOREACH + PARSE");

            string input = "(1,1)--(2,3)--(4,5)";
            Polyline pl = Polyline.Parse(input);

            foreach (var p in pl)
            {
                Console.WriteLine(p);
            }

            var pline = Polyline.Parse("(0,0)--(1,4)--(1,0)");
            Console.WriteLine($"IsClosed? {pline.IsClosed()}");

            var extended1 = pline.AddFirst(new P(0, 0)); 
            Console.WriteLine(ReferenceEquals(pline, extended1));

            var extended3 = pline.AddLast(new P(0, 0));
            Console.WriteLine(ReferenceEquals(pline, extended3));

            var extended4 = pline.AddLast(new P(2, 2));
            Console.WriteLine(extended4);


            var list = new List<Polyline>
            {
                Polyline.Parse("(0,0)--(1,1)"),
                Polyline.Parse("(0,0)--(1,4)--(1,0)"),
                Polyline.Parse("(0,0)--(2,4)--(1,0)"),
                Polyline.Parse("(0,0)--(1,1)")
            };

            Console.WriteLine("Przed sortowaniem:");
            foreach (var pls in list)
            {
                Console.WriteLine($"{pls} (Count={pls.Count}, Length={pls.Length:F2})");
            }
            Console.WriteLine();

            Comparison<Polyline> comparison = (pline1, pline2) =>
            {
                if (pline1.Count < pline2.Count)
                    return -1;
                if (pline1.Count > pline2.Count)
                    return 1;
                int lengthCompare = pline1.Length.CompareTo(pline2.Length);
                if (lengthCompare != 0)
                    return lengthCompare;
                return 0;
            };

            list.Sort(comparison);

            Console.WriteLine("Po sortowaniu:");
            foreach (var pls in list)
            {
                Console.WriteLine($"{pls} (Count={pls.Count}, Length={pls.Length:F2})");
            }


        }
    }
}
