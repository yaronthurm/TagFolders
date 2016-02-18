using System;
using System.Windows.Forms;

namespace TestTagFolders
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            State.LoadFromFile();
            //TagsPlayground.Play();
            //return;

            //string ext = "C:\\cjh cjckd\\test.txt.exe1234".GetFileExtension();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //YaronThurm.TagFolders.BrowseTagsForm f = new YaronThurm.TagFolders.BrowseTagsForm();           
            //f.SetToFolderContent("D:\\heroes");
            //Application.Run(f);

            Application.Run(new Form1());
        }
    }
}
