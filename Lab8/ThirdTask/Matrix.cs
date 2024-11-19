using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdTask
{
    class Matrix
    {
        double[,] matr;
        int rowCount;
        int colCount;

        public Matrix(int rows, int cols)
        {
            rowCount = rows;
            colCount = cols;
            matr = new double[rows, cols];
        }

        public double this[int x, int y]
        {
            get
            {
                return matr[x, y];
            }
            set
            {
                matr[x, y] = value;
            }
        }

        // Заполнение матрицы слева направо, сверху вниз
        public Matrix fill(params double[] elems)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    matr[i, j] = elems[i * colCount + j];
                }
            }
            return this;
        }

        // Заполняет матрицу для преобразований в 2D
        public Matrix fillFor2D(params double[] elems)
        {
            return fill(elems[0], elems[1], 0, elems[2], elems[3], 0, elems[4], elems[5], 1);
        }

        // Умножение матрицы на число
        public static Matrix operator *(Matrix matr, double value)
        {
            var res = new Matrix(matr.rowCount, matr.colCount);
            for (int i = 0; i < matr.rowCount; i++)
            {
                for (int j = 0; j < matr.colCount; j++)
                {
                    res[i, j] = matr[i, j] * value;
                }
            }
            return res;
        }

        // Умножение матриц
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.colCount != matrix2.rowCount)
            {
                throw new Exception("Size incorrect");
            }

            var res = new Matrix(matrix1.rowCount, matrix2.colCount);
            for (int i = 0; i < res.rowCount; i++)
            {
                for (int j = 0; j < res.colCount; j++)
                {
                    for (var k = 0; k < matrix1.colCount; k++)
                    {
                        res[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
            return res;
        }
    }
}
