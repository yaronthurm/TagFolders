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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.listTags.Items.Add("Tagged");
            
            var files = Directory.GetFiles(this.textBox1.Text, "*.*", SearchOption.AllDirectories);
            foreach (var item in files.Take(100))
            {
                var view = new LargeFileWithTag();
                var shellFile = ShellFile.FromFilePath(item);
                shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.Default;
                view.SetData(shellFile.Thumbnail.MediumBitmap, item, new[]{"tag1", "tag2"});
                this.flowLayoutPanel1.Controls.Add(view);
            }
            //this.explorerBrowser1.Navigate((ShellObject)KnownFolders.Desktop);
        }
    }
}
