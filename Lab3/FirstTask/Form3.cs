﻿using System;
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

        public Form3()
        {
            InitializeComponent();
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            Bitmap imageSource = new Bitmap(openFileDialog1.FileName, true);
            image = ResizeBitmap(imageSource, pictureBox1.Size.Width, pictureBox1.Size.Height);

            pictureBox1.Image = image;

        }

        private int norma(Color c1, Color c2)
        {
            return (System.Math.Abs(c1.R - c2.R) +
                System.Math.Abs(c1.G - c2.G) +
                System.Math.Abs(c1.B - c2.B));
        }

        private int differenceBetweenColors = 125;

        private bool colorsEqual(Color c1, Color c2)
        {
            return (System.Math.Abs(c1.R - c2.R) < differenceBetweenColors &&
                System.Math.Abs(c1.G - c2.G) < differenceBetweenColors &&
                System.Math.Abs(c1.B - c2.B) < differenceBetweenColors);
        }

        private bool colorsEqual2(Color c1, Color c2)
        {
            return (norma(c1, c2) < differenceBetweenColors);
        }

        private bool colorsEqual(Color c1, int x, int y)
        {
            Color c2 = image.GetPixel(x, y);
            return (norma(c1, c2) < differenceBetweenColors);
        }

        private static int firstX;
        private static int firstY;

        private void getRightBorder(int x, int y)
        {
            Color pixelColor = image.GetPixel(x, y);
            Color currColor = pixelColor;
            innerColor = pixelColor;

            myBorderColor = Color.FromArgb(255, 0, 0);
            
            while (colorsEqual(innerColor, currColor) && x < image.Width)
            {
                x += 1;
                currColor = image.GetPixel(x, y);
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

        private void pointsToFile(ref LinkedList<Tuple<int, int>> points, string fname = "points.txt")
        {
            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(fname))
            {
                foreach (var t in points)
                    writetext.WriteLine("x = " + t.Item1 + "| y = " + t.Item2);
            }
        }

        private void pointsToFile(ref List<Tuple<int, int>> points, string fname = "points.txt")
        {
            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(fname))
            {
                foreach (var t in points)
                    writetext.WriteLine("x = " + t.Item1 + "| y = " + t.Item2);
            }
        }

        private LinkedList<Tuple<int, LinkedList<Tuple<int, int>>>> getYandBorders(ref List<Tuple<int, int>> points)
        {
            LinkedList<Tuple<int, LinkedList<Tuple<int, int>>>> yBorders = new LinkedList<Tuple<int, LinkedList<Tuple<int, int>>>>();
            int y = points.First().Item2;
            int x = points.First().Item1;
            int x_first = points.First().Item1;
            LinkedList<Tuple<int, int>> borders = new LinkedList<Tuple<int, int>>();

            foreach (var t in points)
            {
                if (t != points.First())
                {
                    int curry = t.Item2;
                    int currx = t.Item1;

                    if (curry == y)
                    {
                        if (currx == x + 1)
                        {
                            x += 1;
                        }
                        else
                        {
                            borders.AddLast(Tuple.Create(x_first, x));
                            x_first = currx;
                            x = currx;
                        }
                    }
                    else
                    {
                        borders.AddLast(Tuple.Create(x_first, x));
                        yBorders.AddLast(Tuple.Create(y, borders));
                        borders = new LinkedList<Tuple<int, int>>();

                        x_first = currx;
                        x = currx;
                        y = curry;
                    }
                }
            }
            borders.AddLast(Tuple.Create(x_first, x));
            yBorders.AddLast(Tuple.Create(y, borders));
            return yBorders;
        }

        private void yBordersToFile(ref LinkedList<Tuple<int, LinkedList<Tuple<int, int>>>> yBorders, string fname = "yBorders.txt")
        {
            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(fname))
            {
                foreach (var t1 in yBorders)
                {
                    writetext.WriteLine("y = " + t1.Item1 + ": ");
                    foreach (var t2 in t1.Item2)
                        writetext.WriteLine("       (x1 = " + t2.Item1 + ", x2 = " + t2.Item2 + ") ");
                    writetext.WriteLine();
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            var loc = e.Location;
            var x = loc.X;
            var y = loc.Y;

            getRightBorder(x, y);
            LinkedList<Tuple<int, int>> points = getFullBorder(firstX, firstY);
            fillMyBorderPoints(ref points);

            pointsToFile(ref points, "points1.txt");
            List<Tuple<int, int>> pointsSorted = new List<Tuple<int, int>>(
                points.OrderBy(t => t.Item2).ThenBy(t => t.Item1).ToList().Distinct().ToList());
            pointsToFile(ref pointsSorted, "points2.txt");

            LinkedList<Tuple<int, LinkedList<Tuple<int, int>>>> yBorders = getYandBorders(ref pointsSorted);
            yBordersToFile(ref yBorders);

            pictureBox1.Image = image;
        }
    }
}