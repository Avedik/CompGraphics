﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Security.Cryptography;

namespace ThirdTask
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

    // Многоугольник (грань фигуры)
    class Polygon
    {
        List<Point> points;

        public Polygon()
        {
            points = new List<Point>();
        }

        public Polygon addEdge(Point point)
        {
            points.Add(point);
            return this;
        }

        public List<Point> Points { get => points; }

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
                face.transformPoints(f);
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

    class Surface: Polyhedron
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
    }
}