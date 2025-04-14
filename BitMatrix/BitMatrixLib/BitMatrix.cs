using System.Collections;

namespace BitMatrixLib
{
    public class BitMatrix : IEquatable<BitMatrix>, IEnumerable<int>, ICloneable
    {
        private BitArray data;
        public int NumberOfRows { get; }
        public int NumberOfColumns { get; }
        public bool IsReadOnly => false;

        // tworzy prostokątną macierz bitową wypełnioną `defaultValue`
        public BitMatrix(int numberOfRows, int numberOfColumns, int defaultValue = 0)
        {   
            if (numberOfRows < 1 || numberOfColumns < 1)
                throw new ArgumentOutOfRangeException("Incorrect size of matrix");
            data = new BitArray(numberOfRows * numberOfColumns, BitToBool(defaultValue));
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
        }
        public BitMatrix(int numberOfRows, int numberOfColumns, params int[] bits)
        {
            if (numberOfRows < 1 || numberOfColumns < 1)
                throw new ArgumentOutOfRangeException("Incorrect size of matrix");
            data = new BitArray(numberOfRows * numberOfColumns, false);
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            if(bits != null)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    if (i >= numberOfColumns * numberOfRows) break;
                    data[i] = bits[i] == 0 ? false : true;
                }
            }
        }

        public BitMatrix(int[,] bits)
        {
            if (bits == null) throw new NullReferenceException("Input is null!");
            if (bits.Length <= 0) throw new ArgumentOutOfRangeException("Input is empty!");

            NumberOfRows = bits.GetLength(0);
            NumberOfColumns = bits.GetLength(1);
            data = new BitArray(NumberOfRows * NumberOfColumns, false);
            int temp = 0;
            for(int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    data[temp] = bits[i, j] == 0 ? false : true;
                    temp++;
                }
            }
        }

        public BitMatrix(bool[,] bits)
        {
            if (bits == null) throw new NullReferenceException("Input is null!");
            if (bits.Length <= 0) throw new ArgumentOutOfRangeException("Input is empty!");

            NumberOfRows = bits.GetLength(0);
            NumberOfColumns = bits.GetLength(1);
            data = new BitArray(NumberOfRows * NumberOfColumns, false);
            int temp = 0;
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    data[temp] = bits[i, j];
                    temp++;
                }
            }
        }

        public override string ToString()
        {
            string res = string.Empty;
            for(int i = 0; i < NumberOfRows * NumberOfColumns; i++)
            {
                if (i != 0 && i % NumberOfColumns == 0) {
                    res += Environment.NewLine;
                }
                res += BoolToBit(data[i]);
            }
            return res + Environment.NewLine;
        }

        public static int BoolToBit(bool boolValue) => boolValue ? 1 : 0;
        public static bool BitToBool(int bit) => bit != 0;

        public override int GetHashCode()
        {
            return HashCode.Combine(data.GetHashCode());
        }
        public bool Equals(BitMatrix other)
        {
            if (ReferenceEquals(other,null)) return false;
            if (NumberOfColumns != other.NumberOfColumns || NumberOfRows != other.NumberOfRows) return false;
            if (data.Length != other.data.Length) return false;
            for(int i = 0; i < data.Length; i++)
            {
                if (data[i] != other.data[i]) return false;
            }
            return true;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, null)) return false;
            BitMatrix b;
            try
            {
                b = (BitMatrix)other;
            }
            catch
            {
                return false;
            }
            return data.Equals(b);
        }

        public static bool Equals(BitMatrix b1, BitMatrix b2) {
            if (ReferenceEquals(b1, null) && ReferenceEquals(b2, null)) return true;
            if (ReferenceEquals(b1, null) || ReferenceEquals(b2, null)) return false;
            if (b1.NumberOfColumns != b2.NumberOfColumns || b1.NumberOfRows != b2.NumberOfRows) return false;
            if (b1.data.Length != b2.data.Length) return false;
            for (int i = 0; i < b1.data.Length; i++)
            {
                if (b1.data[i] != b2.data[i]) return false;
            }
            return true;
        }

        public static bool operator==(BitMatrix b1, BitMatrix b2)
        {
            return Equals(b1, b2);
        }
        public static bool operator !=(BitMatrix b1, BitMatrix b2)
        {
            return !Equals(b1, b2);
        }

        public int this[int i, int j]
        {
            get
            {
                int idx = i * NumberOfColumns + j;
                if (i < 0 || j < 0 || i >= NumberOfRows || j >= NumberOfColumns) throw new IndexOutOfRangeException();
                return BoolToBit(data[idx]);
            }
            set
            {
                int idx = i * NumberOfColumns + j;
                if (i < 0 || j < 0 || i >= NumberOfRows || j >= NumberOfColumns) throw new IndexOutOfRangeException();
                data[idx] = BitToBool(value);
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            for(int i = 0; i < data.Length; i++)
            {
                yield return BoolToBit(this.data[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Clone()
        {
            int[] newData = new int[data.Length];
            data.CopyTo(newData, 0);
            return new BitMatrix(this.NumberOfRows, this.NumberOfColumns, newData);
        }

        public static BitMatrix Parse(string s)
        {
            if (s == null || s == String.Empty) throw new ArgumentNullException();
            string[] data = s.Split(Environment.NewLine);
            int rows = data.Length;
            int cols = data[0].Length;
            bool[,] boolData = new bool[rows,cols];
            int idx = 0;
            for(int i = 0; i < rows; i++)
            {
                int j = 0;
                for(j = 0; j < data[i].Length; j++)
                {
                    if (cols <= j) throw new FormatException();
                    var tmp = data[i][j] - '0';
                    if (tmp != 0 && tmp != 1) throw new FormatException();
                    boolData[idx, j] = BitToBool(tmp);
                }
                if(cols != j) throw new FormatException();
                idx++;
            }

            return new BitMatrix(boolData);
        }

        public static bool TryParse(string s, out BitMatrix result)
        {
            result = null;
            if (s == null || s == String.Empty) return false;
            string[] data = s.Split(Environment.NewLine);
            int rows = data.Length;
            int cols = data[0].Length;
            bool[,] boolData = new bool[rows, cols];
            int idx = 0;
            for (int i = 0; i < rows; i++)
            {
                int j = 0;
                for (j = 0; j < data[i].Length; j++)
                {
                    if (cols <= j) return false;
                    var tmp = data[i][j] - '0';
                    if (tmp != 0 && tmp != 1) return false;
                    boolData[idx, j] = BitToBool(tmp);
                }
                if (cols != j) return false;
                idx++;
            }
            result = new BitMatrix(boolData);
            return true;
        }

        public static explicit operator BitMatrix(int[,] input)
        {
            if (input == null) throw new NullReferenceException();
            if (input.Length == 0) throw new ArgumentOutOfRangeException(); 
            return new BitMatrix(input);
        }

        public static implicit operator int[,](BitMatrix input)
        {
            if (ReferenceEquals(input, null)) throw new NullReferenceException();
            int[,] res = new int[input.NumberOfRows, input.NumberOfColumns];
            for(int i = 0; i < res.GetLength(0); i++)
            {
                for(int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = input[i, j];
                }
            }
            return res;
        }

        public static explicit operator BitMatrix(bool[,] input)
        {
            if (ReferenceEquals(input, null)) throw new NullReferenceException();
            if (input.Length == 0) throw new ArgumentOutOfRangeException();
            return new BitMatrix(input);
        }

        public static implicit operator bool[,](BitMatrix input)
        {
            if (ReferenceEquals(input, null)) throw new NullReferenceException();
            bool[,] res = new bool[input.NumberOfRows, input.NumberOfColumns];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = BitToBool(input[i, j]);
                }
            }
            return res;
        }

        public static explicit operator BitArray(BitMatrix input)
        {
            if (ReferenceEquals(input, null)) throw new NullReferenceException();
            return new BitArray(input.data);
        }
    }
}
