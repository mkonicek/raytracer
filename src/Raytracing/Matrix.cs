using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    /// <summary>
    /// 4x4 matrix - the 4th coordinate is used for translation.
    /// </summary>
    [Serializable]
    public class Matrix
    {
        // TODO: because of XmlSerializer not supporting [,] arrays
        public double[] data;

        private static Matrix identity;
        public static Matrix Identity
        {
            get { return identity; }
        }

        private static Matrix rotateX90;
        public static Matrix RotateX90
        {
            get { return rotateX90; }
        }
        private static Matrix rotateY90;
        public static Matrix RotateY90
        {
            get { return rotateY90; }
        }
        private static Matrix rotateZ90;
        public static Matrix RotateZ90
        {
            get { return rotateZ90; }
        }
        private static Matrix zero;
        public static Matrix Zero
        {
            get { return zero; }
        }

        static Matrix()
        {
            identity = new Matrix();
            identity[0, 0] = identity[1, 1] = identity[2, 2] = identity[3, 3] = 1;

            rotateX90 = new Matrix();
            rotateX90[0, 0] = 1;
            rotateX90[1, 2] = -1;
            rotateX90[2, 1] = 1;

            rotateY90 = new Matrix();
            rotateY90[1, 1] = 1;
            rotateY90[0, 2] = 1;
            rotateY90[2, 0] = -1;

            rotateZ90 = new Matrix();
            rotateZ90[2, 2] = 1;
            rotateZ90[0, 1] = -1;
            rotateZ90[1, 0] = 1;

            zero = new Matrix();
        }

        public Matrix()
        {
            this.data = new double[16];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this[i, j] = 0;
                }
            }
            this[3, 3] = 1;
        }

        public Matrix Clone()
        {
            Matrix copy = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    copy[i, j] = this[i, j];
                }
            }
            return copy;
        }

        /// <summary>
        /// Translates copy of given matrix.
        /// </summary>
        public Matrix Translate(double x, double y, double z)
        {
            Matrix copy = this.Clone();           
            copy[0, 3] += x;
            copy[1, 3] += y;
            copy[2, 3] += z;

            return this;
        }

        /// <summary>
        /// Multiplies vector by matrix including translation.
        /// </summary>
        public static Vector operator * (Matrix m, Vector v)
        {
            Vector result = new Vector();
            // calc result vector coordinates
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i] += m[i, j] * v[j];
                }
                // add translation, we suppose that 4th coord of the vector is always 1
                result[i] += m[i, 3];
            }
            return result;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix result = new Matrix();
            // calc result vector coordinates
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        result[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            // translation
            for (int i = 0; i < 3; i++)
            {
                result[i, 3] = m1[i, 3] + m2[i, 3];
            }
            return result;
        }

        /// <summary>
        /// Gets matrix scaling by given factor.
        /// </summary>
        /// <param name="scale">Scaling factor.</param>
        public static Matrix Scaling(double scale)
        {
            Matrix result = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                result[i, i] = scale;
            }
            return result;
        }

        public double this[int i, int j]
        {
            get
            {
                return this.data[i * 4 + j];
            }
            set
            {
                this.data[i * 4 + j] = value;
            }
        }
    }
}
