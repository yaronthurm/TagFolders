using System;

namespace YaronThurm.TagFolders
{
    public class FileWithTags
    {
        #region Events stuff
        // Event arguments class
        public class FileWithTagsEventArgs : EventArgs
        {
            private FileTag tag;
            public FileTag Tag
            {
                get { return this.tag; }
            }

            public FileWithTagsEventArgs(FileTag tag)
            {
                this.tag = tag;
            }
        }

        // Delegate
        public delegate void FileWithTagsEventHandler(FileWithTags sender, FileWithTagsEventArgs e);    
        
        // Events
        public event FileWithTagsEventHandler TagAdded;
        public event FileWithTagsEventHandler TagRemoved;

        #endregion


        #region Members
        private string fileName;
        private RaisingEventsList<FileTag> tags;

        #endregion


        #region Constructor
        public FileWithTags()
        {
            this.fileName = "";
            this.tags = new RaisingEventsList<FileTag>();

            // Subscribe to the tags list events
            this.tags.ItemAdding += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(this.tags_ItemAdding);
            this.tags.ItemAdded += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(this.tags_ItemAdded);
            this.tags.ItemRemoved += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(this.tags_ItemRemoved);
            this.tags.ItemRemoving += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(this.tags_ItemRemoving);
            this.tags.ItemChanging += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(this.tags_ItemChanging);
        }        
        #endregion


        #region List events handlers
		
        private void tags_ItemRemoving(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            // Empty. Logic inside ItemRemoved
        }
        private void tags_ItemRemoved(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            // Raise event
            if (this.TagRemoved != null)
            {
                FileWithTagsEventArgs e2 = new FileWithTagsEventArgs(e.Item);                
                this.TagRemoved(this, e2);
            }
            // Unregister the tag's ValueChanging event
            e.Item.ValueChanging -= this.tag_ValueChanging;
        }
        private void tags_ItemAdding(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            // Don't allow duplicate tags to be added to the tags list
            if (this.tags.Exists(e.Item.Compare))
                e.Cancel = true;
        }
        private void tags_ItemAdded(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            // Raise event
            if (this.TagAdded != null)
            {
                FileWithTagsEventArgs e2 = new FileWithTagsEventArgs(e.Item);
                this.TagAdded(this, e2);
            }

            // Subscribe to the ValueChanging event of the new tag
            e.Item.ValueChanging += new ValueChangingHandler(this.tag_ValueChanging);            
        }
        private void tags_ItemChanging(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
        }        
        
        private void tag_ValueChanging(FileTag sender, FileTagEventArgs e)
        {
            FileTag newTag = new FileTag(e.NewValue);
            
            // Don't allow the tag to be change into a value that already exists
            if (this.tags.Exists(newTag.Compare))
            {
                e.Cancel = true;
                throw new InvalidOperationException(string.Format(
                    "Can't change tag value from '{1}' to '{0}' because the tag '{0}' already exists in the file's tags list", 
                    e.NewValue, e.OldValue));
            }
            
            // Raise event (removeTag and addTag)
            if (this.TagRemoved != null)
            {
                FileWithTagsEventArgs e2 = new FileWithTagsEventArgs(sender);
                this.TagRemoved(this, e2);
            }
            if (this.TagAdded != null)
            {
                FileWithTagsEventArgs e2 = new FileWithTagsEventArgs(newTag);
                this.TagAdded(this, e2);
            }
        }
        
        #endregion


        #region Properties

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }
        public string FileNameWithoutPath
        {
            get
            {
                int i = this.fileName.LastIndexOf("\\");
                if (i >= 0)
                    return this.fileName.Substring(i + 1);
                else
                    return this.fileName;
            }
        
        }
        public string FilePath
        {
            get
            {
                int i = this.fileName.LastIndexOf("\\");
                if (i >= 0)
                    return this.fileName.Substring(0, i);
                else
                    return this.fileName;
            }
        }                   
        public RaisingEventsList<FileTag> Tags
        {
            get { return this.tags; }            
        }
        
        #endregion


        #region Public methods
        public override string ToString()
        {
            string ret = "File Name: " + this.FileName + ". Tags: " + this.tags.ToString();
            return ret;
        }
         
        #endregion
    }    
}
