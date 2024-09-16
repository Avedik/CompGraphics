using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SecondTask
{
    public partial class Form1 : Form
    {
        private Bitmap redHistogramBitmap;
        private Bitmap greenHistogramBitmap;
        private Bitmap blueHistogramBitmap;

        public Form1()
        {
            InitializeComponent();
            btnLoadImage.Click += btnLoadImage_Click;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                    pbOriginal.Image = bitmap;
                    pbRedChannel.Image = ExtractChannel(bitmap, 'R');
                    pbGreenChannel.Image = ExtractChannel(bitmap, 'G');
                    pbBlueChannel.Image = ExtractChannel(bitmap, 'B');

                    // Создание гистограмм
                    redHistogramBitmap = CreateHistogramImage(BuildHistogram(bitmap, 'R'), panelRedHistogram.Width, panelRedHistogram.Height);
                    greenHistogramBitmap = CreateHistogramImage(BuildHistogram(bitmap, 'G'), panelGreenHistogram.Width, panelGreenHistogram.Height);
                    blueHistogramBitmap = CreateHistogramImage(BuildHistogram(bitmap, 'B'), panelBlueHistogram.Width, panelBlueHistogram.Height);

                    // Отображение гистограмм
                    panelRedHistogram.BackgroundImage = redHistogramBitmap;
                    panelGreenHistogram.BackgroundImage = greenHistogramBitmap;
                    panelBlueHistogram.BackgroundImage = blueHistogramBitmap;
                }
            }
        }

        private Bitmap ExtractChannel(Bitmap bitmap, char channel)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    Color newColor = channel switch
                    {
                        'R' => Color.FromArgb(pixelColor.R, 0, 0),
                        'G' => Color.FromArgb(0, pixelColor.G, 0),
                        'B' => Color.FromArgb(0, 0, pixelColor.B),
                        _ => Color.Black
                    };
                    result.SetPixel(x, y, newColor);
                }
            }
            return result;
        }

        private int[] BuildHistogram(Bitmap bitmap, char channel)
        {
            int[] histogram = new int[256];

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int value = channel switch
                    {
                        'R' => pixelColor.R,
                        'G' => pixelColor.G,
                        'B' => pixelColor.B,
                        _ => 0
                    };
                    histogram[value]++;
                }
            }
            return histogram;
        }

        private Bitmap CreateHistogramImage(int[] histogram, int width, int height)
        {
            Bitmap histogramBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(histogramBitmap))
            {
                g.Clear(Color.White);

                // Нормализация высоты
                int max = histogram.Max();
                for (int x = 0; x < 256; x++)
                {
                    int normalizedHeight = (int)((histogram[x] / (float)max) * height);
                    g.DrawLine(Pens.Black, x, height, x, height - normalizedHeight);
                }
            }
            return histogramBitmap;
        }
    }
}
