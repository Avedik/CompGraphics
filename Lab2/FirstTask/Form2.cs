using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FirstTask
{
    public partial class Form2 : Form
    {
        public int[] hist1;
        public int[] hist2;
        public int[] hist3;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Построение гистограмм
            int[] nums = new int[256];
            for (int i = 0; i < 256; ++i)
                nums[i] = i;
            chart1.Series["Кол-во"].Points.DataBindXY(nums, hist1);
            chart2.Series["Кол-во"].Points.DataBindXY(nums, hist2);
            chart3.Series["Кол-во"].Points.DataBindXY(nums, hist3);
        }
    }
}