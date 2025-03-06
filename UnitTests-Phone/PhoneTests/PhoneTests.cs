using ConsoleApp1;
using ConsoleApp1.Class;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhoneTests
{
    [TestClass]
    public class PhoneCreateTests
    {
        [TestMethod]
        public void Phone_CreateNew()
        {
            string owner = "test";
            string phone_num = "123456789";
            Phone phone = new Phone(owner, phone_num);

            Assert.AreEqual(owner, phone.Owner, "Something is odd... Owner's name is not valid!");
            //Assert.IsTrue(phone.Owner.Contains(owner), "AFASSFA");
            //Assert.ThrowsException<System.ArgumentException>(() => phone.Owner);
        }
        [TestMethod]
        public void Phone_CreateNew_OwnerEmpty()
        {
            string owner = "";
            string phone_num = "123456789";
            try
            {
                Phone phone_empty_owner = new Phone(owner, phone_num);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Owner name is empty or null!"), "Something went wrong...");
            }
        }
        [TestMethod]
        public void Phone_CreateNew_PhoneNumberEmpty()
        {
            string owner = "test";
            string phone_num = "";
            try
            {
                Phone phone_empty_owner = new Phone(owner, phone_num);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Phone number is empty or null!"), "Something went wrong...");
            }
        }
        [TestMethod]
        public void Phone_CreateNew_PhoneNumberIsNaN()
        {
            string owner = "test";
            string phone_num = "test";
            try
            {
                Phone phone_empty_owner = new Phone(owner, phone_num);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains($"Invalid phone number!"), "Something went wrong...");
            }
        }
        [TestMethod]
        public void Phone_CreateNew_PhoneNumberTooLong()
        {
            string owner = "test";
            string phone_num = "1234567891";
            try
            {
                Phone phone_empty_owner = new Phone(owner, phone_num);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ToString().Contains($"Invalid phone number!"), "Something went wrong...");
            }
        }
    }

    [TestClass]
    public class PhoneBookTests
    {
        [TestMethod]
        public void PhoneBook_CheckCount_DifferentNames()
        {
            Phone phone1 = new Phone("Tester1", "123456789");
            Phone phone2 = new Phone("Tester2", "123456789");
            Phone phone3 = new Phone("Tester3", "123456789");

            phone1.AddContact(phone2.Owner, phone2.PhoneNumber);
            phone1.AddContact(phone3.Owner, phone3.PhoneNumber);

            int Expected_PhoneBookCount = 2;

            Assert.AreEqual(Expected_PhoneBookCount, phone1.Count, "Something went wrong... PhoneBook count is not valid!");
        }
        [TestMethod]
        public void PhoneBook_CheckCount_EqualNames()
        {
            Phone phone1 = new Phone("Tester1", "123456789");
            Phone phone2 = new Phone("Tester2", "123456789");
            Phone phone3 = new Phone("Tester2", "123456789");

            phone1.AddContact(phone2.Owner, phone2.PhoneNumber);
            phone1.AddContact(phone3.Owner, phone3.PhoneNumber);

            int Expected_PhoneBookCount = 1;

            Assert.AreEqual(Expected_PhoneBookCount, phone1.Count, "Something went wrong... PhoneBook count is not valid!");
        }
        [TestMethod]
        public void PhoneBook_CheckPhoneBookLimit()
        {
            Phone phone1 = new Phone("Tester1", "123456789");
            string phone_number = "123456789";

            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    phone1.AddContact(i.ToString(), phone_number);
                }
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Phonebook is full!"), "Something went wrong...");
            }
        }
    }

    [TestClass]
    public class PhoneCallTests
    {
        [TestMethod]
        public void PhoneCall()
        {
            Phone phone1 = new Phone("test", "123456789");

            string name = "test2";
            string phone_number = "987654321";
            Phone phone2 = new Phone(name, phone_number);

            phone1.AddContact(phone2.Owner, phone2.PhoneNumber);
            string result = phone1.Call(name);

            Assert.AreEqual(result, $"Calling {phone_number} ({name}) ...", "Something went wrong...");
        }
        [TestMethod]
        public void PhoneCall_InvalidPerson()
        {
            Phone phone1 = new Phone("test", "123456789");

            string name = "test2";
            string phone_number = "987654321";
            try
            {
                string result = phone1.Call(name);
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.ToString().Contains("Person doesn't exists!"), "Something went wrong...");
            }
        }
    }
}