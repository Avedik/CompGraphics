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
    public partial class Form1 : Form
    {
        Bitmap b1;
        Bitmap b2;
        Bitmap b3;
        Bitmap b4;
        int[] hist1;
        int[] hist2;
        int[] hist3;
        private Form2 form2 = new Form2();
        public Form1()
        {
            InitializeComponent();
            AddOwnedForm(form2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    b1 = new Bitmap(open_dialog.FileName);
                    b2 = new Bitmap(open_dialog.FileName);
                    b3 = new Bitmap(open_dialog.FileName);
                    b4 = new Bitmap(open_dialog.FileName);
                    pictureBox1.Image = b1;
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int x, y;
            hist1 = new int[256];
            hist2 = new int[256];
            hist3 = new int[256];

            for (x=0; x<b2.Width; ++x)
            {
                for (y=0; y<b2.Height; ++y)
                {
                    Color pixelColor = b2.GetPixel(x, y);
                    int Y = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    ++hist1[Y];
                    Color newColor = Color.FromArgb(Y, Y, Y);
                    b2.SetPixel(x, y, newColor);
                }
            }

            for (x=0; x<b3.Width; ++x)
            {
                for (y=0; y<b3.Height; ++y)
                {
                    Color pixelColor = b3.GetPixel(x, y);
                    int Y = (int)(0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B);
                    ++hist2[Y];
                    Color newColor = Color.FromArgb(Y, Y, Y);
                    b3.SetPixel(x, y, newColor);
                }
            }

            for (x=0; x<b4.Width; ++x)
            {
                for (y=0; y<b4.Height; ++y)
                {
                    Color pixelColorOne = b2.GetPixel(x, y);
                    Color pixelColorTwo = b3.GetPixel(x, y);
                    int d = Math.Abs(pixelColorTwo.R - pixelColorOne.R);
                    ++hist3[d];
                    Color newColor = Color.FromArgb(d, d, d);
                    b4.SetPixel(x, y, newColor);
                }
            }

            pictureBox2.Image = b2;
            pictureBox3.Image = b3;
            pictureBox4.Image = b4;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form2.hist1 = hist1;
            form2.hist2 = hist2;
            form2.hist3 = hist3;
            form2.Show();
        }
    }
}
