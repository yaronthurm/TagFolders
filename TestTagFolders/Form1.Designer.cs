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
            this.listTags = new System.Windows.Forms.ListView();
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
            // listTags
            // 
            this.listTags.Location = new System.Drawing.Point(12, 48);
            this.listTags.Name = "listTags";
            this.listTags.Size = new System.Drawing.Size(220, 147);
            this.listTags.TabIndex = 1;
            this.listTags.UseCompatibleStateImageBehavior = false;
            this.listTags.View = System.Windows.Forms.View.List;
            // 
            // filesPanel1
            // 
            this.filesPanel1.BackColor = System.Drawing.Color.White;
            this.filesPanel1.Location = new System.Drawing.Point(12, 48);
            this.filesPanel1.Name = "filesPanel1";
            this.filesPanel1.Size = new System.Drawing.Size(838, 322);
            this.filesPanel1.TabIndex = 0;
            this.filesPanel1.Load += new System.EventHandler(this.filesPanel1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 465);
            this.Controls.Add(this.filesPanel1);
            this.Controls.Add(this.listTags);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listTags;
        private FilesPanel filesPanel1;
    }
}