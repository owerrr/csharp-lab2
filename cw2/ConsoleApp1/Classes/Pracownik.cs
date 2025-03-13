using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Classes
{
    class Pracownik : IEquatable<Pracownik>, IComparable<Pracownik>
    {
        private string nazwisko;
        public string Nazwisko
        {
            get => nazwisko;
            set { nazwisko = value.Trim(); }
        }
        private DateTime dataZatrudnienia;
        public DateTime DataZatrudnienia
        {
            get => dataZatrudnienia;
            set
            {
                if (value <= DateTime.Now)
                {
                    dataZatrudnienia = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
        private decimal wynagrodzenie;
        public decimal Wynagrodzenie
        {
            get => wynagrodzenie;
            set
            {
                if (value < 0)
                {
                    wynagrodzenie = 0;
                }
                else
                    wynagrodzenie = value;
            }
        }

        public Pracownik()
        {
            Nazwisko = "Anonim";
            DataZatrudnienia = DateTime.Now;
            Wynagrodzenie = 0;
        }

        public Pracownik(string nazwisko, DateTime dataZatrudnienia, decimal wynagrodzenie)
        {
            Nazwisko = nazwisko;
            DataZatrudnienia = dataZatrudnienia;
            Wynagrodzenie = wynagrodzenie;
        }

        public int CzasZatrudnienia()
        {
            int timeSpan = (DateTime.Now - DataZatrudnienia).Days;
            return timeSpan / 30;
        }

        public override string ToString()
        {
            return $"{Nazwisko}, {DataZatrudnienia} ({this.CzasZatrudnienia()}), {Wynagrodzenie}";
        }

        public bool Equals(Pracownik p)
        {
            return (
                 Nazwisko == p.Nazwisko
              && DataZatrudnienia == p.DataZatrudnienia
              && Wynagrodzenie == p.Wynagrodzenie
                );
        }

        public override bool Equals(object? obj)
        {
            Pracownik p2 = obj as Pracownik;
            if (p2 == null) return false;

            return Equals(p2);
        }

        public static bool Equals(Pracownik p1, Pracownik p2)
        {
            return (
                 p1.Nazwisko == p2.Nazwisko
              && p1.DataZatrudnienia == p2.DataZatrudnienia
              && p1.Wynagrodzenie == p2.Wynagrodzenie
                );
        }

        public int CompareTo(Pracownik other)
        {
            if (other == null) return 1;
            else
            {
                if (!Equals(Nazwisko, other.Nazwisko))
                    return Nazwisko.CompareTo(other.Nazwisko);
                if (!Equals(DataZatrudnienia, other.DataZatrudnienia))
                    return DataZatrudnienia.CompareTo(other.DataZatrudnienia);
                if (!Equals(Wynagrodzenie, other.Wynagrodzenie))
                    return Wynagrodzenie.CompareTo(other.Wynagrodzenie);

                return 1;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nazwisko, dataZatrudnienia, wynagrodzenie);
        }

        public static bool operator !=(Pracownik p1, Pracownik p2)
        {
            return !Equals(p1, p2);
        }
        public static bool operator ==(Pracownik p1, Pracownik p2)
        {
            return Equals(p1, p2);
        }
    }
}
