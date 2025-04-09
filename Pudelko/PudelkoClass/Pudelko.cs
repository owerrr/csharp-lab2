using System.Collections;
using System.Globalization;

namespace PudelkoLibrary
{
    public enum UnitOfMeasure
    {
        milimeter = 1000,
        centimeter = 100,
        meter = 1
    }
    public sealed class Pudelko : IEquatable<Pudelko>, IEnumerable<double>
    {
        private readonly double x;
        public double A => x;
        private readonly double y;
        public double B => y;
        private readonly double z;
        public double C => z;

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            int converter = (int)unit;

            if (a is null)
                x = 0.1d;
            else
                x = Math.Floor((double)a / converter * 1000) / 1000;

            if (b is null)
                y = 0.1d;
            else
                y = Math.Floor((double)b / converter * 1000) / 1000;

            if (c is null)
                z = 0.1d;
            else
                z = Math.Floor((double)c / converter * 1000) / 1000;

            if (x > 10 || x <= 0
              || y > 10 || y <= 0
              || z > 10 || z <= 0)
                throw new ArgumentOutOfRangeException();
        }

        public double Objetosc
        {
            get => Math.Round(A * B * C, 9);
        }
        public double Pole
        {
            get => Math.Round(2 * A * B + 2 * B * C + 2 * A * C , 6);
        }

        public override string ToString()
        {
            return $"{A.ToString("F3", CultureInfo.InvariantCulture)} m × {B.ToString("F3", CultureInfo.InvariantCulture)} m × {C.ToString("F3", CultureInfo.InvariantCulture)} m";
        }

        public string ToString(string format)
        {
            if (format is null) format = "m";
            if (format != "m" && format != "cm" && format != "mm") throw new FormatException("invalid format!");

            int converter = (int)UnitOfMeasure.meter;
            double _a = Math.Floor(A * converter * 1000) / 1000;
            double _b = Math.Floor(B * converter * 1000) / 1000;
            double _c = Math.Floor(C * converter * 1000) / 1000;

            if (format == "cm")
            {
                converter = (int)UnitOfMeasure.centimeter;
                _a = Math.Floor(A * converter * 10) / 10;
                _b = Math.Floor(B * converter * 10) / 10;
                _c = Math.Floor(C * converter * 10) / 10;

                return $"{_a.ToString("F1", CultureInfo.InvariantCulture)} {format} × {_b.ToString("F1", CultureInfo.InvariantCulture)} {format} × {_c.ToString("F1", CultureInfo.InvariantCulture)} {format}";
            }
            else if (format == "mm")
            {
                converter = (int)UnitOfMeasure.milimeter;
                _a = Math.Floor(A * converter);
                _b = Math.Floor(B * converter);
                _c = Math.Floor(C * converter);
                return $"{_a.ToString(CultureInfo.InvariantCulture)} {format} × {_b.ToString(CultureInfo.InvariantCulture)} {format} × {_c.ToString(CultureInfo.InvariantCulture)} {format}";
            }

            double[] values = { _a, _b, _c };

            return $"{values[0].ToString("F3", CultureInfo.InvariantCulture)} {format} × {values[1].ToString("F3", CultureInfo.InvariantCulture)} {format} × {values[2].ToString("F3", CultureInfo.InvariantCulture)} {format}";
        }

        public static bool Equals(Pudelko p1, Pudelko p2)
        {
            if (p1 is null || p2 is null) return false;

            return p1.Pole == p2.Pole;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B, C);
        }

        public bool Equals(Pudelko p)
        {
            if (p is null) return false;

            return Pole == p.Pole;
        }

        public override bool Equals(object? other)
        {
            Pudelko p = other as Pudelko;
            if (p is null) return false;
            return Equals(p);
        }

        public static bool operator ==(Pudelko p1, Pudelko p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator !=(Pudelko p1, Pudelko p2)
        {
            return !Equals(p1, p2);
        }

        public double this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0:
                        return A;
                    case 1:
                        return B;
                    case 2:
                        return C;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        public IEnumerator<double> GetEnumerator()
        {
            for(int i = 0; i < 3; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Pudelko Parse(string input)
        {
            string[] values = input.Replace(" ", String.Empty).Split('×');
            double[] pudelko_abc = new double[3];
            for(int i = 0; i < 3; i++)
            {
                string s_tmp = "";
                string unit_tmp = "";
                foreach(char c in values[i])
                {
                    if (Char.IsDigit(c) || c == '.' || c == ',')
                        s_tmp += c;
                    else
                        unit_tmp += c;
                }
                pudelko_abc[i] = double.Parse(s_tmp, CultureInfo.InvariantCulture);
                if (unit_tmp == "cm")
                    pudelko_abc[i] /= (int)UnitOfMeasure.centimeter;
                else if (unit_tmp == "mm")
                    pudelko_abc[i] /= (int)UnitOfMeasure.milimeter;
            }
            return new Pudelko(pudelko_abc[0], pudelko_abc[1], pudelko_abc[2], UnitOfMeasure.meter);
        }

        public static Pudelko operator+(Pudelko p1, Pudelko p2)
        {
            return new Pudelko(p1.A + p2.A, p1.B + p2.B, p1.C + p2.C, UnitOfMeasure.meter);
        }

        public static explicit operator double[](Pudelko p)
        {
            return [ p.A, p.B, p.C ];
        }

        public static implicit operator Pudelko(ValueTuple<int,int,int> val)
        {
            return new Pudelko(val.Item1 / (double)UnitOfMeasure.milimeter, val.Item2 / (double)UnitOfMeasure.milimeter, val.Item3 / (double)UnitOfMeasure.milimeter);
        }
    }
}
