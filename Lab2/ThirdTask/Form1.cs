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

        }

    }
}
