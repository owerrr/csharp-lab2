
namespace ChmodConverterLib
{
    public class ChmodConverter
    {
        public static int SymbolicToNumeric(string input)
        {
            int len = input.Length;
            if (len != 9) throw new ArgumentException($"Invalid syntax! You typed {len} symbols! Valid pattern is 9 symbols length!");
            int res = 0;
            int count = 0;
            foreach (char c in input)
            {
                if(count >= 3)
                {
                    res *= 10;
                    count = 0;
                }
                switch (c)
                {
                    case 'r':
                        if (count != 0) throw new ArgumentException("Invalid syntax! letter 'r' is not in valid place!");
                        res += 4;
                        break;
                    case 'w':
                        if (count != 1) throw new ArgumentException("Invalid syntax! letter 'w' is not in valid place!");
                        res += 2;
                        break;
                    case 'x':
                        if (count != 2) throw new ArgumentException("Invalid syntax! letter 'x' is not in valid place!");
                        res += 1;
                        break;
                    case '-':
                        break;
                    default:
                        throw new ArgumentException($"Invalid syntax! Symbol {c} is not valid! Valid symbols: r, w, x, -");
                }
                count++;
            }
            return res;
        }

        public static string NumericToSymbolic(int input)
        {
            int temp = input;
            List<int> nums = new List<int>();
            string res = "";
            // 7 7 7
            while(temp > 0)
            {
                int num = temp % 10;
                if (num > 7 || num < 0) throw new ArgumentException($"Invalid syntax! Number {num} is out of range!");
                nums.Add(num);
                temp /= 10;
            }
            if (nums.Count != 3) throw new ArgumentException($"Invalid syntax! Your input is {nums.Count} length! Valid length is 3!");
            nums.Reverse();
            foreach(int n in nums)
            {
                switch (n)
                {
                    case 7:
                        res += "rwx";
                        break;
                    case 6:
                        res += "rw-";
                        break;
                    case 5:
                        res += "r-x";
                        break;
                    case 4:
                        res += "r--";
                        break;
                    case 3:
                        res += "-wx";
                        break;
                    case 2:
                        res += "-w-";
                        break;
                    case 1:
                        res += "--x";
                        break;
                    case 0:
                        res += "---";
                        break;
                    default:
                        throw new ArgumentException($"Invalid syntax! Argument {n} out of range!");
                }
            }

            return res;
        }
    }
}
//rwxrwxrwx
