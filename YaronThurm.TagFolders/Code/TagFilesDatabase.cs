using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace YaronThurm.TagFolders
{
    public class TagFilesDatabase
    {
        private enum DatabaseAction { Idle, AddingFile, RemovingFile, AddingTag, RemovingTag }


        #region - Event arguments -
        public class TagFilesDatabaseEventArgs : EventArgs
        {
            private FileTag tag;
            public FileTag Tag
            {
                get { return this.tag; }
            }

            private FileWithTags file;
            public FileWithTags File
            {
                get {return this.file;}
            }

            public TagFilesDatabaseEventArgs(FileWithTags file,  FileTag tag)
            {
                this.file = file;
                this.tag = tag;
            }
        }
        #endregion


        #region - Delegate declaration -
        public delegate void DatabaseChangedHandler(TagFilesDatabase sender, TagFilesDatabaseEventArgs e);
        #endregion 


        #region Events declaration
        public event DatabaseChangedHandler FileRemoved;
        public event DatabaseChangedHandler FileAdded;
        public event DatabaseChangedHandler TagAdded;
        public event DatabaseChangedHandler TagRemoved;
        #endregion


        #region String consts
        const string fileBeginString = "@File Begin@";
        const string fileEndString = "@File End@";
        const string groupsBeginString = "@Groups Begin@";
        const string groupsEndString = "@Groups End@";        
        const string mappingBeginString = "@Mapping Begin@";
        const string mappingEndString = "@Mapping End@";
        #endregion


        #region Members
        private RaisingEventsList<FileWithTags> files;
        private RaisingEventsList<FileTag> tags;
        private List<int> counters;
        private DatabaseAction currentAction;

        public List<string> groupsNames = new List<string>();
        public List<string> groupsKeys = new List<string>();
        public List<int> tagGroupMapping = new List<int>();

        // ********************************************************************************* //
        private SortedList<string, int> sortedListOfFiles = new SortedList<string, int>();
        private List<string> unsortedListOfFiles = new List<string>();
        private List<List<int>> tagsOfEachFile = new List<List<int>>();

        private SortedList<string, int> sortedListOfTags = new SortedList<string, int>();
        private List<string> unsortedListOfTags = new List<string>();
        private List<List<int>> filesOfEachTag = new List<List<int>>();
        // ********************************************************************************* //
        #endregion


        #region Constructor
        /// <summary>
        /// Constrauctor: Initialize new instance of "TagFilesDatabase"
        /// </summary>
        public TagFilesDatabase()
        {
            this.files = new RaisingEventsList<FileWithTags>();
            this.files.ItemAdded += new RaisingEventsList<FileWithTags>.RaisingEventsListEventsHandler(files_ItemAdded);
            this.files.ItemRemoved += new RaisingEventsList<FileWithTags>.RaisingEventsListEventsHandler(files_ItemRemoved);

            this.tags = new RaisingEventsList<FileTag>();
            this.tags.ItemAdding += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(tags_ItemAdding);
            this.tags.ItemAdded += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(tags_ItemAdded);
            this.tags.ItemRemoving += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(tags_ItemRemoving);
            this.tags.ItemRemoved += new RaisingEventsList<FileTag>.RaisingEventsListEventsHandler(tags_ItemRemoved);

            this.counters = new List<int>();            
            this.currentAction = DatabaseAction.Idle;

            this.groupsNames.Add("ללא קבוצה");
            this.groupsKeys.Add("*");
        }        
        #endregion


        #region Lists event handlers
        
        // Handels manipulation directly to the Tags property (Removing, Adding, Removed, Added)
        private void tags_ItemRemoving(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            if (this.currentAction == DatabaseAction.Idle)
                throw new InvalidOperationException("This object is read-only");
        }
        private void tags_ItemAdding(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            if (this.currentAction == DatabaseAction.Idle)
                throw new InvalidOperationException("This object is read-only");
        }
        private void tags_ItemAdded(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            // Subscribe to the ValueChanging event of the new tag
            e.Item.ValueChanging += new ValueChangingHandler(this.tag_ValueChanging);
        }
        private void tags_ItemRemoved(RaisingEventsList<FileTag> sender, RaisingEventsList<FileTag>.RaisingEventsListEventArgs e)
        {
            // Unregister the tag's ValueChanging event            
            e.Item.ValueChanging -= this.tag_ValueChanging;
        }        
        
        // Handles ValueChanging event for any tag in any file
        private void tag_ValueChanging(FileTag sender, FileTagEventArgs e)
        {
            if (this.currentAction == DatabaseAction.Idle)
                throw new InvalidOperationException("This object is read-only");            
        }
        
        // A file from the list of files notifies that one of its tags has been removed or that a new tag has been added
        private void file_TagRemoved(FileWithTags sender, FileWithTags.FileWithTagsEventArgs e)
        {
            // Set the action of the database
            this.currentAction = DatabaseAction.RemovingTag;
            // Remove the tag
            this.removeTag(e.Tag.Value);
            // If no more tags remains, add to the database the Empty tag
            if (sender.Tags.Count == 0)
            {
                this.addTag("");
            }
            // Set the ending of the action
            this.currentAction = DatabaseAction.Idle;

            // Raise the DatabaseChange event
            this.raiseEvent(DatabaseAction.RemovingTag, sender, e.Tag);
        }        
        private void file_TagAdded(FileWithTags sender, FileWithTags.FileWithTagsEventArgs e)
        {
            // Set the action of the database
            this.currentAction = DatabaseAction.AddingTag;
            // If it's the first tag of the file, remove the Empty tag from the database
            if (sender.Tags.Count == 1) // After the tag was added
                this.removeTag("");
            // Add the tag
            this.addTag(e.Tag.Value);
            // Set the ending of the action
            this.currentAction = DatabaseAction.Idle;

            // Raise the DatabaseChange event
            this.raiseEvent(DatabaseAction.AddingTag, sender, e.Tag);
        }

        // The list of files notifies that a file has been removed or that a new file has been added
        private void files_ItemRemoved(RaisingEventsList<FileWithTags> sender, RaisingEventsList<FileWithTags>.RaisingEventsListEventArgs e)
        {
            // Set the action of the database
            this.currentAction = DatabaseAction.RemovingFile;

            // Remove the file's tags
            FileWithTags file = e.Item;
            foreach (FileTag tag in file.Tags)
            {
                this.removeTag(tag.Value);
            }
            // If the file has no tags, remove the Empty tag
            if (file.Tags.Count == 0)
            {
                this.removeTag("");
            }
            // Set the ending of the current action
            this.currentAction = DatabaseAction.Idle;

            // Unregister to the file's events
            e.Item.TagAdded -= this.file_TagAdded;
            e.Item.TagRemoved -= this.file_TagRemoved;

            // Raise the DatabaseChange event
            this.raiseEvent(DatabaseAction.RemovingFile, file, null);
        }        
        private void files_ItemAdded(RaisingEventsList<FileWithTags> sender, RaisingEventsList<FileWithTags>.RaisingEventsListEventArgs e)
        {
            // Set the action of the database
            this.currentAction = DatabaseAction.AddingFile;

            // Add the file's tags
            FileWithTags file = e.Item;
            foreach (FileTag tag in file.Tags)
            {
                this.addTag(tag.Value);
            }
            // If the file has no tags, add the Empty tag
            if (file.Tags.Count == 0)
            {
                this.addTag("");
            }
            // Set the ending of the current action
            this.currentAction = DatabaseAction.Idle;

            // Register to the file's events
            e.Item.TagAdded += new FileWithTags.FileWithTagsEventHandler(this.file_TagAdded);
            e.Item.TagRemoved += new FileWithTags.FileWithTagsEventHandler(this.file_TagRemoved);

            // Raise the DatabaseChange event
            this.raiseEvent(DatabaseAction.AddingFile, file, null);
        }                
        #endregion

        
        #region Private methods
        private void addTag(string tagValue)
        {
            FileTag newTag = new FileTag(tagValue);
            int i = this.tags.FindIndex(newTag.Compare);
            if (i >= 0)
            {
                this.counters[i]++;
            }
            else
            {
                this.counters.Add(1);
                this.tagGroupMapping.Add(0);
                this.tags.Add(newTag);
            }
        }
        private void removeTag(string tagValue)
        {
            FileTag tag = new FileTag(tagValue);
            int i = this.tags.FindIndex(tag.Compare);
            if (i >= 0) // Found
            {
                this.counters[i]--;
                if (this.counters[i] <= 0)
                {
                    this.counters.RemoveAt(i);
                    this.tagGroupMapping.RemoveAt(i);
                    this.tags.RemoveAt(i);
                }
            }
        }
        private void raiseEvent(DatabaseAction action, FileWithTags file, FileTag tag)
        {
            TagFilesDatabaseEventArgs e = new TagFilesDatabaseEventArgs(file, tag);

            if (action == DatabaseAction.AddingTag)
            {
                if (this.TagAdded != null)
                    this.TagAdded(this, e);
            }
            else if (action == DatabaseAction.RemovingTag)
            {
                if (this.TagRemoved != null)
                    this.TagRemoved(this, e);
            }
            else if (action == DatabaseAction.RemovingFile)
            {
                if (this.FileRemoved != null)
                    this.FileRemoved(this, e);
            }
            else if (action == DatabaseAction.AddingFile)
            {
                if (this.FileAdded != null)
                    this.FileAdded(this, e);
            }            
        }
        #endregion


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public RaisingEventsList<FileTag> Tags
        {
            get { return this.tags; }
        }
      
        /// <summary>
        /// 
        /// </summary>
        public RaisingEventsList<FileWithTags> Files
        {
            get { return this.files; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The zero-base index of the element to get or set</param>
        /// <returns></returns>
        public FileWithTags this[int index]
        {
            get { return this.files[index]; }
        }

        public FileWithTags this[string fileName]
        {
            get
            {
                FileWithTags ret = null;

                fileName = fileName.ToLower();
                foreach (FileWithTags file in this.Files)
                {
                    if (file.FileName.ToLower() == fileName)
                    {
                        ret = file;
                        break;
                    }
                }

                return ret;
            }
        }

        #endregion


        #region Public methods
                              
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void Serialize(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);

            // *** Write the files ***
            foreach (FileWithTags file in this.files)
            {
                writer.WriteLine(fileBeginString);
                writer.WriteLine(file.FileName);
                foreach (FileTag tag in file.Tags)
                {
                    writer.WriteLine(tag.Value);
                }
                writer.WriteLine(fileEndString);
            }

            // *** Write the groups ***
            writer.WriteLine(groupsBeginString);
            for (int i = 1; i < this.groupsNames.Count; i++) // Start from 1 inorder to skip the default group
            {
                writer.WriteLine(this.groupsNames[i]);
                writer.WriteLine(this.groupsKeys[i]);
            }
            writer.WriteLine(groupsEndString);

            // *** Write the mapping ***
            writer.WriteLine(mappingBeginString);
            //foreach (int i in this.tagGroupMapping)
            for (int i = 0; i < this.tagGroupMapping.Count; i++)
            {
                writer.WriteLine(this.tags[i].Value);
                writer.WriteLine(this.tagGroupMapping[i]);
            }
            writer.WriteLine(mappingEndString);

            writer.Dispose();
            fileStream.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static TagFilesDatabase DeSerialize(string fileName)
        {
            TagFilesDatabase ret = new TagFilesDatabase();
            
            if (File.Exists(fileName))
            {
                string line = "";

                FileStream fileStream = new FileStream(fileName, FileMode.Open);
                StreamReader reader = new StreamReader(fileStream);

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    // Read the files
                    if (line == fileBeginString)
                    {
                        // Read the file name
                        FileWithTags file = new FileWithTags();
                        file.FileName = reader.ReadLine();

                        // Read the tags
                        line = reader.ReadLine();
                        while (line != fileEndString)
                        {
                            FileTag tag = new FileTag();
                            tag.Value = line;
                            file.Tags.Add(tag);

                            line = reader.ReadLine();
                        }
                        ret.Files.Add(file);
                    }
                    // Read the groups
                    else if (line == groupsBeginString)
                    {
                        line = reader.ReadLine();
                        while (line != groupsEndString)
                        {
                            string name = line;
                            string key = reader.ReadLine();
                            ret.CreateGroup(name, key);
                            line = reader.ReadLine();
                        }
                    }
                    // Read the mapping between tags and groups
                    else if (line == mappingBeginString)
                    {                        
                        line = reader.ReadLine();                        
                        int index = 0;
                        while (line != mappingEndString)
                        {
                            // read the tag line
                            FileTag tag = new FileTag(line);
                            // read the group index line
                            int groupIndex = int.Parse(reader.ReadLine());

                            // find the tag in the list
                            index = ret.tags.FindIndex(tag.Compare);
                            if (index >= 0)
                            {
                                ret.tagGroupMapping[index] = groupIndex;
                            }                                                      
                            line = reader.ReadLine();
                        }
                    }
                }

                reader.Dispose();
                fileStream.Close();
            }

            return ret;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchTag"></param>
        /// <returns></returns>
        public TagFilesDatabase GetPartialDatabase(FileTag matchTag)
        {
            TagFilesDatabase ret = new TagFilesDatabase();

            List<FileTag> list = new List<FileTag>();
            list.Add(matchTag);

            ret = this.GetPartialDatabase(list);
            return ret;
        }        

        /// <summary>
        /// Retrives a partial database that contains all the files that have a tag in the list of tags
        /// </summary>
        /// <param name="matchTag">List of tags to match with the current database of files</param>
        /// <returns>The partial database</returns>        
        public TagFilesDatabase GetPartialDatabase(List<FileTag> matchTags)
        {
            if (matchTags == null)
                throw new ArgumentNullException();
            if (matchTags.Count == 0)
                throw new ArgumentException();

            // The returned value
            TagFilesDatabase ret = new TagFilesDatabase();

            // Check each file if it needs to be added
            foreach (FileWithTags file in this.files)
            {
                // Search for each tag
                foreach (FileTag tag in matchTags)
                {
                    if (!tag.Inverse)
                    {
                        if ((tag.Value == FileTag.Empty.Value && file.Tags.Count == 0) ||
                            file.Tags.Exists(tag.Compare))
                        {
                            ret.Files.Add(file);
                            break;
                        }
                    }
                    else // Tag is inversed
                    {
                        if ((tag.Value == FileTag.Empty.Value && file.Tags.Count != 0) ||
                            !file.Tags.Exists(tag.Compare))
                        {
                            ret.Files.Add(file);
                            break;
                        }
                    }
                }
            }

            // Create all the groups
            for (int i = 0; i < this.groupsKeys.Count; i++)
            {
                ret.CreateGroup(this.groupsNames[i], this.groupsKeys[i]);
            }
            // Add all the relevant mappings
            for (int i = 0; i < this.tags.Count; i++)
            {
                int index = ret.tags.FindIndex(this.tags[i].Compare);
                if (index >= 0)
                    ret.tagGroupMapping[index] = this.tagGroupMapping[i];
            }
            // Subscribe to the parent's events
            ret.FileAdded = this.FileAdded;
            ret.FileRemoved = this.FileRemoved;
            ret.TagAdded = this.TagAdded;
            ret.TagRemoved = this.TagRemoved;

            return ret;
        }        
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public TagFilesDatabase GetPartialDatabase(string path)
        {            
            if (path == "")
                throw new ArgumentNullException();
            
            // The returned value
            TagFilesDatabase ret = new TagFilesDatabase();

            // Check each file if it needs to be added
            foreach (FileWithTags file in this.files)
            {
                if (file.FileName.ToLower().StartsWith(path.ToLower()))
                    ret.Files.Add(file);
            }

            // Create all the groups
            for (int i = 0; i < this.groupsKeys.Count; i++)
            {
                ret.CreateGroup(this.groupsNames[i], this.groupsKeys[i]);
            }
            // Add all the relevant mappings
            for (int i = 0; i < this.tags.Count; i++)
            {
                int index = ret.tags.FindIndex(this.tags[i].Compare);
                if (index >= 0)
                    ret.tagGroupMapping[index] = this.tagGroupMapping[i];
            }
            // Subscribe to the parent's events
            ret.FileAdded = this.FileAdded;
            ret.FileRemoved = this.FileRemoved;
            ret.TagAdded = this.TagAdded;
            ret.TagRemoved = this.TagRemoved;

            return ret;
        }

        public TagFilesDatabase GetPartialDatabase(TagsCombinaton tagsCombination)
        {
            TagFilesDatabase ret = this;

            for (int i = 0; i < tagsCombination.Count; i++)
            {
                // Set partial database
                ret = ret.GetPartialDatabase(tagsCombination[i]);
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public TagFilesDatabase GetPartialDatabase(List<string> paths)
        {
            if (paths == null)
                throw new ArgumentNullException();
            if (paths.Count == 0)
                throw new ArgumentNullException();

            // The returned value
            TagFilesDatabase ret = new TagFilesDatabase();

            // Check each file if it needs to be added
            foreach (FileWithTags file in this.files)
            {
                // Search for each directory
                foreach (string path in paths)
                {
                    if (file.FileName.ToLower().StartsWith(path.ToLower()))
                    {
                        ret.Files.Add(file);
                        break;
                    }
                }
            }

            // Create all the groups
            for (int i = 0; i < this.groupsKeys.Count; i++)
            {
                ret.CreateGroup(this.groupsNames[i], this.groupsKeys[i]);
            }
            // Add all the relevant mappings
            for (int i = 0; i < this.tags.Count; i++)
            {
                int index = ret.tags.FindIndex(this.tags[i].Compare);
                if (index >= 0)
                    ret.tagGroupMapping[index] = this.tagGroupMapping[i];
            }
            // Subscribe to the parent's events
            ret.FileAdded = this.FileAdded;
            ret.FileRemoved = this.FileRemoved;
            ret.TagAdded = this.TagAdded;
            ret.TagRemoved = this.TagRemoved;

            return ret;
        }

        public TagFilesDatabase GetReversePartialDatabase(FileTag matchTag)
        {
            TagFilesDatabase ret = new TagFilesDatabase();

            List<FileTag> list = new List<FileTag>();
            list.Add(matchTag);

            ret = this.GetReversePartialDatabase(list);
            return ret;
        }

        public TagFilesDatabase GetReversePartialDatabase(List<FileTag> matchTags)
        {
            if (matchTags == null)
                throw new ArgumentNullException();
            if (matchTags.Count == 0)
                throw new ArgumentException();

            // The returned value
            TagFilesDatabase ret = new TagFilesDatabase();

            // Check each file if it needs to be added
            foreach (FileWithTags file in this.files)
            {
                bool skippFile = false;

                // Search for each tag in the file
                foreach (FileTag tag in matchTags)
                {
                    // If the tag exists skipp the file
                    if (file.Tags.Exists(tag.Compare))
                    {
                        skippFile = true;
                        break;
                    }
                }

                if (!skippFile)
                    ret.Files.Add(file);
            }

            // Create all the groups
            for (int i = 0; i < this.groupsKeys.Count; i++)
            {
                ret.CreateGroup(this.groupsNames[i], this.groupsKeys[i]);
            }
            // Add all the relevant mappings
            for (int i = 0; i < this.tags.Count; i++)
            {
                int index = ret.tags.FindIndex(this.tags[i].Compare);
                if (index >= 0)
                    ret.tagGroupMapping[index] = this.tagGroupMapping[i];
            }
            // Subscribe to the parent's events
            ret.FileAdded = this.FileAdded;
            ret.FileRemoved = this.FileRemoved;
            ret.TagAdded = this.TagAdded;
            ret.TagRemoved = this.TagRemoved;

            return ret;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int CountFilesByTag(FileTag tag)
        {
            int i = this.Tags.FindIndex(tag.Compare);

            if (i >= 0)
                return this.counters[i];
            else
                return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="groupName"></param>
        public void MapTagToGroup(string tagValue, string groupName)
        {
            FileTag newTag = new FileTag(tagValue);
            int tagIndex = this.tags.FindIndex(newTag.Compare);
            if (tagIndex >= 0) // Tag found
            {
                int groupIndex = this.groupsNames.IndexOf(groupName);
                if (groupIndex >= 0)
                    this.tagGroupMapping[tagIndex] = groupIndex;
                //else
                    //throw new ArgumentException(string.Format("Group {0} does not exists", groupName));
            }
            //else
                //throw new ArgumentException(string.Format("Tag {0} does not exists", tagValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        public void CreateGroup(string name, string key)
        {
            if (!this.groupsNames.Contains(name))
            {
                this.groupsNames.Add(name);
                this.groupsKeys.Add(key);
            }
        }
        #endregion
    }
}