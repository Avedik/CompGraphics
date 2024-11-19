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
            double d = Math.Sqrt(m*m + n*n);
            centralMatrix = new Matrix(4, 4).fill(1, 0, 0, 0,
                                                  0, 1, 0, 0,
                                                  0, 0, 1, 0,
                                                  -x,-y, -z, 1);

            centralMatrix *= new Matrix(4, 4).fill(1, 0, 0, 0,
                                                   0, n/d, m/d, 0,
                                                   0, -m/d, n/d, 0,
                                                   0, 0, 0, 1);
            double df = (m*m + n*n) / d;
            double q  = Math.Sqrt(2) / Math.Sqrt(3);
            double r  = - 1  / Math.Sqrt(3);
            centralMatrix *= new Matrix(4, 4).fill(q, 0, r, 0,
                                                   0, 1, 0, 0,
                                                   -r, 0, q, 0,
                                                   0, 0, 0, 1) * 
                            new Matrix(4, 4).fill(-1, 0, 0, 0,
                                                  0, -1, 0, 0,
                                                  0, 0, 1, 0,
                                                  0, 0, 0, 1) *
                            new Matrix(4, 4).fill(1.0 / Math.Tan(ShapeGetter.degreesToRadians(50)), 0, 0, 0,
                                                  0, 1.0 / Math.Tan(ShapeGetter.degreesToRadians(50)), 0, 0,
                                                  0, 0, 2, 1,
                                                  0, 0, -40, 0);

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
