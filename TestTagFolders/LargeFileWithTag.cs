using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestTagFolders
{
    public partial class LargeFileWithTag : UserControl
    {
        private TaggedFile _file;
        public event Action OnChange;

        public LargeFileWithTag()
        {
            InitializeComponent();
            
            foreach (Control ctrl in this.GetAllDecendentsContorls(this))
            {
                this.BindCtrlToMouseEnterAndMouseLeave(ctrl);
                this.BindCtrlToMouseDoubleClick(ctrl);
            }
        }

        private IEnumerable<Control> GetAllDecendentsContorls(Control parent)
        {
            var ret = new List<Control>();
            var queue = new Queue<Control>();
            queue.Enqueue(parent);
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                ret.Add(item);
                foreach (Control innerControl in item.Controls)
                    queue.Enqueue(innerControl);
            }
            return ret;
        }

        private void BindCtrlToMouseEnterAndMouseLeave(Control ctrl)
        {
            ctrl.MouseEnter += (s, e) => this.BackColor = Color.LightSkyBlue; 
            ctrl.MouseLeave += (s, e) => this.BackColor = Color.Transparent; 
        }

        private void BindCtrlToMouseDoubleClick(Control ctrl)
        {
            ctrl.DoubleClick += (s, e) => Process.Start(_file.FileName);
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
                this.SetButtonWidth(button);
                this.BindCtrlToMouseEnterAndMouseLeave(button);
                this.panel.Controls.Add(button);
            }
        }

        private void SetButtonWidth(Button btn)
        {
            btn.AutoSize = true;
            btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btn.AutoEllipsis = false;
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