using System;
using System.Collections.Generic;

namespace YaronThurm
{
    public class RaisingEventsList<T> : List<T>
    {
        // Event arguments
        public class RaisingEventsListEventArgs : EventArgs
        {
            private T item;
            public T Item
            {
                get { return this.item; }
                set { this.item = value; }
            }

            private int index;
            public int Index
            {
                get { return this.index; }
                set { this.index = value; }
            }

            private bool cancel;
            public bool Cancel
            {
                get { return this.cancel; }
                set { this.cancel = value; }
            }

            public RaisingEventsListEventArgs()
            {
                this.item = default(T);
                this.index = -1;
                this.cancel = false;
            }
        }

        // Delegates
        public delegate void RaisingEventsListEventsHandler(RaisingEventsList<T> sender, RaisingEventsListEventArgs e);        

        // Events
        public event RaisingEventsListEventsHandler ItemAdding;
        public event RaisingEventsListEventsHandler ItemAdded;
        public event RaisingEventsListEventsHandler ItemRemoving;
        public event RaisingEventsListEventsHandler ItemRemoved;
        public event RaisingEventsListEventsHandler ItemChanging;
        
      
        // Public methods (new ones)
        public new void Add(T item)
        {
            this.Insert(this.Count, item);            
        }
        public new void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                this.Add(item);
            }
        }
        public new void Clear()
        {
            this.RemoveRange(0, this.Count);            
        }
        public new void Insert(int index, T  item)
        {
            // Raise ItemAdding event
            if (this.ItemAdding != null)
            {
                // Set event args
                RaisingEventsListEventArgs e = new RaisingEventsListEventArgs();
                e.Item = item;
                e.Index = index;
                e.Cancel = false;

                // Raise event
                this.ItemAdding(this, e);

                // Cancel operation if necessary
                if (e.Cancel)
                    return;
            }

            // Insert the item to the list
            base.Insert(index, item);

            // Raise ItemAdded event
            if (this.ItemAdded != null)
            {
                // Set event args
                RaisingEventsListEventArgs e = new RaisingEventsListEventArgs();
                e.Item = item;
                e.Index = index;
                e.Cancel = false;

                this.ItemAdded(this, e);
            }
        }
        public new void InsertRange(int index, IEnumerable<T> collection) 
        {
            // There is a need to add the items of the collection from last to first inorder
            // to cope with the possibility that some items did not get permision to be added.
            // The best method (for my opinion) is to add the items of the collection from last to first
            // while keeping the location of the insertion constant.
            // In that way, even if an item wasn't added the next item will still be inserted in the 
            // coerrect location.
            
            // Because i canot iterate an IEnumerable from last to first, I will
            // create a copy of the collection an work on it            
            List<T> collection2 = new List<T>();
            foreach (T item in collection)
                collection2.Add(item);

            // Now insert the items from last to first
            for (int i =  collection2.Count - 1; i >= 0; i--)
                this.Insert(index, collection2[i]);
        }
        public new void RemoveAll(Predicate<T> match)
        {
            // Go throw the list from end to start in order not to change the index 
            // every time a remove accures
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (match(this[i]))
                    this.RemoveAt(i);
            }
        }
        public new void RemoveRange(int index, int count)
        {
            if ((index + count - 1) > this.Count)
                count = this.Count - index;

            for (int i = index + count - 1; i >= index; i--)
            {
                this.RemoveAt(i);
            }
        }
        public new void Remove(T item)
        {
            int index = this.IndexOf(item);
            this.RemoveAt(index);
        }
        public new void RemoveAt(int index)
        {
            T item = this[index];

            // Raise ItemRemoving event
            if (this.ItemRemoving != null)
            {
                // Set event args
                RaisingEventsListEventArgs e = new RaisingEventsListEventArgs();
                e.Item = item;
                e.Index = index;
                e.Cancel = false;

                // Raise event
                this.ItemRemoving(this, e);

                // Cancel operation if necessary
                if (e.Cancel)
                    return;
            }

            // Remove item from the list
            base.Remove(item);

            // Raise ItemRemoved event
            if (this.ItemRemoved != null)
            {
                // Set event args
                RaisingEventsListEventArgs e = new RaisingEventsListEventArgs();
                e.Item = item;
                e.Index = index;
                e.Cancel = false;

                this.ItemRemoved(this, e);
            }
        }
        public new T this[int index]
        {
            get { return base[index]; }
            set
            {
                if (this.ItemChanging != null)
                {
                    RaisingEventsListEventArgs e = new RaisingEventsListEventArgs();
                    e.Index = index;
                    e.Item = value;
                    e.Cancel = false;

                    this.ItemChanging(this, e);

                    if (e.Cancel)
                        return;
                }

                base[index] = value;            
            }
        }

        // Overrided public methods
        public override string ToString()
        {
            string ret = "";
            string seperator = ", ";
            for (int i = 0; i < this.Count; i++)
            {
                ret += this[i].ToString() + seperator;
            }
            ret = ret.Substring(0, ret.Length - seperator.Length);

            return ret;
        }
    }
}
