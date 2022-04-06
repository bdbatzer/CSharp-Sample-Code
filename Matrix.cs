using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpCodeSamples
{
    //  Class: Matrix
    //------------------------------------------------------------------------
    /// @brief Autonodyne Matrix class; strictly 2D
    //------------------------------------------------------------------------
    public class Matrix
    {
        //  Member: values
        //------------------------------------------------------------------------
        /// @brief Values of matrix double array
        //------------------------------------------------------------------------
        private double[,] values;

        //  Property: Rows
        //------------------------------------------------------------------------
        /// @brief Number of rows in the matrix
        //------------------------------------------------------------------------
        public int Rows { get; private set; }

        //  Property: Cols
        //------------------------------------------------------------------------
        /// @brief Number of columns in the matrix
        //------------------------------------------------------------------------
        public int Cols { get; private set; }

        //  Property: T
        //------------------------------------------------------------------------
        /// @brief Property-style shorthand for Transpose
        //------------------------------------------------------------------------
        public Matrix T
        {
            get { return this.Transpose(); }
            private set { }
        }

        //  Constructor: Matrix
        //------------------------------------------------------------------------
        /// @brief Construct a Matrix by specifying the number of rows and columns
        /// 
        /// @param rows Number of rows
        /// @param ccols Number of columns
        //------------------------------------------------------------------------
        public Matrix(int rows, int cols)
        {
            // zero-matrix with dimensions r x cols
            this.Rows = rows;
            this.Cols = cols;
            this.values = new double[rows, cols];
        }

        //  Constructor: Matrix
        //------------------------------------------------------------------------
        /// @brief Construct a Matrix using a 2-dimensional array of values
        /// 
        /// @param vals Array of values
        //------------------------------------------------------------------------
        public Matrix(double[,] vals)
        {
            // create a matrix from a 2d array
            this.Rows = vals.GetLength(0);
            this.Cols = vals.GetLength(1);
            this.values = vals;
        }

        //  Constructor: Matrix
        //------------------------------------------------------------------------
        /// @brief Construct a Matrix by stacking row Vectors
        /// 
        /// @param vecs List of Vectors that will become the rows of the Matrix
        //------------------------------------------------------------------------
        public Matrix(List<Vector> vecs)
        {
            // create a matrix by stacking vectors
            this.Rows = vecs.Count;
            this.Cols = vecs[0].Length;
            this.values = new double[vecs.Count, vecs[0].Length];
            for (int i = 0; i < vecs.Count; i++)
            {
                for (int j = 0; j < vecs[0].Length; j++)
                {
                    if (vecs[j].Length != vecs[0].Length)
                    {
                        throw new ArgumentException($"No handling for jagged 2-d arrays! All vectors need to be {vecs[0].Length} and this one is {vecs[j].Length}.");
                    }
                    this.values[i, j] = vecs[i][j];
                }
            }
        }

        //  Constructor: Matrix
        //------------------------------------------------------------------------
        /// @brief Construct a Matrix by stacking row Vectors
        /// 
        /// @param vecs Vectors that will become the rows of the Matrix
        //------------------------------------------------------------------------
        public Matrix(params Vector[] vecs) : this(new List<Vector>(vecs)) { }

        //  Operator: Indexing
        //------------------------------------------------------------------------
        /// @brief Access an element of the Matrix
        /// 
        /// @param i Row number of element
        /// @param j Column number of element
        //------------------------------------------------------------------------
        public double this[int i, int j]
        {
            // array-like element access
            get
            {
                if (i >= Rows || i < 0 || j >= Cols || j < 0)
                {
                    throw new IndexOutOfRangeException($"Index {new Tuple<int, int>(i, j)} out of bounds for Matrix of shape ({this.Rows}, {this.Cols}).");
                }
                return this.values[i, j];
            }
            set
            {
                if (i >= Rows || i < 0 || j >= Cols || j < 0)
                {
                    throw new IndexOutOfRangeException($"Index {new Tuple<int, int>(i, j)} out of bounds for Matrix of shape ({this.Rows}, {this.Cols}).");
                }
                this.values[i, j] = value;
            }
        }

        //  Operator: * Matrix
        //------------------------------------------------------------------------
        /// @brief Matrix Multiplication
        /// 
        /// @param m Matrix
        /// @param u Vector
        /// 
        /// @return Product of matrix with a vector
        //------------------------------------------------------------------------
        public static Vector operator *(Matrix m, Vector u)
        {
            return Mul(m, u);
        }

        //  Operator: * Matrix
        //------------------------------------------------------------------------
        /// @brief Matrix Multiplication
        /// 
        /// @param u Vector
        /// @param m Matrix
        /// 
        /// @return Product of vector with a matrix
        //------------------------------------------------------------------------
        public static Vector operator *(Vector u, Matrix m)
        {
            return Mul(u, m);
        }

        //  Operator: * Matrix
        //------------------------------------------------------------------------
        /// @brief Matrix Multiplication
        /// 
        /// @param m Matrix
        /// @param n Matrix
        /// 
        /// @return Product of two matrices
        //------------------------------------------------------------------------
        public static Matrix operator *(Matrix m, Matrix n)
        {
            return Mul(m, n);
        }

        //  Operator: + Matrix
        //------------------------------------------------------------------------
        /// @brief Matrix Addition
        /// 
        /// @param m Matrix
        /// @param n Matrix
        /// 
        /// @return Sum of two matrices
        //------------------------------------------------------------------------
        public static Matrix operator +(Matrix m, Matrix n)
        {
            return Add(m, n);
        }

        //  Operator: - Matrix
        //------------------------------------------------------------------------
        /// @brief Matrix Subtraction
        /// 
        /// @param m Matrix
        /// @param n Matrix
        /// 
        /// @return Difference of two matrices
        //------------------------------------------------------------------------
        public static Matrix operator -(Matrix m, Matrix n)
        {
            return Sub(m, n);
        }

        //  Function: Get Row
        //------------------------------------------------------------------------
        /// @brief Return a Row of a Matrix as a Vector object
        /// 
        /// @param i Row number to get
        /// 
        /// @return The desired column
        //------------------------------------------------------------------------
        public Vector GetRow(int i)
        {
            // get a row as a vector
            Vector ret = new Vector(Cols);
            for (int j = 0; j < Cols; j++)
            {
                ret[j] = this[i, j];
            }
            return ret;
        }

        //  Function: Set Row
        //------------------------------------------------------------------------
        /// @brief Set a Row of a Matrix to a Vector object
        /// 
        /// @param u Input Vector
        /// @param i Row number to set
        //------------------------------------------------------------------------
        public void SetRow(Vector u, int i)
        {
            // set a row to a vector
            if (u.Length != Cols)
            {
                throw new ArgumentException($"Vector has length {u.Length}. Length of {Cols} required.");
            }
            else if (i >= Rows || i < 0)
            {
                throw new IndexOutOfRangeException($"Index {i} out of bounds for Matrix with {Rows} rows.");
            }
            for (int j = 0; j < Cols; j++)
            {
                this[i, j] = u[j];
            }
        }

        //  Function: Get Column
        //------------------------------------------------------------------------
        /// @brief Return a Col of a Matrix as a Vector object
        /// 
        /// @param j Col number to get
        /// 
        /// @return The desired column
        //------------------------------------------------------------------------
        public Vector GetCol(int j)
        {
            // get a col to a vector
            Vector ret = new Vector(Rows);
            for (int i = 0; i < Rows; i++)
            {
                ret[i] = this[i, j];
            }
            return ret;
        }

        //  Function: Set Column
        //------------------------------------------------------------------------
        /// @brief Set a Col of a Matrix to a Vector object
        /// 
        /// @param u Input Vector
        /// @param j Col number to set
        //------------------------------------------------------------------------
        public void SetCol(Vector u, int j)
        {
            // set a col to a vector
            if (u.Length != Rows)
            {
                throw new ArgumentException($"Vector has length {u.Length}. Length of {Rows} required.");
            }
            else if (j >= Cols || j < 0)
            {
                throw new IndexOutOfRangeException($"Index {j} out of bounds for Matrix with {Cols} cols.");
            }
            for (int i = 0; i < Rows; i++)
            {
                this[i, j] = u[i];
            }
        }

        //  Function: Transpose
        //------------------------------------------------------------------------
        /// @brief Return the Transpose of a Matrix
        //------------------------------------------------------------------------
        public Matrix Transpose()
        {
            // return a transposed matrix
            Matrix ret = new Matrix(Cols, Rows);
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    ret[i, j] = this.values[j, i];
                }
            }
            return ret;
        }

        //  Function: Copy
        //------------------------------------------------------------------------
        /// @brief Return copy of this Matrix
        //------------------------------------------------------------------------
        public Matrix Copy()
        {
            Matrix ret = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    ret[i, j] = this.values[i, j];
                }
            }
            return ret;
        }

        //  Function: Inverse
        //------------------------------------------------------------------------
        /// @brief Returns inverse of matrix using Doolittle's LU Decomposition method
        //------------------------------------------------------------------------
        public Matrix Inverse()
        {
            int n = Rows;
            Matrix result = Copy();

            int[] perm;
            int toggle;
            Matrix lum = MatrixDecompose(result, out perm, out toggle);
            if (lum == null)
                throw new ArgumentException("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j]) b[j] = 1.0;
                    else b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b);

                for (int j = 0; j < n; j++) result[j, i] = x[j];
            }
            return result;
        }

        //  Function: Decompose
        //------------------------------------------------------------------------
        /// @brief Doolittle LUP decomposition with partial pivoting
        /// 
        /// @param matrix Matrix
        /// @param perm int array
        /// @param toggle int
        /// 
        /// @return Matrix Decomposition
        //------------------------------------------------------------------------
        private static Matrix MatrixDecompose(Matrix matrix, out int[] perm, out int toggle)
        {
            if (matrix.Rows != matrix.Cols)
                throw new ArgumentException("Attempt to decompose a non-square matrix");

            int n = matrix.Rows;

            Matrix result = matrix.Copy();

            // set up row permutation result
            perm = new int[n];
            for (int i = 0; i < n; i++)
            {
                perm[i] = i;
            }

            toggle = 1; // toggle tracks row swaps. +1 = greater-than even, -1 = greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; j++)
            {
                double colMax = Math.Abs(result[j, j]);
                int pRow = j;
                for (int i = j + 1; i < n; i++)
                {
                    if (Math.Abs(result[i, j]) > colMax)
                    {
                        colMax = Math.Abs(result[i, j]);
                        pRow = i;
                    }
                }

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    Vector tmpRow = result.GetRow(pRow);
                    result.SetRow(result.GetRow(j), pRow);
                    result.SetRow(tmpRow, j);

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                if (result[j, j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; row++)
                    {
                        if (result[row, j] != 0.0) goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new ArgumentException("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    Vector tmpRow = result.GetRow(goodRow);
                    result.SetRow(result.GetRow(j), goodRow);
                    result.SetRow(tmpRow, j);

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                for (int i = j + 1; i < n; i++)
                {
                    result[i, j] /= result[j, j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                    }
                }

            } // main j column loop

            return result;
        }

        //  Function: HelperSolve
        //------------------------------------------------------------------------
        /// @brief Helper method for inverting a matrix
        //------------------------------------------------------------------------
        private static double[] HelperSolve(Matrix luMatrix, double[] b)
        {
            // Before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Rows;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; i++)
            {
                double sum = x[i];
                for (int j = 0; j < i; j++)
                {
                    sum -= luMatrix[i, j] * x[j];
                }
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1, n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; j++)
                {
                    sum -= luMatrix[i, j] * x[j];
                }

                x[i] = sum / luMatrix[i, i];
            }

            return x;
        }

        //  Function: MatMul Vector Left
        //------------------------------------------------------------------------
        /// @brief Left multiply a Vector by a Matrix
        /// 
        /// @param m Matrix
        /// @param u Vector 
        /// 
        /// @return Product of matrix with a vector
        //------------------------------------------------------------------------
        private static Vector Mul(Matrix m, Vector u)
        {
            if (u.Length != m.Cols)
            {
                throw new ArgumentException($"Vector has length {u.Length}. Length of {m.Cols} required.");
            }
            Vector ret = new Vector(m.Rows);
            for (int i = 0; i < m.Rows; i++)
            {
                ret[i] = m.GetRow(i).Dot(u);
            }
            return ret;
        }

        //  Function: MatMul Vector Right
        //------------------------------------------------------------------------
        /// @brief Right multiply a Vector by a Matrix
        /// 
        /// @param u Vector
        /// @param m Matrix
        /// 
        /// @return Product of vector with a matrix
        //------------------------------------------------------------------------
        private static Vector Mul(Vector u, Matrix m)
        {
            if (u.Length != m.Rows)
            {
                throw new ArgumentException($"Vector has length {u.Length}. Length of {m.Rows} required.");
            }
            Vector ret = new Vector(m.Cols);
            for (int j = 0; j < m.Cols; j++)
            {
                ret[j] = m.GetCol(j).Dot(u);
            }
            return ret;
        }
        //  Function: Matrix Multiplication
        //------------------------------------------------------------------------
        /// @brief Multiplication of two Matrices
        /// 
        /// @param m Matrix
        /// @param n Matrix
        /// 
        /// @return Product matrix
        //------------------------------------------------------------------------
        private static Matrix Mul(Matrix m, Matrix n)
        {
            if (m.Cols != n.Rows)
            {
                throw new ArgumentException($"Inner dimensions {m.Cols}, {n.Rows} do not match.");
            }
            Matrix ret = new Matrix(m.Rows, n.Cols);
            for (int i = 0; i < m.Rows; i++)
            {
                Vector newrow = new Vector(n.Cols);
                for (int j = 0; j < n.Cols; j++)
                {
                    newrow[j] = m.GetRow(i).Dot(n.GetCol(j));
                }
                ret.SetRow(newrow, i);
            }
            return ret;
        }

        //  Function: Add
        //------------------------------------------------------------------------
        /// @brief Add two matrices element-wise
        /// 
        /// @param m Matrix
        /// @param n Matrix
        /// 
        /// @return The sum of two matrices
        //------------------------------------------------------------------------
        private static Matrix Add(Matrix m, Matrix n)
        {
            // element-wise addition
            if (m.Rows != n.Rows || m.Cols != n.Cols)
            {
                throw new ArgumentException($"Element-wise operations require Matrices of same size: ({m.Rows}, {m.Cols}), ({n.Rows}, {n.Cols})");
            }
            Matrix ret = new Matrix(m.Rows, m.Cols);
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Cols; j++)
                {
                    ret[i, j] = m[i, j] + n[i, j];
                }
            }
            return ret;
        }

        //  Function: Subtract
        //------------------------------------------------------------------------
        /// @brief Subtract two matrices element-wise
        /// 
        /// @param m Matrix
        /// @param n Matrix
        /// 
        /// @return The difference of two matrices
        //------------------------------------------------------------------------
        private static Matrix Sub(Matrix m, Matrix n)
        {
            // element-wise addition
            if (m.Rows != n.Rows || m.Cols != n.Cols)
            {
                throw new ArgumentException($"Element-wise operations require Matrices of same size: ({m.Rows}, {m.Cols}), ({n.Rows}, {n.Cols})");
            }
            Matrix ret = new Matrix(m.Rows, m.Cols);
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Cols; j++)
                {
                    ret[i, j] = m[i, j] - n[i, j];
                }
            }
            return ret;
        }

        //  Function: Identity
        //------------------------------------------------------------------------
        /// @brief Construct an Identity Matrix
        /// 
        /// @param n Dimensions of square Identity Matrix
        //------------------------------------------------------------------------
        public static Matrix Identity(int n)
        {
            // n x n identity matrix
            Matrix ret = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                ret[i, i] = 1;
            }
            return ret;
        }
    }
}
