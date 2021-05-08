using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaProyecto
{

      public class HashNode<T>
    {
        public HashNode<T> Next { get; set; }
        public HashNode<T> current { get; set; }
        public string Key { get; set; }

        public T Value { get; set; }
    }
    public class HashTable<T>
    {
        private readonly HashNode<T>[] buckets;
        public HashNode<T> listNode;
        public HashTable(int size)
        {
            buckets = new HashNode<T>[size];
        }

        public void Add(string key, T item)
        {
            ValidateKey(key);

            var valueNode = new HashNode<T> { Key = key, Value = item, Next = null };
            int position = GetBucketByKey(key);
            listNode = buckets[position];

            if (null == listNode)
            {
                buckets[position] = valueNode;
            }
            else
            {
                while (null != listNode.Next)
                {
                    listNode = listNode.Next;
                }
                listNode.Next = valueNode;
            }
        }


        public T GetNode(string key)
        {
            if (key != "-1")
            {
                ValidateKey(key);

                var (_, node) = GetNodeByKey(key);



                return node.Value;
            }
            var valueNode = new HashNode<T> { Key = key, Next = null };



            return valueNode.Value;

        }

        public bool Remove(string key)
        {
            ValidateKey(key);
            int position = GetBucketByKey(key);

            var (previous, current) = GetNodeByKey(key);

            if (null == current) return false;

            if (null == previous && current.Next == null)
            {
                buckets[position] = null;
                return true;
            }
            if (previous != null)
            {
                previous.Next = current.Next;
                return true;
            }
            else
            {
                current = current.Next;
                buckets[position] = current;
                return true;
            }

        }
        public int GetBucketByKey(string key)
        {
            return key[0] % buckets.Length;
        }
        public (HashNode<T> previous, HashNode<T> current) GetNodeByKey(string key)
        {
            int position = GetBucketByKey(key);
            HashNode<T> listNode = buckets[position];
            HashNode<T> previous = null;

            while (null != listNode)
            {
                if (listNode.Key == key)
                {
                    return (previous, listNode);
                }
                previous = listNode;
                listNode = listNode.Next;
            }
            return (null, null);

        }

        protected void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
        }
    }
}
