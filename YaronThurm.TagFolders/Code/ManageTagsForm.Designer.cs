namespace YaronThurm.TagFolders
{
    partial class ManageTagsForm
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
            this.listTags = new System.Windows.Forms.ListBox();
            this.btnRemoveTag = new System.Windows.Forms.Button();
            this.txtAddTag = new System.Windows.Forms.TextBox();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.listFiles = new System.Windows.Forms.ListBox();
            this.listProposedTags = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddPropesedTags = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listTags
            // 
            this.listTags.FormattingEnabled = true;
            this.listTags.Location = new System.Drawing.Point(89, 48);
            this.listTags.Name = "listTags";
            this.listTags.Size = new System.Drawing.Size(150, 95);
            this.listTags.TabIndex = 15;
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Location = new System.Drawing.Point(8, 54);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTag.TabIndex = 14;
            this.btnRemoveTag.Text = "הסר תג";
            this.btnRemoveTag.UseVisualStyleBackColor = true;
            this.btnRemoveTag.Click += new System.EventHandler(this.btnRemoveTag_Click);
            // 
            // txtAddTag
            // 
            this.txtAddTag.Location = new System.Drawing.Point(89, 27);
            this.txtAddTag.Name = "txtAddTag";
            this.txtAddTag.Size = new System.Drawing.Size(150, 20);
            this.txtAddTag.TabIndex = 0;
            this.txtAddTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAddTag_KeyDown);
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(8, 25);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(75, 23);
            this.btnAddTag.TabIndex = 12;
            this.btnAddTag.Text = "הוסף תג";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(8, 120);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "שמור";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // listFiles
            // 
            this.listFiles.FormattingEnabled = true;
            this.listFiles.HorizontalScrollbar = true;
            this.listFiles.Location = new System.Drawing.Point(8, 162);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(333, 108);
            this.listFiles.TabIndex = 17;
            // 
            // listProposedTags
            // 
            this.listProposedTags.FormattingEnabled = true;
            this.listProposedTags.Location = new System.Drawing.Point(283, 47);
            this.listProposedTags.Name = "listProposedTags";
            this.listProposedTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listProposedTags.Size = new System.Drawing.Size(150, 95);
            this.listProposedTags.TabIndex = 18;
            this.listProposedTags.DoubleClick += new System.EventHandler(this.listProposedTags_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(282, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "תגים מוצעים";
            // 
            // btnAddPropesedTags
            // 
            this.btnAddPropesedTags.Location = new System.Drawing.Point(361, 25);
            this.btnAddPropesedTags.Name = "btnAddPropesedTags";
            this.btnAddPropesedTags.Size = new System.Drawing.Size(46, 19);
            this.btnAddPropesedTags.TabIndex = 20;
            this.btnAddPropesedTags.Text = "הוסף";
            this.btnAddPropesedTags.UseVisualStyleBackColor = true;
            this.btnAddPropesedTags.Click += new System.EventHandler(this.btnAddPropesedTags_Click);
            // 
            // ManageTagsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 154);
            this.Controls.Add(this.btnAddPropesedTags);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listProposedTags);
            this.Controls.Add(this.listFiles);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.listTags);
            this.Controls.Add(this.btnRemoveTag);
            this.Controls.Add(this.txtAddTag);
            this.Controls.Add(this.btnAddTag);
            this.Name = "ManageTagsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MamageTagsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listTags;
        private System.Windows.Forms.Button btnRemoveTag;
        private System.Windows.Forms.TextBox txtAddTag;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.ListBox listProposedTags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddPropesedTags;
    }
}