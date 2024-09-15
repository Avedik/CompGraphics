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

        public Form1()
        {
            InitializeComponent();
        }

        private void RGBToHSV(Color color, out double hue, out double saturation, out double value)
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

                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = true;
                    button2.Enabled = true;

                    trackBar1.Value = 0;
                    trackBar1_Scroll(sender, e);
                    trackBar2.Value = 0;
                    trackBar2_Scroll(sender, e);
                    trackBar3.Value = 0;
                    trackBar3_Scroll(sender, e);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Save("image.png");
            MessageBox.Show("Изображение успешно сохранено.", "Сохранение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            hueView.Text = trackBar1.Value + "";
            double hDelta = trackBar1.Value * 2.0;
            pictureBox1.Image = bitmap.Select(color => {
                double hue, saturation, value;
                RGBToHSV(Color.FromArgb(color.R, color.G, color.B), out hue,
                    out saturation, out value);

                hue = Math.Max(Math.Min(hue + hDelta, 359.99), 0);
                return HSVToRGB(hue, saturation, value);
            });
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            saturationView.Text = trackBar2.Value + "";
            double sDelta = trackBar2.Value / 100.0;
            pictureBox1.Image = bitmap.Select(color => {
                double hue, saturation, value;
                RGBToHSV(Color.FromArgb(color.R, color.G, color.B), out hue,
                    out saturation, out value);

                saturation = Math.Max(Math.Min(saturation + sDelta, 1), 0);
                return HSVToRGB(hue, saturation, value);
            });
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            valueView.Text = trackBar3.Value + "";
            double vDelta = trackBar3.Value / 100.0;

            pictureBox1.Image = bitmap.Select(color => {
                double hue, saturation, value;
                RGBToHSV(Color.FromArgb(color.R, color.G, color.B), out hue,
                    out saturation, out value);

                value = Math.Max(Math.Min(value + vDelta, 1), 0);
                return HSVToRGB(hue, saturation, value);
            });
        }
    }
}
