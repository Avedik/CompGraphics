using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Numerics;

namespace _3DVisualization
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
        static Matrix centralMatrix = new Matrix(4, 4).fill(1, 0, 0, 0,
                                                            0, 1, 0, 0, 
                                                            0, 0, 0, k, 
                                                            0, 0, 0, 1);

        public static Vector3 operator -(Point p1, Point p2)
        {
            return new Vector3((float)(p1.X - p2.X), (float)(p1.Y - p2.Y), (float)(p1.Z - p2.Z));
        }

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

        public Vector3 toVector3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }

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

        public PointF to2D(Camera c)
        {
            if (projection == ProjectionType.PERSPECTIVE)
            {
                Matrix res = new Matrix(4, 1).fill(X, Y, Z, 1);
                if (c == null)
                    res = centralMatrix * res * (1 / (k * Z + 1));
                else
                    res = c.cM * res;

                return new PointF(sceneCenter.X + (float)res[0, 0], sceneCenter.Y + (float)res[1, 0]);
            }
            else
            {
                Matrix res = new Matrix(3, 3).fill(1, 0, 0, 0, 1, 0, 0, 0, 0) * isometricMatrix * new Matrix(3, 1).fill(X, Y, Z);
                return new PointF(sceneCenter.X + (float)res[0, 0], sceneCenter.Y + (float)res[1, 0]);
            }
        }
    }

    class Point2D
    {
        PointF position;
        float intensity;

        public Point2D(PointF position, float intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }

        public PointF Position { get => position; set => position = value; }
        public float Intensity { get => intensity; set => intensity = value; }
    }

    // Многоугольник (грань фигуры)
    class Polygon
    {
        List<Point> points;
        Vector3 normal;

        public Polygon()
        {
            points = new List<Point>();
        }

        public Polygon addEdge(Point point)
        {
            points.Add(point);
            return this;
        }

        // Возвращает единичный вектор нормали к грани
        public Vector3 getNormal()
        {
            Vector3 vectorA = points[0] - points[1];
            Vector3 vectorB = points[2] - points[1];
            return Vector3.Normalize(Vector3.Cross(vectorA, vectorB));
        }

        public List<Point> Points { get => points; }
        public Vector3 Normal { get => normal; set => normal = value; }

        // Получение центра тяжести грани
        public Point getCenter()
        {
            double x = 0, y = 0, z = 0;
            foreach (var point in points)
            {
                x += point.X;
                y += point.Y;
                z += point.Z;
            }
            return new Point(x / points.Count, y / points.Count, z / points.Count);
        }

        public void transformPoints(ActionRef<Point> f)
        {
            Point point;
            int pointCount = points.Count();

            for (int i = 0; i < pointCount; ++i)
            {
                point = points[i];
                f(ref point);
                points[i] = point;
            }
        }
    }

    delegate void ActionRef<T>(ref T item);

    public class Vertex
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Vector3 Normal { get; set; }

        public float Intensity { get; set; }

        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }

    // Объёмная фигура, состоящая из граней
    class Polyhedron
    {
        HashSet<Vertex> vertices;
        // Грани
        List<Polygon> faces;

        public Polyhedron()
        {
            Vertices = new HashSet<Vertex>();
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
        public HashSet<Vertex> Vertices { get => vertices; set => vertices = value; }

        // Преобразует все точки в фигуре по заданной функции
        public void transformPoints(ActionRef<Point> f)
        {
            foreach (var face in Faces)
            {
                face.transformPoints(f);
            }
        }

        public Vector3 getFaceNormal(Polygon face)
        {
            Vector3 normal = face.getNormal();
            Vector3 checkVector = getCenter() - face.Points.First();

            // Если вектор нормали направлен внутрь фигуры, то инвертируем его
            if (Vector3.Dot(normal, checkVector) < 0)
                normal *= -1;

            return normal;
        }

        public bool faceIsVisible(Polygon face, Vector3 viewVector)
        {
            Vector3 normal = getFaceNormal(face);
            return Vector3.Dot(normal, viewVector) > 0;
        }

        public void calculateFaceNormals()
        {
            foreach (Polygon face in faces)
                face.Normal = getFaceNormal(face);
        }

        public void calculateVertexNormals()
        {
            Dictionary<Point, Vector3> normals = new Dictionary<Point, Vector3>();

            foreach (Polygon face in faces)
                foreach (Point point in face.Points)
                    if (normals.ContainsKey(point))
                        normals[point] += face.Normal;
                    else
                        normals.Add(point, face.Normal);

            foreach (var pair in normals)
            {
                Vertices.Add(new Vertex(pair.Key.toVector3(), Vector3.Normalize(pair.Value)));
            }
        }

        public float getIntensity(Point p)
        {
            Vector3 point = p.toVector3();

            foreach (Vertex vertex in vertices)
                if (vertex.Position.Equals(point))
                    return vertex.Intensity;

            return 0;
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

    class Surface: Polyhedron
    {

    }

    class RotationShape : Polyhedron
    {
        List<Point> allpoints;

        public RotationShape()
        {
            allpoints = new List<Point>();
        }

        public RotationShape addPoint(Point p)
        {
            allpoints.Add(p);
            return this;
        }

        public RotationShape addPoints(IEnumerable<Point> points)
        {
            this.allpoints.AddRange(points);
            return this;
        }

        public List<Point> Points { get => allpoints; }
    }

    class Geometry
    {
        // Переводит угол из градусов в радианы
        public static double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        // Перевод точки в другую точку
        public static Point transformPoint(Point p, Matrix matrix)

        {
            var matrfrompoint = new Matrix(4, 1).fill(p.X, p.Y, p.Z, 1);
            var matrPoint = matrix * matrfrompoint;//применение преобразования к точке
            
            Point newPoint = new Point(matrPoint[0, 0], matrPoint[1, 0], matrPoint[2, 0]);
            return newPoint;

        }

        // Перевод всех точек для тела вращения в другие точки
        public static List<Point> transformPointsRotationFig(Matrix matrix, List<Point> allpoints)
        {
            List<Point> clone = allpoints;
            List<Point> res = new List<Point>();
            foreach (var p in clone)
            {
                Point newp = transformPoint(p, matrix);
                res.Add(newp);
            }
            return res;
        }

        // Поворот образующей для фигуры вращения
        public static List<Point> RotatePoint(List<Point> general, AxisType axis, double angle)
        {
            List<Point> res;
            double mysin = Math.Sin(Geometry.degreesToRadians(angle));
            double mycos = Math.Cos(Geometry.degreesToRadians(angle));
            Matrix rotation = new Matrix(0, 0);
            switch (axis)
            {
                case AxisType.X:
                    rotation = new Matrix(4, 4).fill(1, 0, 0, 0, 0, mycos, -mysin, 0, 0, mysin, mycos, 0, 0, 0, 0, 1);
                    break;
                case AxisType.Y:
                    rotation = new Matrix(4, 4).fill(mycos, 0, mysin, 0, 0, 1, 0, 0, -mysin, 0, mycos, 0, 0, 0, 0, 1);
                    break;
                case AxisType.Z:
                    rotation = new Matrix(4, 4).fill(mycos, -mysin, 0, 0, mysin, mycos, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
                    break;
            }
            res = Geometry.transformPointsRotationFig(rotation, general);
            return res;
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
        // Получение поверхности
        public static Surface getSurface(double xStart, double yStart, double xEnd, double yEnd, int s, double kX, double kY, double c)
        {
            Surface res = new Surface();
            double dx = (xEnd - xStart) / s;
            double dy = (yEnd - yStart) / s;

            int count = s + 1;

            List<List<Point>> points = new List<List<Point>>();

            double x = xStart;
            double y = yStart;
            for (int i = 0; i < count; ++i)
            {
                y = yStart;
                points.Add(new List<Point>());
                for (int j = 0; j < count; ++j)
                {
                    points[i].Add(new Point(x, y, kX*x + kY*y + c));
                    y += dy;
                }
                x += dx;
            }

            for (int i = 0; i < count - 1; ++i)
                for (int j = 0; j < count - 1; ++j)
                    res.addFace(new Polygon().addEdge(points[i][j]).addEdge(points[i][j + 1]).addEdge(points[i + 1][j + 1]).addEdge(points[i + 1][j]));
            return res;
        }

        // Переводит угол из градусов в радианы
        public static double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double radiansToDegrees(double angle)
        {
            return 180.0 * angle / Math.PI;
        }

        // Получение фигуры вращения
        public static RotationShape getRotationShape(List<Point> general, int divisions, AxisType axis)
        {
            RotationShape res = new RotationShape();
            List<Point> genline = general;
            int GeneralCount = genline.Count();
            int Count = divisions;
            double angle = (360.0 / Count); //угол 

            res.addPoints(genline); //добавили образующую
            for (int i = 1; i < divisions; i++) //количество разбиений
            {
                res.addPoints(Geometry.RotatePoint(genline, axis, angle * i));
            }

            //Фигура вращения задаётся тремя параметрами: образующей(набор точек), осью вращения и количеством разбиений
            //зададим ребра и грани
            for (int i = 0; i < divisions; i++)
            {
                for (int j = 0; j < GeneralCount; j++)
                {
                    int index = i * GeneralCount + j; //индекс точки
                    int e = (index + GeneralCount) % res.Points.Count;

                    if ((index + 1) % GeneralCount != 0)
                    {
                        int e1 = (index + 1 + GeneralCount) % res.Points.Count;
                        res.addFace(new Polygon().addEdge(res.Points[index]).addEdge(res.Points[index + 1]).addEdge(res.Points[e1]).addEdge(res.Points[e]));
                    }
                }

            }

            return res;
        }

        static int length = 150;

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
            Point c = new Point(length, 0, length);
            Point f = new Point(length, length, 0);
            Point h = new Point(0, length, length);
            res.addFace(new Polygon().addEdge(a).addEdge(f).addEdge(c));
            res.addFace(new Polygon().addEdge(f).addEdge(c).addEdge(h));
            res.addFace(new Polygon().addEdge(c).addEdge(h).addEdge(a));
            res.addFace(new Polygon().addEdge(f).addEdge(h).addEdge(a));
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

            res.addFace(new Polygon().addEdge(a).addEdge(f).addEdge(b));
            res.addFace(new Polygon().addEdge(b).addEdge(c).addEdge(f));
            res.addFace(new Polygon().addEdge(c).addEdge(d).addEdge(f));
            res.addFace(new Polygon().addEdge(d).addEdge(a).addEdge(f));
            res.addFace(new Polygon().addEdge(a).addEdge(e).addEdge(b));
            res.addFace(new Polygon().addEdge(b).addEdge(e).addEdge(c));
            res.addFace(new Polygon().addEdge(c).addEdge(e).addEdge(d));
            res.addFace(new Polygon().addEdge(d).addEdge(e).addEdge(a));
            return res;
        }

        // Получение гексаэдра (куба)
        public static Hexahedron getHexahedron()
        {
            Hexahedron res = new Hexahedron();
            Point a = new Point(0, 0, 0);
            Point b = new Point(length, 0, 0);
            Point c = new Point(length, 0, length);
            Point d = new Point(0, 0, length);
            Point e = new Point(0, length, 0);
            Point f = new Point(length, length, 0);
            Point g = new Point(length, length, length);
            Point h = new Point(0, length, length);

            res.addFace(new Polygon().addEdge(a).addEdge(b).addEdge(c).addEdge(d));
            res.addFace(new Polygon().addEdge(b).addEdge(c).addEdge(g).addEdge(f));
            res.addFace(new Polygon().addEdge(f).addEdge(g).addEdge(h).addEdge(e));
            res.addFace(new Polygon().addEdge(h).addEdge(e).addEdge(a).addEdge(d));
            res.addFace(new Polygon().addEdge(a).addEdge(b).addEdge(f).addEdge(e));
            res.addFace(new Polygon().addEdge(d).addEdge(c).addEdge(g).addEdge(h));
            return res;
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
                res.addFace(new Polygon().addEdge(circlePoints[i]).addEdge(circlePoints[(i + 1) % 10]).addEdge(circlePoints[(i + 2) % 10]));
            }

            res.addFace(new Polygon().addEdge(circlePoints[1]).addEdge(a).addEdge(circlePoints[3]));
            res.addFace(new Polygon().addEdge(circlePoints[3]).addEdge(a).addEdge(circlePoints[5]));
            res.addFace(new Polygon().addEdge(circlePoints[5]).addEdge(a).addEdge(circlePoints[7]));
            res.addFace(new Polygon().addEdge(circlePoints[7]).addEdge(a).addEdge(circlePoints[9]));
            res.addFace(new Polygon().addEdge(circlePoints[9]).addEdge(a).addEdge(circlePoints[1]));

            res.addFace(new Polygon().addEdge(circlePoints[0]).addEdge(b).addEdge(circlePoints[2]));
            res.addFace(new Polygon().addEdge(circlePoints[2]).addEdge(b).addEdge(circlePoints[4]));
            res.addFace(new Polygon().addEdge(circlePoints[4]).addEdge(b).addEdge(circlePoints[6]));
            res.addFace(new Polygon().addEdge(circlePoints[6]).addEdge(b).addEdge(circlePoints[8]));
            res.addFace(new Polygon().addEdge(circlePoints[8]).addEdge(b).addEdge(circlePoints[0]));
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
                    res.addFace(new Polygon().addEdge(centers[i]).addEdge(centers[(i + 1) % 10]).addEdge(centers[(i + 2) % 10]).addEdge(centers[15 + (i / 2 + 1) % 5]).addEdge(centers[15 + i / 2]));

                    continue;
                }
                res.addFace(new Polygon().addEdge(centers[i]).addEdge(centers[(i + 1) % 10]).addEdge(centers[(i + 2) % 10]).addEdge(centers[10 + (i / 2 + 1) % 5]).addEdge(centers[10 + i / 2]));
            }
            res.addFace(new Polygon().addEdge(centers[15]).addEdge(centers[16]).addEdge(centers[17]).addEdge(centers[18]).addEdge(centers[19]));
            res.addFace(new Polygon().addEdge(centers[10]).addEdge(centers[11]).addEdge(centers[12]).addEdge(centers[13]).addEdge(centers[14]));

            return res;
        }
    }
}
