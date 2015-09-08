using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace YaronThurm.TagFolders
{
    /// <summary>
    /// This form is used to add/remove tags to/from a file or to/from multiple files.
    /// In case of a single file, the caller should invoke the public methos "SetFile"
    /// with either the FileWithTags argument, which contains the tags in the object
    /// Tags property, or with the file name as argument, in this case the file will be looked-up
    /// in the database and then the tags will be brouht from its Tags property.
    /// 
    /// In case of multiple files, after all files are resolved into FileWithTag object, 
    /// all mutual tags will be shown in the tags list in order to be manipulated.
    /// </summary>
    public partial class ManageTagsForm : Form
    {
        #region Members
        private FileWithTags file; // in case of single file
        private List<FileWithTags> files; // in case of multiple files
        private List<FileTag> proposedTags;
        private TagFilesDatabase database; // the database to search file names
        private List<FileTag> originalTagsList; // the tags list before any change
        private System.Windows.Forms.AutoCompleteStringCollection autoCompleteSource; // auto-complete source for the add tag textbox

        // Event that will be fired if "Save" was pushed and there are changes from the original tags list
        public event EventHandler ItemsChanged;

        // The database of the file in order to search for file names
        public string DatabaseFileName = "";

        // The option to control whether the "Save" button will also save the modified database into file.
        public bool SaveChangesToFile = false;

        #endregion


        #region Constructor
        public ManageTagsForm(string databaseFileName)
        {
            InitializeComponent();
            this.originalTagsList = new List<FileTag>();
            this.proposedTags = new List<FileTag>();
            
            this.DatabaseFileName = databaseFileName;

            //* In case we have a database to refernce, use its tags list for the auto-complete source
            if (this.DatabaseFileName != "")
            {
                this.database = TagFilesDatabase.DeSerialize(this.DatabaseFileName);

                this.autoCompleteSource = new AutoCompleteStringCollection();
                foreach (FileTag tag in this.database.Tags)
                {
                    this.autoCompleteSource.Add(tag.Value);
                }
                this.txtAddTag.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.txtAddTag.AutoCompleteCustomSource = this.autoCompleteSource;
                this.txtAddTag.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }                       
        }

        #endregion


        private List<FileTag> getMutualTags(List<FileWithTags> files)
        {
            List<FileTag> listOfTags = new List<FileTag>();

            // Check the input
            if (files != null && files.Count > 0)
            {
                // Start with the list of tags of the first file
                listOfTags.AddRange(files[0].Tags);
                
                // Iterate all other files and remove tags that don't exists
                for (int i = 1; i < files.Count; i++)
                {
                    // Iterate each tag in the tags list and search for it in the file's tags list
                    for (int j = listOfTags.Count - 1; j >= 0; j--)
                    {
                        // If the tag is not found, then remove it from the tags list
                        if (!files[i].Tags.Exists(listOfTags[j].Compare))
                            listOfTags.RemoveAt(j);
                    }
                }
            }
            
            return listOfTags;
        }

        private List<FileTag> getUnionOfTags(List<FileWithTags> files)
        {
            FileWithTags tempFile = new FileWithTags();            
            
            // Check the input
            if (files != null && files.Count > 0)
            {
                // Add to the file tags from all other files.
                // The file object will be in charge of removing duplicated tags
                for (int i = 0; i < files.Count; i++)
                {
                    tempFile.Tags.AddRange(files[i].Tags);
                }
            }

            return tempFile.Tags;
        }


        #region SetFiles methods
        /// <summary>
        /// Sets the tags list of the form to show the tags of the input parameter
        /// </summary>
        /// <param name="file"></param>
        public void SetFile(FileWithTags file)
        {
            if (this.files == null)
                this.files = new List<FileWithTags>();
            this.files.Clear();
            this.files.Add(file);

            this.file = file;
            this.Text = file.FileName;

            // Set list box of tags
            this.listTags.Items.Clear();
            foreach (FileTag tag in file.Tags)
            {
                this.listTags.Items.Add(tag);
                this.originalTagsList.Add(tag);
            }

            // Set list box of proposed tags
            this.listProposedTags.Items.Clear();
            foreach (FileTag tag in this.proposedTags)
            {
                if (!file.Tags.Exists(tag.Compare))
                    this.listProposedTags.Items.Add(tag);
            }

            this.listFiles.Items.Clear();
            this.listFiles.Items.Add(file.FileName);
        }

        /// <summary>
        /// Sets the tags list of the form to show all the mutual tags of the input files
        /// </summary>
        /// <param name="files"></param>
        public void SetFiles(List<FileWithTags> files)
        {
            if (files.Count == 1)
            {
                this.SetFile(files[0]);
                return;
            }

            this.files = files;

            // Find all mutual tags
            List<FileTag> listOfTags = this.getMutualTags(this.files);
                        
            this.Text = "Multiple files [" + files.Count.ToString() + "]";

            // Set list box of tags
            this.listTags.Items.Clear();
            foreach (FileTag tag in listOfTags)
            {
                this.listTags.Items.Add(tag);
                this.originalTagsList.Add(tag);
            }

            // Set list box of proposed tags
            this.listProposedTags.Items.Clear();
            foreach (FileTag tag in this.proposedTags)
            {
                if (!listOfTags.Exists(tag.Compare))
                    this.listProposedTags.Items.Add(tag);
            }

            // Set list box of files
            this.listFiles.Items.Clear();
            foreach (FileWithTags file in files)
            {
                this.listFiles.Items.Add(file.FileName);
            }
        }

        /// <summary>
        /// This method gets a list of all selected files and directories names
        /// and sets the form to show all mutual tags between each and every file
        /// within that list and sub-directories of selected directories in the list
        /// </summary>
        /// <param name="filesAndDirectoriesNames"></param>
        public void SetFiles(List<string> filesAndDirectoriesNames)
        {
            // Exit if no database is present
            if (this.database == null)
                return;

            // Get the list of all file names (instead of directories names)
            List<string> fileNames = this.extractAllFilesNames(filesAndDirectoriesNames);

            // Prepare a list of FileWithTags
            List<FileWithTags> files = new List<FileWithTags>();

            // Search each file name in the database
            foreach (string fileName in fileNames)
            {
                // Find file in database
                FileWithTags file = this.database[fileName];
                if (file == null) /* If not found, create the file and add to database */
                {
                    file = new FileWithTags();
                    file.FileName = fileName;
                    this.database.Files.Add(file);
                }
                // Add file to the list
                files.Add(file);
            }

            // Use method to set the visualization
            this.proposedTags = this.getUnionOfTags(this.getOtherFilesInDirectory(filesAndDirectoriesNames));                
            this.SetFiles(files);
        }

        private List<FileWithTags> getOtherFilesInDirectory(List<string> filesAndDirectoriesNames)
        {
            if (this.database == null || filesAndDirectoriesNames == null)
                return new List<FileWithTags>();
            if (filesAndDirectoriesNames.Count == 0)
                return new List<FileWithTags>();


            System.IO.DirectoryInfo parentDirectory = null;

            // Take the first item, Assume it is a file
            System.IO.FileInfo item = new System.IO.FileInfo(filesAndDirectoriesNames[0]);
            // Check if the item is a file
            if (item.Exists /* isFile*/ )
            {
                parentDirectory = item.Directory;
            }
            else /* Maybe a directory*/
            {
                // Assume it is a directory
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(filesAndDirectoriesNames[0]);

                // Check that in fact the name represents a directory
                if (dir.Exists)
                {
                    parentDirectory = dir.Parent;
                }
            }

            if (parentDirectory != null)
            {
                System.IO.FileInfo[] directoryFiles = parentDirectory.GetFiles();
                List<FileWithTags> listOfFiles = new List<FileWithTags>();

                
                // Convert list to lower-case for comparison
                for (int i = 0; i < filesAndDirectoriesNames.Count; i++)
                    filesAndDirectoriesNames[i] = filesAndDirectoriesNames[i].ToLower();
                
                // Create list of all files within the directory that exists in the database
                FileWithTags file;
                foreach (System.IO.FileInfo fi in directoryFiles)
                {
                    if (!filesAndDirectoriesNames.Contains(fi.FullName.ToLower()))
                    {
                        file = this.database[fi.FullName];
                        if (file != null)
                            listOfFiles.Add(file);
                    }
                }

                return listOfFiles;
            }

            return new List<FileWithTags>(); 
        }
              
        #endregion


        #region Raising event method
        private void onItemChanged()
        {
            if (this.ItemsChanged != null)
                this.ItemsChanged(this, EventArgs.Empty);
        }

        #endregion


        #region Files extraction methods
        /// <summary>
        /// This method gets a list of files and directories names and returns a list
        /// of all the files from that list, including files within the directories
        /// in that list.
        /// </summary>
        /// <param name="filesAndDirectoriesNames"></param>
        /// <returns></returns>
        private List<string> extractAllFilesNames(List<string> filesAndDirectoriesNames)
        {
            List<string> filesList = new List<string>();

            foreach (string itemName in filesAndDirectoriesNames)
            {
                // Assume it is a file
                System.IO.FileInfo item = new System.IO.FileInfo(itemName);

                // Check if the item is a file
                if (item.Exists /* isFile*/ )
                {
                    filesList.Add(item.FullName);
                }
                else /* Maybe a directory*/
                {
                    // Assume it is a directory
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(itemName);

                    // Check that in fact the name represents a directory
                    if (dir.Exists)
                    {
                        // Add each file in that directory
                        filesList.AddRange(this.getFilesNamesFromDirectory(dir.FullName));
                    }
                }
            }

            return filesList;
        }

        /// <summary>
        /// This method gets a directory fullname and returns a list of all
        /// file names within that directory and all of its sub-directories
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private List<string> getFilesNamesFromDirectory(string directoryName)
        {
            List<string> filesList = new List<string>();

            // Check that it is a directory
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
            if (!dir.Exists)
                return filesList;

            // Get all files within 1st level
            foreach (System.IO.FileInfo fileInfo in dir.GetFiles())
            {
                filesList.Add(fileInfo.FullName);
            }

            // Add files from each sub-directory
            foreach (System.IO.DirectoryInfo directoryInfo in dir.GetDirectories())
            {
                filesList.AddRange(this.getFilesNamesFromDirectory(directoryInfo.FullName));
            }

            return filesList;
        }

        #endregion


        #region Form controls event handlers
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            this.Hide();

            // Check if the new list of tag is different from the original
            bool isDifferent = false;
            if (this.originalTagsList.Count != this.listTags.Items.Count)
                isDifferent = true;
            else
            {
                foreach (object item in this.listTags.Items)
                {
                    FileTag tag = item as FileTag;
                    if (!this.originalTagsList.Exists(tag.Compare))
                    {
                        isDifferent = true;
                        break;
                    }
                }
            }

            if (isDifferent)
            {
                this.onItemChanged();

                if (this.SaveChangesToFile && this.DatabaseFileName != "")
                    this.database.Serialize(this.DatabaseFileName);
            }

            this.Close();
        }

        private void MamageTagsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason == CloseReason.UserClosing)
                //e.Cancel = true;
            //this.Hide();
        }

        private void btnAddTag_Click(object sender, System.EventArgs e)
        {
            if (this.txtAddTag.Text != "")
            {
                this.listTags.Items.Add(new FileTag(this.txtAddTag.Text));

                foreach (FileWithTags file in this.files)
                    file.Tags.Add(new FileTag(this.txtAddTag.Text));

                this.txtAddTag.Text = "";
            }
        }

        private void btnRemoveTag_Click(object sender, System.EventArgs e)
        {
            if (this.listTags.SelectedItem != null)
            {
                foreach (FileWithTags file in this.files)
                    file.Tags.Remove((FileTag)this.listTags.SelectedItem);
                this.listTags.Items.Remove(this.listTags.SelectedItem);
            }
        }

        private void txtAddTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnAddTag_Click(this, System.EventArgs.Empty);
        }

        #endregion        

        private void btnAddPropesedTags_Click(object sender, EventArgs e)
        {
            // Get selected tags
            List<FileTag> selectedTags = new List<FileTag>();
            for (int i = this.listProposedTags.SelectedItems.Count - 1; i >= 0; i--)
            {
                selectedTags.Insert(0, (FileTag)this.listProposedTags.SelectedItems[i]);
                this.listProposedTags.Items.RemoveAt(this.listProposedTags.SelectedIndices[i]);
            }

            // Add tags
            foreach (FileTag tag in selectedTags)
            {
                this.txtAddTag.Text = tag.Value;
                this.btnAddTag_Click(this, EventArgs.Empty);
            }
        }

        private void listProposedTags_DoubleClick(object sender, EventArgs e)
        {
            if (this.listProposedTags.SelectedItems.Count > 0)
                this.btnAddPropesedTags_Click(this, EventArgs.Empty);
        }
    }
}
