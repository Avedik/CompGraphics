using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        enum Transormations { Shift, Rotation, RotationCenter};
        Bitmap bmp;
        Graphics g;
        bool isReady = false; // флаг готовности полигона
        double[,] transformationMatrix; // матрица преобразования
        List<Point> list = new List<Point>(); // список точек для полигона
        Point myPoint; // Точка, относительно которой происходит поворот

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

        // Добавляет очередной элемент к полигону при нажатии мышкой
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isReady)
            {
                list.Add(new Point(e.X, e.Y));
                if (list.Count() == 3)
                    chainButton.Enabled = true;

                if (list.Count() == 1)
                {
                    button2.Enabled = true;
                    ((Bitmap)pictureBox1.Image).SetPixel(e.X, e.Y, Color.Red);
                }
                else
                {
                    var pen = new Pen(Color.Red, 1);
                    g.DrawLine(pen, list[list.Count() - 2], list.Last());
                    pen.Dispose();
                }
                pictureBox1.Image = pictureBox1.Image;
            }
            else
            {
                myPoint = new Point(e.X, e.Y);
                ((Bitmap)pictureBox1.Image).SetPixel(e.X, e.Y, Color.Red);
            }
        }

        private void RefreshElements()
        {
            // Происходит активация элементов, если в комбобоксе выбрано "Смещение"
            label2.Enabled = textBox1.Enabled =
                textBox2.Enabled = dxLabel.Enabled =
                dyLabel.Enabled = comboBox1.SelectedIndex == (int)Transormations.Shift;
            // Происходит активация элементов, если в комбобоксе выбрано "Поворот вокруг заданной точки"
            label1.Enabled = textBox3.Enabled = degreesLabel.Enabled = 
                comboBox1.SelectedIndex == (int)Transormations.Rotation;
            // Происходит активация элементов, если в комбобоксе выбрано "Поворот вокруг центра"
            label1.Enabled = textBox3.Enabled = degreesLabel.Enabled =
                comboBox1.SelectedIndex == (int)Transormations.RotationCenter;
        }

        // Очистка сцены (удаление всех полигонов)
        private void Clear()
        {
            g.Clear(pictureBox1.BackColor);
            list.Clear();

            comboBox1.SelectedIndex = 0;
            RefreshElements();
            button2.Enabled = chainButton.Enabled = false;
            pictureBox1.Image = pictureBox1.Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isReady = false;
            Clear();
        }

        private void ChooseTransformation()
        {
            double X;
            double Y;
            double angle;
            // Выбор аффинного преобразования
            switch (comboBox1.SelectedIndex)
            {
                case (int)Transormations.Shift:
                    double dX = System.Convert.ToDouble(textBox1.Text);
                    double dY = -System.Convert.ToDouble(textBox2.Text);
                    transformationMatrix = new double[,] { { 1.0, 0, 0 }, { 0, 1.0, 0 }, { dX, dY, 1.0 } };
                    break;
                case (int)Transormations.Rotation:
                    X = System.Convert.ToDouble(myPoint.X);
                    Y = -System.Convert.ToDouble(myPoint.Y);
                    angle = Math.PI * System.Convert.ToDouble(textBox3.Text) / 180.0;
                    transformationMatrix = new double[,] { 
                        { Math.Cos(angle), Math.Sin(angle), 0 }, 
                        { -Math.Sin(angle), Math.Cos(angle), 0 }, 
                        { -X*Math.Cos(angle) - Y*Math.Sin(angle) + X, 
                            -X*Math.Sin(angle)+Y*Math.Cos(angle)-Y, 1.0 } };
                    break;

                default:
                    break;
            }
        }

        // Для очищения экрана (холста)
        private void ClearScreen()
        {
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Image = pictureBox1.Image;
        }

        // Перемножение матриц
        private double[,] MatrixMultiplication(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }

        // Применяет преобразование с использованием матриц
        private void button2_Click(object sender, EventArgs e)
        {
            ChooseTransformation();

            List<Point> newList = new List<Point>();
            foreach (Point p in list)
            {
                double[,] point = new double[,] { { p.X * 1.0, p.Y * 1.0, 1.0 } };
                double[,] res = MatrixMultiplication(point, transformationMatrix);
                newList.Add(new Point(Convert.ToInt32(Math.Round(res[0, 0])), Convert.ToInt32(Math.Round(res[0, 1]))));
            }
            ClearScreen();

            Point prevPoint = newList.First();
            foreach (Point point in newList)
            {
                if (point != prevPoint)
                {
                    Pen pen = new Pen(Color.Red, 1);
                    g.DrawLine(pen, prevPoint, point);

                    prevPoint = point;
                    pen.Dispose();
                }
            }

            list = newList;
            pictureBox1.Image = bmp;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshElements();
        }

        // Замыкает полигон
        private void chainButton_Click(object sender, EventArgs e)
        {
            g.DrawLine(new Pen(Color.Red, 1), list.Last(), list.First());
            list.Add(list.First());
            pictureBox1.Image = pictureBox1.Image;
            isReady = true;
            chainButton.Enabled = false;
        }
    }
}
