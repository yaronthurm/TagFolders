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
            _files = Directory.GetFiles(this.textBox1.Text, "*.*", SearchOption.AllDirectories)
                .Select(x => TaggedFile.Repository.GetOrCreate(x)).ToList();
            this.tagsPanel.TagWasSelected += tagsPanel_TagWasSelected;
            this.tagsCombinationViewer1.RemoveRequested += tagsCombinationViewer1_RemoveRequested;
            this.tagsCombinationViewer1.InverseRequested += tagsCombinationViewer1_InverseRequested;
            this.ApplyFilter();
        }

        private void tagsCombinationViewer1_InverseRequested(TagsCombinationViewer sender, TagsCombinationViewer.TagsCombinationViewerEventArgs e)
        {
            e.Source.Inverse = !e.Source.Inverse;
            this.ApplyFilter();
        }

        private void tagsCombinationViewer1_RemoveRequested(TagsCombinationViewer sender, TagsCombinationViewer.TagsCombinationViewerEventArgs e)
        {
            _filter.Remove(e.Source);
            this.ApplyFilter();
        }

        private void tagsPanel_TagWasSelected(TagsPanel2 sender, TagsPanelEventArgs args)
        {
            _filter.AddUnionCondition(new TagsUnionCondition(InversableTag.GetTag(args.SelectedTag.Value)));
            this.ApplyFilter();
        }


        private void ApplyFilter()
        {
            var filteredFiles = _filter.Apply(_files);
            this.filesPanel.PopulateFiles(filteredFiles);
            this.tagsPanel.PopulateTags(filteredFiles, _filter.AllTags());
            this.tagsCombinationViewer1.Update(_filter);
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            _filter = new TagsIntersectionCondition();
            this.ApplyFilter();
        }
    }
}
