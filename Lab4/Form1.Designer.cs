﻿namespace Lab4
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dxLabel = new System.Windows.Forms.Label();
            this.dyLabel = new System.Windows.Forms.Label();
            this.chainButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.degreesLabel = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(661, 487);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(682, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "Очистить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Смещение",
            "Поворот вокруг заданной точки",
            "Поворот вокруг центра"});
            this.comboBox1.Location = new System.Drawing.Point(724, 112);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(192, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(739, 178);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "0";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(851, 178);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(51, 20);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(679, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Укажите смещение:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(739, 451);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(163, 48);
            this.button2.TabIndex = 11;
            this.button2.Text = "Применить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(679, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Аффинное преобразование:";
            // 
            // dxLabel
            // 
            this.dxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dxLabel.AutoSize = true;
            this.dxLabel.Enabled = false;
            this.dxLabel.Location = new System.Drawing.Point(712, 181);
            this.dxLabel.Name = "dxLabel";
            this.dxLabel.Size = new System.Drawing.Size(21, 13);
            this.dxLabel.TabIndex = 15;
            this.dxLabel.Text = "dx:";
            // 
            // dyLabel
            // 
            this.dyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dyLabel.AutoSize = true;
            this.dyLabel.Enabled = false;
            this.dyLabel.Location = new System.Drawing.Point(824, 181);
            this.dyLabel.Name = "dyLabel";
            this.dyLabel.Size = new System.Drawing.Size(21, 13);
            this.dyLabel.TabIndex = 16;
            this.dyLabel.Text = "dy:";
            // 
            // chainButton
            // 
            this.chainButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chainButton.Enabled = false;
            this.chainButton.Location = new System.Drawing.Point(813, 12);
            this.chainButton.Name = "chainButton";
            this.chainButton.Size = new System.Drawing.Size(132, 40);
            this.chainButton.TabIndex = 17;
            this.chainButton.Text = "Замкнуть полигон";
            this.chainButton.UseVisualStyleBackColor = true;
            this.chainButton.Click += new System.EventHandler(this.chainButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(679, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Укажите угол поворота:";
            // 
            // degreesLabel
            // 
            this.degreesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.degreesLabel.AutoSize = true;
            this.degreesLabel.Enabled = false;
            this.degreesLabel.Location = new System.Drawing.Point(748, 240);
            this.degreesLabel.Name = "degreesLabel";
            this.degreesLabel.Size = new System.Drawing.Size(53, 13);
            this.degreesLabel.TabIndex = 19;
            this.degreesLabel.Text = "Градусы:";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(814, 237);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(51, 20);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(957, 511);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.degreesLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chainButton);
            this.Controls.Add(this.dyLabel);
            this.Controls.Add(this.dxLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Аффинные преобразования";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label dxLabel;
        private System.Windows.Forms.Label dyLabel;
        private System.Windows.Forms.Button chainButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label degreesLabel;
        private System.Windows.Forms.TextBox textBox3;
    }
}
