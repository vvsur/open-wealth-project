namespace owp.FDownloader
{
    partial class SettingsPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxProxy = new System.Windows.Forms.CheckBox();
            this.textBoxProxy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxProxyPassword = new System.Windows.Forms.CheckBox();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialogLoadSetting = new System.Windows.Forms.OpenFileDialog();
            this.buttonLoadSetting = new System.Windows.Forms.Button();
            this.groupBoxProxy = new System.Windows.Forms.GroupBox();
            this.groupBoxLoadSetting = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxFromCSV = new System.Windows.Forms.CheckBox();
            this.checkBoxYesterday = new System.Windows.Forms.CheckBox();
            this.checkBoxToday = new System.Windows.Forms.CheckBox();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.checkBoxMargeCSV = new System.Windows.Forms.CheckBox();
            this.checkBoxLoadFromFinam = new System.Windows.Forms.CheckBox();
            this.checkBoxConvertCSV2WL = new System.Windows.Forms.CheckBox();
            this.buttonCSVDir = new System.Windows.Forms.Button();
            this.labelCSVDir = new System.Windows.Forms.Label();
            this.textBoxCSVDir = new System.Windows.Forms.TextBox();
            this.buttonWLDir = new System.Windows.Forms.Button();
            this.labelWLDir = new System.Windows.Forms.Label();
            this.textBoxWLDir = new System.Windows.Forms.TextBox();
            this.checkBoxDelCSV = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxPeriod = new System.Windows.Forms.ComboBox();
            this.aggregateComboBox = new System.Windows.Forms.ComboBox();
            this.aggregateСheckBox = new System.Windows.Forms.CheckBox();
            this.aggregateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBoxProxy.SuspendLayout();
            this.groupBoxLoadSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aggregateNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxProxy
            // 
            this.checkBoxProxy.AutoSize = true;
            this.checkBoxProxy.Location = new System.Drawing.Point(10, 38);
            this.checkBoxProxy.Name = "checkBoxProxy";
            this.checkBoxProxy.Size = new System.Drawing.Size(138, 17);
            this.checkBoxProxy.TabIndex = 0;
            this.checkBoxProxy.Text = "Использовать прокси";
            this.checkBoxProxy.UseVisualStyleBackColor = true;
            this.checkBoxProxy.CheckedChanged += new System.EventHandler(this.checkBoxProxy_CheckedChanged);
            // 
            // textBoxProxy
            // 
            this.textBoxProxy.Location = new System.Drawing.Point(13, 22);
            this.textBoxProxy.Name = "textBoxProxy";
            this.textBoxProxy.Size = new System.Drawing.Size(168, 20);
            this.textBoxProxy.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Пример: http://ProxyHost:8080";
            // 
            // checkBoxProxyPassword
            // 
            this.checkBoxProxyPassword.AutoSize = true;
            this.checkBoxProxyPassword.Location = new System.Drawing.Point(13, 48);
            this.checkBoxProxyPassword.Name = "checkBoxProxyPassword";
            this.checkBoxProxyPassword.Size = new System.Drawing.Size(145, 17);
            this.checkBoxProxyPassword.TabIndex = 3;
            this.checkBoxProxyPassword.Text = "Прокси требует пароль";
            this.checkBoxProxyPassword.UseVisualStyleBackColor = true;
            this.checkBoxProxyPassword.CheckedChanged += new System.EventHandler(this.checkBoxProxyPassword_CheckedChanged);
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Enabled = false;
            this.textBoxLogin.Location = new System.Drawing.Point(63, 71);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(168, 20);
            this.textBoxLogin.TabIndex = 4;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Enabled = false;
            this.textBoxPassword.Location = new System.Drawing.Point(63, 97);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(168, 20);
            this.textBoxPassword.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(10, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Логин";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(10, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Пароль";
            // 
            // buttonLoadSetting
            // 
            this.buttonLoadSetting.Location = new System.Drawing.Point(10, 7);
            this.buttonLoadSetting.Name = "buttonLoadSetting";
            this.buttonLoadSetting.Size = new System.Drawing.Size(220, 23);
            this.buttonLoadSetting.TabIndex = 8;
            this.buttonLoadSetting.Text = "Загрузить настройки из файла";
            this.buttonLoadSetting.UseVisualStyleBackColor = true;
            // 
            // groupBoxProxy
            // 
            this.groupBoxProxy.Controls.Add(this.label1);
            this.groupBoxProxy.Controls.Add(this.textBoxProxy);
            this.groupBoxProxy.Controls.Add(this.textBoxLogin);
            this.groupBoxProxy.Controls.Add(this.label3);
            this.groupBoxProxy.Controls.Add(this.checkBoxProxyPassword);
            this.groupBoxProxy.Controls.Add(this.label2);
            this.groupBoxProxy.Controls.Add(this.textBoxPassword);
            this.groupBoxProxy.Enabled = false;
            this.groupBoxProxy.Location = new System.Drawing.Point(10, 59);
            this.groupBoxProxy.Name = "groupBoxProxy";
            this.groupBoxProxy.Size = new System.Drawing.Size(385, 130);
            this.groupBoxProxy.TabIndex = 9;
            this.groupBoxProxy.TabStop = false;
            this.groupBoxProxy.Text = "Прокси";
            // 
            // groupBoxLoadSetting
            // 
            this.groupBoxLoadSetting.Controls.Add(this.label5);
            this.groupBoxLoadSetting.Controls.Add(this.label4);
            this.groupBoxLoadSetting.Controls.Add(this.checkBoxFromCSV);
            this.groupBoxLoadSetting.Controls.Add(this.checkBoxYesterday);
            this.groupBoxLoadSetting.Controls.Add(this.checkBoxToday);
            this.groupBoxLoadSetting.Controls.Add(this.dateTimePickerTo);
            this.groupBoxLoadSetting.Controls.Add(this.dateTimePickerFrom);
            this.groupBoxLoadSetting.Enabled = false;
            this.groupBoxLoadSetting.Location = new System.Drawing.Point(10, 260);
            this.groupBoxLoadSetting.Name = "groupBoxLoadSetting";
            this.groupBoxLoadSetting.Size = new System.Drawing.Size(385, 73);
            this.groupBoxLoadSetting.TabIndex = 10;
            this.groupBoxLoadSetting.TabStop = false;
            this.groupBoxLoadSetting.Text = "Параметры загрузки";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(7, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "по";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(7, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "с";
            // 
            // checkBoxFromCSV
            // 
            this.checkBoxFromCSV.AutoSize = true;
            this.checkBoxFromCSV.Checked = true;
            this.checkBoxFromCSV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFromCSV.Location = new System.Drawing.Point(237, 22);
            this.checkBoxFromCSV.Name = "checkBoxFromCSV";
            this.checkBoxFromCSV.Size = new System.Drawing.Size(143, 17);
            this.checkBoxFromCSV.TabIndex = 5;
            this.checkBoxFromCSV.Text = "Дата из CSV и/или WL";
            this.checkBoxFromCSV.UseVisualStyleBackColor = true;
            this.checkBoxFromCSV.CheckedChanged += new System.EventHandler(this.checkBoxFromCSV_CheckedChanged);
            // 
            // checkBoxYesterday
            // 
            this.checkBoxYesterday.AutoSize = true;
            this.checkBoxYesterday.Location = new System.Drawing.Point(175, 22);
            this.checkBoxYesterday.Name = "checkBoxYesterday";
            this.checkBoxYesterday.Size = new System.Drawing.Size(56, 17);
            this.checkBoxYesterday.TabIndex = 4;
            this.checkBoxYesterday.Text = "Вчера";
            this.checkBoxYesterday.UseVisualStyleBackColor = true;
            this.checkBoxYesterday.CheckedChanged += new System.EventHandler(this.checkBoxYesterday_CheckedChanged);
            // 
            // checkBoxToday
            // 
            this.checkBoxToday.AutoSize = true;
            this.checkBoxToday.Checked = true;
            this.checkBoxToday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxToday.Location = new System.Drawing.Point(175, 48);
            this.checkBoxToday.Name = "checkBoxToday";
            this.checkBoxToday.Size = new System.Drawing.Size(68, 17);
            this.checkBoxToday.TabIndex = 3;
            this.checkBoxToday.Text = "Сегодня";
            this.checkBoxToday.UseVisualStyleBackColor = true;
            this.checkBoxToday.CheckedChanged += new System.EventHandler(this.checkBoxToday_CheckedChanged);
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Enabled = false;
            this.dateTimePickerTo.Location = new System.Drawing.Point(48, 45);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(122, 20);
            this.dateTimePickerTo.TabIndex = 1;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Enabled = false;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(48, 19);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(122, 20);
            this.dateTimePickerFrom.TabIndex = 0;
            // 
            // checkBoxMargeCSV
            // 
            this.checkBoxMargeCSV.AutoSize = true;
            this.checkBoxMargeCSV.Checked = true;
            this.checkBoxMargeCSV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMargeCSV.Location = new System.Drawing.Point(17, 361);
            this.checkBoxMargeCSV.Name = "checkBoxMargeCSV";
            this.checkBoxMargeCSV.Size = new System.Drawing.Size(234, 17);
            this.checkBoxMargeCSV.TabIndex = 0;
            this.checkBoxMargeCSV.Text = "Объединять CSV по одному инструменту";
            this.checkBoxMargeCSV.UseVisualStyleBackColor = true;
            this.checkBoxMargeCSV.CheckedChanged += new System.EventHandler(this.checkBoxMargeCSV_CheckedChanged);
            // 
            // checkBoxLoadFromFinam
            // 
            this.checkBoxLoadFromFinam.AutoSize = true;
            this.checkBoxLoadFromFinam.Location = new System.Drawing.Point(10, 239);
            this.checkBoxLoadFromFinam.Name = "checkBoxLoadFromFinam";
            this.checkBoxLoadFromFinam.Size = new System.Drawing.Size(164, 17);
            this.checkBoxLoadFromFinam.TabIndex = 12;
            this.checkBoxLoadFromFinam.Text = "Загрузить CSV c ФИНАМА";
            this.checkBoxLoadFromFinam.UseVisualStyleBackColor = true;
            this.checkBoxLoadFromFinam.CheckedChanged += new System.EventHandler(this.checkBoxOnlyConvert_CheckedChanged);
            // 
            // checkBoxConvertCSV2WL
            // 
            this.checkBoxConvertCSV2WL.AutoSize = true;
            this.checkBoxConvertCSV2WL.Checked = true;
            this.checkBoxConvertCSV2WL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxConvertCSV2WL.Location = new System.Drawing.Point(17, 384);
            this.checkBoxConvertCSV2WL.Name = "checkBoxConvertCSV2WL";
            this.checkBoxConvertCSV2WL.Size = new System.Drawing.Size(158, 17);
            this.checkBoxConvertCSV2WL.TabIndex = 13;
            this.checkBoxConvertCSV2WL.Text = "Преобразовать CSV в WL";
            this.checkBoxConvertCSV2WL.UseVisualStyleBackColor = true;
            this.checkBoxConvertCSV2WL.CheckedChanged += new System.EventHandler(this.checkBoxConvertCSV2WL_CheckedChanged);
            // 
            // buttonCSVDir
            // 
            this.buttonCSVDir.Location = new System.Drawing.Point(355, 337);
            this.buttonCSVDir.Name = "buttonCSVDir";
            this.buttonCSVDir.Size = new System.Drawing.Size(31, 19);
            this.buttonCSVDir.TabIndex = 16;
            this.buttonCSVDir.Text = "...";
            this.buttonCSVDir.UseVisualStyleBackColor = true;
            this.buttonCSVDir.Click += new System.EventHandler(this.buttonCSVDir_Click);
            // 
            // labelCSVDir
            // 
            this.labelCSVDir.AutoSize = true;
            this.labelCSVDir.Location = new System.Drawing.Point(14, 340);
            this.labelCSVDir.Name = "labelCSVDir";
            this.labelCSVDir.Size = new System.Drawing.Size(84, 13);
            this.labelCSVDir.TabIndex = 15;
            this.labelCSVDir.Text = "Папка для CSV";
            // 
            // textBoxCSVDir
            // 
            this.textBoxCSVDir.Location = new System.Drawing.Point(104, 337);
            this.textBoxCSVDir.Name = "textBoxCSVDir";
            this.textBoxCSVDir.Size = new System.Drawing.Size(245, 20);
            this.textBoxCSVDir.TabIndex = 14;
            // 
            // buttonWLDir
            // 
            this.buttonWLDir.Location = new System.Drawing.Point(355, 410);
            this.buttonWLDir.Name = "buttonWLDir";
            this.buttonWLDir.Size = new System.Drawing.Size(31, 19);
            this.buttonWLDir.TabIndex = 19;
            this.buttonWLDir.Text = "...";
            this.buttonWLDir.UseVisualStyleBackColor = true;
            this.buttonWLDir.Click += new System.EventHandler(this.buttonWLDir_Click);
            // 
            // labelWLDir
            // 
            this.labelWLDir.AutoSize = true;
            this.labelWLDir.Location = new System.Drawing.Point(14, 410);
            this.labelWLDir.Name = "labelWLDir";
            this.labelWLDir.Size = new System.Drawing.Size(80, 13);
            this.labelWLDir.TabIndex = 18;
            this.labelWLDir.Text = "Папка для WL";
            // 
            // textBoxWLDir
            // 
            this.textBoxWLDir.Location = new System.Drawing.Point(104, 407);
            this.textBoxWLDir.Name = "textBoxWLDir";
            this.textBoxWLDir.Size = new System.Drawing.Size(245, 20);
            this.textBoxWLDir.TabIndex = 17;
            // 
            // checkBoxDelCSV
            // 
            this.checkBoxDelCSV.AutoSize = true;
            this.checkBoxDelCSV.Checked = true;
            this.checkBoxDelCSV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDelCSV.Location = new System.Drawing.Point(182, 384);
            this.checkBoxDelCSV.Name = "checkBoxDelCSV";
            this.checkBoxDelCSV.Size = new System.Drawing.Size(213, 17);
            this.checkBoxDelCSV.TabIndex = 20;
            this.checkBoxDelCSV.Text = "Удалить CSV после преобразования";
            this.checkBoxDelCSV.UseVisualStyleBackColor = true;
            this.checkBoxDelCSV.CheckedChanged += new System.EventHandler(this.checkBoxDelCSV_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Работаю с";
            // 
            // comboBoxPeriod
            // 
            this.comboBoxPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPeriod.FormattingEnabled = true;
            this.comboBoxPeriod.Items.AddRange(new object[] {
            "тики",
            "1 мин",
            "5 мин",
            "10 мин",
            "15 мин",
            "30 мин",
            "1 час",
            "1 день",
            "1 неделя",
            "1 месяц",
            "1 час (с 10-30)"});
            this.comboBoxPeriod.Location = new System.Drawing.Point(70, 192);
            this.comboBoxPeriod.Name = "comboBoxPeriod";
            this.comboBoxPeriod.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPeriod.TabIndex = 21;
            this.comboBoxPeriod.SelectedIndexChanged += new System.EventHandler(this.comboBoxPeriod_SelectedIndexChanged);
            // 
            // aggregateComboBox
            // 
            this.aggregateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aggregateComboBox.Enabled = false;
            this.aggregateComboBox.FormattingEnabled = true;
            this.aggregateComboBox.Items.AddRange(new object[] {
            "Тикам",
            "Секундам",
            "Минутам",
            "Дням",
            "Неделям",
            "Месяцам",
            "Объемам"});
            this.aggregateComboBox.Location = new System.Drawing.Point(265, 217);
            this.aggregateComboBox.Name = "aggregateComboBox";
            this.aggregateComboBox.Size = new System.Drawing.Size(121, 21);
            this.aggregateComboBox.TabIndex = 23;
            // 
            // aggregateСheckBox
            // 
            this.aggregateСheckBox.AutoSize = true;
            this.aggregateСheckBox.Enabled = false;
            this.aggregateСheckBox.Location = new System.Drawing.Point(10, 217);
            this.aggregateСheckBox.Name = "aggregateСheckBox";
            this.aggregateСheckBox.Size = new System.Drawing.Size(187, 17);
            this.aggregateСheckBox.TabIndex = 24;
            this.aggregateСheckBox.Text = "Дополнительно агригировать к";
            this.aggregateСheckBox.UseVisualStyleBackColor = true;
            // 
            // aggregateNumericUpDown
            // 
            this.aggregateNumericUpDown.Enabled = false;
            this.aggregateNumericUpDown.Location = new System.Drawing.Point(204, 217);
            this.aggregateNumericUpDown.Name = "aggregateNumericUpDown";
            this.aggregateNumericUpDown.Size = new System.Drawing.Size(55, 20);
            this.aggregateNumericUpDown.TabIndex = 25;
            // 
            // SettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.aggregateNumericUpDown);
            this.Controls.Add(this.aggregateСheckBox);
            this.Controls.Add(this.aggregateComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxPeriod);
            this.Controls.Add(this.checkBoxDelCSV);
            this.Controls.Add(this.buttonWLDir);
            this.Controls.Add(this.labelWLDir);
            this.Controls.Add(this.textBoxWLDir);
            this.Controls.Add(this.buttonCSVDir);
            this.Controls.Add(this.labelCSVDir);
            this.Controls.Add(this.textBoxCSVDir);
            this.Controls.Add(this.checkBoxMargeCSV);
            this.Controls.Add(this.checkBoxConvertCSV2WL);
            this.Controls.Add(this.checkBoxLoadFromFinam);
            this.Controls.Add(this.groupBoxLoadSetting);
            this.Controls.Add(this.checkBoxProxy);
            this.Controls.Add(this.groupBoxProxy);
            this.Controls.Add(this.buttonLoadSetting);
            this.Name = "SettingsPage";
            this.Size = new System.Drawing.Size(402, 436);
            this.groupBoxProxy.ResumeLayout(false);
            this.groupBoxProxy.PerformLayout();
            this.groupBoxLoadSetting.ResumeLayout(false);
            this.groupBoxLoadSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aggregateNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxProxy;
        private System.Windows.Forms.TextBox textBoxProxy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxProxyPassword;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialogLoadSetting;
        private System.Windows.Forms.Button buttonLoadSetting;
        private System.Windows.Forms.GroupBox groupBoxProxy;
        private System.Windows.Forms.GroupBox groupBoxLoadSetting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxFromCSV;
        private System.Windows.Forms.CheckBox checkBoxYesterday;
        private System.Windows.Forms.CheckBox checkBoxToday;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.CheckBox checkBoxMargeCSV;
        private System.Windows.Forms.CheckBox checkBoxLoadFromFinam;
        private System.Windows.Forms.CheckBox checkBoxConvertCSV2WL;
        private System.Windows.Forms.Button buttonCSVDir;
        private System.Windows.Forms.Label labelCSVDir;
        private System.Windows.Forms.TextBox textBoxCSVDir;
        private System.Windows.Forms.Button buttonWLDir;
        private System.Windows.Forms.Label labelWLDir;
        private System.Windows.Forms.TextBox textBoxWLDir;
        private System.Windows.Forms.CheckBox checkBoxDelCSV;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxPeriod;
        private System.Windows.Forms.ComboBox aggregateComboBox;
        private System.Windows.Forms.CheckBox aggregateСheckBox;
        private System.Windows.Forms.NumericUpDown aggregateNumericUpDown;
    }
}
