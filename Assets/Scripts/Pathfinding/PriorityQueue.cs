using System;
using System.Collections.Generic;

namespace Pathfinding
{
    public class PriorityQueue<T>
    {
        private List<T> data;
        private Comparison<T> compare;

        public PriorityQueue(Comparison<T> comparison)
        {
            this.data = new List<T>();
            this.compare = comparison;
        }

        public void Enqueue(T item)
        {
            data.Add(item);
            int childIndex = data.Count - 1;
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                if (compare(data[childIndex], data[parentIndex]) >= 0)
                    break;
                T tmp = data[childIndex];
                data[childIndex] = data[parentIndex];
                data[parentIndex] = tmp;
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            int lastIndex = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex);

            lastIndex--;
            int parentIndex = 0;
            while (true)
            {
                int childIndex = parentIndex * 2 + 1;
                if (childIndex > lastIndex)
                    break;
                int rightChild = childIndex + 1;
                if (rightChild <= lastIndex && compare(data[rightChild], data[childIndex]) < 0)
                    childIndex = rightChild;
                if (compare(data[parentIndex], data[childIndex]) <= 0)
                    break;
                T tmp = data[parentIndex];
                data[parentIndex] = data[childIndex];
                data[childIndex] = tmp;
                parentIndex = childIndex;
            }
            return frontItem;
        }

        public int Count => data.Count;
        public bool Contains(T item) => data.Contains(item);
    }
}