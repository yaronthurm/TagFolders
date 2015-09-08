namespace YaronThurm.TagFolders
{
    partial class BrowseTagsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseTagsForm));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("zzz");
            this.imageListLargeIcons = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmallIcons = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuSingleFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSingleFile_ManageTags = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSingleFile_RemoveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSingleFile_OpenTargetDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.txtAddFile = new System.Windows.Forms.TextBox();
            this.listTags = new System.Windows.Forms.ListBox();
            this.contextMenuManyFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuManyFiles_ManageTags = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManyFiles_RemoveFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain_File_Restore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.contextMenuSingleTag = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSingleTag_RenameTag = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSingleTag_AttachTagToGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSingleTag_RemoveTag = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSingleTag_ReverseChoice = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuManyTags = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuManyTags_AttachTagsToGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManyTags_RemoveTags = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManyTags_RevesreChoice = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTotalFilesTitle = new System.Windows.Forms.Label();
            this.fileSystemWatcherAllFiles = new System.IO.FileSystemWatcher();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.listViewTags = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tagsListView1 = new YaronThurm.TagFolders.TagsListView();
            this.virtualListViewFiles = new BrightIdeasSoftware.VirtualObjectListView();
            this.olvColumnFileName = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnTags = new BrightIdeasSoftware.OLVColumn();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboTagsView = new System.Windows.Forms.ComboBox();
            this.comboFilesView = new System.Windows.Forms.ComboBox();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.txtSearchTag = new YaronThurm.Controls.TextBoxes.TextBox2();
            this.btnBack = new System.Windows.Forms.Button();
            this.tagsCombinationViewer = new YaronThurm.TagFolders.TagsCombinationViewer();
            this.contextMenuSingleFile.SuspendLayout();
            this.contextMenuManyFiles.SuspendLayout();
            this.menuStrip_Main.SuspendLayout();
            this.contextMenuSingleTag.SuspendLayout();
            this.contextMenuManyTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcherAllFiles)).BeginInit();
            this.contextMenuNotifyIcon.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.virtualListViewFiles)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListLargeIcons
            // 
            this.imageListLargeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLargeIcons.ImageStream")));
            this.imageListLargeIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListLargeIcons.Images.SetKeyName(0, "folder.bmp");
            this.imageListLargeIcons.Images.SetKeyName(1, "folder2.bmp");
            this.imageListLargeIcons.Images.SetKeyName(2, "file.jpg");
            // 
            // imageListSmallIcons
            // 
            this.imageListSmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmallIcons.ImageStream")));
            this.imageListSmallIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListSmallIcons.Images.SetKeyName(0, "folder.bmp");
            this.imageListSmallIcons.Images.SetKeyName(1, "folder2.bmp");
            this.imageListSmallIcons.Images.SetKeyName(2, "emptyIcon.bmp");
            // 
            // contextMenuSingleFile
            // 
            this.contextMenuSingleFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSingleFile_ManageTags,
            this.menuSingleFile_RemoveFile,
            this.menuSingleFile_OpenTargetDirectory});
            this.contextMenuSingleFile.Name = "contextMenuManageTags";
            this.contextMenuSingleFile.Size = new System.Drawing.Size(162, 70);
            // 
            // menuSingleFile_ManageTags
            // 
            this.menuSingleFile_ManageTags.Name = "menuSingleFile_ManageTags";
            this.menuSingleFile_ManageTags.Size = new System.Drawing.Size(161, 22);
            this.menuSingleFile_ManageTags.Text = "נהל תגים";
            this.menuSingleFile_ManageTags.Click += new System.EventHandler(this.menuSingleFile_ManageTags_Click);
            // 
            // menuSingleFile_RemoveFile
            // 
            this.menuSingleFile_RemoveFile.Name = "menuSingleFile_RemoveFile";
            this.menuSingleFile_RemoveFile.Size = new System.Drawing.Size(161, 22);
            this.menuSingleFile_RemoveFile.Text = "הסר קובץ";
            this.menuSingleFile_RemoveFile.Click += new System.EventHandler(this.menuSingleFile_RemoveFile_Click);
            // 
            // menuSingleFile_OpenTargetDirectory
            // 
            this.menuSingleFile_OpenTargetDirectory.Name = "menuSingleFile_OpenTargetDirectory";
            this.menuSingleFile_OpenTargetDirectory.Size = new System.Drawing.Size(161, 22);
            this.menuSingleFile_OpenTargetDirectory.Text = "פתח תיקית יעד";
            this.menuSingleFile_OpenTargetDirectory.Click += new System.EventHandler(this.menuSingleFile_OpenTargetDirectory_Click);
            // 
            // txtAddFile
            // 
            this.txtAddFile.Location = new System.Drawing.Point(882, 447);
            this.txtAddFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAddFile.Name = "txtAddFile";
            this.txtAddFile.Size = new System.Drawing.Size(221, 20);
            this.txtAddFile.TabIndex = 3;
            // 
            // listTags
            // 
            this.listTags.FormattingEnabled = true;
            this.listTags.Location = new System.Drawing.Point(1001, 475);
            this.listTags.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listTags.Name = "listTags";
            this.listTags.Size = new System.Drawing.Size(102, 95);
            this.listTags.TabIndex = 11;
            // 
            // contextMenuManyFiles
            // 
            this.contextMenuManyFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuManyFiles_ManageTags,
            this.menuManyFiles_RemoveFiles});
            this.contextMenuManyFiles.Name = "contextMenuManyFiles";
            this.contextMenuManyFiles.Size = new System.Drawing.Size(138, 48);
            // 
            // menuManyFiles_ManageTags
            // 
            this.menuManyFiles_ManageTags.Name = "menuManyFiles_ManageTags";
            this.menuManyFiles_ManageTags.Size = new System.Drawing.Size(137, 22);
            this.menuManyFiles_ManageTags.Text = "נהל תגים";
            this.menuManyFiles_ManageTags.Click += new System.EventHandler(this.menuManyFiles_ManageTags_Click);
            // 
            // menuManyFiles_RemoveFiles
            // 
            this.menuManyFiles_RemoveFiles.Name = "menuManyFiles_RemoveFiles";
            this.menuManyFiles_RemoveFiles.Size = new System.Drawing.Size(137, 22);
            this.menuManyFiles_RemoveFiles.Text = "הסר קבצים";
            this.menuManyFiles_RemoveFiles.Click += new System.EventHandler(this.menuManyFiles_RemoveFiles_Click);
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMain_File_Restore,
            this.menuMain_File_Save});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(46, 20);
            this.menuFile.Text = "קובץ";
            // 
            // menuMain_File_Restore
            // 
            this.menuMain_File_Restore.Name = "menuMain_File_Restore";
            this.menuMain_File_Restore.Size = new System.Drawing.Size(102, 22);
            this.menuMain_File_Restore.Text = "אחזר";
            this.menuMain_File_Restore.Click += new System.EventHandler(this.menuMain_File_Restore_Click);
            // 
            // menuMain_File_Save
            // 
            this.menuMain_File_Save.Name = "menuMain_File_Save";
            this.menuMain_File_Save.Size = new System.Drawing.Size(102, 22);
            this.menuMain_File_Save.Text = "שמור";
            this.menuMain_File_Save.Click += new System.EventHandler(this.menuMain_File_Save_Click);
            // 
            // menuStrip_Main
            // 
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile});
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Size = new System.Drawing.Size(832, 24);
            this.menuStrip_Main.TabIndex = 23;
            this.menuStrip_Main.Text = "menuStrip1";
            // 
            // contextMenuSingleTag
            // 
            this.contextMenuSingleTag.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSingleTag_RenameTag,
            this.menuSingleTag_AttachTagToGroup,
            this.menuSingleTag_RemoveTag,
            this.menuSingleTag_ReverseChoice});
            this.contextMenuSingleTag.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuSingleTag.Name = "contextMenuSingleTag";
            this.contextMenuSingleTag.Size = new System.Drawing.Size(164, 92);
            // 
            // menuSingleTag_RenameTag
            // 
            this.menuSingleTag_RenameTag.Name = "menuSingleTag_RenameTag";
            this.menuSingleTag_RenameTag.Size = new System.Drawing.Size(163, 22);
            this.menuSingleTag_RenameTag.Text = "שנה שם";
            // 
            // menuSingleTag_AttachTagToGroup
            // 
            this.menuSingleTag_AttachTagToGroup.Name = "menuSingleTag_AttachTagToGroup";
            this.menuSingleTag_AttachTagToGroup.Size = new System.Drawing.Size(163, 22);
            this.menuSingleTag_AttachTagToGroup.Text = "שייך תג לקבוצה";
            // 
            // menuSingleTag_RemoveTag
            // 
            this.menuSingleTag_RemoveTag.Name = "menuSingleTag_RemoveTag";
            this.menuSingleTag_RemoveTag.Size = new System.Drawing.Size(163, 22);
            this.menuSingleTag_RemoveTag.Text = "הסר תג";
            // 
            // menuSingleTag_ReverseChoice
            // 
            this.menuSingleTag_ReverseChoice.Name = "menuSingleTag_ReverseChoice";
            this.menuSingleTag_ReverseChoice.Size = new System.Drawing.Size(163, 22);
            this.menuSingleTag_ReverseChoice.Text = "בחירה הפוכה";
            this.menuSingleTag_ReverseChoice.Click += new System.EventHandler(this.menuSingleTag_ReverseChoice_Click);
            // 
            // contextMenuManyTags
            // 
            this.contextMenuManyTags.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuManyTags_AttachTagsToGroup,
            this.menuManyTags_RemoveTags,
            this.menuManyTags_RevesreChoice});
            this.contextMenuManyTags.Name = "contextMenuSingleTag";
            this.contextMenuManyTags.Size = new System.Drawing.Size(175, 70);
            // 
            // menuManyTags_AttachTagsToGroup
            // 
            this.menuManyTags_AttachTagsToGroup.Name = "menuManyTags_AttachTagsToGroup";
            this.menuManyTags_AttachTagsToGroup.Size = new System.Drawing.Size(174, 22);
            this.menuManyTags_AttachTagsToGroup.Text = "שייך תגים לקבוצה";
            // 
            // menuManyTags_RemoveTags
            // 
            this.menuManyTags_RemoveTags.Name = "menuManyTags_RemoveTags";
            this.menuManyTags_RemoveTags.Size = new System.Drawing.Size(174, 22);
            this.menuManyTags_RemoveTags.Text = "הסר תגים";
            // 
            // menuManyTags_RevesreChoice
            // 
            this.menuManyTags_RevesreChoice.Name = "menuManyTags_RevesreChoice";
            this.menuManyTags_RevesreChoice.Size = new System.Drawing.Size(174, 22);
            this.menuManyTags_RevesreChoice.Text = "בחירה הפוכה";
            this.menuManyTags_RevesreChoice.Click += new System.EventHandler(this.menuManyTags_RevesreChoice_Click);
            // 
            // lblTotalFilesTitle
            // 
            this.lblTotalFilesTitle.AutoSize = true;
            this.lblTotalFilesTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTotalFilesTitle.Location = new System.Drawing.Point(388, 101);
            this.lblTotalFilesTitle.Name = "lblTotalFilesTitle";
            this.lblTotalFilesTitle.Size = new System.Drawing.Size(67, 13);
            this.lblTotalFilesTitle.TabIndex = 25;
            this.lblTotalFilesTitle.Text = "Total files:";
            // 
            // fileSystemWatcherAllFiles
            // 
            this.fileSystemWatcherAllFiles.EnableRaisingEvents = true;
            this.fileSystemWatcherAllFiles.IncludeSubdirectories = true;
            this.fileSystemWatcherAllFiles.NotifyFilter = ((System.IO.NotifyFilters)((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName)));
            this.fileSystemWatcherAllFiles.Path = "d:\\";
            this.fileSystemWatcherAllFiles.SynchronizingObject = this;
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // listViewTags
            // 
            this.listViewTags.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listViewTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTags.FullRowSelect = true;
            this.listViewTags.GridLines = true;
            listViewItem1.ToolTipText = "zzzzz";
            this.listViewTags.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewTags.LabelWrap = false;
            this.listViewTags.LargeImageList = this.imageListLargeIcons;
            this.listViewTags.Location = new System.Drawing.Point(0, 0);
            this.listViewTags.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listViewTags.Name = "listViewTags";
            this.listViewTags.Size = new System.Drawing.Size(355, 469);
            this.listViewTags.SmallImageList = this.imageListSmallIcons;
            this.listViewTags.TabIndex = 10;
            this.listViewTags.UseCompatibleStateImageBehavior = false;
            this.listViewTags.View = System.Windows.Forms.View.Details;
            this.listViewTags.ItemActivate += new System.EventHandler(this.listViewTags_ItemActivate);
            this.listViewTags.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.listViewTags_ItemMouseHover);
            this.listViewTags.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewTags_MouseUp);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tags";
            this.columnHeader2.Width = 200;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuNotifyIcon;
            this.notifyIcon.Text = "Tag Folders";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // contextMenuNotifyIcon
            // 
            this.contextMenuNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyMenuOpen,
            this.notifyMenuExit});
            this.contextMenuNotifyIcon.Name = "contextMenuNotifyIcon";
            this.contextMenuNotifyIcon.Size = new System.Drawing.Size(172, 48);
            // 
            // notifyMenuOpen
            // 
            this.notifyMenuOpen.Name = "notifyMenuOpen";
            this.notifyMenuOpen.Size = new System.Drawing.Size(171, 22);
            this.notifyMenuOpen.Text = "פתח תצוגת תגים";
            this.notifyMenuOpen.Click += new System.EventHandler(this.notifyMenuOpen_Click);
            // 
            // notifyMenuExit
            // 
            this.notifyMenuExit.Name = "notifyMenuExit";
            this.notifyMenuExit.Size = new System.Drawing.Size(171, 22);
            this.notifyMenuExit.Text = "יציאה";
            this.notifyMenuExit.Click += new System.EventHandler(this.notifyMenuExit_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 116);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tagsListView1);
            this.splitContainer1.Panel1.Controls.Add(this.listViewTags);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.virtualListViewFiles);
            this.splitContainer1.Size = new System.Drawing.Size(809, 469);
            this.splitContainer1.SplitterDistance = 355;
            this.splitContainer1.TabIndex = 32;
            // 
            // tagsListView1
            // 
            this.tagsListView1.LabelWrap = false;
            this.tagsListView1.Location = new System.Drawing.Point(27, 43);
            this.tagsListView1.Name = "tagsListView1";
            this.tagsListView1.ShowGroups = false;
            this.tagsListView1.Size = new System.Drawing.Size(211, 149);
            this.tagsListView1.TabIndex = 40;
            this.tagsListView1.UseCompatibleStateImageBehavior = false;
            this.tagsListView1.View = System.Windows.Forms.View.SmallIcon;
            this.tagsListView1.ItemActivate += new System.EventHandler(this.tagsListView1_ItemActivate);
            // 
            // virtualListViewFiles
            // 
            this.virtualListViewFiles.AllColumns.Add(this.olvColumnFileName);
            this.virtualListViewFiles.AllColumns.Add(this.olvColumnTags);
            this.virtualListViewFiles.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.virtualListViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFileName,
            this.olvColumnTags});
            this.virtualListViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualListViewFiles.EmptyListMsg = "No Files";
            this.virtualListViewFiles.LabelWrap = false;
            this.virtualListViewFiles.LargeImageList = this.imageListLargeIcons;
            this.virtualListViewFiles.Location = new System.Drawing.Point(0, 0);
            this.virtualListViewFiles.Name = "virtualListViewFiles";
            this.virtualListViewFiles.ShowGroups = false;
            this.virtualListViewFiles.ShowItemToolTips = true;
            this.virtualListViewFiles.Size = new System.Drawing.Size(450, 469);
            this.virtualListViewFiles.SmallImageList = this.imageListSmallIcons;
            this.virtualListViewFiles.TabIndex = 41;
            this.virtualListViewFiles.UseCompatibleStateImageBehavior = false;
            this.virtualListViewFiles.View = System.Windows.Forms.View.List;
            this.virtualListViewFiles.VirtualMode = true;
            this.virtualListViewFiles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.virtualListViewFiles_ItemSelectionChanged);
            this.virtualListViewFiles.ItemActivate += new System.EventHandler(this.virtualListViewFiles_ItemActivate);
            this.virtualListViewFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.virtualListViewFiles_MouseUp);
            // 
            // olvColumnFileName
            // 
            this.olvColumnFileName.Text = "FileName";
            this.olvColumnFileName.Width = 160;
            // 
            // olvColumnTags
            // 
            this.olvColumnTags.FillsFreeSpace = true;
            this.olvColumnTags.Text = "Tags";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 587);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(832, 22);
            this.statusStrip.TabIndex = 34;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 10;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(29, 17);
            this.toolStripStatusLabel1.Text = "Info";
            // 
            // comboTagsView
            // 
            this.comboTagsView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTagsView.FormattingEnabled = true;
            this.comboTagsView.Items.AddRange(new object[] {
            "LargeIcons",
            "Details",
            "SmallIcons",
            "List",
            "Tile"});
            this.comboTagsView.Location = new System.Drawing.Point(225, 93);
            this.comboTagsView.Name = "comboTagsView";
            this.comboTagsView.Size = new System.Drawing.Size(87, 21);
            this.comboTagsView.TabIndex = 37;
            this.comboTagsView.SelectedIndexChanged += new System.EventHandler(this.comboTagsView_SelectedIndexChanged);
            // 
            // comboFilesView
            // 
            this.comboFilesView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFilesView.FormattingEnabled = true;
            this.comboFilesView.Items.AddRange(new object[] {
            "LargeIcons",
            "Details",
            "SmallIcons",
            "List",
            "Tile"});
            this.comboFilesView.Location = new System.Drawing.Point(554, 93);
            this.comboFilesView.Name = "comboFilesView";
            this.comboFilesView.Size = new System.Drawing.Size(87, 21);
            this.comboFilesView.TabIndex = 38;
            this.comboFilesView.SelectedIndexChanged += new System.EventHandler(this.comboFilesView_SelectedIndexChanged);
            // 
            // listViewFiles
            // 
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewFiles.LabelEdit = true;
            this.listViewFiles.LargeImageList = this.imageListLargeIcons;
            this.listViewFiles.Location = new System.Drawing.Point(869, 126);
            this.listViewFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.ShowItemToolTips = true;
            this.listViewFiles.Size = new System.Drawing.Size(267, 206);
            this.listViewFiles.SmallImageList = this.imageListSmallIcons;
            this.listViewFiles.TabIndex = 39;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.List;
            this.listViewFiles.VirtualMode = true;
            this.listViewFiles.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewFiles_RetrieveVirtualItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Name";
            // 
            // txtSearchTag
            // 
            this.txtSearchTag.EmptyForeColor = System.Drawing.Color.Silver;
            this.txtSearchTag.EmptyText = "חיפוש תג";
            this.txtSearchTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtSearchTag.ForeColor = System.Drawing.Color.Silver;
            this.txtSearchTag.Location = new System.Drawing.Point(51, 93);
            this.txtSearchTag.Name = "txtSearchTag";
            this.txtSearchTag.NonEmptyForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSearchTag.Size = new System.Drawing.Size(168, 21);
            this.txtSearchTag.TabIndex = 30;
            this.txtSearchTag.Text = "חיפוש תג";
            this.txtSearchTag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSearchTag.TextChanged += new System.EventHandler(this.txtSearchTag_TextChanged);
            this.txtSearchTag.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchTag_KeyUp);
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImage = global::YaronThurm.TagFolders.Properties.Resources.חזור1;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBack.Location = new System.Drawing.Point(12, 81);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(33, 33);
            this.btnBack.TabIndex = 10;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // tagsCombinationViewer
            // 
            this.tagsCombinationViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagsCombinationViewer.AutoScroll = true;
            this.tagsCombinationViewer.BackColor = System.Drawing.Color.Transparent;
            this.tagsCombinationViewer.Location = new System.Drawing.Point(12, 27);
            this.tagsCombinationViewer.Name = "tagsCombinationViewer";
            this.tagsCombinationViewer.Size = new System.Drawing.Size(804, 50);
            this.tagsCombinationViewer.TabIndex = 33;
            // 
            // BrowseTagsForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 609);
            this.Controls.Add(this.comboTagsView);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip_Main);
            this.Controls.Add(this.txtSearchTag);
            this.Controls.Add(this.comboFilesView);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.listTags);
            this.Controls.Add(this.txtAddFile);
            this.Controls.Add(this.lblTotalFilesTitle);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tagsCombinationViewer);
            this.MainMenuStrip = this.menuStrip_Main;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "BrowseTagsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tags Folders";
            this.Load += new System.EventHandler(this.BrowseTagsForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrowseTagsForm_FormClosing);
            this.contextMenuSingleFile.ResumeLayout(false);
            this.contextMenuManyFiles.ResumeLayout(false);
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.contextMenuSingleTag.ResumeLayout(false);
            this.contextMenuManyTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcherAllFiles)).EndInit();
            this.contextMenuNotifyIcon.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.virtualListViewFiles)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAddFile;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ListBox listTags;
        private System.Windows.Forms.ImageList imageListSmallIcons;
        private System.Windows.Forms.ImageList imageListLargeIcons;
        private System.Windows.Forms.ContextMenuStrip contextMenuSingleFile;
        private System.Windows.Forms.ToolStripMenuItem menuSingleFile_ManageTags;
        private System.Windows.Forms.ToolStripMenuItem menuSingleFile_RemoveFile;
        private System.Windows.Forms.ContextMenuStrip contextMenuManyFiles;
        private System.Windows.Forms.ToolStripMenuItem menuManyFiles_RemoveFiles;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.MenuStrip menuStrip_Main;
        private System.Windows.Forms.ContextMenuStrip contextMenuSingleTag;
        private System.Windows.Forms.ToolStripMenuItem menuSingleTag_RenameTag;
        private System.Windows.Forms.ToolStripMenuItem menuSingleTag_AttachTagToGroup;
        private System.Windows.Forms.ToolStripMenuItem menuSingleTag_RemoveTag;
        private System.Windows.Forms.ContextMenuStrip contextMenuManyTags;
        private System.Windows.Forms.ToolStripMenuItem menuManyTags_AttachTagsToGroup;
        private System.Windows.Forms.ToolStripMenuItem menuManyTags_RemoveTags;
        private System.Windows.Forms.ToolStripMenuItem menuMain_File_Restore;
        private System.Windows.Forms.ToolStripMenuItem menuMain_File_Save;
        private System.Windows.Forms.ToolStripMenuItem menuManyFiles_ManageTags;
        private System.Windows.Forms.Label lblTotalFilesTitle;
        private YaronThurm.Controls.TextBoxes.TextBox2 txtSearchTag;
        private System.Windows.Forms.ToolStripMenuItem menuSingleFile_OpenTargetDirectory;
        private System.IO.FileSystemWatcher fileSystemWatcherAllFiles;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem notifyMenuOpen;
        private System.Windows.Forms.ToolStripMenuItem notifyMenuExit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.ListView listViewTags;
        private System.Windows.Forms.ToolStripMenuItem menuSingleTag_ReverseChoice;
        private TagsCombinationViewer tagsCombinationViewer;
        private System.Windows.Forms.ToolStripMenuItem menuManyTags_RevesreChoice;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ComboBox comboTagsView;
        private System.Windows.Forms.ComboBox comboFilesView;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private BrightIdeasSoftware.VirtualObjectListView virtualListViewFiles;
        private BrightIdeasSoftware.OLVColumn olvColumnFileName;
        private BrightIdeasSoftware.OLVColumn olvColumnTags;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private TagsListView tagsListView1;        
    }
}

