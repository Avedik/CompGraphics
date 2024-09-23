using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThirdTask
{
    public partial class Form1 : Form
    {
        private PointF[] triangleVertices;

        public Form1()
        {
            this.Width = 400;
            this.Height = 400;
            this.Paint += new PaintEventHandler(DrawGradientTriangle);

            // Задаем вершины треугольника с разными цветами
            triangleVertices = new PointF[]
            {
            new PointF(100, 100), // Вершина 1
            new PointF(300, 100), // Вершина 2
            new PointF(200, 300)  // Вершина 3
            };
        }

        private void DrawGradientTriangle(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Цвета вершин
            Color color1 = Color.Red;    //Color.FromArgb(0, 255, 0);
            Color color2 = Color.Green;   //Color.FromArgb(0, 155, 0);
            Color color3 = Color.Blue;    //Color.FromArgb(0, 0, 0);

            // Используем алгоритм растеризации треугольника
            FillTriangle(g, triangleVertices[0], triangleVertices[1], triangleVertices[2], color1, color2, color3);
        }

        private void FillTriangle(Graphics g, PointF p1, PointF p2, PointF p3, Color c1, Color c2, Color c3)
        {
            // Находим границы треугольника
            int minX = (int)Math.Min(Math.Min(p1.X, p2.X), p3.X);
            int minY = (int)Math.Min(Math.Min(p1.Y, p2.Y), p3.Y);
            int maxX = (int)Math.Max(Math.Max(p1.X, p2.X), p3.X);
            int maxY = (int)Math.Max(Math.Max(p1.Y, p2.Y), p3.Y);

            // Проходим по всем пикселям в границах треугольника
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (IsInsideTriangle(new PointF(x, y), p1, p2, p3))
                    {
                        Color interpolatedColor = InterpolateColor(x, y, p1, p2, p3, c1, c2, c3);
                        g.FillRectangle(new SolidBrush(interpolatedColor), x, y, 1, 1);
                    }
                }
            }
        }

        private bool IsInsideTriangle(PointF p, PointF p1, PointF p2, PointF p3)
        {
            float area = 0.5f * (-p2.Y * p3.X + p1.Y * (-p2.X + p3.X) + p1.X * (p2.Y - p3.Y) + p2.X * p3.Y);
            float s = 1f / (2f * area) * (p1.Y * p3.X - p1.X * p3.Y + (p3.Y - p1.Y) * p.X + (p1.X - p3.X) * p.Y);
            float t = 1f / (2f * area) * (p1.X * p2.Y - p1.Y * p2.X + (p1.Y - p2.Y) * p.X + (p2.X - p1.X) * p.Y);
            return s >= 0 && t >= 0 && (s + t) <= 1;
        }

        private Color InterpolateColor(int x, int y, PointF p1, PointF p2, PointF p3, Color c1, Color c2, Color c3)
        {
            // Вычисляем площади
            float totalArea = TriangleArea(p1, p2, p3);
            float area1 = TriangleArea(p2, p3, new PointF(x, y));
            float area2 = TriangleArea(p1, p3, new PointF(x, y));
            float area3 = TriangleArea(p1, p2, new PointF(x, y));

            // Нормализуем площади
            float r = (area1 * c1.R + area2 * c2.R + area3 * c3.R) / totalArea;
            float g = (area1 * c1.G + area2 * c2.G + area3 * c3.G) / totalArea;
            float b = (area1 * c1.B + area2 * c2.B + area3 * c3.B) / totalArea;

            return Color.FromArgb(Clamp((int)(r)), Clamp((int)(g)), Clamp((int)(b)));
        }

        private float TriangleArea(PointF p1, PointF p2, PointF p3)
        {
            return 0.5f * Math.Abs(p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y));
        }

        private int Clamp(int value)
        {
            if (value < 0) return 0;
            if (value > 255) return 255;
            return value;
        }
    }
}