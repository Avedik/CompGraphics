using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTask
{
    public partial class Form1
    {
        Polyhedron currentShape;
        Graphics g;

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
            Div = int.Parse(getDiv.Text);
            currentShape = ShapeGetter.getRotationShape(RotationShapePoints, Div, AxisforRotate);

            redraw();
            setFlags(true);
        }

        void Clear()
        {
            setFlags(false);
            g.Clear(Color.White);
            RotationShapePoints.Clear();
        }

        // Рисует фигуры, выделяя цветом некоторые грани у додекаэдра и икосаэра
        void drawShape(Polyhedron shape)
        {
            foreach (var face in shape.Faces)
            {
                Pen pen = new Pen(Color.Black, 3);
                drawFace(face,pen);
            }
        }

        // Рисует заданную границу грани заданным цветом
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
            g.DrawLine(pen, prev.to2D(), point.to2D());
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
            g.DrawString($" X", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Green), axisX.Item2.to2D().X, pictureBox1.Height - axisX.Item2.to2D().Y);
            g.DrawString($" Y", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Blue), axisY.Item2.to2D().X, pictureBox1.Height - axisY.Item2.to2D().Y);
            g.DrawString($" Z", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Red), axisZ.Item2.to2D().X, pictureBox1.Height - axisZ.Item2.to2D().Y);
           
            g.ScaleTransform(1.0F, -1.0F);
            g.TranslateTransform(0.0F, -(float)pictureBox1.Height);
        }

        // Перерисовывает всю сцену
        void redraw()
        {
            g.Clear(Color.White);
            drawShape(currentShape);

            foreach (Point p in RotationShapePoints)
                drawPoint(p);

            if (checkBox1.Checked)
                drawAxis();
        }

        void drawPoint(Point p)
        {
            PointF pNew = p.to2D();
            g.DrawEllipse(Pens.Black, pNew.X, pNew.Y, 2, 2);
        }
    }
}
