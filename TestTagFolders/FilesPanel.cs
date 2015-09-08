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
    public partial class FilesPanel : UserControl
    {
        private List<string> _files;
        private int _currentPage;

        public FilesPanel()
        {
            InitializeComponent();
        }

        private int PageSize
        {
            get
            {
                return (int)this.numericUpDown1.Value;
            }
        }
        private int TotalPages
        {
            get
            {
                if (_files.Count % PageSize == 0)
                    return _files.Count / PageSize;
                else
                    return _files.Count / PageSize + 1;
            }
        }

        public void PopulateFiles(IEnumerable<string> files)
        {
            _files = files.ToList();
            if (_files.Count < PageSize)
            {
                this.btnNext.Enabled = false;
                this.btnPrev.Enabled = false;
            }
            else
            {
                this.btnNext.Enabled = true;
                this.btnPrev.Enabled = false;
            }
            
            _currentPage = 1;
            this.FillPanel();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.btnPrev.Enabled = true;
            this._currentPage++;
            if (_currentPage == TotalPages)
                this.btnNext.Enabled = false;
            this.FillPanel();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.btnNext.Enabled = true;
            this._currentPage--;
            if (_currentPage == 1)
                this.btnPrev.Enabled = false;
            this.FillPanel();
        }

        private void FillPanel()
        {            
            this.flowLayoutPanel1.Controls.Clear();

            int start = (_currentPage - 1) * PageSize + 1;
            int end = Math.Min(start + PageSize - 1, _files.Count);

            lblCount.Text = string.Format("{0}-{1}/{2}", start, end, _files.Count);

            var items = _files
                .Skip(start - 1)
                .Take(PageSize)
                .Select(x =>
            {
                var fileWithTags = FilesWithTagsRepository.GetOrCreate(x);
                var item = new LargeFileWithTag();
                var shellFile = ShellFile.FromFilePath(x);
                shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.Default;
                item.SetData(shellFile.Thumbnail.MediumBitmap, fileWithTags);
                return item;
            }).ToArray();


            this.flowLayoutPanel1.Controls.AddRange(items);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //_currentPage = Math.Min(_currentPage, TotalPages - 1);
        }
    }
}
