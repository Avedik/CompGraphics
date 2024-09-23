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
                float gradient;
                int num;
                b.SetPixel(sX, sY, Color.Black);
                b.SetPixel(fX, fY, Color.Black);
                bool flag = Math.Abs(fY - sY) > Math.Abs(fX - sX);
                if (flag)
                {
                    num = sY;
                    sY = sX;
                    sX = num;
                    num = fY;
                    fY = fX;
                    fX = num;
                }
                if (sX > fX)
                {
                    num = sX;
                    sX = fX;
                    fX = num;
                    num = fY;
                    fY = sY;
                    sY = num;
                }
                int dx = fX - sX;
                int dy = fY - sY;

                if (dx == 0)
                    gradient = 1.0f;
                else
                    gradient = dy / (float)dx;
                float y = sY + gradient;
                if (flag)
                {
                    for (int x = sX + 1; x <= fX - 1; x++)
                    {
                        int f = (int)((1 - (y - (int)y))* 255);
                        int s = (int)((y - (int)y)* 255);
                        b.SetPixel((int)y, x, Color.FromArgb(f, f, f));
                        b.SetPixel((int)y + 1, x, Color.FromArgb(s, s, s));
                        y += gradient;
                    }
                }
                else
                {
                    for (int x = sX + 1; x <= fX - 1; x++)
                    {
                        int f = (int)((1 - (y - (int)y))* 255);
                        int s = (int)((y - (int)y)* 255);
                        b.SetPixel(x, (int)y, Color.FromArgb(f, f, f));
                        b.SetPixel(x, (int)y + 1, Color.FromArgb(s, s, s));
                        y += gradient;
                    }
                }
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
    }
}
