using System;

namespace YaronThurm.TagFolders
{
    // Event arguments
    public class FileTagEventArgs : EventArgs
    {
        private string newValue;
        public string NewValue
        {
            get { return this.newValue; }
        }

        private string oldValue;
        public string OldValue
        {
            get { return this.oldValue; }          
        }

        private bool cancel;
        public bool Cancel
        {
            get { return this.cancel; }
            set { this.cancel = value; }
        }

        public FileTagEventArgs(string oldValue, string newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.cancel = false;
        }
    }

    // Delegates
    public delegate void ValueChangedHandler(FileTag sender, FileTagEventArgs e);
    public delegate void ValueChangingHandler(FileTag sender, FileTagEventArgs e);


    public class FileTag
    {
        /// <summary>
        /// An Empty tag just for convenient
        /// </summary>
        public static readonly FileTag Empty = new FileTag();

        // Members        
        private string value;
        private bool inverse = false;
        
        /// <summary>
        /// Event fired after the value has been changed
        /// </summary>
        public event ValueChangedHandler ValueChanged;
        
        /// <summary>
        /// Event fired before the value is about to change
        /// </summary>
        public event ValueChangingHandler ValueChanging;


        #region Constructors
        public FileTag()
        {
            this.value = "";
        }
        public FileTag(string value)
        {
            if (value == "")
                this.value = FileTag.Empty.Value;
            else
                this.value = value;
        }

        #endregion


        #region Override public methods
        public override string ToString()
        {
            string ret = "";

            if (this.Inverse)
                ret = "Not ";
                
            ret += this.value == ""? "*Empty*": this.value;
            
            return ret;
        }
        public override bool Equals(object obj)
        {
            if (obj is FileTag)
            {
                FileTag a = (FileTag)obj;

                return this.value.Equals(a.value);
            }
            else if (obj is System.DBNull)
                return false;

            throw new ArgumentException("object is not a Tag");
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion


        #region Public methods
        /// <summary>
        /// Compares the "tag" param with this current instance of the FileTag object.
        /// The criteria is that both FileTag value member is equal.
        /// </summary>
        /// <param name="tag">The FileTag instance to compare to</param>
        /// <returns>true if both FileTag value member is equal, false otherwise</returns>
        public bool Compare(FileTag tag)
        {
            // The return value
            bool ret;

            // If the input param was null then the return value should be false, i.e, not equal
            if (tag == null)
                ret = false;
            else // Compare the values of the two FileTag instances
                ret = (this.value == tag.value);

            return ret;
        }

        #endregion


        #region Properties
        public string Value
        {
            get { return this.value; }
            set 
            {
                // Do somthing only if the new value is not equal to the current value
                if (this.value != value) // Change should accure
                {
                    // Notify that a change is about to accure
                    if (this.ValueChanging != null)
                    {
                        // Prepare the event args
                        FileTagEventArgs e = new FileTagEventArgs(this.value, value);
                        e.Cancel = false;

                        // Raise event
                        this.ValueChanging(this, e);

                        // Cancel if require
                        if (e.Cancel)
                            return;
                    }

                    // Set new value
                    this.value = value;

                    // Notify that a change was made
                    if (this.ValueChanged != null)
                    {
                        // Set event args
                        FileTagEventArgs e = new FileTagEventArgs(this.value, value);

                        // Raise event
                        this.ValueChanged(this, e);
                    }
                }
            }
        }

        public bool Inverse
        {
            get { return this.inverse; }
            set { this.inverse = value; }
        }
        #endregion
    }
}