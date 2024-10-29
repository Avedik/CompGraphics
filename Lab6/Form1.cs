using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        bool isInteractiveMode = false;
        double shiftx = 0;
        double shifty = 0;
        double shiftz = 0;

        public Form1()
        {
            InitializeComponent();
            selectShape.SelectedIndex = 0;
            g = pictureBox1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // задаём Декартову систему координат
            g.ScaleTransform(1.0F, -1.0F);
            g.TranslateTransform(0.0F, -(float)pictureBox1.Height);

            // задаём точку начала координат
            Point.sceneCenter = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            setFlags();
        }
        void setFlags(bool interactiveMode = false)
        {
            isInteractiveMode = selectAxis.Enabled = selectPlane.Enabled = 
                buttonRotate.Enabled = buttonScale.Enabled = buttonShift.Enabled = 
                buttonReflection.Enabled = rbPerspective.Enabled = rbIsometric.Enabled = 
                textAngle.Enabled = textScaleX.Enabled = textScaleY.Enabled =
                textScaleZ.Enabled = textShiftX.Enabled = textShiftY.Enabled = 
                textShiftZ.Enabled = rbWorldCenter.Enabled = rbCenter.Enabled = textX1.Enabled = 
                textX2.Enabled = textY1.Enabled = textY2.Enabled = textZ1.Enabled = textZ2.Enabled = 
                buttonRotateAroundLine.Enabled = buttonRoll.Enabled = selectRollAxis.Enabled =
                textAngleForLineRotation.Enabled = textBoxAngleRotCenter.Enabled = interactiveMode;

            buttonShape.Text = interactiveMode ? "Очистить" : "Нарисовать";
            selectShape.Enabled = !interactiveMode;
        }

        private void comboBoxShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectShape.SelectedIndex)
            {
                case 0: currentShapeType = ShapeType.TETRAHEDRON; break;
                case 1: currentShapeType = ShapeType.HEXAHEDRON; break;
                case 2: currentShapeType = ShapeType.OCTAHEDRON; break;
                case 3: currentShapeType = ShapeType.ICOSAHEDRON; break;
                case 4: currentShapeType = ShapeType.DODECAHEDRON; break;
                default: throw new Exception("Bad figure");
            }
        }

        private void selectRollAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectRollAxis.SelectedIndex)
            {
                case 0: currentRollAxis = AxisType.X; break;
                case 1: currentRollAxis = AxisType.Y; break;
                case 2: currentRollAxis = AxisType.Z; break;
                default: throw new Exception("Bad axes");
            }
        }

        private void rbPerspective_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPerspective.Checked)
            {
                Point.projection = ProjectionType.PERSPECTIVE;
                redraw();
            }
        }

        private void textScaleX_TextChanged(object sender, EventArgs e)
        {
            if (textScaleX.Text == "")
            {
                textScaleX.Text = "1";
            }
        }

        private void textScaleY_TextChanged(object sender, EventArgs e)
        {
            if (textScaleY.Text == "")
            {
                textScaleY.Text = "1";
            }
        }

        private void textScaleZ_TextChanged(object sender, EventArgs e)
        {
            if (textScaleZ.Text == "")
            {
                textScaleZ.Text = "1";
            }
        }

        private void textShiftX_TextChanged(object sender, EventArgs e)
        {
            if (textShiftX.Text == "")
            {
                textShiftX.Text = "0";
            }
        }

        private void textShiftY_TextChanged(object sender, EventArgs e)
        {
            if (textShiftY.Text == "")
            {
                textShiftY.Text = "0";
            }
        }

        private void textShiftZ_TextChanged(object sender, EventArgs e)
        {
            if (textShiftZ.Text == "")
            {
                textShiftZ.Text = "0";
            }
        }

        private void rbIsometric_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIsometric.Checked)
            {
                Point.projection = ProjectionType.ISOMETRIC;
                redraw();
            }
        }

        private void selectAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectAxis.SelectedIndex)
            {
                case 0: currentAxis = AxisType.X; break;
                case 1: currentAxis = AxisType.Y; break;
                case 2: currentAxis = AxisType.Z; break;
                default: throw new Exception("Bad axis");
            }
        }

        private void selectPlane_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectPlane.SelectedIndex)
            {
                case 0: currentPlane = PlaneType.XY; break;
                case 1: currentPlane = PlaneType.YZ; break;
                case 2: currentPlane = PlaneType.XZ; break;
                default: throw new Exception("Bad plane");
            }
        }

        private void buttonRoll_Click(object sender, EventArgs e)
        {
            rotationThroughTheCenter(ref currentShape, currentRollAxis, int.Parse(textBoxAngleRotCenter.Text));
            redraw();
        }

        private void buttonRotateAroundLine_Click(object sender, EventArgs e)
        {
            int angle = int.Parse(textAngleForLineRotation.Text);
            Point p1 = new Point(int.Parse(textX1.Text), int.Parse(textY1.Text), int.Parse(textZ1.Text));
            Point p2 = new Point(int.Parse(textX2.Text), int.Parse(textY2.Text), int.Parse(textZ2.Text));
            if (p1.Z == 0 && p1.X == 0 && p1.Y == 0 && (p2.Z != 0 || p2.Y == 0 || p2.X == 0))

            {
                Point tmp = p1;
                p1 = p2;
                p2 = tmp;
            }
            if (p2.Z == 0 && p2.X == 0 && p2.Y == 0 && (p1.Z != 0 || p1.Y == 0 || p1.X == 0))

            {
                Point tmp = p1;
                p1 = p2;
                p2 = tmp;
            }

            rotate_around_line(ref currentShape, angle, p1, p2);
            double A = p1.Yf - p2.Yf;//общее уравнение прямой, проходящей через заданные точки
            double B = p2.Xf - p1.Xf;//вектор нормали 
            double C = p1.Xf * p2.Yf - p2.Xf * p1.Yf;
            Point p3 = new Point(p2.Xf - p1.Xf, p2.Yf - p1.Yf, p2.Zf - p1.Zf);
            shift(ref currentShape, p1.Xf - shiftx, p1.Yf - shifty, p1.Zf - shiftz);
            shiftx = p1.Xf;
            shifty = p1.Yf;
            shiftz = p1.Zf;
            redraw();
            drawLine(new Line(p1, p2), new Pen(Color.Aquamarine, 4));
        }
    }
}
