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
        public LargeFileWithTag()
        {
            InitializeComponent();
        }


        public void SetData(Bitmap thumbnail, string fileFullPath, string[] tags)
        {
            this.pictureBox1.Image = thumbnail;
            this.lblFileName.Text = Path.GetFileName(fileFullPath);

            foreach (var tag in tags)
            {
                var button = new Button();
                button.Text = tag;
                this.panel.Controls.Add(button);
            }
        }
    }
}