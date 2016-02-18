using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TestTagFolders
{
    public partial class LargeFileWithTag : UserControl
    {
        private TaggedFile _file;
        public event Action OnChange;

        public LargeFileWithTag()
        {
            InitializeComponent();
        }


        public void SetData(Bitmap thumbnail, TaggedFile file)
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
                    var ev = new TagsWereAddedToFiles { FileNames = new[] { _file.FileName }, TagsThatWereAdded = tags.ToArray() };
                    State.AddAndSaveEvent(ev);
                    

                    this.panel.Controls.Clear();
                    foreach (var tag in _file.Tags)
                    {
                        var button = new Button();
                        button.Text = tag.Value;
                        this.panel.Controls.Add(button);
                    }
                    if (this.OnChange != null)
                        this.OnChange();
                });
            addTagsForm.Show();
            addTagsForm.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);            
        }
    }
}