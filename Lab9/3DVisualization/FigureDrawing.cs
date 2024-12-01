using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _3DVisualization
{
    // Тип объёмной фигуры
    public enum ShapeType { TETRAHEDRON, HEXAHEDRON, OCTAHEDRON, ICOSAHEDRON, DODECAHEDRON }

    public partial class Form1
    {
        public delegate Color VertexColor(int x, int y, float intensity);

        VertexColor vertexColor;
        ShapeType currentShapeType;
        Polyhedron currentShape;
        Graphics g;
        Camera c;
        Vector3 viewVector;
        Vector3 lightDirection;
        bool viewVectorSelected = false;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            if (checkBox1.Checked)
                drawAxis();

            foreach (Point p in RotationShapePoints)
                drawPoint(p);

            if (currentShape != null)
                drawShape(currentShape);
        }

        private void buttonShape_Click(object sender, EventArgs e)
        {
            if ((textCamX.Text != "") && (textCamY.Text != "") && (textCamZ.Text != "") && (textXView.Text != "") && (textYView.Text != "") && (textZView.Text != ""))
            {
                c = new Camera(double.Parse(textCamX.Text), double.Parse(textCamY.Text), double.Parse(textCamZ.Text),
                double.Parse(textXView.Text), double.Parse(textYView.Text), double.Parse(textZView.Text));
                sphereLength = Math.Sqrt(double.Parse(textCamX.Text) * double.Parse(textCamX.Text)
                    + double.Parse(textCamY.Text) * double.Parse(textCamY.Text)
                    + double.Parse(textCamZ.Text) * double.Parse(textCamZ.Text));
            }

            if (RotationShapePoints.Count() > 0)
            {
                Div = int.Parse(getDiv.Text);
                currentShape = ShapeGetter.getRotationShape(RotationShapePoints, Div, AxisforRotate);
            }
            else
                currentShape = ShapeGetter.getShape(currentShapeType);

            redraw();
            setFlags(true);
        }

        // Рисует коодинатные прямые (с подписями)
        void drawAxis()
        {
            (Point, Point) axisX = (new Point(0, 0, 0), new Point(300, 0, 0));
            (Point, Point) axisY = (new Point(0, 0, 0), new Point(0, 300, 0));
            (Point, Point) axisZ = (new Point(0, 0, 0), new Point(0, 0, 300));
            drawLine(axisX.Item1, axisX.Item2, new Pen(Color.Green, 4));
            drawLine(axisY.Item1, axisY.Item2, new Pen(Color.Blue, 4));
            drawLine(axisZ.Item1, axisZ.Item2, new Pen(Color.Red, 4));

            g.ScaleTransform(1.0F, -1.0F);
            g.TranslateTransform(0.0F, -(float)pictureBox1.Height);
            g.DrawString($" X", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Green), axisX.Item2.to2D(c).X, pictureBox1.Height - axisX.Item2.to2D(c).Y);
            g.DrawString($" Y", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Blue), axisY.Item2.to2D(c).X, pictureBox1.Height - axisY.Item2.to2D(c).Y);
            g.DrawString($" Z", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Red), axisZ.Item2.to2D(c).X, pictureBox1.Height - axisZ.Item2.to2D(c).Y);

            g.ScaleTransform(1.0F, -1.0F);
            g.TranslateTransform(0.0F, -(float)pictureBox1.Height);
        }

        // Перерисовывает всю сцену
        void redraw()
        {
            g.Clear(Color.White);

            if (currentShape != null)
                drawShape(currentShape);

            foreach (Point p in RotationShapePoints)
                drawPoint(p);

            if (checkBox1.Checked)
                drawAxis();
        }

        void Clear()
        {
            setFlags(false);
            g.Clear(Color.White);
            RotationShapePoints.Clear();
            currentShape = null;
            viewVectorSelected = false;
        }

        // Рисует фигуру
        void drawShape(Polyhedron shape)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            foreach (var face in shape.Faces)
            {
                if (viewVectorSelected && !shape.faceIsVisible(face, viewVector))
                    continue;

                //Pen pen = new Pen(Color.Black, 3);
                //drawFace(face, pen);

                using (var fastBitmap = new FastBitmap.FastBitmap(bitmap))
                {
                    rasterizePolygon(shape, face, fastBitmap);
                }
            }
            pictureBox1.Image = bitmap;
        }

        // Рисует заданную грань заданным цветом
        void drawFace(Polygon face, Pen pen)
        {
            Point prev = face.Points.First();
            foreach (var point in face.Points)
            {
                drawLine(prev, point, pen);
                prev = point;
            }
            drawLine(face.Points.Last(), face.Points.First(), pen);
        }

        // Рисует линию, переводя её координаты из 3D в 2D
        void drawLine(Point prev, Point point, Pen pen)
        {
            g.DrawLine(pen, prev.to2D(c), point.to2D(c));
        }

        void drawPoint(Point p)
        {
            PointF pNew = p.to2D();
            g.DrawEllipse(Pens.Black, pNew.X, pNew.Y, 2, 2);
        }

        void rasterizePolygon(Polyhedron shape, Polygon face, FastBitmap.FastBitmap fastBitmap)
        {
            List<(Point2D, Point2D)> edges2D = new List<(Point2D, Point2D)>();

            // Получаем рёбра
            Point prev = face.Points.First();
            Point current;
            int pointCount = face.Points.Count();

            for (int i = 1; i < pointCount; i++)
            {
                current = face.Points[i];
                edges2D.Add((new Point2D(prev.to2D(), shape.getIntensity(prev)),
                    new Point2D(current.to2D(), shape.getIntensity(current))));
                prev = current;
            }
            edges2D.Add((new Point2D(prev.to2D(), shape.getIntensity(prev)),
                    new Point2D(face.Points.First().to2D(), shape.getIntensity(face.Points.First()))));

            // Первый шаг алгоритма растеризации (со списком рёберных точек)
            Dictionary<int, List<(int, float)>> segments = new Dictionary<int, List<(int, float)>>();
            foreach ((Point2D, Point2D) edge in edges2D)
            {
                Point2D start, end;

                if ((int)Math.Ceiling(edge.Item1.Position.Y) == (int)Math.Ceiling(edge.Item2.Position.Y))
                {
                    int horizontalLineY = (int)Math.Ceiling(edge.Item1.Position.Y);
                    if (!segments.ContainsKey(horizontalLineY))
                        segments.Add(horizontalLineY, new List<(int, float)>());

                    segments[horizontalLineY].Add(((int)edge.Item1.Position.X, edge.Item1.Intensity));
                    segments[horizontalLineY].Add(((int)edge.Item2.Position.X, edge.Item2.Intensity));
                    continue;
                }
                else if (edge.Item1.Position.Y > edge.Item2.Position.Y)
                {
                    start = edge.Item2;
                    end = edge.Item1;
                }
                else
                {
                    start = edge.Item1;
                    end = edge.Item2;
                }

                int y = (int)Math.Ceiling(start.Position.Y);
                float dx = (end.Position.X - start.Position.X) / (end.Position.Y - start.Position.Y);
                float x = start.Position.X + dx * (y - start.Position.Y);

                while (y <= end.Position.Y)
                {
                    if (!segments.ContainsKey(y))
                        segments.Add(y, new List<(int, float)>());
                    segments[y].Add(((int)x, linearInterpolation(start.Intensity, end.Intensity,
                        end.Position.X == start.Position.X ? 1f * (end.Position.Y - y) / (end.Position.Y - start.Position.Y)
                        : 1f * (end.Position.X - x) / (end.Position.X - start.Position.X))));

                    y++;
                    x += dx;
                }
            }

            // Второй шаг алгоритма растеризации — сортировка по возрастанию X
            foreach (var pair in segments)
            {
                pair.Value.Sort();
            }

            // Третий шаг алгоритма растеризации
            foreach (var pair in segments)
            {
                for (int i = 0; i < pair.Value.Count() - 1; i += 2)
                {
                    int x1 = pair.Value[i].Item1;
                    int x2 = pair.Value[i + 1].Item1;

                    for (int x = x1; x < x2; x++)
                    {
                        fastBitmap[bitmap.Width - x, bitmap.Height - pair.Key] = vertexColor(x, pair.Key,
                            x2 == x1 ? pair.Value[i].Item2 :
                            linearInterpolation(pair.Value[i].Item2,
                                pair.Value[i + 1].Item2,
                                1f * (x2 - x) / (x2 - x1)));
                    }
                }
            }
        }

        private Color getDefaultColor(int x, int y, float intensity)
        {
            return colorDialog1.Color;
        }

        private float calculateVertexIntensity(Vector3 normal)
        {
            float dotProduct = Vector3.Dot(normal, lightDirection);
            return Math.Max(0, dotProduct); // Убедимся, что интенсивность не отрицательная
        }

        private Color calculateLambertColor(int x, int y, float intensity)
        {
            return Color.FromArgb((int)(colorDialog1.Color.R * intensity),
                (int)(colorDialog1.Color.G * intensity),
                (int)(colorDialog1.Color.B * intensity));
        }

        public float linearInterpolation(float I1, float I2, float t)
        {
            return t * I1 + (1 - t) * I2;
        }

    }
}
