using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstTask
{
    public partial class Form3 : Form
    {
        private static Bitmap image;
        private static Color borderColor, innerColor, myBorderColor;
        List<Tuple<int, int>> border;

        public Form3()
        {
            InitializeComponent();
            border = new List<Tuple<int, int>>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            Bitmap imageSource = new Bitmap(openFileDialog1.FileName);
            image = new Bitmap(imageSource, pictureBox1.Size);

            pictureBox1.Image = image;
        }

        private bool colorsEqual(Color c1, Color c2)
        {
            return (c1.R - c2.R == 0 &&
                c1.G - c2.G == 0 &&
                c1.B - c2.B == 0);
        }

        private void getRightBorder(int x, int y)
        {
            Color pixelColor = image.GetPixel(x, y);
            Color currColor = pixelColor;
            innerColor = pixelColor;

            myBorderColor = Color.FromArgb(255, 0, 0);

            while (colorsEqual(innerColor, currColor) && x < image.Width)
            {
                currColor = image.GetPixel(x, y);
                x += 1;
            }
            borderColor = image.GetPixel(x, y);
            border.Add(Tuple.Create(x, y));
        }

        private Tuple<int, int> moveByDirection(int x, int y, int direction)
        {
            switch (direction)
            {
                case 0:
                    x += 1;
                    break;
                case 1:
                    x += 1;
                    y -= 1;
                    break;
                case 2:
                    y -= 1;
                    break;
                case 3:
                    x -= 1;
                    y -= 1;
                    break;
                case 4:
                    x -= 1;
                    break;
                case 5:
                    x -= 1;
                    y += 1;
                    break;
                case 6:
                    y += 1;
                    break;
                case 7:
                    x += 1;
                    y += 1;
                    break;
            }
            return Tuple.Create(x, y);
        }

        private Color colorByDirection(int x, int y, int direction)
        {
            Tuple<int, int> t = moveByDirection(x, y, direction);
            return image.GetPixel(t.Item1, t.Item2);
        }

        private void getFullBorder()
        {
            for (int index = 0; index < border.Count(); ++index)
            {
                int x = border[index].Item1;
                int y = border[index].Item2;

                for (int i = 0; i < 8; ++i)
                {
                    Tuple<int, int> point = moveByDirection(x, y, i);
                    Color pixColor = image.GetPixel(point.Item1, point.Item2);
                    if (colorsEqual(borderColor, pixColor) && !border.Contains(point))
                        border.Add(point);
                }
            }
        }

        private void fillBorderPoints(ref List<Tuple<int, int>> points)
        {
            foreach (var t in points)
            {
                image.SetPixel(t.Item1, t.Item2, myBorderColor);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var y = e.Y;

            getRightBorder(x, y);
            getFullBorder();
            fillBorderPoints(ref border);

            pictureBox1.Image = image;
        }
    }
}
