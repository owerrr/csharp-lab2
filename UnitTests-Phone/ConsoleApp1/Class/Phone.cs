using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Class
{
    public class Phone
    {
        private string owner;
        // string of 9 digits
        private string phoneNumber;
        // Dictionary of <name, number>
        private readonly Dictionary<string, string> phoneBook;
        public int PhoneBookCapacity => 100;

        public Phone(string owner, string phoneNumber)
        {
            Owner = owner;
            PhoneNumber = phoneNumber;
            phoneBook = new Dictionary<string, string>();
        }

        public string Owner
        {
            get => owner;
            private set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"Owner name is empty or null!");

                owner = value;
            }
        }

        private bool IsCorrectPhoneNumber(string number)
            => (number != null) && number.Length == 9 && number.All(c => char.IsDigit(c));

        public string PhoneNumber
        {
            get => phoneNumber;
            private set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"Phone number is empty or null!");

                if (!IsCorrectPhoneNumber(value))
                    throw new ArgumentException($"Invalid phone number!");

                phoneNumber = value;
            }
        }

        public int Count => phoneBook.Count;

        public bool AddContact(string name, string number)
        {
            if (Count == PhoneBookCapacity)
                throw new InvalidOperationException("Phonebook is full!");

            if (!phoneBook.ContainsKey(name))
            {
                phoneBook.Add(name, number);
                return true;
            }

            return false;
        }

        public string Call(string name)
        {
            if (phoneBook.ContainsKey(name))
            {
                var number = phoneBook[name];
                return $"Calling {number} ({name}) ...";
            }

            throw new InvalidOperationException("Person doesn't exists!");
        }
    }
}
