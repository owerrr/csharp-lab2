
namespace ChmodConverterLib
{
    public class ChmodConverter
    {
        public static int SymbolicToNumeric(string input)
        {
            if (input.Length % 3 != 0) throw new ArgumentException("Invalid syntax!");
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
                        res += 4;
                        break;
                    case 'w':
                        res += 2;
                        break;
                    case 'x':
                        res += 1;
                        break;
                    case '-':
                        break;
                    default:
                        throw new ArgumentException("Invalid syntax!");
                }
                count++;
            }
            return res;
        }

        public static string NumericToSymbolic(int input)
        {
            int temp = input;
            List<int> ints = new List<int>();
            string res = "";
            // 7 7 7
            while(temp > 0)
            {
                int num = temp % 10;
                if (num > 7 || num < 0) throw new ArgumentException("Invalid syntax!");
                ints.Add(num);
                temp /= 10;
            }
            ints.Reverse();
            for(int i = 0; i < ints.Count; i++)
            {
                if (ints[i] >= 4)
                {
                    res += "r";
                    ints[i] -= 4;
                }
                if (ints[i] >= 2)
                {
                    res += "w";
                    ints[i] -= 2;
                }
                if (ints[i] == 1)
                {
                    res += "x";
                    ints[i] -= 1;
                }
                else
                {
                    res += "-";
                }
            }

            return res;
        }
    }
}
//rwxrwxrwx
