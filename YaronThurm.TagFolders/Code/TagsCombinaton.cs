using System;
using System.Collections.Generic;

namespace YaronThurm.TagFolders
{
    public class TagsCombinaton: List<List<FileTag>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int[] GetLocationOf(FileTag item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = 0; j < this[i].Count; j++)
                {
                    if (this[i][j] == item)
                    {
                        return new int[] {i, j};
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="compareInverse"></param>
        /// <returns></returns>
        private int[] GetLocationOfItemByValue(FileTag item, bool compareInverse)
        {
            if (compareInverse == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    for (int j = 0; j < this[i].Count; j++)
                    {
                        if (this[i][j].Value == item.Value)
                        {
                            return new int[] { i, j };
                        }
                    }
                }
            }
            else // compareInverse == true
            {
                for (int i = 0; i < this.Count; i++)
                {
                    for (int j = 0; j < this[i].Count; j++)
                    {
                        if (this[i][j].Value == item.Value && this[i][j].Inverse == item.Inverse)
                        {
                            return new int[] { i, j };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Searches a tag by reference
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(FileTag item)
        {
            if (this.GetLocationOf(item) == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Searches a tag by value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ContainsByValue(FileTag item, bool compareInverse)
        {
            if (this.GetLocationOfItemByValue(item, compareInverse) == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void RemoveAt(int i, int j)
        {
            this[i].RemoveAt(j);
            if (this[i].Count == 0)
                this.RemoveAt(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Remove(FileTag item)
        {
            int[] location = this.GetLocationOf(item);

            if (location != null)
                this.RemoveAt(location[0], location[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(FileTag item)
        {
            List<FileTag> newItem = new List<FileTag>();
            newItem.Add(item);
            this.Add(newItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret = "";
            string OrString = " | ";
            string AndString = " && ";
            string NotString = " ! ";

            for (int i = 0; i < this.Count; i++)
            {
                string s = "";
                for (int j = 0; j < this[i].Count; j++)
                {
                    if (this[i][j].Inverse)
                        s += NotString + this[i][j].Value + OrString;
                    else
                        s += this[i][j].Value + OrString;
                }
                if (s.Length - OrString.Length >= 0)
                    s = s.Substring(0, s.Length - OrString.Length);

                if (this[i].Count > 1)
                    ret += "(" + s + ")" + AndString;
                else
                    ret += s + AndString;

            }
            if (ret.Length - AndString.Length >= 0)
                ret = ret.Substring(0, ret.Length - AndString.Length);

            return ret;
        }
    }
}
