using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstTask
{
    public partial class Form1 : Form
    {
        private Graphics g;
        string fileName = "..\\..\\..\\FirstTask\\КриваяКоха.txt";
        int generation = 0, randFrom = 0, randTo = 0;
        List<string> LSystem = new List<string>();
        List<Tuple<PointF, PointF>> points = new List<Tuple<PointF, PointF>>();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void ComboBoxLSystemChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = ComboBoxLSystemChange.SelectedIndex;
            switch (ind)
            {
                case 0:
                    fileName = "..\\..\\..\\FirstTask\\КриваяКоха.txt";
                    break;
                case 1:
                    fileName = "..\\..\\..\\FirstTask\\СнежинкаКоха.txt";
                    break;
                case 2:
                    fileName = "..\\..\\..\\FirstTask\\НаконечникСерпинского.txt";
                    break;
                case 3:
                    fileName = "..\\..\\..\\FirstTask\\КоверСерпинского.txt";
                    break;
                case 4:
                    fileName = "..\\..\\..\\FirstTask\\ШестиугольнаяКриваяГоспера.txt";
                    break;
                case 5:
                    fileName = "..\\..\\..\\FirstTask\\КриваяГильберта.txt";
                    break;
                case 6:
                    fileName = "..\\..\\..\\FirstTask\\КриваяДракона.txt";
                    break;
                case 7:
                    fileName = "..\\..\\..\\FirstTask\\ВысокоеДерево.txt";
                    break;
                case 8:
                    fileName = "..\\..\\..\\FirstTask\\ШирокоеДерево.txt";
                    break;
                case 9:
                    fileName = "..\\..\\..\\FirstTask\\Куст1.txt";
                    break;
                case 10:
                    fileName = "..\\..\\..\\FirstTask\\Куст2.txt";
                    break;
                case 11:
                    fileName = "..\\..\\..\\FirstTask\\Куст3.txt";
                    break;
            }
        }

        private void textBoxChangeGeneration_KeyPress(object sender, KeyPressEventArgs e)
        {
            char el = e.KeyChar;
            if (!Char.IsDigit(el) && el != (char)Keys.Back) // можно вводить только цифры и стирать
                e.Handled = true;
        }

        private void textBoxRandomFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            char el = e.KeyChar;
            if (!Char.IsDigit(el) && el != (char)Keys.Back && el != '-') // можно вводить только цифры, минус и стирать
                e.Handled = true;
        }

        private void textBoxRandomTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            char el = e.KeyChar;
            if (!Char.IsDigit(el) && el != (char)Keys.Back && el != '-') // можно вводить только цифры, минус и стирать
                e.Handled = true;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            points.Clear();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void buttonDrawFractal_Click(object sender, EventArgs e)
        {
            points.Clear();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

           
            generation = textBoxChangeGeneration.Text != "" ? Int32.Parse(textBoxChangeGeneration.Text) : 1;
            randFrom = textBoxRandomFrom.Text != "" ? Int32.Parse(textBoxRandomFrom.Text) : 0;
            randTo = textBoxRandomTo.Text != "" ? Int32.Parse(textBoxRandomTo.Text) : 0;

            string axiom = "", direction = "";
            float rotate = 0;
            //получаем данные из файла
            try
            {
                StreamReader sr = new StreamReader(fileName);
                string line = sr.ReadLine();
                string[] parseLine = line.Split(' ');
                axiom = parseLine[0];
                rotate = float.Parse(parseLine[1]);
                direction = parseLine[2];

                line = sr.ReadLine();
                while (line != null)
                {
                    LSystem.Add(line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

            DrawFractal(axiom, rotate, direction);
        }

        private void DrawFractal(string rule, float rot, string dir)
        {
            if (randFrom > randTo)
            {
                (randFrom, randTo) = (randTo, randFrom);
            }

            // разбиваем каждое правило
            Dictionary<char, string> systemRules = new Dictionary<char, string>();
            foreach (string line in LSystem)
                systemRules[line[0]] = line.Substring(2);

            // выводим одно общее правило построения для указанного поколения
            for (int i = 0; i < generation; ++i)
            {
                string seq = "";
                foreach (char lex in rule)
                    if (systemRules.ContainsKey(lex))
                        seq += systemRules[lex];
                    else
                        seq += lex;
                rule = seq;
            }

            float angle = 0;
            // находим угол начального направления
            switch (dir)
            {
                case "left":
                    angle = (float)Math.PI; 
                    break;
                case "right":
                    angle = 0; 
                    break;
                case "up":
                    angle = (float)(3 * Math.PI / 2); 
                    break;
                case "down":
                    angle = (float)(Math.PI / 2);
                    break;
            }

            rot = rot * (float)Math.PI / 180;
            Stack<Tuple<PointF, float>> st = new Stack<Tuple<PointF, float>>();
            PointF ANpoint = new PointF(0, 0);
            Random rand = new Random();
            int randRotate = 0;
            int count = 0;
            Color color = Color.FromArgb(64, 0, 0);
            Dictionary<PointF, Tuple<Color, float>> gr = new Dictionary<PointF, Tuple<Color, float>>();

            float width = 7;
            if (fileName == "..\\..\\..\\FirstTask\\ШирокоеДерево.txt")
                width = 14;

            foreach (char lex in rule)
            {
                if (lex == 'F')
                {
                    // следующая точка фрактала переносится от старой по направлению синуса
                    float nextX = (float)(ANpoint.X + Math.Cos(angle)), nextY = (float)(ANpoint.Y + Math.Sin(angle));
                    points.Add(Tuple.Create(ANpoint, new PointF(nextX, nextY)));

                    if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
                    {
                        if (count < 3)
                        {
                            width--;
                            count++;
                        }
                        gr[ANpoint] = new Tuple<Color, float>(color, width);
                    }

                    ANpoint = new PointF(nextX, nextY);
                }
                else if (lex == '[')
                {
                    st.Push(Tuple.Create(ANpoint, angle));
                    if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
                    {
                        color = color.G + 40 > 255 ? Color.FromArgb(color.R, 255, color.B) : Color.FromArgb(color.R, color.G + 40, color.B);
                        width--;
                    }
                }
                else if (lex == ']')
                {
                    Tuple<PointF, float> tuple = st.Pop();
                    ANpoint = tuple.Item1;
                    angle = tuple.Item2;
                    if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
                    {
                        color = color.G - 40 < 0 ? Color.FromArgb(color.R, 0, color.B) : Color.FromArgb(color.R, color.G - 40, color.B);
                        width++;
                    }
                }
                else if (lex == '-')
                {
                    randRotate = rand.Next(randFrom, randTo + 1);
                    angle -= rot + randRotate * (float)Math.PI / 180;
                }
                else if (lex == '+')
                {
                    randRotate = rand.Next(randFrom, randTo + 1);
                    angle += rot + randRotate * (float)Math.PI / 180;
                }
            }

            // находим минимум и максимум полученных точек для масштабирования
            float minX = points.Min(point => Math.Min(point.Item1.X, point.Item2.X)), maxX = points.Max(point => Math.Max(point.Item1.X, point.Item2.X));
            float minY = points.Min(point => Math.Min(point.Item1.Y, point.Item2.Y)), maxY = points.Max(point => Math.Max(point.Item1.Y, point.Item2.Y));

            
            PointF center = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            // центр полученного фрактала
            PointF fractal = new PointF(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);
            // шаг для масштабирования
            float step = Math.Min(pictureBox1.Width / (maxX - minX), pictureBox1.Height / (maxY - minY));

            List<Tuple<PointF, PointF>> scalePoints = new List<Tuple<PointF, PointF>>(points);
            // масштабируем список точек
            for (int i = 0; i < points.Count(); i++)
            {
                float scaleX = center.X + (points[i].Item1.X - fractal.X) * step,
                    scaleY = center.Y + (points[i].Item1.Y - fractal.Y) * step;
                float scaleNextX = center.X + (points[i].Item2.X - fractal.X) * step,
                    scaleNextY = center.Y + (points[i].Item2.Y - fractal.Y) * step;
                scalePoints[i] = new Tuple<PointF, PointF>(new PointF(scaleX, scaleY), new PointF(scaleNextX, scaleNextY));
            }

            if (fileName.Contains("Дерево") || fileName.Contains("Куст"))
            {
                for (int i = 0; i < points.Count(); ++i)
                    g.DrawLine(new Pen(gr[points[i].Item1].Item1, gr[points[i].Item1].Item2), scalePoints[i].Item1, scalePoints[i].Item2);
            }
            else
            {
                for (int i = 0; i < points.Count(); ++i)
                    g.DrawLine(new Pen(Color.Black), scalePoints[i].Item1, scalePoints[i].Item2);
            }

            pictureBox1.Invalidate();
        }

    }
}
