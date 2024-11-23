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

namespace ThirdTask
{
    public partial class Form1 : Form
    {
        bool isInteractiveMode = false;
        double shiftx = 0;
        double shifty = 0;
        double shiftz = 0;

        double sphereLength;
        double angleXZ;
        double angleY;

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
            double A = p1.Y - p2.Y; // общее уравнение прямой, проходящей через заданные точки
            double B = p2.X - p1.X; // вектор нормали 
            double C = p1.X * p2.Y - p2.X * p1.Y;
            Point p3 = new Point(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            shift(ref currentShape, p1.X - shiftx, p1.Y - shifty, p1.Z - shiftz);
            shiftx = p1.X;
            shifty = p1.Y;
            shiftz = p1.Z;
            redraw();
            drawLine(p1, p2, new Pen(Color.Aquamarine, 4));
        }
    }
}
