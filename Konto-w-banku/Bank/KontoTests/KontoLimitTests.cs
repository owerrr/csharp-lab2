using Bank;

namespace KontoLimitTests
{
    [TestClass]
    public class TEST_KontoLimit_Create
    {
        [TestMethod]
        public void KontoCreate()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
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
                KontoLimit k1 = new(klient, bilans, limit);
                Assert.IsTrue(k1.Limit >= 0, "Wystapil blad/y! Ustawiono negatywny limit!");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
    }

    [TestClass]
    public class TEST_KontoLimit_Methods
    {
        [TestMethod]
        public void KontoLimitWplata_ValidInput()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
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
        public void KontoLimitWplata_ValidInput_BlockedAccount()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
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
        public void KontoLimitWplata_InvalidInput_NegativeDeposit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
                decimal kwota = -100M;
                k1.Wplata(kwota);

            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Nieprawidłowa kwota wpłaty!"), "Wystąpił błąd/y!");
            }
        }
        [TestMethod]
        public void KontoLimitWplata_UnlockAccountAfterPayment()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
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
        public void KontoLimitWyplata_LockAccountAfterWithdraw_Limit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
                decimal kwota = 200M;
                k1.Wyplata(kwota);

                Assert.IsTrue(k1.Bilans.Equals(bilans - kwota), "Nieprawidłowy bilans konta!");
                Assert.IsTrue(k1.Zablokowane, "Konto dalej jest odblokowane");
            }
            catch (ArgumentException ex)
            {
                Assert.Fail("Wystąpił/y błąd/y!");
            }
        }
        [TestMethod]
        public void KontoLimitWyplata_WithdrawAfterLockedByLimit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
                decimal kwota = 200M;
                k1.Wyplata(kwota);
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Konto jest zablokowane!"), "Wystąpił błąd/y!");
            }
        }
        [TestMethod]
        public void KontoLimitWyplata_WithdrawedMoreThanLimit()
        {
            string klient = "Andrzej";
            decimal bilans = 100M;
            decimal limit = 100M;

            try
            {
                KontoLimit k1 = new(klient, bilans, limit);
                decimal kwota = 201M;
                k1.Wyplata(kwota);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Nieprawidłowa kwota wypłaty!"), "Wystąpił błąd/y!");
            }
        }
    }
}