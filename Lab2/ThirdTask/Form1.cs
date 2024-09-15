using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastBitmap;

namespace ThirdTask
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        private double hue, saturation, value;

        public Form1()
        {
            InitializeComponent();
        }

        private void RGBToHSV(Color color)
        {
            double R = color.R / 255.0;
            double G = color.G / 255.0;
            double B = color.B / 255.0;

            double MAX = Math.Max(R, Math.Max(G, B));
            double MIN = Math.Min(R, Math.Min(G, B));
            double difference = MAX - MIN;

            if (difference == 0)
                hue = 0;
            else
            {
                if (MAX == R)
                    hue = 60.0 * (G - B) / difference + (color.G >= color.B ? 0 : 360);
                else if (MAX == G)
                    hue = 60.0 * (B - R) / difference + 120;
                else
                    hue = 60.0 * (R - G) / difference + 240;
            }

            saturation = MAX == 0 ? 0 : 1 - MIN / MAX;
            value = MAX;
        }

        private Color HSVToRGB(double H, double S, double V)
        {
            double R, G, B;
            double f = H / 60 - (int)(H / 60);
            double p = V * (1 - S);
            double q = V * (1 - f * S);
            double t = V * (1 - (1 - f) * S);

            switch ((int)(H / 60) % 6)
            {
                case 0:
                    R = V;
                    G = t;
                    B = p;
                    break;
                case 1:
                    R = q;
                    G = V;
                    B = p;
                    break;
                case 2:
                    R = p;
                    G = V;
                    B = t;
                    break;
                case 3:
                    R = p;
                    G = q;
                    B = V;
                    break;
                case 4:
                    R = t;
                    G = p;
                    B = V;
                    break;
                default:
                    R = V;
                    G = p;
                    B = q;
                    break;
            }

            int red = Convert.ToInt32(R * 255);
            int green = Convert.ToInt32(G * 255);
            int blue = Convert.ToInt32(B * 255); ;
            return Color.FromArgb(red, green, blue);
        }

        private void Transform()
        {
            using (var fastBitmap = new FastBitmap.FastBitmap(bitmap))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var color = fastBitmap[x, y];
                        RGBToHSV(Color.FromArgb(color.R, color.G, color.B));

                        hue = Math.Min(hue + trackBar1.Value / 4.0, 359);
                        saturation = Math.Min(saturation + trackBar2.Value / 1000.0, 1);
                        value = Math.Min(value + trackBar3.Value / 1000.0, 1);
                        fastBitmap[x, y] = HSVToRGB(hue, saturation, value);
                    }
            }
            pictureBox1.Image = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Открыть изображение";
                dialog.Filter = "Image Files|" + string.Join(";",
                    ImageCodecInfo.GetImageEncoders().Select(encoder => encoder.FilenameExtension));

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap = new Bitmap(dialog.FileName);
                    pictureBox1.Image = bitmap;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Transform();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Transform();
        }

    }
}
