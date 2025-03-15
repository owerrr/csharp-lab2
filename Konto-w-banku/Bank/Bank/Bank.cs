using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Bank
{
    public class Bank
    {
        private static int BankId { get; set; }
        private List<string> defaultCommands = ["help", "account", "quit"];
        private List<string> accountCommands = ["create plus", "create default", "list", "back"];
        private List<string> accountDetailsCommands = ["deposit", "withdraw", "delete", "back"];
        private IDictionary<int, Konto> accountList = new Dictionary<int, Konto>();
        public int GetBankId() => BankId;
        public Bank()
        {
            BankId = BankId + 1;
            accountList.Add(0, new Konto("Andrzej", 100));
            accountList.Add(1, new KontoPlus("Robert", 500, 100));
            accountList.Add(2, new KontoPlus("Michal", 100, 200));
            WriteInterface();

        }

        ~Bank()
        {
            Console.WriteLine("Destroyed Bank");
            accountList.Clear();
        }

        private void WriteInterface(string GUI = "default", List<object> additionalInfo = null)
        {
            if(GUI == "default")
            {
                Console.WriteLine("==================================\n" +
                                "| WELCOME IN YOUR PERSONAL BANK! |\n" +
                                "|--------------------------------|\n" +
                                "| USAGE:                         |\n" +
                                "|   - Help                       |\n" +
                                "|   - Account                    |\n" +
                                "|   - Quit                       |\n" +
                                "==================================\n");
                WaitForCommand();
            }
            else if(GUI == "help")
            {
                Console.WriteLine("\nDefault commands:\n" +
                                    "\to 'Help' - You are already here...\n" +
                                    "\to 'Account' - Enter Bank account manager\n" +
                                    "\nAccount commands:\n" +
                                    "\to 'Create' 'type'(Default/Plus) - Create new account\n" +
                                    "\to 'List' - Show all accounts\n" +
                                    "\to 'View' 'id' - View account details\n" +
                                    "\nAccount details commands:\n" +
                                    "\to 'Deposit' 'money' - deposits money into account\n" +
                                    "\to 'Withdraw' 'money' - 'withdraws money from account\n" +
                                    "\to 'Delete' 'confirm account name' - deletes account\n" +
                                    "");
            }
            else if(GUI == "account")
            {
                Console.WriteLine("\nAccount commands:\n" +
                                    "\to 'Create' 'type'(Default/Plus) - Create new account\n" +
                                    "\to 'List' - Show all accounts\n" +
                                    "\to 'View' 'id' - View account details\n" +
                                    "\to 'Back' - Back to menu\n");
            }
            else if(GUI == "accdetails")
            {
                Console.WriteLine($"\nVIEWING ACCOUNT NO. {additionalInfo[0]}\n{additionalInfo[1]}\nAccount type:\t{additionalInfo[2]}\n" + 
                                    "\nAccount details commands:\n" +
                                    "\to 'Deposit' 'money' - deposits money into account\n" +
                                    "\to 'Withdraw' 'money' - 'withdraws money from account\n" +
                                    "\to 'Delete' 'confirm account name' - deletes account\n" +
                                    "\to 'Back' - Back to Account commands");
            }
        }

        private void WaitForCommand(string cmdType = "default", bool msgDup = false)
        {
            if(cmdType == "default")
            {
                string answer = "";
                while (true)
                {
                    Console.Write(" > ");
                    answer = Console.ReadLine().ToLower();

                    if (!defaultCommands.Contains(answer))
                        Console.WriteLine("Invalid Command!");
                    else break;
                }

                DefaultCommandHandler(answer);
            }
            else if(cmdType == "account")
            {
                string answer = "";
                while (true)
                {
                    Console.Write("\naccount > ");
                    answer = Console.ReadLine().ToLower();

                    if (!accountCommands.Contains(answer) && !answer.StartsWith("view "))
                        Console.WriteLine("Invalid Command!");
                    else break;
                }

                AccountCommandHandler(answer);
            }
            else if(cmdType.StartsWith("accdetails"))
            {

                int accId = Int32.Parse(cmdType.Split(' ')[1]);
                var Account = accountList.First(x => x.Key == accId).Value;
                string accType = Account is KontoPlus ? "Plus" : "Default";
                if (!msgDup)
                {
                    List<object> temp = [accId, Account, accType];
                    WriteInterface("accdetails", temp);
                }
                string answer = "";
                while (true)
                {
                    Console.Write("\ndetails > ");
                    answer = Console.ReadLine().ToLower();
                    //Console.WriteLine(answer);

                    if (!accountDetailsCommands.Contains(answer)
                        && !answer.StartsWith("deposit ")
                        && !answer.StartsWith("withdraw ")
                        && !answer.StartsWith("delete "))
                        Console.WriteLine("Invalid Command!");
                    else break;
                }

                AccountDetailsCommandHandler(answer, Account, cmdType);
            }
        }

        private void DefaultCommandHandler(string command)
        {
            if(command == "help")
            {
                WriteInterface("help");
                WaitForCommand();
            }
            else if(command == "account")
            {
                WriteInterface("account");
                WaitForCommand("account");
            }
            else if(command == "quit")
            {
                return;
            }
        }

        private void AccountCommandHandler(string command)
        {
            Console.WriteLine(command);
            if(command.StartsWith("create "))
            {
                //Console.WriteLine(command);
                var cmd = command.Split(' ');
                //Console.WriteLine(cmd[0] + " | " + cmd[1]);
                if (cmd[1] != "default" && cmd[1] != "plus")
                {
                    Console.WriteLine("Unknown account type!\nThere are only two types: Default and Plus!");
                    WaitForCommand("account");
                }
                Console.WriteLine("\n===ACCOUNT CREATOR=============\n" +
                                    "Account type: " + cmd[1]);
                Console.Write("Accout name: ");
                string name = Console.ReadLine();
                decimal startingBalance = 0M;

                try
                {
                    Console.Write("Account starting balance: ");
                    startingBalance = Convert.ToDecimal(Console.ReadLine());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand("account");
                }
               

                if (cmd[1] == "plus")
                {
                    Console.Write("Limit: ");
                    decimal limit = Convert.ToDecimal(Console.ReadLine());
                    accountList.Add(accountList.Count, new KontoPlus(name, startingBalance, limit));
                }
                else
                {
                    accountList.Add(accountList.Count, new Konto(name, startingBalance));
                }

                WaitForCommand("account");
            }
            else if(command == "list")
            {
                if(accountList.Count <= 0)
                {
                    Console.WriteLine("There are no accounts yet!");
                    WaitForCommand("account");
                }

                Console.WriteLine("\n===ACCOUNT LIST================");
                foreach (var key in accountList)
                {
                    string accType = key.Value is KontoPlus ? "Plus" : "Default";
                    Console.WriteLine($"\nId:\t\t{key.Key}\n{key.Value}\nAccount type:\t{accType}");
                }

                WaitForCommand("account");
            }
            else if(command.StartsWith("view "))
            {
                try
                {
                    var cmd = command.Split(' ');
                    int accId = Int32.Parse(cmd[1]);
                    if (accId < 0 || !accountList.Keys.Contains(accId)) {
                        Console.WriteLine($"account no. {accId} not found. Check 'List' command for account list!");
                        WaitForCommand("account");
                    }

                    WaitForCommand($"accdetails {cmd[1]}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.ToString());
                }
            }
            else if(command == "back")
            {
                WriteInterface();
            }
        }

        private void AccountDetailsCommandHandler(string command, object Account = null, string cmdType = "")
        {
            if(command == "back")
            {
                WaitForCommand("account");
            }
            else if(command.StartsWith("deposit ") && command.Length > 7)
            {
                try
                {
                    int money = Int32.Parse(command.Split(' ')[1]);
                    Console.WriteLine($"Depositing {money}...");
                    if (Account is Konto) (Account as Konto).Wplata(money);
                    else if (Account is KontoPlus) (Account as KontoPlus).Wplata(money);
                    else throw new Exception("Invalid account!");

                    WaitForCommand(cmdType, true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand("account");
                }
            }
            else if(command.StartsWith("withdraw ") && command.Length > 7)
            {
                try
                {
                    int money = Int32.Parse(command.Split(' ')[1]);
                    Console.WriteLine($"Withdrawing {money}...");
                    if (Account is Konto) (Account as Konto).Wyplata(money);
                    else if (Account is KontoPlus) (Account as KontoPlus).Wyplata(money);
                    else throw new Exception("Invalid account!");

                    WaitForCommand(cmdType,true );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand("account");
                }
            }
            else if(command.StartsWith("delete ") && command.Length > 7)
            {
                var cmd = command.Split(' ');
                if (cmd[1] == (Account as Konto).Klient.ToLower())
                {
                    var key = accountList.First(x => x.Value == Account);
                    accountList.Remove(key.Key);
                    Console.WriteLine("Succesfully deleted account!");
                    WaitForCommand("account");
                }
                Console.WriteLine("Invalid account name");
                WaitForCommand("account");
            }

            Console.WriteLine("Something went wrong...");
            WaitForCommand("account");
        }
    }


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

        public virtual string Statystyki()
        {
            var status = zablokowane ? "Zablokowane" : "Odblokowane";
            return $"Nazwa:\t\t{klient}\n" +
                   $"Bilans:\t\t{bilans}\n" +
                   $"Status konta:\t{status}";
        }

        protected void UpdateBalance(decimal money)
        {
            bilans += money;
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
                bilans -= kwota;
            }
            else
            {
                throw new ArgumentException("Nieprawidlowa kwota wyplaty!!");
            }
        }

        public void BlokujKonto() => zablokowane = true;
        public void OdblokujKonto() => zablokowane = false;

        public override string ToString()
        {
            var status = zablokowane ? "Zablokowane" : "Odblokowane";
            return $"Nazwa:\t\t{klient}\n" +
                   $"Bilans:\t\t{bilans}\n" +
                   $"Status konta:\t{status}";
        }
    }

    public class KontoPlus : Konto
    {
        private decimal limit;
        public decimal Limit {
            get => limit;
            set => limit = value > 0 ? value : 0;
        }
        public KontoPlus(string klient, decimal bilansNaStart = 0, decimal limit = 100) : base(klient, bilansNaStart)
        {
            Limit = limit;
        }

        public override void Wyplata(decimal kwota) {
            if (Zablokowane)
                throw new ArgumentException("Konto jest zablokowane!");

            if(kwota > (Bilans + limit))
                throw new ArgumentException("Nieprawidłowa kwota wypłaty!");

            base.UpdateBalance(-kwota);

            if (Bilans < 0)
                base.BlokujKonto();
        }

        public override void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentException("Nieprawidłowa kwota wpłaty!!");
            
            base.UpdateBalance(kwota);

            if (kwota > 0)
                base.OdblokujKonto();
        }

        public override string Statystyki()
        {
            var status = Zablokowane ? "Zablokowane" : "Odblokowane";
            return $"Nazwa:\t\t{Klient}\n" +
                   $"Bilans:\t\t{Bilans} ({Limit})\n" +
                   $"Status konta:\t{status}";
        }

        public override string ToString()
        {
            var status = Zablokowane ? "Zablokowane" : "Odblokowane";
            return $"Nazwa:\t\t{Klient}\n" +
                   $"Bilans:\t\t{Bilans} ({Limit})\n" +
                   $"Status konta:\t{status}";
        }
    }
}
