using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    // Тип проекции на экран
    public enum ProjectionType { ISOMETRIC, PERSPECTIVE }

    // Точка в пространстве
    class Point
    {
        double x, y, z;
        public static ProjectionType projection = ProjectionType.ISOMETRIC;
        public static PointF sceneCenter;

        static Matrix isometricMatrix = new Matrix(3, 3).fill(Math.Sqrt(3), 0, -Math.Sqrt(3), 1, 2, 1, Math.Sqrt(2), -Math.Sqrt(2), Math.Sqrt(2)) * (1 / Math.Sqrt(6));
        static Matrix centralMatrix = new Matrix(4, 4).fill(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, k, 0, 0, 0, 1);
        const double k = 0.001f;

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        // Перевод точки из 3D в 2D
        public PointF to2D()
        {
            if (projection == ProjectionType.PERSPECTIVE)
            {
                Matrix res = new Matrix(1, 4).fill(X, Y, Z, 1) * centralMatrix * (1 / (k * Z + 1));
                return new PointF(sceneCenter.X + (float)res[0, 0], sceneCenter.Y + (float)res[0, 1]);
            }
            else
            {
                Matrix res = new Matrix(3, 3).fill(1, 0, 0, 0, 1, 0, 0, 0, 0) * isometricMatrix * new Matrix(3, 1).fill(X, Y, Z);
                return new PointF(sceneCenter.X + (float)res[0, 0], sceneCenter.Y + (float)res[1, 0]);
            }
        }
    }

    // Отрезок в пространстве
    class Line
    {
        public Point start, end;

        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Point Start { get => start; set => start = value; }
        public Point End { get => end; set => end = value; }
    }

    // Многогранник (грань фигуры, состоящая из конечного числа отрезков)
    class Polygon
    {
        List<Line> edges;

        public Polygon()
        {
            edges = new List<Line>();
        }

        public Polygon(IEnumerable<Line> edges) : this()
        {
            this.edges.AddRange(edges);
        }

        public Polygon addEdge(Line edge)
        {
            edges.Add(edge);
            return this;
        }
        public Polygon addEdges(IEnumerable<Line> edges)
        {
            this.edges.AddRange(edges);
            return this;
        }

        public List<Line> Edges { get => edges; }

        // Получение центра тяжести грани
        public Point getCenter()
        {
            double x = 0, y = 0, z = 0;
            foreach (var line in edges)
            {
                x += line.Start.X;
                y += line.Start.Y;
                z += line.Start.Z;
            }
            return new Point(x / edges.Count, y / edges.Count, z / edges.Count);
        }
    }

    delegate void ActionRef<T>(ref T item);

    // Объёмная фигура, состоящая из граней
    class Polyhedron
    {
        // Грани
        List<Polygon> faces;

        public Polyhedron()
        {
            faces = new List<Polygon>();
        }

        public Polyhedron(IEnumerable<Polygon> faces) : this()
        {
            this.faces.AddRange(faces);
        }

        public Polyhedron addFace(Polygon face)
        {
            faces.Add(face);
            return this;
        }
        public Polyhedron addFaces(IEnumerable<Polygon> faces)
        {
            this.faces.AddRange(faces);
            return this;
        }

        public List<Polygon> Faces { get => faces; }

        // Преобразует все точки в фигуре по заданной функции
        public void transformPoints(ActionRef<Point> f)
        {
            foreach (var face in Faces)
            {
                foreach (var line in face.Edges)
                {
                    f(ref line.start);
                    f(ref line.end);
                }
            }
        }

        // Получение центра тяжести многогранника
        public Point getCenter()
        {
            double x = 0, y = 0, z = 0;
            foreach (var face in faces)
            {
                x += face.getCenter().X;
                y += face.getCenter().Y;
                z += face.getCenter().Z;
            }
            return new Point(x / faces.Count, y / faces.Count, z / faces.Count);
        }
    }

    class Tetrahedron : Polyhedron
    {

    }

    class Octahedron : Polyhedron
    {

    }

    class Hexahedron : Polyhedron
    {

    }

    class Icosahedron : Polyhedron
    {

    }

    class Dodecahedron : Polyhedron
    {

    }


    // Класс для получения фигур различного типа
    class ShapeGetter
    {
        // Получает фигуру фиксированного размера
        public static Polyhedron getShape(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.TETRAHEDRON: return getTetrahedron();
                case ShapeType.OCTAHEDRON: return getOctahedron();
                case ShapeType.HEXAHEDRON: return getHexahedron();
                case ShapeType.ICOSAHEDRON: return getIcosahedron();
                case ShapeType.DODECAHEDRON: return getDodecahedron();

                default: throw new Exception();
            }
        }

        // Получение тетраэдра
        public static Tetrahedron getTetrahedron()
        {
            Tetrahedron res = new Tetrahedron();
            Point a = new Point(0, 0, 0);
            Point c = new Point(200, 0, 200);
            Point f = new Point(200, 200, 0);
            Point h = new Point(0, 200, 200);
            res.addFace(new Polygon().addEdge(new Line(a, f)).addEdge(new Line(f, c)).addEdge(new Line(c, a)));
            res.addFace(new Polygon().addEdge(new Line(f, c)).addEdge(new Line(c, h)).addEdge(new Line(h, f)));
            res.addFace(new Polygon().addEdge(new Line(c, h)).addEdge(new Line(h, a)).addEdge(new Line(a, c)));
            res.addFace(new Polygon().addEdge(new Line(f, h)).addEdge(new Line(h, a)).addEdge(new Line(a, f)));
            return res;
        }

        // Получение октаэдра
        public static Octahedron getOctahedron()
        {
            Octahedron res = new Octahedron();
            var cube = getHexahedron();
            Point a = cube.Faces[0].getCenter();
            Point b = cube.Faces[1].getCenter();
            Point c = cube.Faces[2].getCenter();
            Point d = cube.Faces[3].getCenter();
            Point e = cube.Faces[4].getCenter();
            Point f = cube.Faces[5].getCenter();

            res.addFace(new Polygon().addEdge(new Line(a, f)).addEdge(new Line(f, b)).addEdge(new Line(b, a)));
            res.addFace(new Polygon().addEdge(new Line(b, c)).addEdge(new Line(c, f)).addEdge(new Line(f, b)));
            res.addFace(new Polygon().addEdge(new Line(c, d)).addEdge(new Line(d, f)).addEdge(new Line(f, c)));
            res.addFace(new Polygon().addEdge(new Line(d, a)).addEdge(new Line(a, f)).addEdge(new Line(f, d)));
            res.addFace(new Polygon().addEdge(new Line(a, e)).addEdge(new Line(e, b)).addEdge(new Line(b, a)));
            res.addFace(new Polygon().addEdge(new Line(b, e)).addEdge(new Line(e, c)).addEdge(new Line(c, b)));
            res.addFace(new Polygon().addEdge(new Line(c, e)).addEdge(new Line(e, d)).addEdge(new Line(d, c)));
            res.addFace(new Polygon().addEdge(new Line(d, e)).addEdge(new Line(e, a)).addEdge(new Line(a, d)));
            return res;
        }

        // Получение гексаэдра (куба)
        public static Hexahedron getHexahedron()
        {
            Hexahedron res = new Hexahedron();
            Point a = new Point(0, 0, 0);
            Point b = new Point(200, 0, 0);
            Point c = new Point(200, 0, 200);
            Point d = new Point(0, 0, 200);
            Point e = new Point(0, 200, 0);
            Point f = new Point(200, 200, 0);
            Point g = new Point(200, 200, 200);
            Point h = new Point(0, 200, 200);
            res.addFace(new Polygon().addEdge(new Line(a, b)).addEdge(new Line(b, c)).addEdge(new Line(c, d)).addEdge(new Line(d, a)));
            res.addFace(new Polygon().addEdge(new Line(b, c)).addEdge(new Line(c, g)).addEdge(new Line(g, f)).addEdge(new Line(f, b)));
            res.addFace(new Polygon().addEdge(new Line(f, g)).addEdge(new Line(g, h)).addEdge(new Line(h, e)).addEdge(new Line(e, f)));
            res.addFace(new Polygon().addEdge(new Line(h, e)).addEdge(new Line(e, a)).addEdge(new Line(a, d)).addEdge(new Line(d, h)));
            res.addFace(new Polygon().addEdge(new Line(a, b)).addEdge(new Line(b, f)).addEdge(new Line(f, e)).addEdge(new Line(e, a)));
            res.addFace(new Polygon().addEdge(new Line(d, c)).addEdge(new Line(c, g)).addEdge(new Line(g, h)).addEdge(new Line(h, d)));
            return res;
        }

        // Переводит угол из градусов в радианы
        public static double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        // Получение икосаэдра
        public static Icosahedron getIcosahedron()
        {
            Icosahedron res = new Icosahedron();
            Point circleCenter = new Point(100, 100, 100);
            List<Point> circlePoints = new List<Point>();
            for (int angle = 0; angle < 360; angle += 36)
            {
                if (angle % 72 == 0)
                {
                    circlePoints.Add(new Point(circleCenter.X + (100 * Math.Cos(degreesToRadians(angle))), circleCenter.Y + 100, circleCenter.Z + (100 * Math.Sin(degreesToRadians(angle)))));
                    continue;
                }
                circlePoints.Add(new Point(circleCenter.X + (100 * Math.Cos(degreesToRadians(angle))), circleCenter.Y, circleCenter.Z + (100 * Math.Sin(degreesToRadians(angle)))));
            }
            Point a = new Point(100, 50, 100);
            Point b = new Point(100, 250, 100);
            for (int i = 0; i < 10; i++)
            {
                res.addFace(new Polygon().addEdge(new Line(circlePoints[i], circlePoints[(i + 1) % 10])).addEdge(new Line(circlePoints[(i + 1) % 10], circlePoints[(i + 2) % 10])).addEdge(new Line(circlePoints[(i + 2) % 10], circlePoints[i])));
            }
            res.addFace(new Polygon().addEdge(new Line(circlePoints[1], a)).addEdge(new Line(a, circlePoints[3])).addEdge(new Line(circlePoints[3], circlePoints[1])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[3], a)).addEdge(new Line(a, circlePoints[5])).addEdge(new Line(circlePoints[5], circlePoints[3])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[5], a)).addEdge(new Line(a, circlePoints[7])).addEdge(new Line(circlePoints[7], circlePoints[5])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[7], a)).addEdge(new Line(a, circlePoints[9])).addEdge(new Line(circlePoints[9], circlePoints[7])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[9], a)).addEdge(new Line(a, circlePoints[1])).addEdge(new Line(circlePoints[1], circlePoints[9])));

            res.addFace(new Polygon().addEdge(new Line(circlePoints[0], b)).addEdge(new Line(b, circlePoints[2])).addEdge(new Line(circlePoints[2], circlePoints[0])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[2], b)).addEdge(new Line(b, circlePoints[4])).addEdge(new Line(circlePoints[4], circlePoints[2])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[4], b)).addEdge(new Line(b, circlePoints[6])).addEdge(new Line(circlePoints[6], circlePoints[4])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[6], b)).addEdge(new Line(b, circlePoints[8])).addEdge(new Line(circlePoints[8], circlePoints[6])));
            res.addFace(new Polygon().addEdge(new Line(circlePoints[8], b)).addEdge(new Line(b, circlePoints[0])).addEdge(new Line(circlePoints[0], circlePoints[8])));
            return res;
        }

        // Получение додекаэдра
        public static Dodecahedron getDodecahedron()
        {
            Dodecahedron res = new Dodecahedron();
            var icosahedron = getIcosahedron();
            List<Point> centers = new List<Point>();
            for (int i = 0; i < icosahedron.Faces.Count; i++)
            {
                Polygon face = icosahedron.Faces[i];
                var c = face.getCenter();
                centers.Add(c);
            }

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    res.addFace(new Polygon().addEdge(new Line(centers[i], centers[(i + 1) % 10])).addEdge(new Line(centers[(i + 1) % 10], centers[(i + 2) % 10])).addEdge(new Line(centers[(i + 2) % 10], centers[15 + (i / 2 + 1) % 5])).addEdge(new Line(centers[15 + (i / 2 + 1) % 5], centers[15 + i / 2])).addEdge(new Line(centers[15 + i / 2], centers[i])));

                    continue;
                }
                res.addFace(new Polygon().addEdge(new Line(centers[i], centers[(i + 1) % 10])).addEdge(new Line(centers[(i + 1) % 10], centers[(i + 2) % 10])).addEdge(new Line(centers[(i + 2) % 10], centers[10 + (i / 2 + 1) % 5])).addEdge(new Line(centers[10 + (i / 2 + 1) % 5], centers[10 + i / 2])).addEdge(new Line(centers[10 + i / 2], centers[i])));
            }
            res.addFace(new Polygon().addEdge(new Line(centers[15], centers[16])).addEdge(new Line(centers[16], centers[17])).addEdge(new Line(centers[17], centers[18])).addEdge(new Line(centers[18], centers[19])).addEdge(new Line(centers[19], centers[15])));
            res.addFace(new Polygon().addEdge(new Line(centers[10], centers[11])).addEdge(new Line(centers[11], centers[12])).addEdge(new Line(centers[12], centers[13])).addEdge(new Line(centers[13], centers[14])).addEdge(new Line(centers[14], centers[10])));

            return res;
        }
    }

}
