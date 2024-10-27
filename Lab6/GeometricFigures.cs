using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    // Точка в пространстве
    class Point
    {
        double x, y, z;

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
    }

    // Ребро (отрезок) в пространстве
    class Edge
    {
        public Point start, end;

        public Edge(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Point Start { get => start; set => start = value; }
        public Point End { get => end; set => end = value; }
    }

    // Многоугольник (грань фигуры)
    class Polygon
    {
        List<Edge> edges;

        public Polygon()
        {
            edges = new List<Edge>();
        }

        public Polygon(IEnumerable<Edge> edges) : this()
        {
            this.edges.AddRange(edges);
        }

        public List<Edge> Edges { get => edges; }
    }

    // Многогранник
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

        public List<Polygon> Faces { get => faces; }
    }

}
