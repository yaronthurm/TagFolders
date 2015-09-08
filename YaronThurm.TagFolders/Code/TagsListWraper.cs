using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YaronThurm.TagFolders
{
    public class TagsListWraper
    {
        #region Constants
        private const string No_Tags_String = "*No Tags*";
        private const string No_More_Tags_String = "*No More Tags*";
        
        #endregion

        public class TagItem
        {
            public string GroupKey;
            public string GroupName;
            public string Text;
            public FileTag RawFileTag;

            public string Value { get { return RawFileTag.Value; } }
        }
        
        public static List<TagItem> GetTagsList(TagFilesDatabase currentDb, TagsCombinaton tagsCombination, string searchText)
        {
            if (currentDb == null) return null;

            List<TagItem> ret = new List<TagItem>();
            // Add each tag from the database
            for (int i = 0; i < currentDb.Tags.Count; i++)
            {
                // Extract the tag
                FileTag tag = currentDb.Tags[i];

                // Show the tag only if it's not in the history of tags being selected - 
                // (You should not show tag 'X' after selecting tag 'X')                                    
                if (!tagsCombination.ContainsByValue(tag, false) && tag.Value.ToLower().Contains(searchText.ToLower()))
                {
                    TagItem item = new TagItem();
                    // Set the tag item (including text of the tag with the tag's file-count                    
                    item.Text = (tag.Value == "" ? No_Tags_String : tag.ToString()) +
                        "\n  [" + currentDb.CountFilesByTag(tag).ToString() + "]";
                    item.RawFileTag = tag;

                    // Map the tag to it's group
                    int groupKeyIndex = currentDb.tagGroupMapping[i];
                    item.GroupName = currentDb.groupsNames[groupKeyIndex];
                    item.GroupKey = currentDb.groupsKeys[groupKeyIndex];

                    ret.Add(item);
                }
            }
            return ret;
        }
    }
}
