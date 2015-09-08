using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YaronThurm.TagFolders
{
    public partial class TagsCombinationViewer : UserControl
    {
        #region Events stuff
        public class TagsCombinationViewerEventArgs : System.EventArgs
        {
            public int IndexI;
            public int IndexJ;

            public TagsCombinationViewerEventArgs(int i, int j)
            {
                this.IndexI = i;
                this.IndexJ = j;
            }
        }
        
        // Removing tag event
        public delegate void RemoveHandler(TagsCombinationViewer sender, TagsCombinationViewerEventArgs e);
        public event RemoveHandler RemoveRequested;
        
        // Inversing tag event
        public delegate void InverseHandler(TagsCombinationViewer sender, TagsCombinationViewerEventArgs e);
        public event InverseHandler InverseRequested;
        #endregion        
        

        public TagsCombinationViewer()
        {
            InitializeComponent();
        }

        private Label CreateLabel(string text, float fontSize, int top)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font(lbl.Font.FontFamily, fontSize);
            lbl.AutoSize = true;
            lbl.Top = top;
            lbl.BackColor = Color.Transparent;            

            return lbl;
        }

        public void Update(TagsCombinaton newTagsCombination)
        {            
            int leftLocation = 0;
            int correction = 0;
            string OrString = "|";
            string AndString = "&&";

            Button btn;
            Label lbl;

            // First, clear old controls
            for (int c = this.Controls.Count - 1; c >= 0; c--)
            {
                if (this.Controls[c] is Button || this.Controls[c] is Label)
                    this.Controls.RemoveAt(c);
            }
            

            // Now, paint each control according with the new tags combination
            for (int i = 0; i < newTagsCombination.Count; i++)
            {
                // Paint '('
                if (newTagsCombination[i].Count > 1)
                {                    
                    lbl = this.CreateLabel("(", 16, 0);
                    lbl.Left = leftLocation;
                    this.Controls.Add(lbl);                    
                    leftLocation = lbl.Right - correction;
                }
            
                for (int j = 0; j < newTagsCombination[i].Count; j++)
                {
                    // Paint the button for the tag
                    btn = new Button();
                    btn.Click += new EventHandler(this.btn_Click);
                    btn.MouseUp += new MouseEventHandler(this.btn_MouseUp);
                    btn.Tag = new TagsCombinationViewerEventArgs(i, j);
                    btn.Left = leftLocation;
                    btn.Top = 3;
                    btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    btn.AutoSize = true;
                    btn.Text = newTagsCombination[i][j].Inverse? "Not " + newTagsCombination[i][j].Value: newTagsCombination[i][j].Value;
                    this.Controls.Add(btn);
                    btn.BringToFront();
                    leftLocation = btn.Right;

                    // Paint OR char: '|'
                    if (j < newTagsCombination[i].Count - 1)
                    {
                        lbl = this.CreateLabel(OrString, 8, 7);
                        lbl.Left = leftLocation;
                        this.Controls.Add(lbl);                        
                        leftLocation = lbl.Right;
                    }                    
                }

                // Paint ')'
                bool closeBracesExists = false;
                if (newTagsCombination[i].Count > 1)
                {
                    lbl = this.CreateLabel(")", 16, 0);
                    lbl.Left = leftLocation - correction;
                    this.Controls.Add(lbl);                    
                    leftLocation = lbl.Right - correction;
                    closeBracesExists = true;
                }

                // Paint AND char: '&'
                if (i < newTagsCombination.Count - 1)
                {
                    lbl = this.CreateLabel(AndString, 16, 0);
                    if (closeBracesExists)
                        lbl.Left = leftLocation;
                    else
                        lbl.Left = leftLocation - correction;
                                        
                    this.Controls.Add(lbl);                    
                    leftLocation = lbl.Right - correction;
                }
            }
        }


        #region Events handlers
        
        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            // If right-clicked, then show context menu
            if (e.Button == MouseButtons.Right)
            {
                Button btn = (Button)sender;                
                this.contextMenuStrip1.Tag = btn.Tag;
                this.contextMenuStrip1.Show(btn, e.Location);
            }
        }
        
        private void btn_Click(object sender, EventArgs e)
        {
            // Normal click means removing the tag associated with the button
            if (this.RemoveRequested != null)
            {
                Button btn = (Button)sender;
                TagsCombinationViewerEventArgs ev = (TagsCombinationViewerEventArgs)btn.Tag;
                this.RemoveRequested(this, ev);
            }
        }

        private void menuRemove_Click(object sender, EventArgs e)
        {
            if (this.RemoveRequested != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                ContextMenuStrip menu = (ContextMenuStrip)menuItem.GetCurrentParent();                
                TagsCombinationViewerEventArgs ev = (TagsCombinationViewerEventArgs)menu.Tag;
                this.RemoveRequested(this, ev);
            }
        }

        private void menuReverse_Click(object sender, EventArgs e)
        {
            if (this.InverseRequested != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                ContextMenuStrip menu = (ContextMenuStrip)menuItem.GetCurrentParent();
                TagsCombinationViewerEventArgs ev = (TagsCombinationViewerEventArgs)menu.Tag;
                this.InverseRequested(this, ev);
            }
        }

        #endregion
    }   
}
