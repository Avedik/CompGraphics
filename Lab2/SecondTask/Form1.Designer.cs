namespace SecondTask
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.pbOriginal = new System.Windows.Forms.PictureBox();
            this.pbRedChannel = new System.Windows.Forms.PictureBox();
            this.pbGreenChannel = new System.Windows.Forms.PictureBox();
            this.pbBlueChannel = new System.Windows.Forms.PictureBox();
            this.panelRedHistogram = new System.Windows.Forms.Panel();
            this.panelGreenHistogram = new System.Windows.Forms.Panel();
            this.panelBlueHistogram = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGreenChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBlueChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(307, 334);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(75, 23);
            this.btnLoadImage.TabIndex = 0;
            this.btnLoadImage.Text = "Загрузить";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            // 
            // pbOriginal
            // 
            this.pbOriginal.Location = new System.Drawing.Point(21, 205);
            this.pbOriginal.Name = "pbOriginal";
            this.pbOriginal.Size = new System.Drawing.Size(256, 256);
            this.pbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbOriginal.TabIndex = 1;
            this.pbOriginal.TabStop = false;
            // 
            // pbRedChannel
            // 
            this.pbRedChannel.Location = new System.Drawing.Point(352, 32);
            this.pbRedChannel.Name = "pbRedChannel";
            this.pbRedChannel.Size = new System.Drawing.Size(256, 256);
            this.pbRedChannel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbRedChannel.TabIndex = 2;
            this.pbRedChannel.TabStop = false;
            // 
            // pbGreenChannel
            // 
            this.pbGreenChannel.Location = new System.Drawing.Point(614, 32);
            this.pbGreenChannel.Name = "pbGreenChannel";
            this.pbGreenChannel.Size = new System.Drawing.Size(256, 256);
            this.pbGreenChannel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbGreenChannel.TabIndex = 3;
            this.pbGreenChannel.TabStop = false;
            // 
            // pbBlueChannel
            // 
            this.pbBlueChannel.Location = new System.Drawing.Point(876, 32);
            this.pbBlueChannel.Name = "pbBlueChannel";
            this.pbBlueChannel.Size = new System.Drawing.Size(256, 256);
            this.pbBlueChannel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBlueChannel.TabIndex = 4;
            this.pbBlueChannel.TabStop = false;
            // 
            // panelRedHistogram
            // 
            this.panelRedHistogram.Location = new System.Drawing.Point(352, 391);
            this.panelRedHistogram.Name = "panelRedHistogram";
            this.panelRedHistogram.Size = new System.Drawing.Size(256, 256);
            this.panelRedHistogram.TabIndex = 5;
            // 
            // panelGreenHistogram
            // 
            this.panelGreenHistogram.Location = new System.Drawing.Point(614, 391);
            this.panelGreenHistogram.Name = "panelGreenHistogram";
            this.panelGreenHistogram.Size = new System.Drawing.Size(256, 256);
            this.panelGreenHistogram.TabIndex = 6;
            // 
            // panelBlueHistogram
            // 
            this.panelBlueHistogram.Location = new System.Drawing.Point(876, 391);
            this.panelBlueHistogram.Name = "panelBlueHistogram";
            this.panelBlueHistogram.Size = new System.Drawing.Size(256, 256);
            this.panelBlueHistogram.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 678);
            this.Controls.Add(this.panelBlueHistogram);
            this.Controls.Add(this.panelGreenHistogram);
            this.Controls.Add(this.panelRedHistogram);
            this.Controls.Add(this.pbBlueChannel);
            this.Controls.Add(this.pbGreenChannel);
            this.Controls.Add(this.pbRedChannel);
            this.Controls.Add(this.pbOriginal);
            this.Controls.Add(this.btnLoadImage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGreenChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBlueChannel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.PictureBox pbOriginal;
        private System.Windows.Forms.PictureBox pbRedChannel;
        private System.Windows.Forms.PictureBox pbGreenChannel;
        private System.Windows.Forms.PictureBox pbBlueChannel;
        private System.Windows.Forms.Panel panelRedHistogram;
        private System.Windows.Forms.Panel panelGreenHistogram;
        private System.Windows.Forms.Panel panelBlueHistogram;
    }
}

