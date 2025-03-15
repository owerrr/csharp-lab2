using Bank;

namespace KontoTests
{
    [TestClass]
    public class TEST_Konto_Create
    {
        [TestMethod]
        public void KontoCreate()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                Assert.IsTrue(k1 != null);
            }
            catch (ArgumentException ex)
            {

            }
        }

        [TestMethod]
        public void KontoCreate_NameIsNull()
        {
            string klient = "";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                Assert.IsTrue(k1 == null, "Wystapil blad! Utworzono klienta bez nazwy!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Nazwa klienta nie moze byc pusta!"), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoCreate_NegativeStartingMoney()
        {
            string klient = "Andrzej";
            decimal bilans = -100M;

            try
            {
                Konto k1 = new(klient, bilans);
                Assert.IsTrue(k1.Bilans >= 0, "Wystapil blad! Utworzono startowy bilans na minusie!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }
    }

    [TestClass]
    public class TEST_Konto_Functions
    {
        [TestMethod]
        public void KontoStatistics_UnlockedAccount()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                k1.OdblokujKonto();

                string res = k1.Statystyki();
                string pred = $"Nazwa:\t\t{klient}\nBilans:\t\t{bilans}\nStatus konta:\tOdblokowane";

                Assert.IsTrue(res.Equals(pred), "Wystapil blad! Sprawdz funkcje Statystyki");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoStatistics_LockedAccount()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                k1.BlokujKonto();

                string res = k1.Statystyki();
                string pred = $"Nazwa:\t\t{klient}\nBilans:\t\t{bilans}\nStatus konta:\tZablokowane";

                Assert.IsTrue(res.Equals(pred), "Wystapil blad! Sprawdz funkcje Statystyki");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }
    }

    [TestClass]
    public class TEST_Konto_Methods
    {
        [TestMethod]
        public void Konto_UnlockAccount()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                k1.OdblokujKonto();
                Assert.IsTrue(!k1.Zablokowane, "Wystapil blad! Konto dalej jest zablokowane!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void Konto_LockAccount()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                k1.BlokujKonto();
                Assert.IsTrue(k1.Zablokowane, "Wystapil blad! Konto dalej jest odblokowane!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoWplata_UnlockedAccount_ValidInput()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                decimal kwota = 100M;
                k1.Wplata(kwota);
                Assert.IsTrue(k1.Bilans == (bilans + kwota), "Wystapil blad! Bilans konta sie nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoWplata_LockedAccount_ValidInput()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                decimal kwota = 100M;
                k1.Wplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Konto jest zablokowane!"), "Wystapil blad! Sprawdz dokladnie metode Wplata()");
            }
        }

        [TestMethod]
        public void KontoWyplata_UnlockedAccount_ValidInput()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                decimal kwota = 100M;
                k1.Wyplata(kwota);
                Assert.IsTrue(k1.Bilans == (bilans - kwota), "Wystapil blad! Bilans konta sie nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains(""), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoWyplata_LockedAccount_ValidInput()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                decimal kwota = 100M;
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Konto jest zablokowane!"), "Wystapil blad! Sprawdz dokladnie metode Wyplata()");
            }
        }

        /* */

        [TestMethod]
        public void KontoWplata_UnlockedAccount_InvalidInput_NegativeMoney()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                decimal kwota = -100M;
                k1.Wplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Nieprawidlowa kwota wplaty!!"), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoWplata_LockedAccount_InvalidInput_NegativeMoney()
        {
            string klient = "Andrzej";
            decimal bilans = -100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                decimal kwota = 100M;
                k1.Wplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Konto jest zablokowane!"), "Wystapil blad! Sprawdz dokladnie metode Wplata()");
            }
        }

        [TestMethod]
        public void KontoWyplata_UnlockedAccount_InvalidInput_NegativeMoney()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                decimal kwota = -100M;
                k1.Wplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Nieprawidlowa kwota wplaty!!"), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoWyplata_LockedAccount_InvalidInput_NegativeMoney()
        {
            string klient = "Andrzej";
            decimal bilans = -100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                decimal kwota = 100M;
                k1.Wplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Konto jest zablokowane!"), "Wystapil blad! Sprawdz dokladnie metode Wplata()");
            }
        }

        [TestMethod]
        public void KontoWyplata_UnlockedAccount_InvalidInput_TooMuchMoney()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.OdblokujKonto();
                decimal kwota = 200M;
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Nieprawidlowa kwota wyplaty!!"), "Wystapil blad!");
            }
        }

        [TestMethod]
        public void KontoWyplata_LockedAccount_InvalidInput_TooMuchMoney()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1.BlokujKonto();
                decimal kwota = 200M;
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Konto jest zablokowane!"), "Wystapil blad! Sprawdz dokladnie metode Wyplata()");
            }
        }
    }
}