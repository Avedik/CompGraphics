namespace FirstTask
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ComboBoxLSystemChange = new System.Windows.Forms.ComboBox();
            this.textBoxChangeGeneration = new System.Windows.Forms.TextBox();
            this.labelGeneration = new System.Windows.Forms.Label();
            this.buttonDrawFractal = new System.Windows.Forms.Button();
            this.labelRandom = new System.Windows.Forms.Label();
            this.labelRandomFrom = new System.Windows.Forms.Label();
            this.labelRandomTo = new System.Windows.Forms.Label();
            this.textBoxRandomFrom = new System.Windows.Forms.TextBox();
            this.textBoxRandomTo = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ComboBoxLSystemChange
            // 
            this.ComboBoxLSystemChange.FormattingEnabled = true;
            this.ComboBoxLSystemChange.Items.AddRange(new object[] {
            "Кривая Коха",
            "Снежинка Коха",
            "Наконечник Серпинского",
            "Ковер Серпинского",
            "Шестиугольная кривая Госпера",
            "Кривая Гильберта",
            "Кривая Дракона",
            "Высокое Дерево",
            "Широкое Дерево",
            "Куст1",
            "Куст2",
            "Куст3"});
            this.ComboBoxLSystemChange.Location = new System.Drawing.Point(9, 57);
            this.ComboBoxLSystemChange.Name = "ComboBoxLSystemChange";
            this.ComboBoxLSystemChange.Size = new System.Drawing.Size(171, 21);
            this.ComboBoxLSystemChange.TabIndex = 0;
            this.ComboBoxLSystemChange.Text = "Кривая Коха";
            this.ComboBoxLSystemChange.SelectedIndexChanged += new System.EventHandler(this.ComboBoxLSystemChange_SelectedIndexChanged);
            // 
            // textBoxChangeGeneration
            // 
            this.textBoxChangeGeneration.Location = new System.Drawing.Point(9, 109);
            this.textBoxChangeGeneration.Name = "textBoxChangeGeneration";
            this.textBoxChangeGeneration.Size = new System.Drawing.Size(86, 20);
            this.textBoxChangeGeneration.TabIndex = 1;
            this.textBoxChangeGeneration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxChangeGeneration_KeyPress);
            // 
            // labelGeneration
            // 
            this.labelGeneration.AutoSize = true;
            this.labelGeneration.Location = new System.Drawing.Point(9, 93);
            this.labelGeneration.Name = "labelGeneration";
            this.labelGeneration.Size = new System.Drawing.Size(63, 13);
            this.labelGeneration.TabIndex = 2;
            this.labelGeneration.Text = "Поколение";
            // 
            // buttonDrawFractal
            // 
            this.buttonDrawFractal.Location = new System.Drawing.Point(9, 208);
            this.buttonDrawFractal.Name = "buttonDrawFractal";
            this.buttonDrawFractal.Size = new System.Drawing.Size(114, 20);
            this.buttonDrawFractal.TabIndex = 3;
            this.buttonDrawFractal.Text = "Нарисовать";
            this.buttonDrawFractal.UseVisualStyleBackColor = true;
            this.buttonDrawFractal.Click += new System.EventHandler(this.buttonDrawFractal_Click);
            // 
            // labelRandom
            // 
            this.labelRandom.AutoSize = true;
            this.labelRandom.Location = new System.Drawing.Point(43, 143);
            this.labelRandom.Name = "labelRandom";
            this.labelRandom.Size = new System.Drawing.Size(71, 13);
            this.labelRandom.TabIndex = 4;
            this.labelRandom.Text = "Случайность";
            // 
            // labelRandomFrom
            // 
            this.labelRandomFrom.AutoSize = true;
            this.labelRandomFrom.Location = new System.Drawing.Point(9, 168);
            this.labelRandomFrom.Name = "labelRandomFrom";
            this.labelRandomFrom.Size = new System.Drawing.Size(18, 13);
            this.labelRandomFrom.TabIndex = 5;
            this.labelRandomFrom.Text = "от";
            // 
            // labelRandomTo
            // 
            this.labelRandomTo.AutoSize = true;
            this.labelRandomTo.Location = new System.Drawing.Point(74, 175);
            this.labelRandomTo.Name = "labelRandomTo";
            this.labelRandomTo.Size = new System.Drawing.Size(19, 13);
            this.labelRandomTo.TabIndex = 6;
            this.labelRandomTo.Text = "до";
            // 
            // textBoxRandomFrom
            // 
            this.textBoxRandomFrom.Location = new System.Drawing.Point(33, 168);
            this.textBoxRandomFrom.Name = "textBoxRandomFrom";
            this.textBoxRandomFrom.Size = new System.Drawing.Size(35, 20);
            this.textBoxRandomFrom.TabIndex = 7;
            this.textBoxRandomFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxRandomFrom_KeyPress);
            // 
            // textBoxRandomTo
            // 
            this.textBoxRandomTo.Location = new System.Drawing.Point(99, 168);
            this.textBoxRandomTo.Name = "textBoxRandomTo";
            this.textBoxRandomTo.Size = new System.Drawing.Size(35, 20);
            this.textBoxRandomTo.TabIndex = 8;
            this.textBoxRandomTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxRandomTo_KeyPress);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(185, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(575, 403);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(9, 243);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(114, 20);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Очистка";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Выбрать L-систему";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 424);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBoxRandomTo);
            this.Controls.Add(this.textBoxRandomFrom);
            this.Controls.Add(this.labelRandomTo);
            this.Controls.Add(this.labelRandomFrom);
            this.Controls.Add(this.labelRandom);
            this.Controls.Add(this.buttonDrawFractal);
            this.Controls.Add(this.labelGeneration);
            this.Controls.Add(this.textBoxChangeGeneration);
            this.Controls.Add(this.ComboBoxLSystemChange);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBoxLSystemChange;
        private System.Windows.Forms.TextBox textBoxChangeGeneration;
        private System.Windows.Forms.Label labelGeneration;
        private System.Windows.Forms.Button buttonDrawFractal;
        private System.Windows.Forms.Label labelRandom;
        private System.Windows.Forms.Label labelRandomFrom;
        private System.Windows.Forms.Label labelRandomTo;
        private System.Windows.Forms.TextBox textBoxRandomFrom;
        private System.Windows.Forms.TextBox textBoxRandomTo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label1;
    }
}

