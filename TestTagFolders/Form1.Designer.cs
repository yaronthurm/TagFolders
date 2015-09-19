namespace TestTagFolders
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tagsPanel21 = new TestTagFolders.TagsPanel2();
            this.filesPanel1 = new TestTagFolders.FilesPanel();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "D:\\Users\\yaron\\Desktop\\תמונות";
            // 
            // tagsPanel21
            // 
            this.tagsPanel21.BackColor = System.Drawing.Color.White;
            this.tagsPanel21.Location = new System.Drawing.Point(12, 48);
            this.tagsPanel21.Name = "tagsPanel21";
            this.tagsPanel21.Size = new System.Drawing.Size(264, 322);
            this.tagsPanel21.TabIndex = 1;
            // 
            // filesPanel1
            // 
            this.filesPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesPanel1.BackColor = System.Drawing.Color.White;
            this.filesPanel1.Location = new System.Drawing.Point(282, 48);
            this.filesPanel1.Name = "filesPanel1";
            this.filesPanel1.Size = new System.Drawing.Size(875, 322);
            this.filesPanel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 465);
            this.Controls.Add(this.tagsPanel21);
            this.Controls.Add(this.filesPanel1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private FilesPanel filesPanel1;
        private TagsPanel2 tagsPanel21;
    }
}