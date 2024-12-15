using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecondTask
{
    public partial class Form1 : Form
    {
        BindingList<Shape> sceneShapes;
        List<Shape> scene;
        bool isMoving = false;
        Camera camera;
        List<Color> colorrange;
        Shape shapeWithoutNonFacial; // фигура без нелицевых граней
        bool isPruningFaces = false;

        public Form1()
        {
            sceneShapes = new BindingList<Shape>();
            colorrange = GenerateColors();
            scene = new List<Shape>();
            InitializeComponent();
            listBox.DataSource = sceneShapes;
            canvas.Image = new Bitmap(canvas.Width, canvas.Height);
            camera = new Camera();
            Point.worldCenter = new PointF(canvas.Width / 2, canvas.Height / 2);
            Point.projection = ProjectionType.PERSPECTIVE;
            Point.setProjection(canvas.Size, 1, 100, 45);
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Shape s = Shape.readShape(openFileDialog1.FileName);
                sceneShapes.Add(s);
                scene.Add(s);
                changeToolsAccessibility(true);
                redrawScene();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            sceneShapes.Remove((Shape)listBox.SelectedValue);
            if (sceneShapes.Count == 0)
            {
                changeToolsAccessibility(false);
            }
            redrawScene();
        }

        void changeToolsAccessibility(bool isOn)
        {
            btnDelete.Enabled = isOn;
            textShiftX.Enabled = isOn;
            textShiftY.Enabled = isOn;
            textShiftZ.Enabled = isOn;
            buttonShift.Enabled = isOn;
            //btnShowAxis.Enabled = isOn;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            redrawScene();
        }

        private void rbParallel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPerspective.Checked)
            {
                Point.projection = ProjectionType.PERSPECTIVE;
            }
            else if (radioButton1.Checked)
            {
                Point.projection = ProjectionType.TRIMETRIC;
            }
            else
            {
                Point.projection = ProjectionType.CAVALIER;
            }

            redrawScene();
        }

        System.Drawing.Point previousLocation;
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving = true;
            previousLocation = e.Location;
        }

       

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            isMoving = false;
        }

        private void buttonShift_Click(object sender, EventArgs e)
        {
            sceneShapes[listBox.SelectedIndex] = AffineTransformations.shift(sceneShapes[listBox.SelectedIndex], int.Parse(textShiftX.Text), int.Parse(textShiftY.Text), int.Parse(textShiftZ.Text));
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w': camera.move(forwardbackward: 5); break;
                case 'a': camera.move(leftright: 5); break;
                case 's': camera.move(forwardbackward: -5); break;
                case 'd': camera.move(leftright: -5); break;
                case 'q': camera.move(updown: 5); break;
                case 'e': camera.move(updown: -5); break;
                case 'i': camera.changeView(shiftY: 2); break;
                case 'j': camera.changeView(shiftX: -2); break;
                case 'k': camera.changeView(shiftY: -2); break;
                case 'l': camera.changeView(shiftX: 2); break;
                default: return;
            }
            if (isPruningFaces)
            {
                shapeWithoutNonFacial = findNonFacial(sceneShapes[listBox.SelectedIndex], camera);
                redrawShapeWithoutNonFacial();
            }
            else
                redrawScene();
            //label7.Text = $"{camera.Location}";
            e.Handled = true;
        }

        private void z_buffer_Click(object sender, EventArgs e)
        {
           
            List<Shape> l = sceneShapes.ToList();
            Bitmap bmp = Z_buffer.z_buf(canvas.Width, canvas.Height, l, camera, colorrange);
            canvas.Image = bmp;
            canvas.Invalidate();
        }
        


    }
}
