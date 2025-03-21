using MatrixClass;

namespace Matrix2D_Tests
{
    [TestClass]
    public class Matrix2D_TEST_Create
    {
        [TestMethod]
        public void Create_Using_DefaultConstructor()
        {
            Matrix2D test = new();
            Matrix2D res = new(Matrix2D.Id);

            Assert.IsTrue(test == res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }
        [TestMethod]
        public void Create_Using_Id()
        {
            Matrix2D test = new(Matrix2D.Id);
            Matrix2D res = new(1, 0, 0, 1);

            Assert.IsTrue(test == res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }
        [TestMethod]
        public void Create_Using_Zero()
        {
            Matrix2D test = new(Matrix2D.Zero);
            Matrix2D res = new(0, 0, 0, 0);

            Assert.IsTrue(test == res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }
        [TestMethod]
        public void Create_Using_Array()
        {
            Matrix2D test = new([1, 2, 3, 4]);
            Matrix2D res = new(1, 2, 3, 4);

            Assert.IsTrue(test == res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }
        [TestMethod]
        public void Create_Using_Parse()
        {
            Matrix2D test = Matrix2D.Parse("[1, 2], [2, 4]");
            Matrix2D res = new(1, 2, 2, 4);

            Assert.IsTrue(test == res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }
    }

    [TestClass]
    public class Matrix2D_TEST_Methods
    {
        [TestMethod]
        public void Transpose_ValidInput()
        {
            Matrix2D test = new(1, 1, 2, 2);
            Matrix2D res = new(1, 2, 1, 2);
            Matrix2D.Transpose(test);

            Assert.IsTrue(test == res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }
        [TestMethod]
        public void Transpose_InvalidInput()
        {
            Matrix2D test = new(1, 1, 2, 2);
            Matrix2D res = new(Matrix2D.Zero);
            Matrix2D.Transpose(test);

            Assert.IsTrue(test != res, $"Coś poszło nie tak! {test.Data} != {res.Data}");
        }

        [TestMethod]
        public void Determinant_Static_ValidInput()
        {
            Matrix2D test = new(1, 2, 2, 1);
            int res = Matrix2D.Determinant(test);
            int pred = 5;

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res} != {pred}");
        }
        [TestMethod]
        public void Determinant_Static_InvalidInput()
        {
            try
            {
                int res = Matrix2D.Determinant(null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Matrix not found!"), ex.Message);
            }
        }

        [TestMethod]
        public void Determinant_Method_ValidInput()
        {
            Matrix2D test = new(1, 2, 2, 1);
            int res = test.Det();
            int pred = 5;

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res} != {pred}");
        }
        
        [TestMethod]
        public void Overrided_To_string()
        {
            Matrix2D test = new(Matrix2D.Id);
            string res = test.ToString();
            string pred = "[[1 0], [0 1]]";

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res} != {pred}");
        }
    }

    [TestClass]
    public class Matrix2D_TEST_MathematicOperations
    {
        [TestMethod]
        public void Addition_ValidInput()
        {
            Matrix2D a = new(2, 2, 2, 2);
            Matrix2D b = new(1, 2, 1, 2);
            Matrix2D res = a + b;
            Matrix2D pred = new(3, 4, 3, 4);

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res.Data} != {pred.Data}");
        }
        [TestMethod]
        public void Substraction_ValidInput()
        {
            Matrix2D a = new(2, 2, 2, 2);
            Matrix2D b = new(1, 2, 1, 2);
            Matrix2D res = a - b;
            Matrix2D pred = new(1, 0, 1, 0);

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res.Data} != {pred.Data}");
        }
        [TestMethod]
        public void Multiplication_By_Another_Matrix2D()
        {
            Matrix2D a = new(4, 3, 1, 5);
            Matrix2D b = new(3, 2, 1, 0);
            Matrix2D res = a * b;
            Matrix2D pred = new(15, 8, 8, 2);

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res.Data} != {pred.Data}");
        }
        [TestMethod]
        public void Multiplication_By_Int()
        {
            Matrix2D a = new(1, 2, 3, 4);
            Matrix2D res = a * 4;
            Matrix2D pred = new(4, 8, 12, 16);

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res.Data} != {pred.Data}");
        }
        [TestMethod]
        public void Multiplication_By_Int_Backwards()
        {
            Matrix2D a = new(1, 2, 3, 4);
            Matrix2D res = 4 * a;
            Matrix2D pred = new(4, 8, 12, 16);

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res.Data} != {pred.Data}");
        }
        [TestMethod]
        public void Change_Matrix_Sign()
        {
            Matrix2D a = new(1, 2, 3, 4);
            Matrix2D res = -a;
            Matrix2D pred = new(-1, -2, -3, -4);

            Assert.IsTrue(res == pred, $"Coś poszło nie tak! {res.Data} != {pred.Data}");
        }
    }
    [TestClass]
    public class Matrix2D_TEST_Projections
    {
        [TestMethod]
        public void Matrix2D_To_Int_Array()
        {
            Matrix2D a = new(1, 2, 3, 4);
            int[,] res = (int[,])a;
            int[,] pred = { { 1, 2 }, { 3, 4 } };
            bool isEqual = 
                    res[0, 0] == pred[0, 0]
                &&  res[0, 1] == pred[0, 1]
                &&  res[1, 0] == pred[1, 0]
                &&  res[1, 1] == pred[1, 1];

            Assert.IsTrue(isEqual, $"Coś poszło nie tak! {res[0,0]} {res[0, 1]} {res[1, 0]} {res[1, 1]} != {pred[0,0]} {pred[0, 1]} {pred[1, 0]} {pred[1, 1]}");
        }
    }
}
