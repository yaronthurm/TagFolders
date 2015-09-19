using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;

namespace TestTagFolders
{
    public partial class TagsPanel2 : UserControl
    {
        public TagsPanel2()
        {
            InitializeComponent();
        }

        public event Action<TagsPanel2, TagsPanelEventArgs> TagWasSelected;


        public void PopulateTags(IEnumerable<TaggedFile> files, IEnumerable<Tag> filterTags)
        {
            var filesAsList = files.ToList();

            var allTags = filesAsList.SelectMany(x => x.Tags);
            var tags = new Dictionary<Tag, int>();
            foreach (var tag in allTags)
            {
                if (tags.ContainsKey(tag))
                {
                    tags[tag]++;
                }
                else
                {
                    tags[tag] = 1;
                }
            }

            this.flowLayoutPanel1.Controls.Clear();

            foreach (var kvp in tags)
            {
                if (kvp.Value == filesAsList.Count && filterTags.Contains(kvp.Key))
                    continue;

                var button = new Button();
                button.Text = kvp.Key.Value + " [" + kvp.Value.ToString() + "]";
                button.Tag = kvp.Key;
                button.Click += button_Click;
                if (kvp.Value == filesAsList.Count)
                    button.BackColor = Color.DodgerBlue;
                this.flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void FillPanel()
        {
            
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (this.TagWasSelected == null) return;
            var button = sender as Button;
            var tag = button.Tag as Tag;
            this.TagWasSelected(this, new TagsPanelEventArgs { SelectedTag = tag });
        }
    }


    public class TagsPanelEventArgs : EventArgs
    {
        public Tag SelectedTag;
    }
}
