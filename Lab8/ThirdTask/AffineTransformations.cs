using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdTask
{
    // Тип координатной прямой (для поворотов)
    public enum AxisType { X, Y, Z };
    public enum PlaneType { XY, YZ, XZ };

    public partial class Form1
    {
        public AxisType currentAxis;
        public PlaneType currentPlane;
        public AxisType currentMirrorAxis;
        public AxisType currentRollAxis;

        private void buttonScale_Click(object sender, EventArgs e)
        {
            if (rbWorldCenter.Checked == true)
            {
                scale(ref currentShape, double.Parse(textScaleX.Text), double.Parse(textScaleY.Text), double.Parse(textScaleZ.Text));
            }
            else
            if (rbCenter.Checked == true)
            {
                scaleCenter(ref currentShape, double.Parse(textScaleX.Text), double.Parse(textScaleY.Text), double.Parse(textScaleZ.Text));
            }
            redraw();

        }
        private void buttonRotate_Click(object sender, EventArgs e)
        {
            rotate(ref currentShape, currentAxis, int.Parse(textAngle.Text));
            redraw();
        }

        private void buttonShift_Click(object sender, EventArgs e)
        {
            shift(ref currentShape, int.Parse(textShiftX.Text), int.Parse(textShiftY.Text), int.Parse(textShiftZ.Text));
            redraw();
        }

        private void buttonReflection_Click(object sender, EventArgs e)
        {
            reflect(ref currentShape, currentPlane);
            redraw();
        }

        // Сдвинуть фигуру на заданные расстояния
        void shift(ref Polyhedron shape, double dx, double dy, double dz)
        {
            Matrix shift = new Matrix(4, 4).fill(1, 0, 0, dx, 0, 1, 0, dy, 0, 0, 1, dz, 0, 0, 0, 1);
            shape.transformPoints((ref Point p) =>
            {
                var res = shift * new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
                p = new Point(res[0, 0], res[1, 0], res[2, 0]);
            });
        }

        // Растянуть фигуру на заданные коэффициенты
        void scale(ref Polyhedron shape, double cx, double cy, double cz)
        {
            Matrix scale = new Matrix(4, 4).fill(cx, 0, 0, 0, 0, cy, 0, 0, 0, 0, cz, 0, 0, 0, 0, 1);
            shape.transformPoints((ref Point p) =>
            {
                var res = scale * new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
                p = new Point(res[0, 0], res[1, 0], res[2, 0]);
            });
        }

        // Повернуть фигуру на заданный угол вокруг заданной оси
        void rotate(ref Polyhedron shape, AxisType type, int angle)
        {
            Matrix rotation = new Matrix(0, 0);
            switch (type)
            {
                case AxisType.X:
                    rotation = new Matrix(4, 4).fill(1, 0, 0, 0, 0, Math.Cos(ShapeGetter.degreesToRadians(angle)), Math.Sin(ShapeGetter.degreesToRadians(angle)), 0, 0, -Math.Sin(ShapeGetter.degreesToRadians(angle)), Math.Cos(ShapeGetter.degreesToRadians(angle)), 0, 0, 0, 0, 1);
                    break;
                case AxisType.Y:
                    rotation = new Matrix(4, 4).fill(Math.Cos(ShapeGetter.degreesToRadians(angle)), 0, Math.Sin(ShapeGetter.degreesToRadians(angle)), 0, 0, 1, 0, 0, -Math.Sin(ShapeGetter.degreesToRadians(angle)), 0, Math.Cos(ShapeGetter.degreesToRadians(angle)), 0, 0, 0, 0, 1);
                    break;
                case AxisType.Z:
                    rotation = new Matrix(4, 4).fill(Math.Cos(ShapeGetter.degreesToRadians(angle)), -Math.Sin(ShapeGetter.degreesToRadians(angle)), 0, 0, Math.Sin(ShapeGetter.degreesToRadians(angle)), Math.Cos(ShapeGetter.degreesToRadians(angle)), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                    break;
            }

            shape.transformPoints((ref Point p) =>
            {
                var res = rotation * new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
                p = new Point(res[0, 0], res[1, 0], res[2, 0]);
            });
        }

        void rotate_around_line(ref Polyhedron shape, int angle, Point p1, Point p2)
        {
            //матрица
            Matrix rotation = new Matrix(0, 0);
            if (p2.Z < p1.Z || (p2.Z == p1.Z && p2.Y < p1.Y) ||
               (p2.Z == p1.Z && p2.Y == p1.Y) && p2.X < p1.X)
            {
                Point tmp = p1;
                p1 = p2;
                p2 = tmp;
            }

            Point vector = new Point(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);//прямая, вокруг которой будем вращать
            double length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            double l = vector.X / length;
            double m = vector.Y / length;
            double n = vector.Z / length;
            double anglesin = Math.Sin(ShapeGetter.degreesToRadians(angle));
            double anglecos = Math.Cos(ShapeGetter.degreesToRadians(angle));
            rotation = new Matrix(4, 4).fill(l * l + anglecos * (1 - l * l), l * (1 - anglecos) * m - n * anglesin, l * (1 - anglecos) * n + m * anglesin, 0,
                                 l * (1 - anglecos) * m + n * anglesin, m * m + anglecos * (1 - m * m), m * (1 - anglecos) * n - l * anglesin, 0,
                                 l * (1 - anglecos) * n - m * anglesin, m * (1 - anglecos) * n + l * anglesin, n * n + anglecos * (1 - n * n), 0,
                                 0, 0, 0, 1);

            shape.transformPoints((ref Point p) =>
            {
                var res = rotation * new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
                p = new Point(res[0, 0], res[1, 0], res[2, 0]);
            });
        }

        // Отразить фигуру относительно заданной координатной плоскости
        void reflect(ref Polyhedron shape, PlaneType type)
        {
            Matrix rotation = new Matrix(0, 0);
            switch (type)
            {
                case PlaneType.XY:
                    rotation = new Matrix(4, 4).fill(1, 0, 0, 0,  0, 1, 0, 0,  0, 0, -1, 0,  0, 0, 0, 1);
                    break;
                case PlaneType.YZ:
                    rotation = new Matrix(4, 4).fill(-1, 0, 0, 0,  0, 1, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1);
                    break;
                case PlaneType.XZ:
                    rotation = new Matrix(4, 4).fill(1, 0, 0, 0,  0, -1, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1);
                    break;
            }

            shape.transformPoints((ref Point p) =>
            {
                var res = new Matrix(1, 4).fill(p.X, p.Y, p.Z, 1) * rotation;
                p = new Point(res[0, 0], res[0, 1], res[0, 2]);
            });
        }

        // Растянуть фигуру на заданные коэффициенты относительно своего центра
        void scaleCenter(ref Polyhedron shape, double cx, double cy, double cz)
        {
            Point Center = shape.getCenter();
            Matrix scaleCenter = new Matrix(4, 4).fill(cx, 0, 0, 0,  0, cy, 0, 0,  0, 0, cz, 0,  (1-cx)* Center.X, (1-cy)* Center.Y, (1-cz)* Center.Z, 1);
            shape.transformPoints((ref Point p) =>
            {
                var res = new Matrix(1, 4).fill(p.X, p.Y, p.Z, 1) * scaleCenter;
                p = new Point(res[0, 0], res[0, 1], res[0, 2]);
            });
        }

        // Вращение многогранника вокруг прямой проходящей через центр многогранника, параллельно выбранной координатной оси
        void rotationThroughTheCenter(ref Polyhedron shape, AxisType axis, int angle)
        {
            double sumX = 0, sumY = 0, sumZ = 0;
            foreach (var face in shape.Faces)
            {
                sumX += face.getCenter().X;
                sumY += face.getCenter().Y;
                sumZ += face.getCenter().Z;
            }

            // центр фигуры
            Point center = new Point(sumX / shape.Faces.Count(), sumY / shape.Faces.Count(), sumZ / shape.Faces.Count());

            Matrix rotationMatrix;
            // переносим в начало координат
            rotationMatrix = new Matrix(4, 4).fill(1, 0, 0, -center.X, 0, 1, 0, -center.Y, 0, 0, 1, -center.Z, 0, 0, 0, 1);
            shape.transformPoints((ref Point p) =>
            {
                var res = rotationMatrix * new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
                p = new Point(res[0, 0], res[1, 0], res[2, 0]);
            });

            // поворачиваем относительно оси
            rotate(ref shape, axis, angle);

            // возвращаем на исходное место
            rotationMatrix = new Matrix(4, 4).fill(1, 0, 0, center.X, 0, 1, 0, center.Y, 0, 0, 1, center.Z, 0, 0, 0, 1);
            shape.transformPoints((ref Point p) =>
            {
                var res = rotationMatrix * new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
                p = new Point(res[0, 0], res[1, 0], res[2, 0]);
            });

        }
    }
}
