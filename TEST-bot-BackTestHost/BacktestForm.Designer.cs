namespace TEST_bot_BackTestHost
{
    partial class BacktestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.testButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tillDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.optimComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.robotComboBox = new System.Windows.Forms.ComboBox();
            this.robotDLLTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.robotDLLButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.canShortCheckBox = new System.Windows.Forms.CheckBox();
            this.selectSymbol1 = new OpenWealth.Simple.SelectSymbol();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(13, 515);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(477, 23);
            this.progressBar.TabIndex = 0;
            // 
            // testButton
            // 
            this.testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.testButton.Location = new System.Drawing.Point(337, 486);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 7;
            this.testButton.Text = "Тест";
            this.testButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(415, 486);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Стоп";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fromDateTimePicker.Location = new System.Drawing.Point(31, 356);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new System.Drawing.Size(130, 20);
            this.fromDateTimePicker.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 362);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "C";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 388);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "До";
            // 
            // tillDateTimePicker
            // 
            this.tillDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tillDateTimePicker.Location = new System.Drawing.Point(31, 382);
            this.tillDateTimePicker.Name = "tillDateTimePicker";
            this.tillDateTimePicker.Size = new System.Drawing.Size(130, 20);
            this.tillDateTimePicker.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(246, 486);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Оптимизация";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // optimComboBox
            // 
            this.optimComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optimComboBox.Enabled = false;
            this.optimComboBox.FormattingEnabled = true;
            this.optimComboBox.Location = new System.Drawing.Point(13, 486);
            this.optimComboBox.Name = "optimComboBox";
            this.optimComboBox.Size = new System.Drawing.Size(136, 21);
            this.optimComboBox.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 414);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Параметры";
            // 
            // robotComboBox
            // 
            this.robotComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.robotComboBox.FormattingEnabled = true;
            this.robotComboBox.Location = new System.Drawing.Point(249, 28);
            this.robotComboBox.Name = "robotComboBox";
            this.robotComboBox.Size = new System.Drawing.Size(241, 21);
            this.robotComboBox.TabIndex = 16;
            // 
            // robotDLLTextBox
            // 
            this.robotDLLTextBox.Location = new System.Drawing.Point(13, 29);
            this.robotDLLTextBox.Name = "robotDLLTextBox";
            this.robotDLLTextBox.Size = new System.Drawing.Size(200, 20);
            this.robotDLLTextBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Робот";
            // 
            // robotDLLButton
            // 
            this.robotDLLButton.Location = new System.Drawing.Point(219, 29);
            this.robotDLLButton.Name = "robotDLLButton";
            this.robotDLLButton.Size = new System.Drawing.Size(24, 23);
            this.robotDLLButton.TabIndex = 19;
            this.robotDLLButton.Text = "...";
            this.robotDLLButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(155, 486);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Сохранить";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(231, 356);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(113, 20);
            this.numericUpDown1.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(184, 362);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Сумма";
            // 
            // canShortCheckBox
            // 
            this.canShortCheckBox.AutoSize = true;
            this.canShortCheckBox.Location = new System.Drawing.Point(187, 387);
            this.canShortCheckBox.Name = "canShortCheckBox";
            this.canShortCheckBox.Size = new System.Drawing.Size(107, 17);
            this.canShortCheckBox.TabIndex = 23;
            this.canShortCheckBox.Text = "Шорт возможен";
            this.canShortCheckBox.UseVisualStyleBackColor = true;
            // 
            // selectSymbol1
            // 
            this.selectSymbol1.Location = new System.Drawing.Point(14, 52);
            this.selectSymbol1.Name = "selectSymbol1";
            this.selectSymbol1.Size = new System.Drawing.Size(477, 300);
            this.selectSymbol1.TabIndex = 24;
            // 
            // BacktestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 550);
            this.Controls.Add(this.selectSymbol1);
            this.Controls.Add(this.canShortCheckBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.robotDLLButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.robotDLLTextBox);
            this.Controls.Add(this.robotComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.optimComboBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tillDateTimePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fromDateTimePicker);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.progressBar);
            this.Name = "BacktestForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker tillDateTimePicker;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox optimComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox robotComboBox;
        private System.Windows.Forms.TextBox robotDLLTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button robotDLLButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox canShortCheckBox;
        private OpenWealth.Simple.SelectSymbol selectSymbol1;
    }
}