using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTask
{
    public delegate void ActionRef<T>(ref T item);
    /// Тип проекции на экран
    public enum ProjectionType { ISOMETRIC, PERSPECTIVE, TRIMETRIC, DIMETRIC, PARALLEL, CAVALIER }
    public class Point2d
    {
        double x, y;
        public Point2d(int x, int y)
        {
            this.x = x;
            this.y = y;

        }
    }
    /// Точка в пространстве
    public class Point
    {
        double x, y, z;
        public static ProjectionType projection = ProjectionType.TRIMETRIC;
        public static PointF worldCenter;
        static Size screenSize;
        static double zScreenNear, zScreenFar, fov;
        static Matrix isometricMatrix = new Matrix(3,3).fill(Math.Sqrt(3),0,-Math.Sqrt(3),1,2,1, Math.Sqrt(2),-Math.Sqrt(2), Math.Sqrt(2)) * (1/ Math.Sqrt(6));
        static Matrix trimetricMatrix = new Matrix(4, 4).fill(Math.Sqrt(3)/2, Math.Sqrt(2)/4, 0, 1, 0, Math.Sqrt(2)/2, 0, 1, 0.5,-Math.Sqrt(6)/4,0,0,0,0,0,1);
        static Matrix dimetricMatrix = new Matrix(4, 4).fill(0.926, 0.134, 0, 0, 0, 0.935, 0, 0, 0.378, -0.327, 0, 0, 0, 0, 0, 1);
        static Matrix centralMatrix = new Matrix(4, 4).fill(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, k, 0, 0, 0, 1);
        static Matrix cavalierMatrix = new Matrix(4, 4).fill(1, 0, 0, 0, 0, 1, 0, 0, Math.Cos(Math.PI / 4), Math.Cos(Math.PI / 4), 0, 0, 0, 0, 0, 1);
        static Matrix parallelProjectionMatrix, perspectiveProjectionMatrix;
        const double k = 0.001f;
        public Point(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Point objAsPart = obj as Point;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public bool Equals(Point other)
        {
            if (other == null) return false;
            return (this.X.Equals(other.X)&& this.Y.Equals(other.Y)&& this.Z.Equals(other.Z));
        }
        public static void setProjection(Size screenSize, double zScreenNear, double zScreenFar, double fov)
        {
            Point.screenSize = screenSize;
            Point.zScreenNear = zScreenNear;
            Point.zScreenFar = zScreenFar;
            Point.fov = fov;
            parallelProjectionMatrix = new Matrix(4, 4).fill(1.0 / screenSize.Width, 0, 0, 0, 0, 1.0 / screenSize.Height, 0, 0, 0, 0, -2.0 / (zScreenFar - zScreenNear), -(zScreenFar + zScreenNear) / (zScreenFar - zScreenNear), 0, 0, 0, 1);
            perspectiveProjectionMatrix = new Matrix(4,4).fill(screenSize.Height/(Math.Tan(Geometry.degreesToRadians(fov / 2)) * screenSize.Width), 0, 0, 0, 0, 1.0/Math.Tan(Geometry.degreesToRadians(fov/2)), 0, 0, 0, 0, -(zScreenFar + zScreenNear) / (zScreenFar - zScreenNear), -2 * (zScreenFar * zScreenNear) / (zScreenFar - zScreenNear), 0, 0, -1, 0);
        }

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int X { get => (int)x; set => x = value; }
        public int Y { get => (int)y; set => y = value; }
        public int Z { get => (int)z; set => z = value; }

        public double Xf { get => x; set => x = value; }
        public double Yf { get => y; set => y = value; }
        public double Zf { get => z; set => z = value; }

        /// Перевод точки из 3D в 2D
        public PointF to2D(){
            switch (projection)
            {
                case ProjectionType.PERSPECTIVE:
                    {
                        Matrix res = new Matrix(1, 4).fill(Xf, Yf, Zf, 1) * centralMatrix * (1 / (k * Zf + 1));
                        return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y + (float)res[0, 1]);
                    }

                case ProjectionType.TRIMETRIC:
                    {
                        Matrix res = new Matrix(1, 4).fill(Yf, Zf, Xf, 1) * trimetricMatrix;
                        return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y - (float)res[0, 1]);
                    }

                case ProjectionType.DIMETRIC:
                    {
                        Matrix res = new Matrix(1, 4).fill(Xf, Yf, Zf, 1) * dimetricMatrix;
                        return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y + (float)res[0, 1]);
                    }
                case ProjectionType.CAVALIER:
                    {
                        Matrix res = new Matrix(1, 4).fill(Xf, Yf, Zf, 1) * cavalierMatrix;
                        return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y + (float)res[0, 1]);
                    }

                case ProjectionType.PARALLEL:
                    return new PointF(worldCenter.X + (float)Xf, worldCenter.Y + (float)Yf);
                case ProjectionType.ISOMETRIC:
                    {
                        Matrix res = new Matrix(3, 3).fill(1, 0, 0, 0, 1, 0, 0, 0, 0) * isometricMatrix * new Matrix(3, 1).fill(Xf, Yf, Zf);
                        return new PointF(worldCenter.X + (float)res[0, 0], worldCenter.Y + (float)res[1, 0]);
                    }
                default: throw new Exception("C# сломался...");
            }
        }
        public static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public Tuple<PointF?,double> to2D(Camera cam)
        {
            var viewCoord = cam.toCameraView(this);

            if (projection == ProjectionType.PARALLEL) {
                if (viewCoord.Zf > 0)
                {
                    return Tuple.Create<PointF?, double>(new PointF(worldCenter.X + (float)viewCoord.Xf, worldCenter.Y + (float)viewCoord.Yf), (float)this.Zf);
                }
                return null;
            }
            else if (projection == ProjectionType.PERSPECTIVE)
            {
                if(viewCoord.Zf < 0)
                {
                    return Tuple.Create<PointF?, double>(null, (float)this.Zf);
                }
                Matrix res = new Matrix(1, 4).fill(viewCoord.Xf, viewCoord.Yf, viewCoord.Zf, 1) * perspectiveProjectionMatrix;
                if(res[0,3] == 0)
                {
                    return Tuple.Create<PointF?, double>(null, (float)this.Zf);
                }
                res *= 1.0 / res[0, 3];
                res[0, 0] = Clamp(res[0, 0], -1, 1);
                res[0, 1] = Clamp(res[0, 1], -1, 1);
                if(res[0,2] < 0)
                {
                    return Tuple.Create<PointF?, double>(null, (float)this.Zf);
                }
                return Tuple.Create<PointF?, double>(new PointF(worldCenter.X + (float)res[0, 0] * worldCenter.X, worldCenter.Y + (float)res[0, 1] * worldCenter.Y), (float)this.Zf);
            } else
            {
                return Tuple.Create<PointF?,double>(to2D(), (float)this.Zf);
            }
        }
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

        /// Отрезок в пространстве
        public class Line
    {
        public Point start,end;

        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Point Start { get => start; set => start = value; }
        public Point End { get => end; set => end = value; }

        public Point getVectorCoordinates()
        {
            return new Point(end.Xf - start.Xf, end.Yf - start.Yf, end.Zf - start.Zf);
        }

        public Point getReverseVectorCoordinates()
        {
            return new Point(start.Xf - end.Xf, start.Yf - end.Yf, start.Zf - end.Zf);
        }
    }
    /// Грань фигуры, состоящая из конечного числа отрезков
    public class Face
    {
        List<Line> edges;
        Vector normVector;
        public List<Point> verticles;

        public Face()
        {
            edges = new List<Line>();
            verticles = new List<Point>();
            normVector = new Vector(0, 0, 0);
        }

        public Face(IEnumerable<Line> edges) : this()
        {
            this.edges.AddRange(edges);
        }

        public Face addEdge(Line edge)
        {
            edges.Add(edge);
            recalculateNormVector();
            return this;
        }
        public Face addEdges(IEnumerable<Line> edges)
        {
            this.edges.AddRange(edges);
            recalculateNormVector();
            return this;
        }
        public Face addVerticle(Point p)
        {
            verticles.Add(p);
            return this;
        }
        public Face addVerticles(IEnumerable<Point> points)
        {
            this.verticles.AddRange(points);
            return this;
        }

        public List<Point> Verticles { get => verticles; }
        void recalculateNormVector()
        {

        }

        public Vector NormVector { get {
                Vector a = new Vector(edges.First().getVectorCoordinates()), b = new Vector(edges.Last().getReverseVectorCoordinates());
                normVector = (b * a).normalize();
                return normVector;
            } }

        public List<Line> Edges { get => edges; }

        /// Получение центра тяжести грани
        public Point getCenter()
        {
            double x = 0, y = 0, z = 0;
            foreach(var line in edges)
            {
                x += line.Start.Xf;
                y += line.Start.Yf;
                z += line.Start.Zf;
            }
            return new Point(x / edges.Count, y / edges.Count, z / edges.Count);
        }
    }

    /// Объёмная фигура, состоящая из граней
    public class Shape
    {
        List<Face> faces;
        List<Point> verticles;
        public Shape()
        {
            faces = new List<Face>();
            verticles = new List<Point>();
        }

        public Shape(IEnumerable<Face> faces) : this()
        {
            this.faces.AddRange(faces);
        }

        public Shape addFace(Face face)
        {
            faces.Add(face);
            return this;
        }
        public Shape addFaces(IEnumerable<Face> faces)
        {
            this.faces.AddRange(faces);
            return this;
        }

        public List<Face> Faces { get => faces; }
        public Shape(IEnumerable<Point> verticles) : this()
        {
            this.verticles.AddRange(verticles);
        }

        public Shape addVerticle(Point verticle)
        {
            verticles.Add(verticle);
            return this;
        }
        public Shape addVerticles(IEnumerable<Point> verticles)
        {
            this.verticles.AddRange(verticles);
            return this;
        }

        public List<Point> Verticles { get => verticles; }

        /// Преобразует все точки в фигуре по заданной функции
        public void transformPoints(Func<Point, Point> f)
        {
            foreach (var face in Faces)
            {
                foreach (var line in face.Edges)
                {
                    line.start = f(line.start);
                    line.end = f(line.end);
                }
                face.verticles = face.verticles.Select(x => f(x)).ToList();
            }
        }


        /// Виртуальный метод, чтобы наследники могли возвращать какую-то инфу для сохранения в файл
        public virtual String getAdditionalInfo()
        {
            return "";
        }

        /// Виртуальный метод, чтобы наследники могли возвращать свои имена
        public virtual String getShapeName()
        {
            return "SHAPE";
        }
        // читает модель многогранника из файла
        public static Shape readShape(string fileName)
        {
            Shape res = new Shape();
            List<Line> edges = new List<Line>();
            List<Point> vertices = new List<Point>();
            List<Face> faces = new List<Face>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("g")) // Название фигуры
                    {
                        string shapeName = line.Substring(2).Trim(); // Убираем "g "
                        switch (shapeName)
                        {
                            case "Tetrahedron":
                                res = new Tetrahedron();
                                break;
                            case "Hexahedron":
                                res = new Hexahedron();
                                break;
                            case "Octahedron":
                                res = new Octahedron();
                                break;
                            case "Icosahedron":
                                res = new Icosahedron();
                                break;
                            case "Dodecahedron":
                                res = new Dodecahedron();
                                break;
                            case "Object":
                                res = new ObjectShape();
                                break;
                            default:
                                throw new Exception("Неизвестная фигура: " + shapeName);
                        }
                    }
                    else if (line.StartsWith("v")) // Вершины
                    {
                        string[] parts = line.Substring(2).Trim().Split(' ');
                        if (parts.Length == 3)
                        {
                            Point vertex = new Point(
                                int.Parse(parts[0]),
                                int.Parse(parts[1]),
                                int.Parse(parts[2])
                            );
                            vertices.Add(vertex);
                        }
                    }
                    else if (line.StartsWith("f")) // Грани
                    {
                        string[] parts = line.Substring(2).Trim().Split(' ');
                        List<Point> faceVertices = new List<Point>();
                        foreach (string part in parts)
                        {
                            if (int.TryParse(part, out int vertexIndex))
                            {
                                faceVertices.Add(vertices[vertexIndex - 1]); // Индексы в .obj начинаются с 1
                            }
                        }

                        // Генерация ребер и создание грани
                        for (int i = 0; i < faceVertices.Count; i++)
                        {
                            Point start = faceVertices[i];
                            Point end = faceVertices[(i + 1) % faceVertices.Count]; // Замыкаем ребра
                            edges.Add(new Line(start, end));
                        }

                        Face face = new Face(edges).addVerticles(faceVertices);
                        res.addFace(face);
                        edges.Clear();
                    }
                }
            }
            return res;
        }
        public static List<Point> Distinct<Point>(List<Point> l)
        {
            List<Point> uniq = new List<Point>();
            foreach (Point p in l)
            {
                if (!uniq.Contains(p))
                    uniq.Add(p);
            }
            return uniq;
        }
        

        public override string ToString()
        {
            return $"{getShapeName()} ({faces.Count})";
        }
    }

    class Tetrahedron : Shape
    {
        public override String getShapeName()
        {
            return "TETRAHEDRON";
        }
    }

    class Octahedron : Shape
    {
        public override String getShapeName()
        {
            return "OCTAHEDRON";
        }
    }

    class Hexahedron : Shape
    {
        public override String getShapeName()
        {
            return "HEXAHEDRON";
        }
    }

    class ObjectShape : Shape
    {
        public override String getShapeName()
        {
            return "OBJECT";
        }
    }

    class Icosahedron : Shape
    {
        public override String getShapeName()
        {
            return "ICOSAHEDRON";
        }
    }

    class Dodecahedron : Shape
    {
        public override String getShapeName()
        {
            return "DODECAHEDRON";
        }
    }
    

    
    class Geometry
    {
        /// Переводит угол из градусов в радианы
        public static double degreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double radiansToDegrees(double angle)
        {
            return angle * 180 / Math.PI;
        }

        /// Косинус из угла в градусах, ограниченный 5 знаками после запятой
        public static double Cos (double angle)
        {
            return Math.Cos(degreesToRadians(angle));
        }
        /// Синус из угла в градусах, ограниченный 5 знаками после запятой
        public static double Sin(double angle)
        {
            return Math.Sin(degreesToRadians(angle));
        }
        /// Перевод точки в другую точку
        ///
        public static Point transformPoint(Point p, Matrix matrix)

        {
            var matrfrompoint = new Matrix(4, 1).fill(p.X, p.Y, p.Z,1);

            var matrPoint = matrix * matrfrompoint;//применение преобразования к точке
            Point newPoint = new Point(matrPoint[0, 0], matrPoint[1, 0], matrPoint[2, 0]);
            return newPoint;

        }
        /// Перевод всех точек для тела вращения в другие точки
        public static List<Point> transformPointsRotationFig(Matrix matrix,List<Point> allpoints)
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
        /// Поворот образующей для фигуры вращения

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

    /// Класс для получения фигур различного типа
    class ShapeGetter
    {
        /// Получает фигуру фиксированного размера (в среднем до 200 пикселей по размеру)
        public static Shape getShape(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.TETRAHEDRON: return getTetrahedron();
                case ShapeType.OCTAHEDRON: return getOctahedron();
                case ShapeType.HEXAHEDRON: return getHexahedron();
                case ShapeType.ICOSAHEDRON: return getIcosahedron();
                case ShapeType.DODECAHEDRON: return getDodecahedron();


                default: throw new Exception("C# очень умный (нет)");
            }
        }

        /// Получение тетраэдра
        public static Tetrahedron getTetrahedron()
        {
            Tetrahedron res = new Tetrahedron();
            Point a = new Point(0, 0, 0);
            Point c = new Point(200, 0, 200);
            Point f = new Point(200, 200, 0);
            Point h = new Point(0, 200, 200);
            res.addFace(new Face().addEdge(new Line(a, c)).addEdge(new Line(c, f)).addEdge(new Line(f, a))); // ok
            res.addFace(new Face().addEdge(new Line(f, c)).addEdge(new Line(c, h)).addEdge(new Line(h, f))); // ok
            res.addFace(new Face().addEdge(new Line(a, h)).addEdge(new Line(h, c)).addEdge(new Line(c, a))); // ok
            res.addFace(new Face().addEdge(new Line(f, h)).addEdge(new Line(h, a)).addEdge(new Line(a, f))); // ok
            return res;
        }

        /// Получение октаэдра
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
            res.addVerticles(new List<Point> { a,b, c, d,e,f });
            res.addFace(new Face().addEdge(new Line(a, f)).addEdge(new Line(f, b)).addEdge(new Line(b, a)));
            res.addFace(new Face().addEdge(new Line(b, f)).addEdge(new Line(f, c)).addEdge(new Line(c, b))); 
            res.addFace(new Face().addEdge(new Line(c, f)).addEdge(new Line(f, d)).addEdge(new Line(d, c))); 
            res.addFace(new Face().addEdge(new Line(d, f)).addEdge(new Line(f, a)).addEdge(new Line(a, d))); 
            res.addFace(new Face().addEdge(new Line(a, b)).addEdge(new Line(b, e)).addEdge(new Line(e, a))); 
            res.addFace(new Face().addEdge(new Line(b, c)).addEdge(new Line(c, e)).addEdge(new Line(e, b))); 
            res.addFace(new Face().addEdge(new Line(c, d)).addEdge(new Line(d, e)).addEdge(new Line(e, c))); 
            res.addFace(new Face().addEdge(new Line(d, a)).addEdge(new Line(a, e)).addEdge(new Line(e, d))); 
            return res;
        }

        /// Получение гексаэдра (куба)
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
            res.addFace(new Face().addEdge(new Line(a, d)).addEdge(new Line(d, c)).addEdge(new Line(c, b)).addEdge(new Line(b, a)));
            res.addFace(new Face().addEdge(new Line(b, c)).addEdge(new Line(c, g)).addEdge(new Line(g, f)).addEdge(new Line(f, b)));
            res.addFace(new Face().addEdge(new Line(f, g)).addEdge(new Line(g, h)).addEdge(new Line(h, e)).addEdge(new Line(e, f)));
            res.addFace(new Face().addEdge(new Line(h, d)).addEdge(new Line(d, a)).addEdge(new Line(a, e)).addEdge(new Line(e, h)));
            res.addFace(new Face().addEdge(new Line(a, b)).addEdge(new Line(b, f)).addEdge(new Line(f, e)).addEdge(new Line(e, a)));
            res.addFace(new Face().addEdge(new Line(d, h)).addEdge(new Line(h, g)).addEdge(new Line(g, c)).addEdge(new Line(c, d)));
            return res;
        }

        /// Получение икосаэдра
        public static Icosahedron getIcosahedron()
        {
            Icosahedron res = new Icosahedron();
            Point circleCenter = new Point(100, 100, 100);
            List<Point> circlePoints = new List<Point>();
            for(int angle = 0; angle < 360; angle += 36)
            {
                if( angle % 72 == 0)
                {
                    circlePoints.Add(new Point(circleCenter.X + (100 * Math.Cos(Geometry.degreesToRadians(angle))), circleCenter.Y + 100, circleCenter.Z + (100 * Math.Sin(Geometry.degreesToRadians(angle)))));
                    continue;
                }
                circlePoints.Add(new Point(circleCenter.X + (100 * Math.Cos(Geometry.degreesToRadians(angle))), circleCenter.Y, circleCenter.Z + (100 * Math.Sin(Geometry.degreesToRadians(angle)))));
            }
            Point a = new Point(100, 50, 100);
            Point b = new Point(100, 250, 100);
            for(int i = 0; i < 10; i++)
            {
                res.addFace(new Face().addEdge(new Line(circlePoints[i], circlePoints[(i + 1) % 10])).addEdge(new Line(circlePoints[(i + 1) % 10], circlePoints[(i + 2) % 10])).addEdge(new Line(circlePoints[(i+2) % 10], circlePoints[i])).addVerticles(new List<Point> { circlePoints[i], circlePoints[(i + 1) % 10], circlePoints[(i + 2) % 10] }));
            }
            res.addFace(new Face().addEdge(new Line(circlePoints[1], a)).addEdge(new Line(a, circlePoints[3])).addEdge(new Line(circlePoints[3], circlePoints[1])).addVerticles(new List<Point> { circlePoints[1], a, circlePoints[3] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[3], a)).addEdge(new Line(a, circlePoints[5])).addEdge(new Line(circlePoints[5], circlePoints[3])).addVerticles(new List<Point> { circlePoints[3], a, circlePoints[5] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[5], a)).addEdge(new Line(a, circlePoints[7])).addEdge(new Line(circlePoints[7], circlePoints[5])).addVerticles(new List<Point> { circlePoints[5], a , circlePoints[7] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[7], a)).addEdge(new Line(a, circlePoints[9])).addEdge(new Line(circlePoints[9], circlePoints[7])).addVerticles(new List<Point> {  circlePoints[7],a, circlePoints[9] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[9], a)).addEdge(new Line(a, circlePoints[1])).addEdge(new Line(circlePoints[1], circlePoints[9])).addVerticles(new List<Point> { circlePoints[9], a, circlePoints[1] }));

            res.addFace(new Face().addEdge(new Line(circlePoints[0], b)).addEdge(new Line(b, circlePoints[2])).addEdge(new Line(circlePoints[2], circlePoints[0])).addVerticles(new List<Point> { circlePoints[0], b, circlePoints[2] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[2], b)).addEdge(new Line(b, circlePoints[4])).addEdge(new Line(circlePoints[4], circlePoints[2])).addVerticles(new List<Point> { circlePoints[2], b, circlePoints[4] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[4], b)).addEdge(new Line(b, circlePoints[6])).addEdge(new Line(circlePoints[6], circlePoints[4])).addVerticles(new List<Point> { circlePoints[4], b, circlePoints[6] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[6], b)).addEdge(new Line(b, circlePoints[8])).addEdge(new Line(circlePoints[8], circlePoints[6])).addVerticles(new List<Point> { circlePoints[6], b, circlePoints[8] }));
            res.addFace(new Face().addEdge(new Line(circlePoints[8], b)).addEdge(new Line(b, circlePoints[0])).addEdge(new Line(circlePoints[0], circlePoints[8])).addVerticles(new List<Point> { circlePoints[8], b, circlePoints[0] }));
            return res;
        }

        /// Получение додекаэдра
        public static Dodecahedron getDodecahedron()
        {
            Dodecahedron res = new Dodecahedron();
            var icosahedron = getIcosahedron();
            List<Point> centers = new List<Point>();
            for (int i = 0; i < icosahedron.Faces.Count; i++)
            {
                Face face = icosahedron.Faces[i];
                var c = face.getCenter();
                centers.Add(c);
            }

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    res.addFace(new Face().addEdge(new Line(centers[i], centers[(i + 1) % 10])).addEdge(new Line(centers[(i + 1) % 10], centers[(i + 2) % 10])).addEdge(new Line(centers[(i + 2) % 10], centers[15 + (i / 2 + 1) % 5])).addEdge(new Line(centers[15 + (i / 2 + 1) % 5], centers[15 + i / 2])).addEdge(new Line(centers[15 + i/ 2], centers[i])).addVerticles(new List<Point> { centers[i], centers[(i + 1) % 10], centers[(i + 2) % 10], centers[15 + (i / 2 + 1) % 5], centers[15 + i / 2] }));

                    continue;
                }
                res.addFace(new Face().addEdge(new Line(centers[i], centers[(i + 1) % 10])).addEdge(new Line(centers[(i + 1) % 10], centers[(i + 2) % 10])).addEdge(new Line(centers[(i + 2) % 10], centers[10 + (i / 2 + 1) % 5])).addEdge(new Line(centers[10 + (i / 2 + 1) % 5], centers[10 + i / 2])).addEdge(new Line(centers[10 + i / 2], centers[i]))).addVerticles(new List<Point> { centers[i], centers[(i + 1) % 10], centers[(i + 2) % 10], centers[10 + (i / 2 + 1) % 5], centers[10 + i / 2]});
            }
            res.addFace(new Face().addEdge(new Line(centers[15], centers[16])).addEdge(new Line(centers[16], centers[17])).addEdge(new Line(centers[17], centers[18])).addEdge(new Line(centers[18], centers[19])).addEdge(new Line(centers[19], centers[15])).addVerticles(new List<Point> { centers[15], centers[16] , centers[17], centers[18] , centers[19]}));
            res.addFace(new Face().addEdge(new Line(centers[10], centers[11])).addEdge(new Line(centers[11], centers[12])).addEdge(new Line(centers[12], centers[13])).addEdge(new Line(centers[13], centers[14])).addEdge(new Line(centers[14], centers[10])).addVerticles(new List<Point> { centers[10], centers[11], centers[12], centers[13], centers[14] }));

            return res;
        }

    }

    public class Vector
    {
        public double x, y, z;
        public Vector(double x, double y, double z, bool isVectorNeededToBeNormalized = false)
        {
            double normalization = isVectorNeededToBeNormalized ? Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)) : 1;
            this.x = x / normalization;
            this.y = y / normalization;
            this.z = z / normalization;
        }

        public Vector(Point p, bool isVectorNeededToBeNormalized = false) : this(p.Xf,p.Yf,p.Zf,isVectorNeededToBeNormalized)
        {

        }

        public Vector normalize()
        {
            double normalization = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            x = x / normalization;
            y = y / normalization;
            z = z / normalization;
            return this;
        }

        public int X { get => (int)x; set => x = value; }
        public int Y { get => (int)y; set => y = value; }
        public int Z { get => (int)z; set => z = value; }

        public double Xf { get => x; set => x = value; }
        public double Yf { get => y; set => y = value; }
        public double Zf { get => z; set => z = value; }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public static Vector operator *(double k, Vector b)
        {
            return new Vector(k*b.x,k*b.y,k*b.z);
        }
    }

}
