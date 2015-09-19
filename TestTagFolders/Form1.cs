using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTagFolders
{
    public partial class Form1 : Form
    {
        private TagsIntersectionCondition _filter = new TagsIntersectionCondition();
        private List<TaggedFile> _files;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(this.textBox1.Text, "*.*", SearchOption.AllDirectories);
            _files = files.Select(x => TaggedFile.Repository.GetOrCreate(x)).ToList();
            this.filesPanel1.PopulateFiles(_files);
            this.tagsPanel21.PopulateTags(_files);
            this.tagsPanel21.TagWasSelected += tagsPanel21_TagWasSelected;
        }

        void tagsPanel21_TagWasSelected(TagsPanel2 sender, TagsPanelEventArgs args)
        {
            _filter = new TagsIntersectionCondition(
               new TagsUnionCondition(InversableTag.GetTag(args.SelectedTag.Value)));

            var filteredFiles = _filter.Apply(_files);
            this.filesPanel1.PopulateFiles(filteredFiles);
            this.tagsPanel21.PopulateTags(filteredFiles);
        }

        private void filesPanel1_Load(object sender, EventArgs e)
        {

        }
    }
}
