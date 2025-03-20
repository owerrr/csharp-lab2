using Bank;

namespace KontoTests
{
    [TestClass]
    public class TEST_Konto_GetProperties
    {
        [TestMethod]
        public void Konto_GetKlient()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                Assert.IsTrue(k1.Klient == klient, "Wystapil blad/y! Nazwa klienta siê nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wyst¹pi³/y b³¹d/y!");
            }
        }
        [TestMethod]
        public void Konto_GetBilans()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                Assert.IsTrue(k1.Bilans == bilans, "Wystapil blad/y! bilans klienta siê nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wyst¹pi³/y b³¹d/y!");
            }
        }
        [TestMethod]
        public void Konto_GetBlock()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                Assert.IsTrue(k1.Zablokowane == false, "Wystapil blad/y! stan konta klienta siê nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wyst¹pi³/y b³¹d/y!");
            }
        }
    }

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
                Assert.IsTrue(ex.ToString().Contains("Account name can't be empty!"), "Wystapil blad!");
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
                string pred = $"Name:\t\t{klient}\nBalance:\t{bilans}\nAccount status:\tUnlocked";

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
                string pred = $"Name:\t\t{klient}\nBalance:\t{bilans}\nAccount status:\tLocked";

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
                Assert.IsTrue(ex.ToString().Contains("Account is locked!"), "Wystapil blad! Sprawdz dokladnie metode Wplata()");
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
                Assert.IsTrue(ex.ToString().Contains("Account is locked!"), "Wystapil blad! Sprawdz dokladnie metode Wyplata()");
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
                Assert.IsTrue(ex.ToString().Contains("Invalid deposit value!!"), "Wystapil blad!");
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
                Assert.IsTrue(ex.ToString().Contains("Account is locked!"), "Wystapil blad! Sprawdz dokladnie metode Wplata()");
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
                decimal kwota = 100M;
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Invalid withdraw value!!"), "Wystapil blad!");
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
                Assert.IsTrue(ex.ToString().Contains("Account is locked!"), "Wystapil blad! Sprawdz dokladnie metode Wplata()");
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
                Assert.IsTrue(ex.ToString().Contains("Invalid withdraw value!!"), "Wystapil blad!");
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
                Assert.IsTrue(ex.ToString().Contains("Account is locked!"), "Wystapil blad! Sprawdz dokladnie metode Wyplata()");
            }
        }
    }

    [TestClass]
    public class TEST_Konto_Conversions
    {
        [TestMethod]
        public void Konto_ConvertTo_KontoPlus_Valid()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;

            try
            {
                Konto k1 = new(klient, bilans);
                k1 = k1.ConvertToPlus();
                Assert.IsTrue(k1 is KontoPlus, "Rzutowanie nie wykona³o siê poprawnie.");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}