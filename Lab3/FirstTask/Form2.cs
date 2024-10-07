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
    public partial class Form2 : Form
    {
        private Point start;
        private bool drawing = false;
        int left, right, up, down;
        Bitmap image;
        Bitmap back;
        List<Tuple<Point, Point>> listOfLines = new List<Tuple<Point, Point>>();

        public Form2()
        {
            InitializeComponent();
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            radioButton1.Checked = true;
            Clear();
        }

        private void findBorders(Point our_p, ref Point left_b, ref Point right_b, Bitmap b, Color c)
        {
            while (left_b.X > 0 && equalColors(b.GetPixel(left_b.X, left_b.Y), c))
            {
                left_b.X -= 1;
            }

            while (right_b.X < b.Width && equalColors(b.GetPixel(right_b.X, right_b.Y), c))
                right_b.X += 1;
        }

        private bool equalColors(Color c1, Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }

        private void fill(Point p)
        {
            back = new Bitmap(pictureBox.Image);
            int pixelXOffset = (p.X - image.Width / 2) % image.Width;
            int pixelYOffset = (p.Y - image.Height / 2) % image.Height;

            var g = Graphics.FromImage(pictureBox.Image);
            foreach (var t in listOfLines)
            {
                int i = t.Item1.X + 1;
                int start = (i % image.Width >= pixelXOffset ? 0 : image.Width) +
                        i % image.Width - pixelXOffset;
                int yImage = (t.Item1.Y % image.Height >= pixelYOffset ? 0 : image.Height) +
                        t.Item1.Y % image.Height - pixelYOffset;

                for (int width = 0; i < t.Item2.X; i += width)
                {
                    start = (start + width) % image.Width;
                    width = Math.Min(image.Width - start, t.Item2.X - i);
                    
                    for (int k = 0; k < width; ++k)
                        back.SetPixel(i + k, t.Item1.Y, image.GetPixel(start + k, yImage));
                }
                pictureBox.Image = back;
            }
        }

        private void findArea(Point point, Color color)
        {
            Bitmap bitmap = (Bitmap)pictureBox.Image;

            if (listOfLines.Exists(t => t.Item1.Y == point.Y && t.Item1.X <= point.X && point.X <= t.Item2.X))
                return;
            // если пиксель не закрашен
            if (0 < point.X && point.X < bitmap.Width && 0 < point.Y && point.Y < bitmap.Height)
            {
                Point left_b = point, right_b = point;
                findBorders(point, ref left_b, ref right_b, bitmap, color);
                if (left_b.X < left)
                    left = left_b.X;
                if (right_b.X > right)
                    right = right_b.X;

                if (left_b.Y < down)
                    down = left_b.Y;
                if (right_b.Y > right)
                    up = right_b.Y;

                listOfLines.Add(Tuple.Create(left_b, right_b));

                for (int i = left_b.X + 1; i < right_b.X; ++i)
                    findArea(new Point(i, point.Y + 1), color);

                for (int i = left_b.X + 1; i < right_b.X; ++i)
                    findArea(new Point(i, point.Y - 1), color);
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            start = new Point(e.X, e.Y);
            if (radioButton1.Checked) // режим рисования
            {
                drawing = true;
            }
            else
            {
                left = e.Location.X;
                right = e.Location.X;
                up = e.Location.Y;
                down = e.Location.Y;

                findArea(start, pictureBox.BackColor);
                fill(start);
                listOfLines.Clear();
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!drawing) return;
            var finish = new Point(e.X, e.Y);
            var pen = new Pen(Color.Black, 1f);

            var g = Graphics.FromImage(pictureBox.Image);
            g.DrawLine(pen, start, finish);
            pen.Dispose();
            g.Dispose();
            pictureBox.Image = pictureBox.Image;
            pictureBox1.Invalidate();
            start = finish;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Clear();
        }

        private void Clear()
        {
            var g = Graphics.FromImage(pictureBox.Image);
            g.Clear(pictureBox.BackColor);
            pictureBox.Image = pictureBox.Image;
            listOfLines.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;**.PNG)|*.BMP;*.JPG;**.PNG|All files (*.*)|*.*";
            DialogResult dr = open_dialog.ShowDialog();
            Clear();

            if (dr == DialogResult.OK)
            {
                image = new Bitmap(open_dialog.FileName);
                pictureBox1.Image = new Bitmap(image, pictureBox1.Size);
            }
        }
    }
}
