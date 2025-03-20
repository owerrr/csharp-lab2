#nullable disable

namespace MatrixClass
{
    public class Matrix2D : IEquatable<Matrix2D>
    {
        private int[] matrix_data = new int[4];
        public int[] Data { 
            get => matrix_data; 
            private set => matrix_data = value;
        }
        public static readonly int[] Id = [1, 0, 0, 1];
        public static readonly int[] Zero = [0, 0, 0, 0];


        public Matrix2D(int a1, int a2, int b1, int b2)
        {
            Data = [ a1, a2, b1, b2 ];
        }

        public Matrix2D()
        {
            Data = [1, 0, 0, 1];
        }

        public static void Transpose(Matrix2D a)
        {
            if (a.Data == null) return;

            int temp = a.Data[1];
            a.Data[1] = a.Data[2];
            a.Data[2] = temp;
        }

        public static int Determinant(Matrix2D a)
        {
            if (a.Data == null)
                throw new ArgumentException("Matrix not found!");

            int det = a.Data[0] * a.Data[3] + a.Data[1] * a.Data[2];
            return det;
        }
        public int Det()
        {
            if (Data == null)
                throw new ArgumentException("Matrix not found!");

            int det = Data[0] * Data[3] + Data[1] * Data[2];
            return det;
        }

        public override string ToString()
        {
            return $"[[{Data[0]} {Data[1]}], [{Data[2]} {Data[3]}]]";
        }

        public bool Equals(Matrix2D? other)
        {
            if (other == null) return false;

            bool equal = true;
            for(int i = 0; i < Data.Length; i++)
            {
                if (Data[i] != other.Data[i])
                    equal = false;
            }
            return equal;
        }

        public override bool Equals(object? obj)
        {
            Matrix2D m2 = obj as Matrix2D;
            if (m2 == null) return false;

            return Equals(m2);
        }

        public static bool Equals(Matrix2D? a, Matrix2D? b)
        {
            if (a == null || b == null) return false;

            bool equal = true;
            for (int i = 0; i < a.Data.Length; i++)
            {
                if (a.Data[i] != b.Data[i])
                    equal = false;
            }
            return equal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data);
        }

        public static bool operator ==(Matrix2D a, Matrix2D b) { 
            return Equals(a, b);
        }
        public static bool operator !=(Matrix2D a, Matrix2D b)
        {
            return !Equals(a, b);
        }
        public static Matrix2D operator +(Matrix2D a, Matrix2D b)
        {
            if (a.Data == null || b.Data == null)
                throw new ArgumentException("Invalid input!");

            try
            {
                for (int i = 0; i < b.Data.Length; i++)
                {
                    a.Data[i] += b.Data[i];
                }

                return a;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public static Matrix2D operator -(Matrix2D a, Matrix2D b)
        {
            if (a.Data == null || b.Data == null)
                throw new ArgumentException("Invalid input!");

            try
            {
                for (int i = 0; i < b.Data.Length; i++)
                {
                    a.Data[i] -= b.Data[i];
                }

                return a;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public static Matrix2D operator *(Matrix2D a, Matrix2D b)
        {
            if (a.Data == null || b.Data == null)
                throw new ArgumentException("Invalid input!");

            try
            {
                a.Data[0] = a.Data[0] * b.Data[0] + a.Data[1] * b.Data[2];
                a.Data[1] = a.Data[0] * b.Data[1] + a.Data[1] * b.Data[3];
                a.Data[2] = a.Data[2] * b.Data[0] + a.Data[3] * b.Data[2];
                a.Data[3] = a.Data[2] * b.Data[1] + a.Data[3] * b.Data[3];

                return a;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }   
        }

        public static Matrix2D operator *(Matrix2D a, int b)
        {
            if (a.Data == null)
                throw new ArgumentException("Invalid input!");

            try
            {
                for(int i = 0; i < a.Data.Length; i++)
                {
                    a.Data[i] *= b;
                }

                return a;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public static Matrix2D operator *(int b, Matrix2D a)
        {
            if (a.Data == null)
                throw new ArgumentException("Invalid input!");

            try
            {
                for (int i = 0; i < a.Data.Length; i++)
                {
                    a.Data[i] *= b;
                }

                return a;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public static Matrix2D operator -(Matrix2D a)
        {
            if (a.Data == null)
                throw new ArgumentException("Invalid input!");

            try
            {
                for (int i = 0; i < a.Data.Length; i++)
                {
                    a.Data[i] *= -1;
                }

                return a;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public static explicit operator int[,](Matrix2D a)
        {
            int[,] temp = { { a.Data[0], a.Data[1] }, { a.Data[2], a.Data[3] } };
            return temp;
        }

        private const string m2D_syntax = "[x, x], [x, x]";
        public static Matrix2D Parse(string s)
        {
            if (s == null)
                throw new ArgumentException("Invalid syntax!");

            try
            {
                //"[2, 1], [3, 2]"
                List<int> temp = new List<int>();

                var str = s.Split(',');
                // [x | x] | [x | x]
                bool opened = false;
                for(int i = 0; i < str.Length; i++)
                {
                    //Console.WriteLine($"\nSPRAWDZAM DLA: {str[i]}\n");
                    if ((!opened && str[i].Trim()[0] != '[') || (opened && str[i].Trim()[str[i].Trim().Length-1] != ']'))
                        throw new FormatException("Invalid syntax!");
                    
                    if(!opened)
                        str[i] = str[i].Replace("[", String.Empty);
                    else
                        str[i] = str[i].Replace("]", String.Empty);

                    opened = !opened;
                    //Console.WriteLine($"\nDODAJE DLA: {str[i]}\n");
                    temp.Add(Int32.Parse(str[i]));
                }

                Matrix2D m2D = new(temp[0], temp[1], temp[2], temp[3]);
                return m2D;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

    }
}
