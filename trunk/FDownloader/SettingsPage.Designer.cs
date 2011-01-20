namespace FDownloader
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
            this.groupBoxProxy = new System.Windows.Forms.GroupBox();
            this.groupBoxLoadSetting = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxFromType = new System.Windows.Forms.CheckBox();
            this.checkBoxToday = new System.Windows.Forms.CheckBox();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.saveCSVCheckBox = new System.Windows.Forms.CheckBox();
            this.saveCSVFolderTextBox = new System.Windows.Forms.TextBox();
            this.saveCSVFolderButton = new System.Windows.Forms.Button();
            this.analyzeCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBoxProxy.SuspendLayout();
            this.groupBoxLoadSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxProxy
            // 
            this.checkBoxProxy.AutoSize = true;
            this.checkBoxProxy.Location = new System.Drawing.Point(10, 11);
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
            this.groupBoxProxy.Location = new System.Drawing.Point(10, 32);
            this.groupBoxProxy.Name = "groupBoxProxy";
            this.groupBoxProxy.Size = new System.Drawing.Size(385, 130);
            this.groupBoxProxy.TabIndex = 9;
            this.groupBoxProxy.TabStop = false;
            this.groupBoxProxy.Text = "Прокси";
            // 
            // groupBoxLoadSetting
            // 
            this.groupBoxLoadSetting.Controls.Add(this.radioButton3);
            this.groupBoxLoadSetting.Controls.Add(this.radioButton1);
            this.groupBoxLoadSetting.Controls.Add(this.radioButton2);
            this.groupBoxLoadSetting.Controls.Add(this.label5);
            this.groupBoxLoadSetting.Controls.Add(this.label4);
            this.groupBoxLoadSetting.Controls.Add(this.checkBoxFromType);
            this.groupBoxLoadSetting.Controls.Add(this.checkBoxToday);
            this.groupBoxLoadSetting.Controls.Add(this.dateTimePickerTo);
            this.groupBoxLoadSetting.Controls.Add(this.dateTimePickerFrom);
            this.groupBoxLoadSetting.Location = new System.Drawing.Point(10, 169);
            this.groupBoxLoadSetting.Name = "groupBoxLoadSetting";
            this.groupBoxLoadSetting.Size = new System.Drawing.Size(385, 154);
            this.groupBoxLoadSetting.TabIndex = 10;
            this.groupBoxLoadSetting.TabStop = false;
            this.groupBoxLoadSetting.Text = "Параметры загрузки";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(10, 90);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(123, 17);
            this.radioButton3.TabIndex = 10;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Последний день +1";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(10, 44);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(55, 17);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Вчера";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(10, 67);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(306, 17);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Последняя день, присутствующий в локальных данных";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(7, 124);
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
            // checkBoxFromType
            // 
            this.checkBoxFromType.AutoSize = true;
            this.checkBoxFromType.Checked = true;
            this.checkBoxFromType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFromType.Location = new System.Drawing.Point(211, 22);
            this.checkBoxFromType.Name = "checkBoxFromType";
            this.checkBoxFromType.Size = new System.Drawing.Size(111, 17);
            this.checkBoxFromType.TabIndex = 4;
            this.checkBoxFromType.Text = "Рассчетная дата";
            this.checkBoxFromType.UseVisualStyleBackColor = true;
            this.checkBoxFromType.CheckedChanged += new System.EventHandler(this.checkBoxFromType_CheckedChanged);
            // 
            // checkBoxToday
            // 
            this.checkBoxToday.AutoSize = true;
            this.checkBoxToday.Checked = true;
            this.checkBoxToday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxToday.Location = new System.Drawing.Point(211, 120);
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
            this.dateTimePickerTo.Location = new System.Drawing.Point(48, 120);
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
            // saveCSVCheckBox
            // 
            this.saveCSVCheckBox.AutoSize = true;
            this.saveCSVCheckBox.Location = new System.Drawing.Point(10, 352);
            this.saveCSVCheckBox.Name = "saveCSVCheckBox";
            this.saveCSVCheckBox.Size = new System.Drawing.Size(162, 17);
            this.saveCSVCheckBox.TabIndex = 11;
            this.saveCSVCheckBox.Text = "Сохранять полученный csv";
            this.saveCSVCheckBox.UseVisualStyleBackColor = true;
            this.saveCSVCheckBox.CheckedChanged += new System.EventHandler(this.saveCSVCheckBox_CheckedChanged);
            // 
            // saveCSVFolderTextBox
            // 
            this.saveCSVFolderTextBox.Location = new System.Drawing.Point(10, 375);
            this.saveCSVFolderTextBox.Name = "saveCSVFolderTextBox";
            this.saveCSVFolderTextBox.Size = new System.Drawing.Size(346, 20);
            this.saveCSVFolderTextBox.TabIndex = 12;
            // 
            // saveCSVFolderButton
            // 
            this.saveCSVFolderButton.Location = new System.Drawing.Point(363, 375);
            this.saveCSVFolderButton.Name = "saveCSVFolderButton";
            this.saveCSVFolderButton.Size = new System.Drawing.Size(32, 23);
            this.saveCSVFolderButton.TabIndex = 13;
            this.saveCSVFolderButton.Text = "...";
            this.saveCSVFolderButton.UseVisualStyleBackColor = true;
            this.saveCSVFolderButton.Click += new System.EventHandler(this.saveCSVFolderButton_Click);
            // 
            // analyzeCheckBox
            // 
            this.analyzeCheckBox.AutoSize = true;
            this.analyzeCheckBox.Location = new System.Drawing.Point(10, 401);
            this.analyzeCheckBox.Name = "analyzeCheckBox";
            this.analyzeCheckBox.Size = new System.Drawing.Size(285, 17);
            this.analyzeCheckBox.TabIndex = 14;
            this.analyzeCheckBox.Text = "Анализировать полученный файл или csv из папки";
            this.analyzeCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadCheckBox
            // 
            this.downloadCheckBox.AutoSize = true;
            this.downloadCheckBox.Location = new System.Drawing.Point(10, 329);
            this.downloadCheckBox.Name = "downloadCheckBox";
            this.downloadCheckBox.Size = new System.Drawing.Size(158, 17);
            this.downloadCheckBox.TabIndex = 15;
            this.downloadCheckBox.Text = "Загружать файл с finam.ru";
            this.downloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.downloadCheckBox);
            this.Controls.Add(this.analyzeCheckBox);
            this.Controls.Add(this.saveCSVFolderButton);
            this.Controls.Add(this.saveCSVFolderTextBox);
            this.Controls.Add(this.saveCSVCheckBox);
            this.Controls.Add(this.groupBoxLoadSetting);
            this.Controls.Add(this.checkBoxProxy);
            this.Controls.Add(this.groupBoxProxy);
            this.Name = "SettingsPage";
            this.Size = new System.Drawing.Size(402, 426);
            this.groupBoxProxy.ResumeLayout(false);
            this.groupBoxProxy.PerformLayout();
            this.groupBoxLoadSetting.ResumeLayout(false);
            this.groupBoxLoadSetting.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBoxProxy;
        private System.Windows.Forms.GroupBox groupBoxLoadSetting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxToday;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.CheckBox checkBoxFromType;
        private System.Windows.Forms.CheckBox saveCSVCheckBox;
        private System.Windows.Forms.TextBox saveCSVFolderTextBox;
        private System.Windows.Forms.Button saveCSVFolderButton;
        private System.Windows.Forms.CheckBox analyzeCheckBox;
        private System.Windows.Forms.CheckBox downloadCheckBox;
    }
}
