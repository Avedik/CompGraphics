using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace FirstTask
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
            double A = p1.Y - p2.Y;//общее уравнение прямой, проходящей через заданные точки
            double B = p2.X - p1.X;//вектор нормали 
            double C = p1.X * p2.Y - p2.X * p1.Y;
            Point p3 = new Point(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            shift(ref currentShape, p1.X - shiftx, p1.Y - shifty, p1.Z - shiftz);
            shiftx = p1.X;
            shifty = p1.Y;
            shiftz = p1.Z;
            redraw();
            drawLine(p1, p2, new Pen(Color.Aquamarine, 4));
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OBJ files (*.obj)|*.obj";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentShape = LoadOBJ(openFileDialog.FileName);
                    redraw();
                    setFlags(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки модели: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentShape != null)
            {
                SaveFileDialog save_dialog = new SaveFileDialog();
                save_dialog.Filter = "OBJ files (*.obj)|*.obj";
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

                        sw.WriteLine("");
                        List<int> indexes;
                        switch (currentShapeType)
                        {
                            case ShapeType.TETRAHEDRON: indexes = new List<int>() { -1, -1, -1 }; sw.WriteLine("g Tetrahedron"); break;
                            case ShapeType.OCTAHEDRON: indexes = new List<int>() { -1, -1, -1 }; sw.WriteLine("g Octahedron"); break;
                            case ShapeType.HEXAHEDRON: indexes = new List<int>() { -1, -1, -1, -1 }; sw.WriteLine("g Hexahedron"); break;
                            case ShapeType.ICOSAHEDRON: indexes = new List<int>() { -1, -1, -1 }; sw.WriteLine("g Icosahedron"); break;
                            case ShapeType.DODECAHEDRON: indexes = new List<int>() { -1, -1, -1, -1, -1 }; sw.WriteLine("g Dodecahedron"); break;

                            default: throw new Exception();
                        }

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
                            switch (currentShapeType)
                            {
                                case ShapeType.TETRAHEDRON: sw.WriteLine("f {0} {1} {2}", indexes[0], indexes[1], indexes[2]); break;
                                case ShapeType.OCTAHEDRON: sw.WriteLine("f {0} {1} {2}", indexes[0], indexes[1], indexes[2]); break;
                                case ShapeType.HEXAHEDRON: sw.WriteLine("f {0} {1} {2} {3}", indexes[0], indexes[1], indexes[2], indexes[3]); break;
                                case ShapeType.ICOSAHEDRON: sw.WriteLine("f {0} {1} {2}", indexes[0], indexes[1], indexes[2]); break;
                                case ShapeType.DODECAHEDRON: sw.WriteLine("f {0} {1} {2} {3} {4}", indexes[0], indexes[1], indexes[2], indexes[3], indexes[4]); break;
                            }
                        }
                    }
                }
            }
        }
        // ---  Функции работы с OBJ-файлами  ---
        private Polyhedron LoadOBJ(string filePath)
        {
            Polyhedron shape = new Polyhedron();
            List<string> lines = File.ReadAllLines(filePath).ToList();
            List<Point> points = new List<Point>();


            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("v "))
                {
                    string[] parts = lines[i].Split(' ');
                    points.Add(new Point(double.Parse(parts[1]), double.Parse(parts[2]), double.Parse(parts[3])));
                }
            }

            List<int> faceIndices;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("f "))
                {
                    string[] parts = lines[i].Split(' ');
                    faceIndices = new List<int>();
                    for (int j = 1; j < parts.Length; j++)
                        faceIndices.Add(int.Parse(parts[j].Split('/')[0]) - 1); // OBJ indices are 1-based
                    Polygon face = new Polygon();
                    foreach (int index in faceIndices)
                        face.addEdge(points[index]);
                    shape.addFace(face);
                }
            }

            return shape;
        }

    }
}



