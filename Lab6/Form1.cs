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
            isInteractiveMode = selectAxis.Enabled = buttonRotate.Enabled = 
                buttonScale.Enabled = buttonShift.Enabled = rbPerspective.Enabled = 
                rbIsometric.Enabled = textAngle.Enabled = textScaleX.Enabled = 
                textScaleY.Enabled = textScaleZ.Enabled = textShiftX.Enabled = 
                textShiftY.Enabled = textShiftZ.Enabled = rbWorldCenter.Enabled = interactiveMode;

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

    }
}
