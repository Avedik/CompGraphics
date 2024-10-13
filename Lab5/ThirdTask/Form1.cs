using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThirdTask
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;
        List<Point> list = new List<Point>();
        List<Point> listCenter = new List<Point>();
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            g = Graphics.FromImage(pictureBox1.Image);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            g.FillEllipse(Brushes.Red, e.X - 2, e.Y - 2, 4, 4);
            list.Add(new Point(e.X, e.Y));
            pictureBox1.Image = bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 2; i < list.Count; i = i + 2) // сделать для произвольного кол-ва
            {
                var pen = new Pen(Color.Yellow, 1);
                g.DrawLine(pen, list[i], list[i + 1]);
                listCenter.Add(new Point((list[i].X + list[i + 1].X) / 2, (list[i].Y + list[i + 1].Y) / 2));
                pen.Dispose();
            }

            Point p1 = list[0];
            Point p2;
            Point p3;
            Point p4;
            for (int i = 0; i < listCenter.Count; ++i)
            {
                p2 = list[2*i + 1];
                p3 = list[2*i + 2];
                p4 = listCenter[i];
                for (double t = 0; t <= 1; t = t + 0.01)
                    DrawPoint(p1, p2, p3, p4, t);
                p1 = p4;
            }
            pictureBox1.Image = bmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Image = bmp;
            list.Clear();
            listCenter.Clear();
        }

        void DrawPoint(Point p1, Point p2, Point p3, Point p4, double t)
        {
            double[,] points = new double[,] { { p1.X, p2.X, p3.X, p4.X },
                                                { p1.Y, p2.Y, p3.Y, p4.Y }};
            double[,] tMatrix = new double[,] { { 1 },{ t }, { t*t }, {t*t*t } };
            double[,] matr = new double[,] { { 1, -3, 3, -1 },
                                             { 0, 3, -6, 3 },
                                             { 0, 0, 3, -3 },
                                             { 0, 0, 0, 1 }};
            double[,] res = MatrixMultiplication(points, matr);
            res = MatrixMultiplication(res, tMatrix);
            bmp.SetPixel((int)res[0, 0], (int)res[1, 0], Color.Brown);
        }

        // Перемножение матриц
        private double[,] MatrixMultiplication(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                        res[i, j] += m1[i, k] * m2[k, j];
            return res;
        }
    }
}
