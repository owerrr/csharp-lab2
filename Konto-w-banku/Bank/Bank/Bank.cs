namespace Bank
{
    public class Konto
    {
        private string klient;
        public string Klient {
            get => klient;
            private set => klient = value.Trim() != "" ? value.Trim() : throw new ArgumentException("Nazwa klienta nie moze byc pusta!"); }
        private decimal bilans;
        public decimal Bilans {
            get => bilans; 
            private set => bilans = value > 0 ? value : 0; }
        private bool zablokowane;
        public bool Zablokowane { get => zablokowane; private set => zablokowane = false; }

        //private Konto() { }
        public Konto(string klient, decimal bilansNaStart = 0)
        {
            Klient = klient;
            Bilans = bilansNaStart;
        }

        public string Statystyki()
        {
            var status = zablokowane ? "Zablokowane" : "Odblokowane";
            return $"Nazwa:\t\t{klient}\n" +
                   $"Bilans:\t\t{bilans}\n" +
                   $"Status konta:\t{status}";
        }

        public virtual void Wplata(decimal kwota)
        {
            if (zablokowane) throw new ArgumentException("Konto jest zablokowane!");

            if (kwota > 0)
            {
                bilans += kwota;
            }
            else
            {
                throw new ArgumentException("Nieprawidlowa kwota wplaty!!");
            }
        }
        public virtual void Wyplata(decimal kwota)
        {
            if (zablokowane) throw new ArgumentException("Konto jest zablokowane!");

            if (kwota > 0 && kwota <= bilans)
            {
                bilans += kwota;
            }
            else
            {
                throw new ArgumentException("Nieprawidlowa kwota wyplaty!!");
            }
        }

        public void BlokujKonto() => zablokowane = true;
        public void OdblokujKonto() => zablokowane = false;
    }

    
}
