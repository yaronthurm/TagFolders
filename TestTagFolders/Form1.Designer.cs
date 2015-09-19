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
            this.tagsPanel = new TestTagFolders.TagsPanel2();
            this.filesPanel = new TestTagFolders.FilesPanel();
            this.tagsCombinationViewer = new TestTagFolders.TagsCombinationViewer();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 84);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "D:\\Users\\yaron\\Desktop\\תמונות";
            // 
            // tagsPanel
            // 
            this.tagsPanel.BackColor = System.Drawing.Color.White;
            this.tagsPanel.Location = new System.Drawing.Point(14, 110);
            this.tagsPanel.Name = "tagsPanel";
            this.tagsPanel.Size = new System.Drawing.Size(264, 322);
            this.tagsPanel.TabIndex = 1;
            // 
            // filesPanel
            // 
            this.filesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesPanel.BackColor = System.Drawing.Color.White;
            this.filesPanel.Location = new System.Drawing.Point(284, 110);
            this.filesPanel.Name = "filesPanel";
            this.filesPanel.Size = new System.Drawing.Size(875, 322);
            this.filesPanel.TabIndex = 0;
            // 
            // tagsCombinationViewer1
            // 
            this.tagsCombinationViewer.AutoScroll = true;
            this.tagsCombinationViewer.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tagsCombinationViewer.Dock = System.Windows.Forms.DockStyle.Top;
            this.tagsCombinationViewer.Location = new System.Drawing.Point(0, 0);
            this.tagsCombinationViewer.Name = "tagsCombinationViewer1";
            this.tagsCombinationViewer.Size = new System.Drawing.Size(1171, 50);
            this.tagsCombinationViewer.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 465);
            this.Controls.Add(this.tagsCombinationViewer);
            this.Controls.Add(this.tagsPanel);
            this.Controls.Add(this.filesPanel);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private FilesPanel filesPanel;
        private TagsPanel2 tagsPanel;
        private TagsCombinationViewer tagsCombinationViewer;
    }
}