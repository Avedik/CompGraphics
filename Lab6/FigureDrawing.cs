using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    // Тип объёмной фигуры
    public enum ShapeType { TETRAHEDRON, HEXAHEDRON, OCTAHEDRON, ICOSAHEDRON, DODECAHEDRON }

    public partial class Form1
    {
        ShapeType currentShapeType;
        Polyhedron currentShape;
        Graphics g;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            if (checkBox1.Checked)
            {
                drawAxis();
            }

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
                currentShape = ShapeGetter.getShape(currentShapeType);
                redraw();
                setFlags(true);
            }
            
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
            foreach(var line in face.Edges)
            {
                drawLine(line, pen);
            }
        }

        // Рисует линию, переводя её координаты из 3D в 2D
        void drawLine(Line line, Pen pen)
        {
            g.DrawLine(pen, line.Start.to2D(), line.End.to2D());
        }

        // Рисует коодинатные прямые (с подписями)
        void drawAxis()
        {
            Line axisX = new Line(new Point(0, 0, 0), new Point(300, 0, 0));
            Line axisY = new Line(new Point(0, 0, 0), new Point(0, 300, 0));
            Line axisZ = new Line(new Point(0, 0, 0), new Point(0, 0, 300));
            drawLine(axisX, new Pen(Color.Green, 4));
            drawLine(axisY, new Pen(Color.Blue, 4));
            drawLine(axisZ, new Pen(Color.Red, 4));

            g.ScaleTransform(1.0F, -1.0F);
            g.TranslateTransform(0.0F, -(float)pictureBox1.Height);
            g.DrawString($" X", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Green), axisX.End.to2D().X, pictureBox1.Height - axisX.End.to2D().Y);
            g.DrawString($" Y", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Blue), axisY.End.to2D().X, pictureBox1.Height - axisY.End.to2D().Y);
            g.DrawString($" Z", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(Color.Red), axisZ.End.to2D().X, pictureBox1.Height - axisZ.End.to2D().Y);
           
            g.ScaleTransform(1.0F, -1.0F);
            g.TranslateTransform(0.0F, -(float)pictureBox1.Height);
        }

        // Перерисовывает всю сцену
        void redraw()
        {
            g.Clear(Color.White);
            
            drawShape(currentShape);
            
            if (checkBox1.Checked)
            {
                drawAxis();
            }
        }

        
    }
}
