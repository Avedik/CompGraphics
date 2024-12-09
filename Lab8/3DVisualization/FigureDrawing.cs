﻿using System;
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
        ShapeType currentShapeType;
        Polyhedron currentShape;
        Graphics g;
        Camera c;
        Vector3 viewVector;
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
            if (isInteractiveMode)
            {
                setFlags(false);
                g.Clear(Color.White);
            }
            else
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
            foreach (var face in shape.Faces)
            {
                if (viewVectorSelected && !shape.faceIsVisible(face, viewVector))
                    continue;

                Pen pen = new Pen(Color.Black, 3);
                drawFace(face, pen);
            }
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
    }
}
