namespace OpenWealth.Simple
{
    partial class SelectSymbol
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
            this.allSymbolButton = new System.Windows.Forms.Button();
            this.symbolCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.allMarketButton = new System.Windows.Forms.Button();
            this.marketCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // allSymbolButton
            // 
            this.allSymbolButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.allSymbolButton.Location = new System.Drawing.Point(448, 137);
            this.allSymbolButton.Name = "allSymbolButton";
            this.allSymbolButton.Size = new System.Drawing.Size(26, 23);
            this.allSymbolButton.TabIndex = 12;
            this.allSymbolButton.Text = "all";
            this.allSymbolButton.UseVisualStyleBackColor = true;
            this.allSymbolButton.Click += new System.EventHandler(this.allSymbolButton_Click);
            // 
            // symbolCheckedListBox
            // 
            this.symbolCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.symbolCheckedListBox.FormattingEnabled = true;
            this.symbolCheckedListBox.Location = new System.Drawing.Point(0, 159);
            this.symbolCheckedListBox.Name = "symbolCheckedListBox";
            this.symbolCheckedListBox.Size = new System.Drawing.Size(474, 139);
            this.symbolCheckedListBox.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Инструмент";
            // 
            // allMarketButton
            // 
            this.allMarketButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.allMarketButton.Location = new System.Drawing.Point(448, 2);
            this.allMarketButton.Name = "allMarketButton";
            this.allMarketButton.Size = new System.Drawing.Size(26, 23);
            this.allMarketButton.TabIndex = 9;
            this.allMarketButton.Text = "all";
            this.allMarketButton.UseVisualStyleBackColor = true;
            this.allMarketButton.Click += new System.EventHandler(this.allMarketButton_Click);
            // 
            // marketCheckedListBox
            // 
            this.marketCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.marketCheckedListBox.FormattingEnabled = true;
            this.marketCheckedListBox.Location = new System.Drawing.Point(0, 24);
            this.marketCheckedListBox.Name = "marketCheckedListBox";
            this.marketCheckedListBox.Size = new System.Drawing.Size(474, 109);
            this.marketCheckedListBox.TabIndex = 8;
            this.marketCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.marketCheckedListBox_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Рынок";
            // 
            // SelectSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.allSymbolButton);
            this.Controls.Add(this.symbolCheckedListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.allMarketButton);
            this.Controls.Add(this.marketCheckedListBox);
            this.Controls.Add(this.label1);
            this.Name = "SelectSymbol";
            this.Size = new System.Drawing.Size(477, 300);
            this.Load += new System.EventHandler(this.SelectSymbol_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button allSymbolButton;
        private System.Windows.Forms.CheckedListBox symbolCheckedListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button allMarketButton;
        private System.Windows.Forms.CheckedListBox marketCheckedListBox;
        private System.Windows.Forms.Label label1;
    }
}
