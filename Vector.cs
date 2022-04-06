using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpCodeSamples
{
    // Class: Vector
    //------------------------------------------------------------------------
    /// @brief Basic vector class for representing n-dimensional state
    //------------------------------------------------------------------------
    public class Vector : IEquatable<Vector>
    {
        //  Member: values
        //------------------------------------------------------------------------
        /// @brief Values of vector array
        //------------------------------------------------------------------------
        private double[] values;

        //  Property: Length
        //------------------------------------------------------------------------
        /// @brief Length of vector array
        //------------------------------------------------------------------------
        public int Length { get; private set; }


        //  Constructor: Vector
        //------------------------------------------------------------------------
        /// @brief Simple Vector class
        /// 
        /// @param len Length of zero-vector to initialize
        //------------------------------------------------------------------------
        public Vector(int len)
        {
            this.Length = len;
            this.values = new double[len];
        }

        //  Constructor: Vector
        //------------------------------------------------------------------------
        /// @brief Vector class
        /// 
        /// @param vals List of values with which to initialize a Vector
        //------------------------------------------------------------------------
        public Vector(List<double> vals)
        {
            this.Length = vals.Count;
            this.values = vals.ToArray();
        }

        //  Constructor: Vector
        //------------------------------------------------------------------------
        /// @brief Vector class
        /// 
        /// @param vals Values with which to initialize a Vector; Length implied
        //------------------------------------------------------------------------
        public Vector(params double[] vals)
        {
            this.Length = vals.Length;
            this.values = vals;
        }

        //  Constructor: Vector
        //------------------------------------------------------------------------
        /// @brief Vector class
        /// 
        /// @param vals Values with which to initialize a Vector; Length implied
        //------------------------------------------------------------------------
        public Vector(Vector v)
        {
            this.Length = v.Length;
            this.values = new double[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                this.values[i] = v[i];
            }
        }

        //  Operator: Indexing
        //------------------------------------------------------------------------
        /// @brief Operator overload for Vector element access
        /// 
        /// @param i Index of element
        //------------------------------------------------------------------------
        public double this[int i]
        {
            get
            {
                if (i >= this.Length || i < 0)
                {
                    throw new IndexOutOfRangeException($"Index {i} out of bounds for Vector with length {this.Length}.");
                }
                return this.values[i];
            }
            set
            {
                if (i >= this.Length || i < 0)
                {
                    throw new IndexOutOfRangeException($"Index {i} out of bounds for Vector with length {this.Length}.");
                }
                this.values[i] = value;
            }
        }

        //  Function: Norm
        //------------------------------------------------------------------------
        /// @brief Gives the L2 norm for this Vector
        //------------------------------------------------------------------------
        public double Norm()
        {
            return Math.Sqrt(this.Dot(this));
        }

        //  Function: Dot
        //------------------------------------------------------------------------
        /// @brief Dot product of this Vector and another
        /// 
        /// @param v The vector to perform dot product with
        /// 
        /// @return The dot product of this vector and another v
        //------------------------------------------------------------------------
        public double Dot(in Vector v)
        {
            if (this.Length != v.Length)
            {
                throw new ArgumentException($"Dot product with incompatible vector lengths {this.Length} and {v.Length}.");
            }
            double sum = 0;
            for (int i = 0; i < this.Length; i++)
            {
                sum += this[i] * v[i];
            }
            return sum;
        }

        //  Operator: + vector
        //------------------------------------------------------------------------
        /// @brief Operator overload for Vector addition
        /// 
        /// @param u The vector to subtract from
        /// @param v The vector to subtract
        //------------------------------------------------------------------------
        public static Vector operator +(in Vector u, in Vector v)
        {
            if (u.Length != v.Length)
            {
                throw new ArgumentException($"Subtraction of incompatible vector lengths {u.Length} and {v.Length}.");
            }
            Vector ret = new Vector(u.Length);
            for (int i = 0; i < u.Length; i++)
            {
                ret[i] = u[i] + v[i];
            }
            return ret;
        }

        //  Operator: - vector
        //------------------------------------------------------------------------
        /// @brief Operator overload for Vector subtraction
        /// 
        /// @param u The vector to subtract from
        /// @param v The vector to subtract
        //------------------------------------------------------------------------
        public static Vector operator -(in Vector u, in Vector v)
        {
            if (u.Length != v.Length)
            {
                throw new ArgumentException($"Subtraction of incompatible vector lengths {u.Length} and {v.Length}.");
            }
            Vector ret = new Vector(u.Length);
            for (int i = 0; i < u.Length; i++)
            {
                ret[i] = u[i] - v[i];
            }
            return ret;
        }

        //  Function: Equals (object)
        //------------------------------------------------------------------------
        /// @brief Equate a Vector object with a generic object
        /// 
        /// @param obj object to compare to
        //------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            Vector u = obj as Vector;
            if (u == null) { return false; }
            return Equals(u);
        }

        //  Function: Equals (Vector)
        //------------------------------------------------------------------------
        /// @brief Equate a Vector object with another Vector
        /// 
        /// @param u Vector to compare to
        //------------------------------------------------------------------------
        public bool Equals(Vector u)
        {
            // checks equality of contents rather than equality of objects
            if (this.Length != u.Length) return false;
            for (int i = 0; i < this.Length; i++)
            {
                if (Math.Abs(this[i] - u[i]) > 1e-6) return false;
            }
            return true;
        }

        //  Function: ToString
        //------------------------------------------------------------------------
        /// @brief Return string representation of Vector
        //------------------------------------------------------------------------
        public override string ToString()
        {
            string ret = "[ ";
            for (int i = 0; i < Length - 1; i++)
            {
                ret += $"{this[i]}, ";
            }
            ret += $"{this[Length - 1]} ]";
            return ret;
        }
    }
}
