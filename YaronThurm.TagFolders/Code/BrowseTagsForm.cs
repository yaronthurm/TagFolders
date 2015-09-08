using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace YaronThurm.TagFolders
{
    public partial class BrowseTagsForm : Form
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        #region Constants
        private const string noTagsString = "*No Tags*";
        private const string noMoreTagsString = "*No More Tags*";
        private const string databaseFileName = "D:\\database.txt";
        #endregion


        #region Members
        private TagFilesDatabase fullDatabase;
        private TagFilesDatabase currentDatabase;
        private TagsCombinaton tagsCombination;
        private ManageTagsForm manageTagsForm;
        private IconExtractor iconExtractor;        
        
        /// <summary>
        /// This membera stores the location of the mouse whenever the user right-clicks on a file
        /// </summary>
        private Point mouseLocation = new Point();

        #endregion


        #region Constructor
        public BrowseTagsForm()
        {
            InitializeComponent();
            

            // Initialize all the members of the form
            this.fullDatabase = new TagFilesDatabase();
            this.currentDatabase = new TagFilesDatabase();
            this.tagsCombination = new TagsCombinaton();
            this.iconExtractor = new IconExtractor(this.imageListSmallIcons,
                this.imageListLargeIcons, 2);            

            // Subscribe to the FileSystemWatcher events
            this.fileSystemWatcherAllFiles.Renamed += new System.IO.RenamedEventHandler(fileSystemWatcher1_Renamed);
            this.fileSystemWatcherAllFiles.Deleted += new System.IO.FileSystemEventHandler(fileSystemWatcher1_Deleted);
            this.fileSystemWatcherAllFiles.Created += new System.IO.FileSystemEventHandler(fileSystemWatcher1_Created);                        

            // Subscribe to the TagsCombiationViewer events
            this.tagsCombinationViewer.RemoveRequested += new TagsCombinationViewer.RemoveHandler(tagsCombinationViewer1_RemoveRequested);
            this.tagsCombinationViewer.InverseRequested += new TagsCombinationViewer.InverseHandler(tagsCombinationViewer1_InverseRequested);

                        
            InitializeVirtualObjectListView();
            InitializeTagsListView();
            SetWindowTheme(this.listViewTags.Handle, "explorer", null);
            SetWindowTheme(this.virtualListViewFiles.Handle, "explorer", null);
        }

        #endregion

        private void InitializeVirtualObjectListView()
        {
            this.virtualListViewFiles.RowGetter = delegate(int i)
            {
                FileWithTags file = this.currentDatabase.Files[i];
                return file;
            };
            // Show the files
            this.olvColumnFileName.AspectGetter = delegate(object x)
            {
                FileWithTags file = (FileWithTags)x;
                return file.FileNameWithoutPath;

            };
            this.olvColumnFileName.AspectPutter = delegate(object x, object newValue)
            {
                FileWithTags file = (FileWithTags)x;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file.FileName);
                string newFileName = file.FilePath + "\\" + newValue;

                try
                {
                    fileInfo.MoveTo(newFileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            };
            // Draw the icon next to the name
            this.olvColumnFileName.ImageGetter = delegate(object x)
            {
                FileWithTags file = (FileWithTags)x;
                //CShItem
                return this.iconExtractor.GetIndexByFileName(file.FileName);                
            };
            // Show the tags
            this.olvColumnTags.AspectGetter = delegate(object x)
            {
                FileWithTags file = (FileWithTags)x;                
                return file.Tags.ToString();
            };
        }

        private void InitializeTagsListView()
        {            
            // Show the tags
            /*this.olvColumnTag.AspectGetter = delegate(object x)
            {
                FileTag tag = (FileTag)x;
                int fileCount = this.currentDatabase.CountFilesByTag(tag);
                return (tag.Value == "" ? 
                    noTagsString : 
                    tag.ToString()) + "\n  [" + fileCount + "]";

            };
            this.olvColumnTag.ImageGetter = delegate(object x)
            {
                FileTag tag = (FileTag)x;
                int fileCount = this.currentDatabase.CountFilesByTag(tag);
                if (fileCount < this.currentDatabase.Files.Count)
                    return 0;
                else
                    return 1;
            };
            this.olvColumnFilesCount.AspectGetter = delegate(object x)
            {
                FileTag tag = (FileTag)x;
                return this.currentDatabase.CountFilesByTag(tag);
            };
            this.olvColumnGroup.AspectGetter = delegate(object x)
            {
                FileTag tag = (FileTag)x;
                // Map the tag to it's group
                int tagIndex = this.currentDatabase.Tags.IndexOf(tag);
                int groupKeyIndex = this.currentDatabase.tagGroupMapping[tagIndex];
                string groupKey = this.currentDatabase.groupsKeys[groupKeyIndex];
                return groupKey;                
            };*/
        }

        #region FileSystemWatcher

        private List<string> deletedItemsName = new List<string>();
        private List<string> createdItemsName = new List<string>();
        private System.Timers.Timer timer;
        delegate void Deleting(string fullPath);

        
        private void fileSystemWatcher1_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            // Only looking for moveing events, so if the deletedFile is empty no moveing was made
            if (this.deletedItemsName.Count == 0)
                return;

            this.createdItemsName.Add(e.FullPath);

            lock (this)
            {
                int index = this.createdItemsName.Count - 1;
                // Check if the file was moved by comparing the name of the created one with the deleted one
                if (this.createdItemsName[index].GetFileName() == this.deletedItemsName[index].GetFileName()) /* moved*/
                {
                    this.doRename(this.deletedItemsName[index], this.createdItemsName[index]);
                    this.deletedItemsName.RemoveAt(index);
                }
                this.createdItemsName.RemoveAt(index);
            }
        }
        private void fileSystemWatcher1_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            lock (this)
            {
                // Save to memory
                this.deletedItemsName.Add(e.FullPath);
            }

            // Start the timer
            if (this.timer == null)
            {
                this.timer = new System.Timers.Timer(500);
                this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            }
            this.timer.Start();
        }
        private void fileSystemWatcher1_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            // Perform renaming
            this.doRename(e.OldFullPath, e.FullPath);
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                // Items were deleteded, not moved
                //Deleting del = new Deleting(this.doDelete);
                //foreach (string itemFullPath in this.deletedItemsName)
                    //this.Invoke(del, new object[] {itemFullPath});

                this.deletedItemsName.Clear();
                this.timer.Stop();
            }
        }

        private void doDelete(string itemFullPath)
        {
            // Assume it is a file:
            // find the file in the database
            FileWithTags file = this.fullDatabase[itemFullPath];
            if (file != null) // If it is a file, and it is in the database
            {
                // Remove the file
                this.fullDatabase.Files.Remove(file);
                // Show the updated database 
                this.showCurrentDatabse();
                // Finish
                return;
            }

            // Assume it is a directory:
            // Remove each file that is inside that directory
            for (int i = this.fullDatabase.Files.Count - 1; i >= 0; i--)
            {
                file = this.fullDatabase.Files[i];

                // Findout if the file begins with the directory path
                if (file.FileName.ToLower().StartsWith(itemFullPath.ToLower() + "\\"))
                {
                    // Remove the file
                    this.fullDatabase.Files.Remove(file);
                }
                // Show the updated database 
                this.showCurrentDatabse();
            }
        }
        private void doRename(string oldFullPath, string newFullPath)
        {
            bool actionPerformd = false;
            
            // If the item changed is a file
            if (newFullPath.IsFile())
            {
                // find old file in database
                FileWithTags file = this.fullDatabase[oldFullPath];
                if (file != null) // If file was found in the database
                {
                    // Change its name
                    file.FileName = newFullPath;
                    
                    // Update the icon mapping

                    // Show the updated database 
                    this.showCurrentDatabse();

                    // Mark that an action was performed
                    actionPerformd = true;
                }
            }
            else if (newFullPath.IsDirectory()) // If the item changed is a directory
            {
                // Change the directory name of each affected file in the database
                foreach (FileWithTags file in this.fullDatabase.Files)
                {
                    // Findout if the file begins with the old directory path
                    if (file.FileName.ToLower().StartsWith(oldFullPath.ToLower() + "\\"))
                    {
                        // Remove the old path
                        file.FileName = file.FileName.Remove(0, oldFullPath.Length);
                        // Insert the new path
                        file.FileName = newFullPath + file.FileName;
                        actionPerformd = true;
                    }
                }
            }
            // Show balloon with information if there was a change
            if (actionPerformd)
            {
                this.notifyIcon.ShowBalloonTip(200, "TagFolders", 
                    "File\\Directory changed\n" +
                    oldFullPath + " changed to: \n" + 
                    newFullPath, ToolTipIcon.Info);

                // Save changes
                this.menuMain_File_Save_Click(this, EventArgs.Empty);
            }
        }
        
        #endregion
          

        #region Methods
        /// <summary>
        /// This method is in charge of updating the tags listView and the files listView in accordence
        /// with the data stored in the object 'database' and in 'tagsCombination'.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tagsCombination"></param>
        private void updateView(TagFilesDatabase database, TagsCombinaton tagsCombination)
        {
            // Update tags listview
            this.updateTagsListView(this.listViewTags, database, tagsCombination, this.txtSearchTag.Text);

            // Update files listview
            this.updateFilesListView(this.listViewFiles);            

            // Update the TagsCombinationViewer control
            this.tagsCombinationViewer.Update(tagsCombination);
        }

        private void setTagsListViewObject()
        {
            /*List<FileTag> list = new List<FileTag>();
            foreach (FileTag tag in this.currentDatabase.Tags)
            {
                int filesCount = this.currentDatabase.CountFilesByTag(tag);

                if (filesCount < this.currentDatabase.Files.Count)
                    list.Add(tag);
                else if (this.tagsCombination.ContainsByValue(tag, false))
                {
                    // Don't add to list
                }
                else
                    list.Add(tag);

            }
            this.objectListView1.SetObjects(list);*/
        }

        /// <summary>
        /// 
        /// </summary>
        private void updateTagsListView(ListView tagsListView, TagFilesDatabase database, TagsCombinaton tagsCombination, string searchTagText)
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    tagsListView1.Groups.Add("key_"+i.ToString(), "name_" + i.ToString());
            //}
            //tagsListView.BeginUpdate();
            //tagsListView.Clear();
            //for (int i = 0; i < 100; i++)
            //{
            //    ListViewItem item = new ListViewItem();
            //    item.Text = "T".PadLeft(12, 'a');
            //    item.ImageIndex = i % 2;
            //    item.Group = tagsListView1.Groups[i % 5];
            //    item.Tag = new object();
            //    this.tagsListView1.Items.Add(item);
            //}
            //tagsListView.EndUpdate();
            //return;

            this.setTagsListViewObject();

            this.tagsListView1.UpdateItems(database, tagsCombination, searchTagText);

            // Create the groups list. (Skip the default group - index 0)
            tagsListView.Groups.Clear();
            for (int i = 1; i < database.groupsNames.Count; i++)
            {
                string groupName = database.groupsNames[i];
                string groupKey = database.groupsKeys[i];
                tagsListView.Groups.Add(groupKey, groupName);
            }
            // Add the default group (with index 0) as the last group
            tagsListView.Groups.Add(database.groupsKeys[0], database.groupsNames[0]);

            // Create item for the listView:
            List<ListViewItem> tagsList = new List<ListViewItem>();
            ListViewItem tagItem;
            // Add each tag from the database
            for (int i = 0; i < database.Tags.Count; i++)
            {
                // Extract the tag
                FileTag tag = database.Tags[i];

                // Show the tag only if it's not in the history of tags being selected - 
                // (You should not show tag 'X' after selecting tag 'X')
                bool showTagCondition;
                showTagCondition =                   
                    !tagsCombination.ContainsByValue(tag, false) && 
                    tag.Value.ToLower().Contains(searchTagText.ToLower());
                if (showTagCondition)
                {
                    // Set the tag item (including text of the tag with the tag's file-count
                    tagItem = new ListViewItem();
                    tagItem.Text = (tag.Value == "" ? noTagsString : tag.ToString()) +
                        "\n  [" + database.CountFilesByTag(tag).ToString() + "]";
                    tagItem.Tag = tag;

                    // Set image of the tag: special image if the tag's file-count is equals the
                    // database file-count. a regular folder image if not.
                    if (database.Files.Count == database.CountFilesByTag(tag))
                        tagItem.ImageIndex = 1;
                    else
                        tagItem.ImageIndex = 0;

                    // Map the tag to it's group
                    int groupKeyIndex = database.tagGroupMapping[i];
                    string groupKey = database.groupsKeys[groupKeyIndex];
                    tagItem.Group = tagsListView.Groups[groupKey];
                    tagsList.Add(tagItem);
                }
            }

            // ** Sort the tags by groups, and inside of each group
            ListViewItem tmp = new ListViewItem();
            for (int i = 0; i < tagsList.Count; i++)
            {
                for (int j = i + 1; j < tagsList.Count; j++)
                {
                    int comperator = tagsList[i].Group.Name.CompareTo(tagsList[j].Group.Name);
                    if (comperator == 1) // item[i].Group > item[j].Group
                    { // Swap items
                        tmp = (ListViewItem)tagsList[i].Clone();
                        tagsList[i] = (ListViewItem)tagsList[j].Clone();
                        tagsList[j] = tmp;
                    }
                    else if (comperator == 0) // Equal groups
                    { // Sort internally

                        comperator = ((FileTag)tagsList[i].Tag).ToString().CompareTo(
                            ((FileTag)tagsList[j].Tag).ToString());

                        if (comperator == 1)
                        {
                            tmp = (ListViewItem)tagsList[i].Clone();
                            tagsList[i] = (ListViewItem)tagsList[j].Clone();
                            tagsList[j] = tmp;
                        }
                    }
                }
            }


            // ** Add to list view
            tagsListView.BeginUpdate();
            tagsListView.Clear();
            foreach (ListViewItem item in tagsList)
                tagsListView.Items.Add(item);
            
            tagsListView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void updateFilesListView(ListView filesListView)
        {
             
            if (this.listViewFiles.VirtualListSize != this.currentDatabase.Files.Count)
            {
                this.listViewFiles.VirtualListSize = 0;
                this.virtualListViewFiles.VirtualListSize = 0;
            }
            
            this.listViewFiles.VirtualListSize = this.currentDatabase.Files.Count;
            this.lblTotalFilesTitle.Text = "Total files: " + this.currentDatabase.Files.Count.ToString();
           
            this.virtualListViewFiles.VirtualListSize = this.currentDatabase.Files.Count;
            this.virtualListViewFiles.BuildList();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void showCurrentDatabse()
        {
            this.currentDatabase = this.fullDatabase.GetPartialDatabase(this.tagsCombination);
            this.updateView(this.currentDatabase, this.tagsCombination);
        }

        /// <summary>
        /// 
        /// </summary>
        private void moveBack()
        {
            if (this.tagsCombination.Count == 0 && this.txtSearchTag.Text == "")
                return;

            // First clear the search text if exists
            if (this.txtSearchTag.Text != "")
            {
                this.txtSearchTag.Text = "";
                this.updateTagsListView(this.listViewTags, this.currentDatabase, this.tagsCombination, this.txtSearchTag.Text);
                return;
            }

            // Then remove the current level from the tagsCombination list
            if (this.tagsCombination.Count > 0)
                this.tagsCombination.RemoveAt(this.tagsCombination.Count - 1);

            this.showCurrentDatabse();
        }
        #endregion


        #region Forms events handlers
        private void BrowseTagsForm_Load(object sender, EventArgs e)
        {
            // Set initial visualization to form                        
            this.comboTagsView.SelectedIndex = (int)View.SmallIcon;
            this.comboFilesView.SelectedIndex = (int)View.List;

            // In case where the database is already loaded, show the files
            if (this.currentDatabase != null && this.currentDatabase.Files.Count > 0)
                this.updateView(this.currentDatabase, this.tagsCombination);

            // Set icon for the NotifyIcon object
            this.notifyIcon.Icon = this.Icon;            

            // Load database
            this.menuMain_File_Restore_Click(this, EventArgs.Empty);
        }
        private void BrowseTagsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the reason for closing was UserClosing, hide the form instead of closing it
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        /// <summary>
        /// This method is the events handler method of the ItemsChanges event
        /// of the ManageTagsForm form. It is being raised whenever a change was made by
        /// the form (tags added or removed).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manageTagsForm_ItemsChanged(object sender, EventArgs e)
        {
            // Show the current databse (this includes refreshing of the current view)
            this.showCurrentDatabse();
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.moveBack();
        }

        private void txtSearchTag_TextChanged(object sender, EventArgs e)
        {
            this.updateTagsListView(this.listViewTags, this.currentDatabase, this.tagsCombination, this.txtSearchTag.Text);
        }
        private void txtSearchTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.listViewTags.Items.Count == 1)
            {
                this.listViewTags.Items[0].Selected = true;
                this.listViewTags_ItemActivate(this.listViewFiles, EventArgs.Empty);
            }
        }

        private void listViewTags_ItemActivate(object sender, EventArgs e)
        {
            if (this.listViewTags.SelectedItems.Count == 0)
                return;

            // Create a tag for every selected item (from the tags listView)
            List<FileTag> tags = new List<FileTag>();
            foreach (ListViewItem item in this.listViewTags.SelectedItems)
            {
                FileTag tag = (FileTag)item.Tag;
                tags.Add(tag);
            }
            // Add the tags to the tags combination structure
            this.tagsCombination.Add(tags);

            // Reset the search text box for the next search
            if (this.txtSearchTag.Text != "")
                this.txtSearchTag.Text = "";

            // Update the current database and view
            this.showCurrentDatabse();
        }
        private void listViewTags_MouseUp(object sender, MouseEventArgs e)
        {
            // Add a contxt menu strip for the right mouse click
            if (e.Button == MouseButtons.Right)
            {
                if (this.listViewTags.SelectedItems.Count == 0)
                    this.listViewTags.ContextMenuStrip = null;
                else if (this.listViewTags.SelectedItems.Count > 1)
                {
                    // Clear the groups menu
                    this.menuManyTags_AttachTagsToGroup.DropDownItems.Clear();
                    // Add an item for each group
                    foreach (string group in this.fullDatabase.groupsNames)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(group);
                        item.Click += new EventHandler(this.menuTags_Group_Click);
                        this.menuManyTags_AttachTagsToGroup.DropDownItems.Add(item);
                    }
                    // Add a text-box for a new group
                    ToolStripTextBox menuTags_NewGroupTxtBox = new ToolStripTextBox();
                    menuTags_NewGroupTxtBox.Text = "קבוצה חדשה";
                    menuTags_NewGroupTxtBox.BackColor = Color.Yellow;
                    menuTags_NewGroupTxtBox.KeyDown += new KeyEventHandler(this.menuTags_NewGroupTxtBox_KeyDown);
                    this.menuManyTags_AttachTagsToGroup.DropDownItems.Add(menuTags_NewGroupTxtBox);

                    this.listViewTags.ContextMenuStrip = this.contextMenuManyTags;
                }
                else if (this.listViewTags.SelectedItems.Count == 1)
                {
                    // Clear the groups menu
                    this.menuSingleTag_AttachTagToGroup.DropDownItems.Clear();
                    // Add an item for each group
                    foreach (string group in this.fullDatabase.groupsNames)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(group);
                        item.Click += new EventHandler(this.menuTags_Group_Click);
                        this.menuSingleTag_AttachTagToGroup.DropDownItems.Add(item);
                    }
                    // Add a text-box for a new group
                    ToolStripTextBox menuTags_NewGroupTxtBox = new ToolStripTextBox();
                    menuTags_NewGroupTxtBox.Text = "קבוצה חדשה";
                    menuTags_NewGroupTxtBox.BackColor = Color.Yellow;
                    menuTags_NewGroupTxtBox.KeyDown += new KeyEventHandler(this.menuTags_NewGroupTxtBox_KeyDown);
                    this.menuSingleTag_AttachTagToGroup.DropDownItems.Add(menuTags_NewGroupTxtBox);

                    this.listViewTags.ContextMenuStrip = this.contextMenuSingleTag;
                }

                // Save the location of the mouse
                this.mouseLocation.X = e.X;
                this.mouseLocation.Y = e.Y;
            }
        }
        private void listViewTags_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            return;

            ListViewItem item = e.Item;
            FileTag tag = (FileTag)item.Tag;

            string tooltipText = "Tags:\n";

            foreach (FileTag tag1 in this.currentDatabase.GetPartialDatabase(tag).Tags)
            {
                if (tag1.Value != tag.Value)
                    tooltipText += tag1.Value + "\n";
            }

            item.ToolTipText = tooltipText;
        }

        private void listViewFiles_ItemActivate(object sender, EventArgs e)
        {
            // Start a proccess for each selected file
            foreach (int fileItemIndex in this.listViewFiles.SelectedIndices)
            {
                FileWithTags file = this.currentDatabase.Files[fileItemIndex];

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = file.FileName;
                try
                {
                    p.Start();
                }
                catch { }
            }
        }
        private void listViewFiles_MouseUp(object sender, MouseEventArgs e)
        {
            // Add a contxt menu strip for the right mouse click
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuManyFiles.Tag = this.listViewFiles;
                this.contextMenuSingleFile.Tag = this.listViewFiles;

                if (this.listViewFiles.SelectedIndices.Count > 1)
                    this.listViewFiles.ContextMenuStrip = this.contextMenuManyFiles;
                else if (this.listViewFiles.SelectedIndices.Count == 1)
                    this.listViewFiles.ContextMenuStrip = this.contextMenuSingleFile;
                else
                    this.listViewFiles.ContextMenuStrip = null;

                // Save the mouse location
                this.mouseLocation.X = e.X;
                this.mouseLocation.Y = e.Y;
            }            
        }        
        private void listViewFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
                return;

            // Extract the file
            FileWithTags file = (FileWithTags)e.Item.Tag; 

            // Set text box to show the file
            this.txtAddFile.Text = file.FileName;

            // Set list box to show the file's tags
            this.listTags.Items.Clear();
            foreach (FileTag tag in file.Tags)
            {
                this.listTags.Items.Add(tag);
            }

            // Set status bar to show the file
            this.toolStripStatusLabel1.Text = file.ToString();
        }
        private void listViewFiles_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            FileWithTags file = this.currentDatabase.Files[e.ItemIndex];
            ListViewItem item = new ListViewItem();

            item.Text = file.FileNameWithoutPath;
            item.Tag = file;
            item.ImageIndex = this.iconExtractor.GetIndexByFileName(file.FileName);

            e.Item = item;
        }


        public void LoadFromExternal()
        {
            this.menuMain_File_Restore_Click(null, null);
        }
        private void menuMain_File_Restore_Click(object sender, EventArgs e)
        {
            // Load database
            this.fullDatabase = TagFilesDatabase.DeSerialize(databaseFileName);

            SQL_Manager sql = new SQL_Manager(this.fullDatabase);
            sql.GetFilesByTag(new List<string> {"ירון", "תמר"});
            // Show the database
            this.showCurrentDatabse();
        }
        private void menuMain_File_Save_Click(object sender, EventArgs e)
        {
            this.fullDatabase.Serialize(databaseFileName);
        }
        
        private void menuSingleFile_RemoveFile_Click(object sender, EventArgs e)
        {
            this.menuManyFiles_RemoveFiles_Click(sender, e);
        }
        private void menuSingleFile_ManageTags_Click(object sender, EventArgs e)
        {
            this.menuManyFiles_ManageTags_Click(sender, e);
        }
        private void menuSingleFile_OpenTargetDirectory_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ListView listView = (ListView)item.GetCurrentParent().Tag;
            foreach (int fileItemIndex in listView.SelectedIndices)
            {
                FileWithTags file = this.currentDatabase.Files[fileItemIndex];

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "explorer.exe";
                p.StartInfo.Arguments = "/select," + file.FileName;
                p.StartInfo.UseShellExecute = false;
                try
                {
                    p.Start();
                }
                catch { }
            }
        }
        
        private void menuManyFiles_RemoveFiles_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ListView listView = (ListView)item.GetCurrentParent().Tag;
            foreach (int fileItemIndex in listView.SelectedIndices)
            {
                FileWithTags file = this.currentDatabase.Files[fileItemIndex];
                this.fullDatabase.Files.Remove(file);
            }
            this.showCurrentDatabse();
        }        
        private void menuManyFiles_ManageTags_Click(object sender, EventArgs e)
        {
            // Dispose form if already exists
            if (this.manageTagsForm != null)
                this.manageTagsForm.Dispose();

            this.manageTagsForm = new ManageTagsForm(databaseFileName);
            this.manageTagsForm.ItemsChanged += new EventHandler(this.manageTagsForm_ItemsChanged);

            // Prepare files list
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ListView listView = (ListView)item.GetCurrentParent().Tag;
            List<FileWithTags> files = new List<FileWithTags>();
            foreach (int fileItemIndex in listView.SelectedIndices)
            {
                FileWithTags file = this.currentDatabase.Files[fileItemIndex];
                files.Add(file);
            }

            // Set files
            this.manageTagsForm.SetFiles(files);

            // Show form
            this.manageTagsForm.Show();
            this.manageTagsForm.Focus();

            // Move to the click area
            this.manageTagsForm.Left = this.Left + listView.Left + this.mouseLocation.X;
            this.manageTagsForm.Top = this.Top + listView.Top + this.mouseLocation.Y;
        }                                
        
        private void menuSingleTag_ReverseChoice_Click(object sender, EventArgs e)
        {
            this.menuManyTags_RevesreChoice_Click(sender, e);
        }
        private void menuManyTags_RevesreChoice_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listViewTags.SelectedItems)
            {
                FileTag tag = (FileTag)item.Tag;
                FileTag newTag = new FileTag(tag.Value);
                newTag.Inverse = true;
                this.tagsCombination.Add(newTag);
            }
            this.showCurrentDatabse();
        }
        private void menuTags_NewGroupTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            string groupName = ((ToolStripTextBox)sender).Text;
            if (e.KeyCode == Keys.Enter)
            {
                this.fullDatabase.CreateGroup(groupName, groupName);
                foreach (ListViewItem item in this.listViewTags.SelectedItems)
                {
                    string tagValue = ((FileTag)item.Tag).Value;
                    this.fullDatabase.MapTagToGroup(tagValue, groupName);
                }
                this.contextMenuSingleTag.Close();
                this.showCurrentDatabse();
            }
        }        
        private void menuTags_Group_Click(object sender, EventArgs e)
        {
            string groupName = ((ToolStripMenuItem)sender).Text;
            foreach (ListViewItem item in this.listViewTags.SelectedItems)
            {
                string tagValue = ((FileTag)item.Tag).Value;
                this.fullDatabase.MapTagToGroup(tagValue, groupName);
            }
            this.showCurrentDatabse();
        }

        private void notifyMenuOpen_Click(object sender, EventArgs e)
        {
            // Show the BrowseTagsForm
            this.Show();
        }
        private void notifyMenuExit_Click(object sender, EventArgs e)
        {
            // Exit the program
            Application.Exit();
        }
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.notifyMenuOpen_Click(sender, e);
        }

        private void tagsCombinationViewer1_RemoveRequested(TagsCombinationViewer sender, TagsCombinationViewer.TagsCombinationViewerEventArgs e)
        {
            this.tagsCombination.RemoveAt(e.IndexI, e.IndexJ);
            this.showCurrentDatabse();
        }
        private void tagsCombinationViewer1_InverseRequested(TagsCombinationViewer sender, TagsCombinationViewer.TagsCombinationViewerEventArgs e)
        {
            this.tagsCombination[e.IndexI][e.IndexJ].Inverse = !this.tagsCombination[e.IndexI][e.IndexJ].Inverse;
            this.showCurrentDatabse();
        }
        #endregion
        
        
        #region Public Methods
        public void Browse(string path)
        {
            List<string> paths = new List<string>();
            paths.Add(path);
            this.Browse(paths);            
        }

        public void Browse(List<string> paths)
        {
            this.fullDatabase = TagFilesDatabase.DeSerialize(databaseFileName);
            this.currentDatabase = this.fullDatabase.GetPartialDatabase(paths);
        }

        #endregion        

        private void listViewFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;

            FileWithTags file = this.currentDatabase.Files[e.Item];
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file.FileName);
            string newFileName = file.FilePath + "\\" + e.Label;

            try
            {
                if (newFileName.ToLower() != file.FileName.ToLower())
                    fileInfo.MoveTo(newFileName);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboTagsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.listViewTags.View = (View)this.comboTagsView.SelectedIndex;
                //this.objectListView1.View = (View)this.comboTagsView.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboFilesView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.listViewFiles.View = (View)this.comboFilesView.SelectedIndex;
                this.virtualListViewFiles.View = (View)this.comboFilesView.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void virtualListViewFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
                return;

            // Extract the file
            FileWithTags file = (FileWithTags)this.currentDatabase.Files[e.ItemIndex];

            // Set text box to show the file
            this.txtAddFile.Text = file.FileName;

            // Set list box to show the file's tags
            this.listTags.Items.Clear();
            foreach (FileTag tag in file.Tags)
            {
                this.listTags.Items.Add(tag);
            }

            // Set status bar to show the file
            this.toolStripStatusLabel1.Text = file.ToString();
        }

        private void virtualListViewFiles_ItemActivate(object sender, EventArgs e)
        {
            // Start a proccess for each selected file
            foreach (int fileItemIndex in this.virtualListViewFiles.SelectedIndices)
            {
                FileWithTags file = this.currentDatabase.Files[fileItemIndex];

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = file.FileName;
                try
                {
                    p.Start();
                }
                catch { }
            }
        }

        private void virtualListViewFiles_MouseUp(object sender, MouseEventArgs e)
        {
            // Add a contxt menu strip for the right mouse click
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuManyFiles.Tag = this.virtualListViewFiles;
                this.contextMenuSingleFile.Tag = this.virtualListViewFiles;

                if (this.virtualListViewFiles.SelectedIndices.Count > 1)
                    this.virtualListViewFiles.ContextMenuStrip = this.contextMenuManyFiles;
                else if (this.virtualListViewFiles.SelectedIndices.Count == 1)
                    this.virtualListViewFiles.ContextMenuStrip = this.contextMenuSingleFile;
                else
                    this.virtualListViewFiles.ContextMenuStrip = null;

                // Save the mouse location
                this.mouseLocation.X = e.X;
                this.mouseLocation.Y = e.Y;
            }
        }

        private void tagsListView1_ItemActivate(object sender, EventArgs e)
        {
            if (this.tagsListView1.SelectedItems.Count == 0)
                return;

            // Create a tag for every selected item (from the tags listView)
            List<FileTag> tags = new List<FileTag>();
            foreach (ListViewItem item in this.tagsListView1.SelectedItems)
            {
                FileTag tag = (FileTag)item.Tag;
                tags.Add(tag);
            }
            // Add the tags to the tags combination structure
            this.tagsCombination.Add(tags);

            // Reset the search text box for the next search
            if (this.txtSearchTag.Text != "")
                this.txtSearchTag.Text = "";

            // Update the current database and view
            this.showCurrentDatabse();
        }
    }
}
