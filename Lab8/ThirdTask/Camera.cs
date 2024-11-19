using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdTask
{
    class Camera
    {
        double x, y, z;
        double l, m, n;
        Matrix centralMatrix;

        public Camera(double x, double y, double z, double l, double m, double n)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            double length = Math.Sqrt(l*l + m*m + n*n);
            this.l = l / length;
            this.m = m / length;
            this.n = n / length;
            double d = Math.Sqrt(M*M + N*N);
            centralMatrix = new Matrix(4, 4).fill(1.0 / Math.Tan(ShapeGetter.degreesToRadians(50)), 0, 0, 0,
                                                  0, 1.0 / Math.Tan(ShapeGetter.degreesToRadians(50)), 0, 0,
                                                  0, 0, 2, 1,
                                                  0, 0, -10, 0);

           /* centralMatrix *= new Matrix(4, 4).fill(-1, 0, 0, 0,
                                                  0, -1, 0, 0,
                                                  0, 0, 1, 0,
                                                  0, 0, 0, 1);*/

            centralMatrix *= new Matrix(4, 4).fill(d, 0, -L, 0,
                                                   0, 1, 0, 0,
                                                   L, 0, d, 0,
                                                   0, 0, 0, 1);

            centralMatrix *= new Matrix(4, 4).fill(1, 0, 0, 0,
                                                   0, N/d, -M/d, 0,
                                                   0, M/d, N/d, 0,
                                                   0, 0, 0, 1);

            centralMatrix *= new Matrix(4, 4).fill(1, 0, 0, -X,
                                                  0, 1, 0, -Y,
                                                  0, 0, 1, -Z,
                                                  0, 0, 0, 1);
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        public double L { get => l; set => l = value; }

        public double M { get => m; set => m = value; }

        public double N { get => n; set => n = value; }

        public Matrix cM { get => centralMatrix;}
    }
}
