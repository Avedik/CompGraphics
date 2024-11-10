using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecondTask
{
    public partial class Form1 : Form
    {
        double shiftx = 0;
        double shifty = 0;
        double shiftz = 0;
        List<Point> RotationShapePoints = new List<Point>();
        AxisType AxisforRotate;
        int Div;

        public Form1()
        {
            InitializeComponent();
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
            selectAxis.Enabled = selectPlane.Enabled = 
                buttonRotate.Enabled = buttonScale.Enabled = buttonShift.Enabled = 
                buttonReflection.Enabled = rbPerspective.Enabled = rbIsometric.Enabled = 
                textAngle.Enabled = textScaleX.Enabled = textScaleY.Enabled =
                textScaleZ.Enabled = textShiftX.Enabled = textShiftY.Enabled = 
                textShiftZ.Enabled = rbWorldCenter.Enabled = rbCenter.Enabled = textX1.Enabled = 
                textX2.Enabled = textY1.Enabled = textY2.Enabled = textZ1.Enabled = textZ2.Enabled = 
                buttonRotateAroundLine.Enabled = buttonRoll.Enabled = selectRollAxis.Enabled =
                textAngleForLineRotation.Enabled = textBoxAngleRotCenter.Enabled = interactiveMode;
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

        private void AddPoint_Click(object sender, EventArgs e)
        {
            int x = int.Parse(getX.Text);
            int y = int.Parse(getY.Text);
            int z = int.Parse(getZ.Text);

            Point p = new Point(x, y, z);
            RotationShapePoints.Add(p);
            drawPoint(p);
        }

        private void axizRotate_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (axizRotate.SelectedIndex)
            {
                case 0: AxisforRotate = AxisType.X; break;
                case 1: AxisforRotate = AxisType.Y; break;
                case 2: AxisforRotate = AxisType.Z; break;
                default: throw new Exception("Bad axis");
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveFigure();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            loadFigure();
        }

        private void loadFigure()
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Text Files(*.txt)|*.txt|All files (*.*)|*.*";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                char[] delimiterChars = { ' ', '\r', '\n' };
                string[] textElems = File.ReadAllText(open_dialog.FileName).Split(delimiterChars);
                Surface surf = new Surface();
                List<Point> points = new List<Point>();
                int i = 0;
                while (i < textElems.Length)
                {
                    if (textElems[i] == "")
                        ++i;
                    else if (textElems[i] == "v")
                    {
                        points.Add(new Point(double.Parse(textElems[i + 1]), double.Parse(textElems[i + 2]), double.Parse(textElems[i + 3])));
                        i += 4;
                    }

                    else if (textElems[i] == "f")
                    {
                        surf.addFace(new Polygon().addEdge(points[int.Parse(textElems[i + 1]) - 1]).
                            addEdge(points[int.Parse(textElems[i + 2]) - 1]).
                            addEdge(points[int.Parse(textElems[i + 3]) - 1]).
                            addEdge(points[int.Parse(textElems[i + 4]) - 1]));
                        i += 5;
                    }
                }
                currentShape = surf;
                redraw();
                setFlags(true);
            }
        }

        private void saveFigure()
        {
            if (currentShape != null)
            {
                SaveFileDialog save_dialog = new SaveFileDialog();
                save_dialog.Filter = "Text Files(*.txt)|*.txt|All files (*.*)|*.*";
                if (save_dialog.ShowDialog() == DialogResult.OK)
                {
                    HashSet<Point> points = new HashSet<Point>();
                    foreach (Polygon f in currentShape.Faces)
                        foreach (Point p in f.Points)
                            points.Add(p);
                    using (StreamWriter sw = new StreamWriter(save_dialog.FileName))
                    {
                        foreach (Point p in points)
                            sw.WriteLine("v {0} {1} {2}", p.X, p.Y, p.Z);

                        List<int> indexes = new List<int>() { -1, -1, -1, -1 };
                        foreach (Polygon f in currentShape.Faces)
                        {
                            for (int i = 0; i < f.Points.Count; ++i)
                            {
                                int j = 0;
                                foreach (Point p in points)
                                {
                                    if (f.Points[i].X == p.X && f.Points[i].Y == p.Y && f.Points[i].Z == p.Z)
                                    {
                                        indexes[i] = j + 1;
                                        break;
                                    }
                                    ++j;
                                }
                            }
                            sw.WriteLine("f {0} {1} {2} {3}", indexes[0], indexes[1], indexes[2], indexes[3]);
                        }
                    }
                }
            }
        }
    }
}
