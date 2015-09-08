using System;
using System.Collections.Generic;

namespace YaronThurm
{
    class SortedList2<TKey, TValue>
    {
        private List<TKey> keys = new List<TKey>();
        private List<TValue> values = new List<TValue>();

        private LinkedList<TKey> keys2 = new LinkedList<TKey>();
        private LinkedList<TValue> values2 = new LinkedList<TValue>();
        private LinkedList<LinkedListNode<TKey>> steps = new LinkedList<LinkedListNode<TKey>>();
        private int stepSize = 1000;


        public SortedList2()
        {            
        }

        public void Add(TKey key, TValue value)
        {
            LinkedListNode<TKey> node = new LinkedListNode<TKey>(key);
            this.keys2.AddLast(node);
            this.values2.AddLast(value);

            if ((this.keys2.Count - 1) % this.stepSize == 0)
                this.steps.AddLast(node);            
        }

        public LinkedListNode<TKey> GetNodeByIndex(int index)
        {
            int bigSteps = (index - 0) / stepSize;
            int smallSteps = (index - 0) % stepSize;
            
            LinkedListNode<LinkedListNode<TKey>> p = this.steps.First;
            for (int i = 0; i < bigSteps; i++)
                p = p.Next;

            LinkedListNode<TKey> node = p.Value;            
            for (int i = 0; i < smallSteps; i++)
                node = node.Next;


            return node;
        }

        public void TryAdd(TKey key, TValue value, out int index, out bool added)
        {
            index = -1;
            added = false;

            int comparison = ((IComparable)key).CompareTo(keys[0]);
            if (comparison == -1)
            {
                index = 0;
                this.keys.Insert(index, key);
                this.values.Insert(index, value);
                added = true;
            }
            else if (comparison == 0)
            {
                throw new InvalidOperationException("The key: " + key.ToString() + " is already in the list.");
            }
            else if (comparison == 1)
            {
                index = -1;
                added = false;
            }
        }
    }
}
