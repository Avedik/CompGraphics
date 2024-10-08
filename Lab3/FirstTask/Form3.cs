using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstTask
{
    public partial class Form3 : Form
    {
        private static Bitmap image;
        private static Color borderColor, innerColor, myBorderColor;
        private static int firstX;
        private static int firstY;

        public Form3()
        {
            InitializeComponent();
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
            firstX = x - 1;
            firstY = y;
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

        private void getNextPixel(ref int x, ref int y, ref int whereBorder)
        {
            if (x > 0 && x < image.Width && y > 0 && y < image.Height)
            {
                int d = whereBorder;
                while (colorsEqual(borderColor, colorByDirection(x, y, d)))
                    d = (d + 2) % 8;
                if (colorsEqual(borderColor, colorByDirection(x, y, (d - 1 + 8) % 8)))
                {
                    Tuple<int, int> t = moveByDirection(x, y, d);
                    x = t.Item1;
                    y = t.Item2;
                    whereBorder = (d + 8 - 2) % 8;
                }
                else
                {
                    Tuple<int, int> t = moveByDirection(x, y, (d - 1 + 8) % 8);
                    x = t.Item1;
                    y = t.Item2;
                    whereBorder = (d - 1 + 8 - 3) % 8;
                }
            }
        }

        private LinkedList<Tuple<int, int>> getFullBorder(int x, int y)
        {
            LinkedList<Tuple<int, int>> points = new LinkedList<Tuple<int, int>>();
            int whereBorder = 0;
            do
            {
                Tuple<int, int> newt = Tuple.Create(x, y);
                if (points.Count() == 0 || points.Last() != newt)
                    points.AddLast(newt);
                getNextPixel(ref x, ref y, ref whereBorder);

            } while (((x != firstX) || (y != firstY)) && (points.Count() < (image.Width + image.Height) * 10));
            return points;
        }

        private void fillMyBorderPoints(ref LinkedList<Tuple<int, int>> points)
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
            LinkedList<Tuple<int, int>> points = getFullBorder(firstX, firstY);
            fillMyBorderPoints(ref points);

            pictureBox1.Image = image;
        }
    }
}
