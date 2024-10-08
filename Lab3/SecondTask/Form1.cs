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
        Bitmap b;
        private int sX;
        private int sY;
        private int fX;
        private int fY;
        private bool type = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            sX = e.X;
            sY = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            fX = e.X;
            fY = e.Y;
            DrawSegment();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            type = false;
            pictureBox1.Image = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            type = true;
            pictureBox1.Image = null;
        }

        private float fpart(float x)
        {
            return x - (float)Math.Floor(x);
        }

        private int ipart(float x)
        {
            return (int)Math.Floor(x);
        }

        private void plot(Bitmap btp, int x, int y, float brightness, Color color)
        {
            Color newColor = Color.FromArgb((int)(color.A * brightness), color.R, color.G, color.B);
            btp.SetPixel(x, y, newColor);
        }

        private void DrawSegment()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            if (!type)
            {
                if (Math.Abs(fY - sY) < Math.Abs(fX - sX))
                    if (sX > fX)
                        DrawSegmentLowBresen(fX, fY, sX, sY);
                    else
                        DrawSegmentLowBresen(sX, sY, fX, fY);
                else
                    if (sY > fY)
                    DrawSegmentHighBresen(fX, fY, sX, sY);
                else
                    DrawSegmentHighBresen(sX, sY, fX, fY);
            }
            else
            {
                WuLine(sX, sY, fX, fY);
            }
            pictureBox1.Image = b;
        }

        private void DrawSegmentLowBresen(int sX, int sY, int fX, int fY)
        {
            int dx = fX - sX;
            int dy = fY - sY;
            int step = 1;
            if (dy < 0)
            {
                step = -1;
                dy = -dy;
            }
            int d = 2 * dy - dx;
            for (int x = sX; x <= fX; ++x)
            {
                b.SetPixel(x, sY, Color.Black);
                if (d > 0)
                {
                    sY += step;
                    d += 2*(dy - dx);
                }
                else
                    d += 2*dy;
            }
        }

        private void DrawSegmentHighBresen(int sX, int sY, int fX, int fY)
        {
            int dx = fX - sX;
            int dy = fY - sY;
            int step = 1;
            if (dx < 0)
            {
                step = -1;
                dx = -dx;
            }
            int d = 2 * dx - dy;
            for (int y = sY; y <= fY; ++y)
            {
                b.SetPixel(sX, y, Color.Black);
                if (d > 0)
                {
                    sX += step;
                    d += 2*(dx - dy);
                }
                else
                    d += 2*dx;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ActiveControl
        }

        public void WuLine(int x1, int y1, int x2, int y2)
        {
            int num;
            bool flag = Math.Abs((int)(y2 - y1)) > Math.Abs(x2 - x1);
            if (flag)
            {
                num = y1;
                y1 = x1;
                x1 = num;
                num = y2;
                y2 = x2;
                x2 = num;
            }
            if (x1 > x2)
            {
                num = x1;
                x1 = x2;
                x2 = num;
                num = y2;
                y2 = y1;
                y1 = num;
            }


            int dx = x2 - x1;
            int dy = y2 - y1;
            float gradient = (float)dy / (float)dx;

            float xgap = 1 - fpart((float)(x1 + 0.5f));

            plot(b, x1, ipart(y1), (1 - fpart(y1)) * xgap, Color.Black);
            plot(b, x1, ipart(y1) + 1, fpart(y1) * xgap, Color.Black);


            float y = y1 + gradient;

            xgap = fpart(x2 + 0.5f);
            plot(b, x2, ipart(y2), (1 - fpart(y1)) * xgap, Color.Black);
            plot(b, x2, ipart(y2) + 1, fpart(y1) * xgap, Color.Black);


            if (dx == 0)
            {
                for (int i = y1; i <= y2; ++i)
                    plot(b, x1, i, 1.0f, Color.Black);
                return;
            }

            if (flag)
            {
                for (int x = x1 + 1; x <= x2 - 1; x++)
                {
                    plot(b, ipart(y), x, 1 - fpart(y), Color.Black);
                    plot(b, ipart(y) + 1, x, fpart(y), Color.Black);
                    y += gradient;
                }
            }
            else
            {
                for (int x = x1 + 1; x <= x2 - 1; x++)
                {
                    plot(b, x, ipart(y), 1 - fpart(y), Color.Black);
                    plot(b, x, ipart(y) + 1, fpart(y), Color.Black);
                    y += gradient;
                }
            }
        }
    }
}