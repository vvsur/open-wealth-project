namespace FDownloader
{
    partial class FDownloaderForm
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
            this.components = new System.ComponentModel.Container();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.labelOnlyOne = new System.Windows.Forms.Label();
            this.timerOnlyOne = new System.Windows.Forms.Timer(this.components);
            this.labelStarting = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(331, 440);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "Закрыть";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Enabled = false;
            this.buttonNext.Location = new System.Drawing.Point(250, 440);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "Далее";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrevious.Enabled = false;
            this.buttonPrevious.Location = new System.Drawing.Point(169, 440);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(75, 23);
            this.buttonPrevious.TabIndex = 2;
            this.buttonPrevious.Text = "Назад";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // labelOnlyOne
            // 
            this.labelOnlyOne.AutoSize = true;
            this.labelOnlyOne.Location = new System.Drawing.Point(13, 13);
            this.labelOnlyOne.Name = "labelOnlyOne";
            this.labelOnlyOne.Size = new System.Drawing.Size(190, 13);
            this.labelOnlyOne.TabIndex = 3;
            this.labelOnlyOne.Text = "Запущена другая копия программы";
            this.labelOnlyOne.Visible = false;
            // 
            // timerOnlyOne
            // 
            this.timerOnlyOne.Enabled = true;
            this.timerOnlyOne.Tick += new System.EventHandler(this.timerOnlyOne_Tick);
            // 
            // labelStarting
            // 
            this.labelStarting.AutoSize = true;
            this.labelStarting.Location = new System.Drawing.Point(13, 26);
            this.labelStarting.Name = "labelStarting";
            this.labelStarting.Size = new System.Drawing.Size(82, 13);
            this.labelStarting.TabIndex = 4;
            this.labelStarting.Text = "Загружаюсь ...";
            // 
            // FDownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 465);
            this.Controls.Add(this.labelStarting);
            this.Controls.Add(this.labelOnlyOne);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonExit);
            this.Name = "FDownloaderForm";
            this.Text = "owp.FDownloader";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Label labelOnlyOne;
        private System.Windows.Forms.Timer timerOnlyOne;
        private System.Windows.Forms.Label labelStarting;
    }
}