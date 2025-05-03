
namespace ChmodConverterLib
{
    public class ChmodConverter
    {
        public static string SymbolicToNumeric(string input)
        {
            int len = input.Length;
            if (len != 9) throw new ArgumentException($"Invalid syntax! You typed {len} symbols! Valid pattern is 9 symbols length!");
            int num_res = 0;
            int count = 0;
            foreach (char c in input)
            {
                if(count >= 3)
                {
                    num_res *= 10;
                    count = 0;
                }
                switch (c)
                {
                    case 'r':
                        if (count != 0) throw new ArgumentException("Invalid syntax! letter 'r' is not in valid place!");
                        num_res += 4;
                        break;
                    case 'w':
                        if (count != 1) throw new ArgumentException("Invalid syntax! letter 'w' is not in valid place!");
                        num_res += 2;
                        break;
                    case 'x':
                        if (count != 2) throw new ArgumentException("Invalid syntax! letter 'x' is not in valid place!");
                        num_res += 1;
                        break;
                    case '-':
                        break;
                    default:
                        throw new ArgumentException($"Invalid syntax! Symbol {c} is not valid! Valid symbols: r, w, x, -");
                }
                count++;
            }
            string res = "";
            if (num_res < 10)
                res = "00" + num_res.ToString();
            else if (num_res < 100)
                res = "0" + num_res.ToString();
            else
                res = num_res.ToString();
            return res;
        }

        public static string NumericToSymbolic(string input)
        {
            string res = "";
            if (input.Length != 3) throw new ArgumentException($"Invalid syntax! Your input is {input.Length} length! Valid length is 3!");
            foreach(char n in input)
            {
                switch (n)
                {
                    case '7':
                        res += "rwx";
                        break;
                    case '6':
                        res += "rw-";
                        break;
                    case '5':
                        res += "r-x";
                        break;
                    case '4':
                        res += "r--";
                        break;
                    case '3':
                        res += "-wx";
                        break;
                    case '2':
                        res += "-w-";
                        break;
                    case '1':
                        res += "--x";
                        break;
                    case '0':
                        res += "---";
                        break;
                    default:
                        throw new ArgumentException($"Invalid syntax! Number {n} is out of range!");
                }
            }

            return res;
        }
    }
}
//rwxrwxrwx
