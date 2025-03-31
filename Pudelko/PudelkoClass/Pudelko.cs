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
        //private UnitOfMeasure _unitOfMeasure{get;set;}
        //public UnitOfMeasure unitOfMeasure { get => _unitOfMeasure; set => _unitOfMeasure = value; }
        private double x { get; set; }
        public double A { 
            get => Math.Round(x, 3);
            set{
                if (value > 10 || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid data!");
                x = value;
            }
        }
        private double y { get; set; }
        public double B
        {
            get => y;
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid data!");
                y = value;
            }
        }
        private double z { get; set; }
        public double C
        {
            get => z;
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid data!");
                z = value;
            }
        }

        public double Objetosc
        {
            get => Math.Round(A * B * C, 9);
        }
        public double Pole
        {
            get => Math.Round(2 * A * B + 2 * B * C + 2 * A * C , 6);
        }

        public Pudelko(double? a, double? b, double? c, UnitOfMeasure unitOfMeasure = UnitOfMeasure.meter) 
        {
            int converter = (int)unitOfMeasure;
            //this.unitOfMeasure = unitOfMeasure;

            if (a is null)
                A = 0.1f;
            else
                A = (double)a / converter;

            if (b is null)
                B = 0.1f;
            else
                B = (double)b / converter;

            if (c is null)
                C = 0.1f;
            else
                C = (double)c / converter;

        }

        public override string ToString()
        {
            return $"{A} m × {B} m × {C} m";
        }
        
        public string ToString(string format)
        {
            if (format != "m" && format != "cm" && format != "mm") throw new ArgumentException("invalid format!");

            int converter = (int)UnitOfMeasure.meter;
            double _a = A;
            double _b = B;
            double _c = C;

            if (format == "cm")
            {
                converter = (int)UnitOfMeasure.centimeter;
                _a = Math.Round(_a * converter, 1);
                _b = Math.Round(_b * converter, 1);
                _c = Math.Round(_c * converter, 1);
            }
            else if (format == "mm")
            {
                converter = (int)UnitOfMeasure.milimeter;
                _a = Math.Floor(_a * converter);
                _b = Math.Floor(_b * converter);
                _c = Math.Floor(_c * converter);
            }

            double[] values = { _a, _b, _c };



            return $"{values[0]} {format} × {values[1]} {format} × {values[2]} {format}";
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
            return Equals(p1, p2);
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
            //double[] data = new double[3];

            //data[0] = p1.A > p2.A ? p1.A : p2.A;
            //data[1] = p1.B > p2.B ? p1.B : p2.B;
            //data[2] = p1.C > p2.C ? p1.C : p2.C;

            //return new Pudelko(data[0], data[1], data[2], UnitOfMeasure.meter);

            return new Pudelko(p1.A + p2.A, p1.B + p2.B, p1.C + p2.C, UnitOfMeasure.meter);
        }

        public static explicit operator double[](Pudelko p)
        {
            return [ p.A, p.B, p.C ];
        }

        public static implicit operator Pudelko(ValueTuple<int,int,int> val)
        {
            return new Pudelko(val.Item1 / (int)UnitOfMeasure.milimeter, val.Item2 / (int)UnitOfMeasure.milimeter, val.Item3 / (int)UnitOfMeasure.milimeter);
        }
    }
}
