using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YaronThurm.TagFolders
{
    public class TagsListView: System.Windows.Forms.ListView
    {
        private ImageList imageListSmallIcons;
        private ImageList imageListLargeIcons;
        private System.ComponentModel.IContainer components;
    
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();

            SetWindowTheme(this.Handle, "explorer", null);

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagsListView));
            this.imageListLargeIcons = new ImageList();
            this.imageListLargeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLargeIcons.ImageStream")));
            this.imageListLargeIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListLargeIcons.Images.SetKeyName(0, "folder.bmp");
            this.imageListLargeIcons.Images.SetKeyName(1, "folder2.bmp");

            this.imageListSmallIcons = new ImageList();
            this.imageListSmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmallIcons.ImageStream")));
            this.imageListSmallIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListSmallIcons.Images.SetKeyName(0, "folder.bmp");
            this.imageListSmallIcons.Images.SetKeyName(1, "folder2.bmp");
            
            this.SmallImageList = this.imageListSmallIcons;
            this.LargeImageList = this.imageListLargeIcons;
        }              

        

        public void UpdateItems(TagFilesDatabase currentDb, TagsCombinaton tagsCombination, string searchText)
        {
            var tagItems = TagsListWraper.GetTagsList(currentDb, tagsCombination, searchText);            
            Dictionary<string, string> groups = new Dictionary<string,string>();
            if (tagItems != null)
            for (int i = 0; i < tagItems.Count; i++)
                groups[tagItems[i].GroupKey] = tagItems[i].GroupName;

            // Create the groups list. (Skip the default group - index 0)
            this.Groups.Clear();
            foreach (var kvp in groups)
                this.Groups.Add(kvp.Key, kvp.Value);            
            
            // Create item for the listView:
            List<ListViewItem> tagsList = new List<ListViewItem>();
            ListViewItem tagItem;
            // Add each tag from the database
            int j = 0;
            foreach (var t in tagItems)
            {
                // Show the tag only if it's not in the history of tags being selected - 
                // (You should not show tag 'X' after selecting tag 'X')
                if (!tagsCombination.ContainsByValue(t.RawFileTag, false) && 
                    t.Value.ToLower().Contains(searchText.ToLower()))
                {
                    // Set the tag item (including text of the tag with the tag's file-count
                    tagItem = new ListViewItem();
                    //tagItem.Text = j++.ToString().PadLeft(9, 'a') + "\n [10]";
                    tagItem.Text = t.Text;
                    tagItem.Tag = t.RawFileTag;

                    // Set image of the tag: special image if the tag's file-count is equals the
                    // database file-count. a regular folder image if not.
                    if (currentDb.Files.Count == currentDb.CountFilesByTag(t.RawFileTag))
                        tagItem.ImageIndex = 1;
                    else
                        tagItem.ImageIndex = 0;

                    // Map the tag to it's group
                    tagItem.Group = this.Groups[t.GroupKey];
                    tagsList.Add(tagItem);
                    //this.Items.Add(tagItem);
                }
            }

            // ** Sort the tags by groups, and inside of each group
            //ListViewItem tmp = new ListViewItem();
            //for (int i = 0; i < tagsList.Count; i++)
            //{
            //    for (int j = i + 1; j < tagsList.Count; j++)
            //    {
            //        int comperator = tagsList[i].Group.Name.CompareTo(tagsList[j].Group.Name);
            //        if (comperator == 1) // item[i].Group > item[j].Group
            //        { // Swap items
            //            tmp = (ListViewItem)tagsList[i].Clone();
            //            tagsList[i] = (ListViewItem)tagsList[j].Clone();
            //            tagsList[j] = tmp;
            //        }
            //        else if (comperator == 0) // Equal groups
            //        { // Sort internally

            //            comperator = ((FileTag)tagsList[i].Tag).ToString().CompareTo(
            //                ((FileTag)tagsList[j].Tag).ToString());

            //            if (comperator == 1)
            //            {
            //                tmp = (ListViewItem)tagsList[i].Clone();
            //                tagsList[i] = (ListViewItem)tagsList[j].Clone();
            //                tagsList[j] = tmp;
            //            }
            //        }
            //    }
            //}


            // ** Add to list view
            this.BeginUpdate();
            this.Clear();
            foreach (ListViewItem item in tagsList)
               this.Items.Add(item);
            
            this.EndUpdate();        
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagsListView));
            this.imageListSmallIcons = new System.Windows.Forms.ImageList(this.components);
            this.imageListLargeIcons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageListSmallIcons
            // 
            this.imageListSmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmallIcons.ImageStream")));
            this.imageListSmallIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListSmallIcons.Images.SetKeyName(0, "folder.bmp");
            this.imageListSmallIcons.Images.SetKeyName(1, "folder2.bmp");
            // 
            // imageListLargeIcons
            // 
            this.imageListLargeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLargeIcons.ImageStream")));
            this.imageListLargeIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListLargeIcons.Images.SetKeyName(0, "folder.bmp");
            this.imageListLargeIcons.Images.SetKeyName(1, "folder2.bmp");
            // 
            // TagsListView
            // 
            this.LabelWrap = false;
            this.ShowGroups = false;
            this.View = System.Windows.Forms.View.SmallIcon;
            this.ResumeLayout(false);

        }
    }
}
