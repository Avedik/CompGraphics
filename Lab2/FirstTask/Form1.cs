using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastBmap;

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

        // Кнопка загрузки картинки
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

        // Кнопка построения картинок
        private void button2_Click(object sender, EventArgs e)
        {
            int x, y;
            hist1 = new int[256];
            hist2 = new int[256];
            hist3 = new int[256];

            // PAL / NTSC
            b2 = b2.Select(color => {
                        int Y = (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
                        ++hist1[Y];
                        return Color.FromArgb(Y, Y, Y);
            });

            // HDTV
            b3 = b3.Select(color => {
                        int Y = (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
                        ++hist2[Y];
                        return Color.FromArgb(Y, Y, Y);
            });

            // Разница
            using (var fastBitmap2 = new FastBitmap(b2))
            using (var fastBitmap3 = new FastBitmap(b3))
            using (var fastBitmap4 = new FastBitmap(b4))
            {
                for (x = 0; x < fastBitmap4.Width; x++)
                    for (y = 0; y < fastBitmap4.Height; y++)
                    {
                        Color ColorOne = fastBitmap2[x, y];
                        Color ColorTwo = fastBitmap3[x, y];
                        int d = Math.Abs(ColorTwo.R - ColorOne.R);
                        ++hist3[d];
                        fastBitmap4[x, y] = Color.FromArgb(d, d, d);
                    }
            }

            // Вывод картинок
            pictureBox2.Image = b2;
            pictureBox3.Image = b3;
            pictureBox4.Image = b4;
        }

        // Кнопка вывода гистограмм
        private void button3_Click(object sender, EventArgs e)
        {
            form2.hist1 = hist1;
            form2.hist2 = hist2;
            form2.hist3 = hist3;
            form2.Show();
        }
    }
}
