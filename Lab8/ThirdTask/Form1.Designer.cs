using System;

namespace ThirdTask
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rbPerspective = new System.Windows.Forms.RadioButton();
            this.buttonShape = new System.Windows.Forms.Button();
            this.selectShape = new System.Windows.Forms.ComboBox();
            this.buttonShift = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.textAngle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonScale = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textShiftZ = new System.Windows.Forms.TextBox();
            this.textShiftY = new System.Windows.Forms.TextBox();
            this.textShiftX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textScaleZ = new System.Windows.Forms.TextBox();
            this.textScaleY = new System.Windows.Forms.TextBox();
            this.textScaleX = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.selectAxis = new System.Windows.Forms.ComboBox();
            this.rbIsometric = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbCenter = new System.Windows.Forms.RadioButton();
            this.rbWorldCenter = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.selectPlane = new System.Windows.Forms.ComboBox();
            this.buttonReflection = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxAngleRotCenter = new System.Windows.Forms.TextBox();
            this.selectRollAxis = new System.Windows.Forms.ComboBox();
            this.buttonRoll = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.textX1 = new System.Windows.Forms.TextBox();
            this.textX2 = new System.Windows.Forms.TextBox();
            this.textY1 = new System.Windows.Forms.TextBox();
            this.textY2 = new System.Windows.Forms.TextBox();
            this.textZ1 = new System.Windows.Forms.TextBox();
            this.textZ2 = new System.Windows.Forms.TextBox();
            this.textAngleForLineRotation = new System.Windows.Forms.TextBox();
            this.buttonRotateAroundLine = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.textCamX = new System.Windows.Forms.TextBox();
            this.textCamY = new System.Windows.Forms.TextBox();
            this.textCamZ = new System.Windows.Forms.TextBox();
            this.textZView = new System.Windows.Forms.TextBox();
            this.textYView = new System.Windows.Forms.TextBox();
            this.textXView = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.buttonW = new System.Windows.Forms.Button();
            this.buttonA = new System.Windows.Forms.Button();
            this.buttonS = new System.Windows.Forms.Button();
            this.buttonD = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(789, 666);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // rbPerspective
            // 
            this.rbPerspective.AutoSize = true;
            this.rbPerspective.BackColor = System.Drawing.Color.White;
            this.rbPerspective.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbPerspective.Location = new System.Drawing.Point(11, 11);
            this.rbPerspective.Margin = new System.Windows.Forms.Padding(2);
            this.rbPerspective.Name = "rbPerspective";
            this.rbPerspective.Size = new System.Drawing.Size(155, 17);
            this.rbPerspective.TabIndex = 17;
            this.rbPerspective.Text = "Перспективная проекция";
            this.rbPerspective.UseVisualStyleBackColor = false;
            this.rbPerspective.CheckedChanged += new System.EventHandler(this.rbPerspective_CheckedChanged);
            // 
            // buttonShape
            // 
            this.buttonShape.Location = new System.Drawing.Point(810, 95);
            this.buttonShape.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShape.Name = "buttonShape";
            this.buttonShape.Size = new System.Drawing.Size(141, 23);
            this.buttonShape.TabIndex = 1;
            this.buttonShape.Text = "Нарисовать";
            this.buttonShape.UseVisualStyleBackColor = true;
            this.buttonShape.Click += new System.EventHandler(this.buttonShape_Click);
            // 
            // selectShape
            // 
            this.selectShape.FormattingEnabled = true;
            this.selectShape.Items.AddRange(new object[] {
            "Тетраэдр",
            "Гексаэдр",
            "Октаэдр",
            "Икосаэдр",
            "Додекаэдр"});
            this.selectShape.Location = new System.Drawing.Point(810, 45);
            this.selectShape.Margin = new System.Windows.Forms.Padding(2);
            this.selectShape.Name = "selectShape";
            this.selectShape.Size = new System.Drawing.Size(148, 21);
            this.selectShape.TabIndex = 2;
            this.selectShape.SelectedIndexChanged += new System.EventHandler(this.comboBoxShape_SelectedIndexChanged);
            // 
            // buttonShift
            // 
            this.buttonShift.Enabled = false;
            this.buttonShift.Location = new System.Drawing.Point(841, 189);
            this.buttonShift.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShift.Name = "buttonShift";
            this.buttonShift.Size = new System.Drawing.Size(141, 23);
            this.buttonShift.TabIndex = 1;
            this.buttonShift.Text = "Сместить";
            this.buttonShift.UseVisualStyleBackColor = true;
            this.buttonShift.Click += new System.EventHandler(this.buttonShift_Click);
            // 
            // buttonRotate
            // 
            this.buttonRotate.Enabled = false;
            this.buttonRotate.Location = new System.Drawing.Point(1063, 189);
            this.buttonRotate.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(141, 23);
            this.buttonRotate.TabIndex = 1;
            this.buttonRotate.Text = "Повернуть";
            this.buttonRotate.UseVisualStyleBackColor = true;
            this.buttonRotate.Click += new System.EventHandler(this.buttonRotate_Click);
            // 
            // textAngle
            // 
            this.textAngle.Enabled = false;
            this.textAngle.Location = new System.Drawing.Point(1087, 157);
            this.textAngle.Margin = new System.Windows.Forms.Padding(2);
            this.textAngle.MaxLength = 5;
            this.textAngle.Name = "textAngle";
            this.textAngle.Size = new System.Drawing.Size(42, 20);
            this.textAngle.TabIndex = 3;
            this.textAngle.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1051, 161);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Угол:";
            // 
            // buttonScale
            // 
            this.buttonScale.Enabled = false;
            this.buttonScale.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonScale.Location = new System.Drawing.Point(949, 362);
            this.buttonScale.Margin = new System.Windows.Forms.Padding(2);
            this.buttonScale.Name = "buttonScale";
            this.buttonScale.Size = new System.Drawing.Size(141, 23);
            this.buttonScale.TabIndex = 1;
            this.buttonScale.Text = "Масштабировать";
            this.buttonScale.UseVisualStyleBackColor = true;
            this.buttonScale.Click += new System.EventHandler(this.buttonScale_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(957, 158);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Z:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(879, 158);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(798, 158);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Х:";
            // 
            // textShiftZ
            // 
            this.textShiftZ.Enabled = false;
            this.textShiftZ.Location = new System.Drawing.Point(976, 156);
            this.textShiftZ.Margin = new System.Windows.Forms.Padding(2);
            this.textShiftZ.MaxLength = 5;
            this.textShiftZ.Name = "textShiftZ";
            this.textShiftZ.Size = new System.Drawing.Size(32, 20);
            this.textShiftZ.TabIndex = 5;
            this.textShiftZ.Text = "0";
            // 
            // textShiftY
            // 
            this.textShiftY.Enabled = false;
            this.textShiftY.Location = new System.Drawing.Point(899, 156);
            this.textShiftY.Margin = new System.Windows.Forms.Padding(2);
            this.textShiftY.MaxLength = 5;
            this.textShiftY.Name = "textShiftY";
            this.textShiftY.Size = new System.Drawing.Size(32, 20);
            this.textShiftY.TabIndex = 6;
            this.textShiftY.Text = "0";
            // 
            // textShiftX
            // 
            this.textShiftX.Enabled = false;
            this.textShiftX.Location = new System.Drawing.Point(819, 156);
            this.textShiftX.Margin = new System.Windows.Forms.Padding(2);
            this.textShiftX.MaxLength = 5;
            this.textShiftX.Name = "textShiftX";
            this.textShiftX.Size = new System.Drawing.Size(32, 20);
            this.textShiftX.TabIndex = 7;
            this.textShiftX.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1055, 252);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "coeffZ:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(980, 252);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "coeffY:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(905, 252);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "coeffХ:";
            // 
            // textScaleZ
            // 
            this.textScaleZ.Enabled = false;
            this.textScaleZ.Location = new System.Drawing.Point(1097, 249);
            this.textScaleZ.Margin = new System.Windows.Forms.Padding(2);
            this.textScaleZ.MaxLength = 5;
            this.textScaleZ.Name = "textScaleZ";
            this.textScaleZ.Size = new System.Drawing.Size(32, 20);
            this.textScaleZ.TabIndex = 11;
            this.textScaleZ.Text = "1";
            // 
            // textScaleY
            // 
            this.textScaleY.Enabled = false;
            this.textScaleY.Location = new System.Drawing.Point(1021, 249);
            this.textScaleY.Margin = new System.Windows.Forms.Padding(2);
            this.textScaleY.MaxLength = 5;
            this.textScaleY.Name = "textScaleY";
            this.textScaleY.Size = new System.Drawing.Size(32, 20);
            this.textScaleY.TabIndex = 12;
            this.textScaleY.Text = "1";
            // 
            // textScaleX
            // 
            this.textScaleX.Enabled = false;
            this.textScaleX.Location = new System.Drawing.Point(946, 249);
            this.textScaleX.Margin = new System.Windows.Forms.Padding(2);
            this.textScaleX.MaxLength = 5;
            this.textScaleX.Name = "textScaleX";
            this.textScaleX.Size = new System.Drawing.Size(32, 20);
            this.textScaleX.TabIndex = 13;
            this.textScaleX.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1154, 162);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Ось:";
            // 
            // selectAxis
            // 
            this.selectAxis.Enabled = false;
            this.selectAxis.FormattingEnabled = true;
            this.selectAxis.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.selectAxis.Location = new System.Drawing.Point(1184, 157);
            this.selectAxis.Margin = new System.Windows.Forms.Padding(2);
            this.selectAxis.Name = "selectAxis";
            this.selectAxis.Size = new System.Drawing.Size(32, 21);
            this.selectAxis.TabIndex = 2;
            this.selectAxis.SelectedIndexChanged += new System.EventHandler(this.selectAxis_SelectedIndexChanged);
            // 
            // rbIsometric
            // 
            this.rbIsometric.AutoSize = true;
            this.rbIsometric.BackColor = System.Drawing.Color.White;
            this.rbIsometric.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbIsometric.Checked = true;
            this.rbIsometric.Location = new System.Drawing.Point(11, 38);
            this.rbIsometric.Margin = new System.Windows.Forms.Padding(2);
            this.rbIsometric.Name = "rbIsometric";
            this.rbIsometric.Size = new System.Drawing.Size(162, 17);
            this.rbIsometric.TabIndex = 17;
            this.rbIsometric.TabStop = true;
            this.rbIsometric.Text = "Изометрическая проекция";
            this.rbIsometric.UseVisualStyleBackColor = false;
            this.rbIsometric.CheckedChanged += new System.EventHandler(this.rbIsometric_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(966, 287);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Относительно:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbCenter);
            this.panel1.Controls.Add(this.rbWorldCenter);
            this.panel1.Location = new System.Drawing.Point(922, 311);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 36);
            this.panel1.TabIndex = 19;
            // 
            // rbCenter
            // 
            this.rbCenter.AutoSize = true;
            this.rbCenter.Location = new System.Drawing.Point(70, 7);
            this.rbCenter.Name = "rbCenter";
            this.rbCenter.Size = new System.Drawing.Size(98, 17);
            this.rbCenter.TabIndex = 1;
            this.rbCenter.TabStop = true;
            this.rbCenter.Text = "своего центра";
            this.rbCenter.UseVisualStyleBackColor = true;
            // 
            // rbWorldCenter
            // 
            this.rbWorldCenter.AutoSize = true;
            this.rbWorldCenter.Checked = true;
            this.rbWorldCenter.Location = new System.Drawing.Point(3, 7);
            this.rbWorldCenter.Margin = new System.Windows.Forms.Padding(2);
            this.rbWorldCenter.Name = "rbWorldCenter";
            this.rbWorldCenter.Size = new System.Drawing.Size(55, 17);
            this.rbWorldCenter.TabIndex = 0;
            this.rbWorldCenter.TabStop = true;
            this.rbWorldCenter.Text = "(0,0,0)";
            this.rbWorldCenter.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(969, 50);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(46, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Оси";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(807, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Многогранник";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(875, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Смещение";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1043, 131);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(184, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Поворот относительно оси на угол";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(957, 223);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Масштабирование";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(865, 390);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Отражение";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(791, 420);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(138, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "Относительно плоскости:";
            // 
            // selectPlane
            // 
            this.selectPlane.FormattingEnabled = true;
            this.selectPlane.Items.AddRange(new object[] {
            "XY",
            "YZ",
            "XZ"});
            this.selectPlane.Location = new System.Drawing.Point(932, 417);
            this.selectPlane.Name = "selectPlane";
            this.selectPlane.Size = new System.Drawing.Size(76, 21);
            this.selectPlane.TabIndex = 26;
            this.selectPlane.SelectedIndexChanged += new System.EventHandler(this.selectPlane_SelectedIndexChanged);
            // 
            // buttonReflection
            // 
            this.buttonReflection.Location = new System.Drawing.Point(833, 453);
            this.buttonReflection.Name = "buttonReflection";
            this.buttonReflection.Size = new System.Drawing.Size(141, 23);
            this.buttonReflection.TabIndex = 27;
            this.buttonReflection.Text = "Отразить";
            this.buttonReflection.UseVisualStyleBackColor = true;
            this.buttonReflection.Click += new System.EventHandler(this.buttonReflection_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1037, 421);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 28;
            this.label16.Text = "Угол:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1140, 421);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 29;
            this.label17.Text = "Ось:";
            // 
            // textBoxAngleRotCenter
            // 
            this.textBoxAngleRotCenter.Location = new System.Drawing.Point(1078, 418);
            this.textBoxAngleRotCenter.Name = "textBoxAngleRotCenter";
            this.textBoxAngleRotCenter.Size = new System.Drawing.Size(45, 20);
            this.textBoxAngleRotCenter.TabIndex = 30;
            this.textBoxAngleRotCenter.Text = "0";
            // 
            // selectRollAxis
            // 
            this.selectRollAxis.Enabled = false;
            this.selectRollAxis.FormattingEnabled = true;
            this.selectRollAxis.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.selectRollAxis.Location = new System.Drawing.Point(1178, 418);
            this.selectRollAxis.Margin = new System.Windows.Forms.Padding(2);
            this.selectRollAxis.Name = "selectRollAxis";
            this.selectRollAxis.Size = new System.Drawing.Size(49, 21);
            this.selectRollAxis.TabIndex = 31;
            this.selectRollAxis.SelectedIndexChanged += new System.EventHandler(this.selectRollAxis_SelectedIndexChanged);
            // 
            // buttonRoll
            // 
            this.buttonRoll.Enabled = false;
            this.buttonRoll.Location = new System.Drawing.Point(1078, 453);
            this.buttonRoll.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRoll.Name = "buttonRoll";
            this.buttonRoll.Size = new System.Drawing.Size(141, 26);
            this.buttonRoll.TabIndex = 32;
            this.buttonRoll.Text = "Вращать вокруг центра";
            this.buttonRoll.UseVisualStyleBackColor = true;
            this.buttonRoll.Click += new System.EventHandler(this.buttonRoll_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(966, 494);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(113, 13);
            this.label18.TabIndex = 33;
            this.label18.Text = "Координаты прямой:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(920, 527);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(21, 13);
            this.label19.TabIndex = 34;
            this.label19.Text = "x1:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(920, 550);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(21, 13);
            this.label20.TabIndex = 35;
            this.label20.Text = "x2:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(991, 527);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(21, 13);
            this.label21.TabIndex = 36;
            this.label21.Text = "y1:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(991, 550);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(21, 13);
            this.label22.TabIndex = 37;
            this.label22.Text = "y2:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(1060, 527);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(21, 13);
            this.label23.TabIndex = 38;
            this.label23.Text = "z1:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(1060, 549);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(21, 13);
            this.label24.TabIndex = 39;
            this.label24.Text = "z2:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(976, 579);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(35, 13);
            this.label25.TabIndex = 40;
            this.label25.Text = "Угол:";
            // 
            // textX1
            // 
            this.textX1.Enabled = false;
            this.textX1.Location = new System.Drawing.Point(944, 524);
            this.textX1.Margin = new System.Windows.Forms.Padding(2);
            this.textX1.Name = "textX1";
            this.textX1.Size = new System.Drawing.Size(38, 20);
            this.textX1.TabIndex = 41;
            this.textX1.Text = "0";
            // 
            // textX2
            // 
            this.textX2.Location = new System.Drawing.Point(944, 546);
            this.textX2.Name = "textX2";
            this.textX2.Size = new System.Drawing.Size(38, 20);
            this.textX2.TabIndex = 42;
            this.textX2.Text = "0";
            // 
            // textY1
            // 
            this.textY1.Location = new System.Drawing.Point(1013, 523);
            this.textY1.Name = "textY1";
            this.textY1.Size = new System.Drawing.Size(38, 20);
            this.textY1.TabIndex = 43;
            this.textY1.Text = "0";
            // 
            // textY2
            // 
            this.textY2.Location = new System.Drawing.Point(1013, 547);
            this.textY2.Name = "textY2";
            this.textY2.Size = new System.Drawing.Size(38, 20);
            this.textY2.TabIndex = 44;
            this.textY2.Text = "0";
            // 
            // textZ1
            // 
            this.textZ1.Location = new System.Drawing.Point(1084, 524);
            this.textZ1.Name = "textZ1";
            this.textZ1.Size = new System.Drawing.Size(38, 20);
            this.textZ1.TabIndex = 45;
            this.textZ1.Text = "0";
            // 
            // textZ2
            // 
            this.textZ2.Location = new System.Drawing.Point(1084, 548);
            this.textZ2.Name = "textZ2";
            this.textZ2.Size = new System.Drawing.Size(38, 20);
            this.textZ2.TabIndex = 46;
            this.textZ2.Text = "0";
            // 
            // textAngleForLineRotation
            // 
            this.textAngleForLineRotation.Enabled = false;
            this.textAngleForLineRotation.Location = new System.Drawing.Point(1014, 576);
            this.textAngleForLineRotation.Margin = new System.Windows.Forms.Padding(2);
            this.textAngleForLineRotation.Name = "textAngleForLineRotation";
            this.textAngleForLineRotation.Size = new System.Drawing.Size(38, 20);
            this.textAngleForLineRotation.TabIndex = 47;
            this.textAngleForLineRotation.Text = "0";
            // 
            // buttonRotateAroundLine
            // 
            this.buttonRotateAroundLine.Enabled = false;
            this.buttonRotateAroundLine.Location = new System.Drawing.Point(958, 611);
            this.buttonRotateAroundLine.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRotateAroundLine.Name = "buttonRotateAroundLine";
            this.buttonRotateAroundLine.Size = new System.Drawing.Size(138, 43);
            this.buttonRotateAroundLine.TabIndex = 48;
            this.buttonRotateAroundLine.Text = "Повернуть вокруг прямой";
            this.buttonRotateAroundLine.UseVisualStyleBackColor = true;
            this.buttonRotateAroundLine.Click += new System.EventHandler(this.buttonRotateAroundLine_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(1037, 15);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(38, 13);
            this.label26.TabIndex = 49;
            this.label26.Text = "CamX:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(1037, 42);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(38, 13);
            this.label27.TabIndex = 50;
            this.label27.Text = "CamY:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(1037, 66);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(38, 13);
            this.label28.TabIndex = 51;
            this.label28.Text = "CamZ:";
            // 
            // textCamX
            // 
            this.textCamX.Location = new System.Drawing.Point(1084, 10);
            this.textCamX.Name = "textCamX";
            this.textCamX.Size = new System.Drawing.Size(45, 20);
            this.textCamX.TabIndex = 52;
            this.textCamX.Text = "600";
            // 
            // textCamY
            // 
            this.textCamY.Location = new System.Drawing.Point(1084, 39);
            this.textCamY.Name = "textCamY";
            this.textCamY.Size = new System.Drawing.Size(45, 20);
            this.textCamY.TabIndex = 53;
            this.textCamY.Text = "600";
            // 
            // textCamZ
            // 
            this.textCamZ.Location = new System.Drawing.Point(1084, 65);
            this.textCamZ.Name = "textCamZ";
            this.textCamZ.Size = new System.Drawing.Size(45, 20);
            this.textCamZ.TabIndex = 54;
            this.textCamZ.Text = "600";
            // 
            // textZView
            // 
            this.textZView.Location = new System.Drawing.Point(1194, 65);
            this.textZView.Name = "textZView";
            this.textZView.Size = new System.Drawing.Size(45, 20);
            this.textZView.TabIndex = 60;
            this.textZView.Text = "0";
            // 
            // textYView
            // 
            this.textYView.Location = new System.Drawing.Point(1194, 39);
            this.textYView.Name = "textYView";
            this.textYView.Size = new System.Drawing.Size(45, 20);
            this.textYView.TabIndex = 59;
            this.textYView.Text = "0";
            // 
            // textXView
            // 
            this.textXView.Location = new System.Drawing.Point(1194, 10);
            this.textXView.Name = "textXView";
            this.textXView.Size = new System.Drawing.Size(45, 20);
            this.textXView.TabIndex = 58;
            this.textXView.Text = "0";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(1147, 66);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(38, 13);
            this.label29.TabIndex = 57;
            this.label29.Text = "zView:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(1147, 42);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(38, 13);
            this.label30.TabIndex = 56;
            this.label30.Text = "yView:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(1147, 15);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(38, 13);
            this.label31.TabIndex = 55;
            this.label31.Text = "xView:";
            // 
            // buttonW
            // 
            this.buttonW.Location = new System.Drawing.Point(983, 95);
            this.buttonW.Name = "buttonW";
            this.buttonW.Size = new System.Drawing.Size(61, 23);
            this.buttonW.TabIndex = 61;
            this.buttonW.Text = "W";
            this.buttonW.UseVisualStyleBackColor = true;
            this.buttonW.Click += new System.EventHandler(this.W_Click);
            // 
            // buttonA
            // 
            this.buttonA.Location = new System.Drawing.Point(1050, 95);
            this.buttonA.Name = "buttonA";
            this.buttonA.Size = new System.Drawing.Size(61, 23);
            this.buttonA.TabIndex = 62;
            this.buttonA.Text = "A";
            this.buttonA.UseVisualStyleBackColor = true;
            this.buttonA.Click += new System.EventHandler(this.A_Click);
            // 
            // buttonS
            // 
            this.buttonS.Location = new System.Drawing.Point(1117, 95);
            this.buttonS.Name = "buttonS";
            this.buttonS.Size = new System.Drawing.Size(61, 23);
            this.buttonS.TabIndex = 63;
            this.buttonS.Text = "S";
            this.buttonS.UseVisualStyleBackColor = true;
            this.buttonS.Click += new System.EventHandler(this.S_Click);
            // 
            // buttonD
            // 
            this.buttonD.Location = new System.Drawing.Point(1184, 95);
            this.buttonD.Name = "buttonD";
            this.buttonD.Size = new System.Drawing.Size(61, 23);
            this.buttonD.TabIndex = 64;
            this.buttonD.Text = "D";
            this.buttonD.UseVisualStyleBackColor = true;
            this.buttonD.Click += new System.EventHandler(this.D_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1261, 666);
            this.Controls.Add(this.buttonD);
            this.Controls.Add(this.buttonS);
            this.Controls.Add(this.buttonA);
            this.Controls.Add(this.buttonW);
            this.Controls.Add(this.textZView);
            this.Controls.Add(this.textYView);
            this.Controls.Add(this.textXView);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.textCamZ);
            this.Controls.Add(this.textCamY);
            this.Controls.Add(this.textCamX);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.buttonRotateAroundLine);
            this.Controls.Add(this.textAngleForLineRotation);
            this.Controls.Add(this.textZ2);
            this.Controls.Add(this.textZ1);
            this.Controls.Add(this.textY2);
            this.Controls.Add(this.textY1);
            this.Controls.Add(this.textX2);
            this.Controls.Add(this.textX1);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.buttonRoll);
            this.Controls.Add(this.selectRollAxis);
            this.Controls.Add(this.textBoxAngleRotCenter);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.buttonReflection);
            this.Controls.Add(this.selectPlane);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rbPerspective);
            this.Controls.Add(this.rbIsometric);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textScaleZ);
            this.Controls.Add(this.textScaleY);
            this.Controls.Add(this.textScaleX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textShiftZ);
            this.Controls.Add(this.textShiftY);
            this.Controls.Add(this.textShiftX);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textAngle);
            this.Controls.Add(this.selectAxis);
            this.Controls.Add(this.selectShape);
            this.Controls.Add(this.buttonScale);
            this.Controls.Add(this.buttonRotate);
            this.Controls.Add(this.buttonShift);
            this.Controls.Add(this.buttonShape);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Affine3D";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonShape;
        private System.Windows.Forms.ComboBox selectShape;
        private System.Windows.Forms.Button buttonShift;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.TextBox textAngle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonScale;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textShiftZ;
        private System.Windows.Forms.TextBox textShiftY;
        private System.Windows.Forms.TextBox textShiftX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textScaleZ;
        private System.Windows.Forms.TextBox textScaleY;
        private System.Windows.Forms.TextBox textScaleX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox selectAxis;
        private System.Windows.Forms.RadioButton rbPerspective;
        private System.Windows.Forms.RadioButton rbIsometric;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbWorldCenter;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton rbCenter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox selectPlane;
        private System.Windows.Forms.Button buttonReflection;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxAngleRotCenter;
        private System.Windows.Forms.ComboBox selectRollAxis;
        private System.Windows.Forms.Button buttonRoll;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textX1;
        private System.Windows.Forms.TextBox textX2;
        private System.Windows.Forms.TextBox textY1;
        private System.Windows.Forms.TextBox textY2;
        private System.Windows.Forms.TextBox textZ1;
        private System.Windows.Forms.TextBox textZ2;
        private System.Windows.Forms.TextBox textAngleForLineRotation;
        private System.Windows.Forms.Button buttonRotateAroundLine;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textCamX;
        private System.Windows.Forms.TextBox textCamY;
        private System.Windows.Forms.TextBox textCamZ;
        private System.Windows.Forms.TextBox textZView;
        private System.Windows.Forms.TextBox textYView;
        private System.Windows.Forms.TextBox textXView;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button buttonW;
        private System.Windows.Forms.Button buttonA;
        private System.Windows.Forms.Button buttonS;
        private System.Windows.Forms.Button buttonD;
    }
}

