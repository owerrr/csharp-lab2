using Bank;

namespace KontoPlusTests
{
    [TestClass]
    public class TEST_KontoPlus_GetProperties
    {
        [TestMethod]
        public void KontoPlus_GetKlient()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1.Klient == klient, "Wystapil blad/y! Nazwa klienta się nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlus_GetBilans()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1.Bilans == bilans, "Wystapil blad/y! bilans klienta się nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlus_GetLimit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1.Limit == limit, "Wystapil blad/y! limit klienta się nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlus_GetAccountBlock()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1.Zablokowane == false, "Wystapil blad/y! stan konta klienta się nie zgadza!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
    }

    [TestClass]
    public class TEST_KontoPlus_Create
    {
        [TestMethod]
        public void KontoCreate()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1 != null, "Wystapil blad/y!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoCreate_NegativeLimit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = -100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1.Limit >= 0, "Wystapil blad/y! Ustawiono negatywny limit!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
    }

    [TestClass]
    public class TEST_KontoPlus_Methods
    {
        [TestMethod]
        public void KontoPlusWplata_ValidInput()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal kwota = 100M;
                k1.Wplata(kwota);
                Assert.IsTrue(k1.Bilans.Equals(bilans + kwota), "Wystąpił błąd! Bilans się nie zgadza");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlusWplata_ValidInput_BlockedAccount()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal kwota = 100M;
                k1.BlokujKonto();
                k1.Wplata(kwota);
                Assert.IsTrue(k1.Bilans.Equals(bilans + kwota), "Wystąpił błąd! Bilans się nie zgadza");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlusWplata_InvalidInput_NegativeDeposit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal kwota = -100M;
                k1.Wplata(kwota);
                
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Invalid deposit value!"), "Wystąpił błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlusWplata_UnlockAccountAfterPayment()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal debet = 200M;
                k1.Wyplata(debet);
                decimal kwota = 200M;
                k1.Wplata(kwota);

                Assert.IsTrue(k1.Bilans.Equals(100M), "Nieprawidłowy bilans konta!");
                Assert.IsTrue(!k1.Zablokowane, "Konto dalej jest zablokowane");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlusWyplata_LockAccountAfterWithdraw_Limit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal kwota = 200M;
                k1.Wyplata(kwota);

                Assert.IsTrue(k1.Bilans.Equals(bilans-kwota), "Nieprawidłowy bilans konta!");
                Assert.IsTrue(k1.Zablokowane, "Konto dalej jest odblokowane");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlusWyplata_WithdrawAfterLockedByLimit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal kwota = 200M;
                k1.Wyplata(kwota);
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Account is locked!"), "Wystąpił błąd/y!");
            }
        }
        [TestMethod]
        public void KontoPlusWyplata_WithdrawedMoreThanLimit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoPlus k1 = new(klient, bilans, limit);
                decimal kwota = 201M;
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Invalid withdraw value!"), "Wystąpił błąd/y!");
            }
        }
    }

    [TestClass]
    public class TEST_KontoPlus_Conversions
    {
        [TestMethod]
        public void KontoPlus_ConvertTo_Konto_Valid()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                var k1 = new KontoPlus("Molenda", 100M, 100M);
                var k2 = k1.ConvertToKonto();

                Assert.IsFalse(k2 is KontoPlus, "Rzutowanie nie wykonało się poprawnie.");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}