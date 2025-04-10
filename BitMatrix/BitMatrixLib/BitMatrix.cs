using System.Collections;

namespace BitMatrixLib
{
    public class BitMatrix : IEquatable<BitMatrix>
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
            if(data.Length != other.data.Length) return false;
            for(int i = 0; i < data.Length; i++)
            {
                if (data[i] != other.data[i]) return false;
            }
            return true;
        }

        public bool Equals(object? other)
        {
            BitMatrix b = (BitMatrix)other;
            if (b == null) return false;
            return data.Equals(b);
        }

        public static bool Equals(BitMatrix b1, BitMatrix b2) {
            if (b1.data.Length != b2.data.Length) return false;
            for (int i = 0; i < b1.data.Length; i++)
            {
                if (b1.data[i] != b2.data[i]) return false;
            }
            return true;
        }

    }
}
