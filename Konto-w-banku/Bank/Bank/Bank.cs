namespace Bank
{
    #region UI
    public class Bank
    {
        private static int BankId { get; set; }
        private List<string> defaultCommands = ["help", "account", "quit"];
        private List<string> accountCommands = ["create plus", "create default", "list", "back", "help"];
        private List<string> accountDetailsCommands = ["deposit", "withdraw", "changeplan", "delete", "back", "limit", "help"];
        private IDictionary<int, Konto> accountList = new Dictionary<int, Konto>();
        public int GetBankId() => BankId;
        public Bank()
        {
            BankId = BankId + 1;
            accountList.Add(0, new Konto("Andrzej", 100));
            accountList.Add(1, new KontoPlus("Robert", 500, 100));
            accountList.Add(2, new KontoPlus("Michal", 100, 200));

            WriteInterface();
            WaitForCommand();
        }

        ~Bank()
        {
            Console.WriteLine("Destroyed Bank");
            accountList.Clear();
        }

        private void WriteInterface(string GUI = "default", List<object> additionalInfo = null)
        {
            string TEXT = "Something went wrong writing GUI...\n";
            if(GUI == "default")
            {
                TEXT =  "==================================\n" +
                        "| WELCOME IN YOUR PERSONAL BANK! |\n" +
                        "|--------------------------------|\n" +
                        "| USAGE:                         |\n" +
                        "|   - Help                       |\n" +
                        "|   - Account                    |\n" +
                        "|   - Quit                       |\n" +
                        "==================================\n";
                
            }
            else if(GUI == "help")
            {
                TEXT =  "\nDefault commands:\n" +
                        "\to 'Help' - You are already here...\n" +
                        "\to 'Account' - Enter Bank account manager\n" +
                        "\to 'Quit' - Quit Bank\n" +
                        "\nAccount commands:\n" +
                        "\to 'Create' 'type'(Default/Plus) - Create new account\n" +
                        "\to 'List' - Show all accounts\n" +
                        "\to 'View' 'id' - View account details\n" +
                        "\nAccount details commands:\n" +
                        "\to 'Deposit' 'money' - deposits money into account\n" +
                        "\to 'Withdraw' 'money' - 'withdraws money from account\n" +
                        "\to 'Changeplan' - Changes plan for eg. Default => Plus\n" +
                        "\to 'Delete' 'confirm account name' - deletes account\n" +
                        "Account detals additional commands (For Plus Accounts)\n" +
                        "\to 'Limit' 'set/add/remove' 'value' - limit manipulation\n" +
                        "";
            }
            else if(GUI == "account")
            {
                TEXT =  "\nAccount commands:\n" +
                        "\to 'Create' 'type'(Default/Plus) - Create new account\n" +
                        "\to 'List' - Show all accounts\n" +
                        "\to 'View' 'id' - View account details\n" +
                        "\to 'Back' - Back to menu\n";
            }
            else if(GUI == "accdetails")
            {
                TEXT  = "\nAccount details commands:\n" +
                        "\to 'Deposit' 'money' - deposits money into account\n" +
                        "\to 'Withdraw' 'money' - 'withdraws money from account\n";
                if (additionalInfo[0] == "Plus") TEXT += "\to 'Limit' 'set/add/remove' 'value' - limit manipulation\n";
                TEXT += "\to 'Changeplan' - Change your account plan\n" +
                        "\to 'Delete' 'confirm account name' - deletes account\n" +
                        "\to 'Back' - Back to Account commands";
            }

            Console.WriteLine(TEXT);
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
                Console.WriteLine($"\nVIEWING ACCOUNT NO. {accId}\n{Account}\nAccount type:\t{accType}\n");
                if (!msgDup)
                {
                    List<object> temp = [accType];
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
                        && !answer.StartsWith("delete ")
                        && !answer.StartsWith("limit "))
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
            if(command == "help")
            {
                WriteInterface("account");
                WaitForCommand("account");
            }
            else if(command.StartsWith("create "))
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
                    if (cmd[1] == "plus")
                    {
                        Console.Write("Limit: ");
                        decimal limit = Convert.ToDecimal(Console.ReadLine());
                        int accId = accountList.OrderBy(x => x.Key).Select(x => x.Key).Last() + 1;
                        accountList.Add(accId, new KontoPlus(name, startingBalance, limit));
                    }
                    else
                    {
                        int accId = accountList.OrderBy(x => x.Key).Select(x => x.Key).Last() + 1;
                        accountList.Add(accId, new Konto(name, startingBalance));
                    }
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    WaitForCommand("account");
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
                WaitForCommand();
            }
        }

        private void AccountDetailsCommandHandler(string command, object Account = null, string cmdType = "")
        {
            if(command == "help")
            {
                WaitForCommand(cmdType);
            }
            else if (command == "back")
            {
                WaitForCommand("account");
            }
            else if (command.StartsWith("deposit ") && command.Length > 7)
            {
                try
                {
                    int money = Int32.Parse(command.Split(' ')[1]);
                    Console.WriteLine($"Depositing {money}...");
                    if (Account is Konto) (Account as Konto).Wplata(money);
                    else if (Account is KontoPlus) (Account as KontoPlus).Wplata(money);
                    else throw new Exception("Something went wrong while depositing... Try again!");

                    WaitForCommand(cmdType, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand("account");
                }
            }
            else if (command.StartsWith("withdraw ") && command.Length > 7)
            {
                try
                {
                    int money = Int32.Parse(command.Split(' ')[1]);
                    Console.WriteLine($"Withdrawing {money}...");
                    if (Account is Konto) (Account as Konto).Wyplata(money);
                    else if (Account is KontoPlus) (Account as KontoPlus).Wyplata(money);
                    else throw new Exception("Something went wrong while withdrawing...");

                    WaitForCommand(cmdType, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand(cmdType, true);
                }
            }
            else if (command.StartsWith("limit "))
            {
                try
                {
                    var cmd = command.Split(" ");
                    if (cmd.Length != 3) throw new Exception("Invalid command usage!");

                    if (cmd[1] == "set")
                    {
                        if (Convert.ToDecimal(cmd[2]) < 0)
                            throw new Exception("Limit can't be a negative value!");
                        (Account as KontoPlus).Limit = Convert.ToDecimal(cmd[2]);
                        Console.WriteLine($"Setting new limit to {cmd[2]}");

                    }
                    else if (cmd[1] == "add")
                    {
                        if (Convert.ToDecimal(cmd[2]) < 0)
                            throw new Exception("You can't add negative value to a limit!");
                        (Account as KontoPlus).Limit += Convert.ToDecimal(cmd[2]);
                        Console.WriteLine($"Adding {cmd[2]} to limit. New limit: {(Account as KontoPlus).Limit}");

                    }
                    else if (cmd[1] == "remove")
                    {
                        if ((Account as KontoPlus).Limit < Convert.ToDecimal(cmd[2]) || Convert.ToDecimal(cmd[2]) < 0)
                            throw new Exception("Limit can't be a negative value!");
                        (Account as KontoPlus).Limit -= Convert.ToDecimal(cmd[2]);
                        Console.WriteLine($"Removing {cmd[2]} from limit. New limit: {(Account as KontoPlus).Limit}");

                    }
                    else
                    {
                        throw new Exception($"limit {cmd[1]} {cmd[2]} is not valid command!");
                    }

                    WaitForCommand(cmdType, true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand("account");
                }
            }
            else if (command == "changeplan")
            {
                try
                {
                    int accId = Int32.Parse(cmdType.Split(' ')[1]);
                    if(Account is KontoPlus)
                    {
                        var tmp = Account as KontoPlus;
                        if (tmp.Bilans < 0) throw new Exception("Cannot convert account! Default accounts can't have negative balance!");
                        accountList[accId] = new Konto(tmp.Klient, tmp.Bilans);
                    }
                    else
                    {
                        var tmp = Account as Konto;
                        accountList[accId] = new KontoPlus(tmp.Klient, tmp.Bilans, 100M);
                    }
                    WaitForCommand("account");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WaitForCommand("account");
                }
            }
            else if (command.StartsWith("delete ") && command.Length > 7)
            {
                var cmd = command.Split(' ');
                if (cmd[1] == (Account as Konto).Klient.ToLower())
                {
                    int accId = Int32.Parse(cmdType.Split(' ')[1]);
                    accountList.Remove(accId);
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
    #endregion

    public class Konto
    {
        public KontoPlus ConvertToPlus()
        {
            KontoPlus tmp = new(Klient, Bilans, 100M);
            return tmp;
        }
        public Konto ConvertToKonto()
        {
            if (Bilans < 0)
                throw new ArgumentException("Cannot create account!\nFirstly pay your debts!");

            Konto tmp = new(Klient, Bilans);
            return tmp;
        }

        private string klient;
        public string Klient {
            get => klient;
            private set => klient = value.Trim() != "" ? value.Trim() : throw new ArgumentException("Account name can't be empty!"); }
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
            var status = zablokowane ? "Locked" : "Unlocked";
            return $"Name:\t\t{klient}\n" +
                   $"Balance:\t{bilans}\n" +
                   $"Account status:\t{status}";
        }

        public void UpdateBalance(decimal money)
        {
            bilans += money;
        }

        public virtual void Wplata(decimal kwota)
        {
            if (zablokowane) throw new ArgumentException("Account is locked!");

            if (kwota > 0)
            {
                bilans += kwota;
            }
            else
            {
                throw new ArgumentException("Invalid deposit value!!");
            }
        }
        
        public virtual void Wyplata(decimal kwota)
        {
            if (zablokowane) throw new ArgumentException("Account is locked!");

            if (kwota > 0 && kwota <= bilans)
            {
                bilans -= kwota;
            }
            else
            {
                throw new ArgumentException("Invalid withdraw value!!");
            }
        }

        public void BlokujKonto() => zablokowane = true;
        public void OdblokujKonto() => zablokowane = false;

        public override string ToString()
        {
            var status = zablokowane ? "Locked" : "Unlocked";
            return $"Name:\t\t{klient}\n" +
                   $"Balance:\t{bilans}\n" +
                   $"Account status:\t{status}";
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
                throw new ArgumentException("Account is locked!");

            if(kwota > (Bilans + limit) || kwota <= 0)
                throw new ArgumentException("Invalid withdraw value!");

            base.UpdateBalance(-kwota);

            if (Bilans < 0)
                base.BlokujKonto();
        }

        public override void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentException("Invalid deposit value!!");
            
            base.UpdateBalance(kwota);

            if (Bilans >= 0)
                base.OdblokujKonto();
        }

        public override string Statystyki()
        {
            var status = Zablokowane ? "Locked" : "Unlocked";
            return $"Name:\t\t{Klient}\n" +
                   $"Balance:\t{Bilans} ({Limit})\n" +
                   $"Account status:\t{status}";
        }

        public override string ToString()
        {
            var status = Zablokowane ? "Locked" : "Unlocked";
            return $"Name:\t\t{Klient}\n" +
                   $"Balance:\t{Bilans} ({Limit})\n" +
                   $"Account status:\t{status}";
        }
    }

    public class KontoLimit
    {
        public static explicit operator KontoPlus(KontoLimit k)
        {
            return new KontoPlus(k.Klient, k.Bilans, k.Limit);
        }

        public static explicit operator Konto(KontoLimit k)
        {
            return new Konto(k.Klient, k.Bilans);
        }

        private Konto konto { get; set; }
        private decimal limit { get; set; }
        public decimal Bilans { get => konto.Bilans; }
        public string Klient { get => konto.Klient; }
        public bool Zablokowane { get => konto.Zablokowane; }
        public decimal Limit
        {
            get => limit;
            set => limit = value > 0 ? value : 0;
        }

        public KontoLimit(string name, decimal bilansNaStart = 0, decimal limit = 100)
        {
            konto = new Konto(name, bilansNaStart);
            Limit = limit;
        }

        public string Statystyki()
        {
            var status = Zablokowane ? "Locked" : "Unlocked";
            return $"Name:\t\t{Klient}\n" +
                   $"Balance:\t{Bilans} ({Limit})\n" +
                   $"Account status:\t{status}";
        }

        public void Wyplata(decimal kwota)
        {
            if (Zablokowane)
                throw new ArgumentException("Account is locked!");

            if (kwota > (Bilans + limit) || kwota <= 0)
                throw new ArgumentException("Invalid withdraw value!");

            konto.UpdateBalance(-kwota);

            if (Bilans < 0)
                konto.BlokujKonto();
        }

        public void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentException("Invalid deposit value!!");

            konto.UpdateBalance(kwota);

            if (Bilans >= 0)
                konto.OdblokujKonto();
        }

        public void BlokujKonto() => konto.BlokujKonto();
        public void OdblokujeKonto() => konto.OdblokujKonto();

        public override string ToString()
        {
            var status = Zablokowane ? "Locked" : "Unlocked";
            return $"Name:\t\t{Klient}\n" +
                   $"Balance:\t{Bilans} ({Limit})\n" +
                   $"Account status:\t{status}";
        }
    }
}
