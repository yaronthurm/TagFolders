using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TestTagFolders
{
    public partial class LargeFileWithTag : UserControl
    {
        private FileWithTags _file;

        public LargeFileWithTag()
        {
            InitializeComponent();
        }


        public void SetData(Bitmap thumbnail, FileWithTags file)
        {
            _file = file;

            this.pictureBox1.Image = thumbnail;
            this.lblFileName.Text = Path.GetFileName(file.FileName);

            foreach (var tag in file.Tags)
            {
                var button = new Button();
                button.Text = tag.Value;
                this.panel.Controls.Add(button);
            }
        }

        
        

        private void LargeFileWithTag_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.AliceBlue;
        }

        private void LargeFileWithTag_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var addTagsForm = new AddTagsForm();
            addTagsForm.SetApplyCallback(tags =>
                {
                    var ev = new FilesWereTagged { FileNames = new[] { _file.FileName }, TagNames = tags.ToArray() };
                    State.AddAndSaveEvent(ev);
                    

                    this.panel.Controls.Clear();
                    foreach (var tag in _file.Tags)
                    {
                        var button = new Button();
                        button.Text = tag.Value;
                        this.panel.Controls.Add(button);
                    }
                });
            addTagsForm.Show();
            addTagsForm.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);            
        }
    }
}