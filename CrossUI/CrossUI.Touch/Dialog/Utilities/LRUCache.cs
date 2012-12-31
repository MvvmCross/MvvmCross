#region Copyright

// <copyright file="LRUCache.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;

namespace CrossUI.Touch.Dialog.Utilities
{
    public class LRUCache<TKey, TValue> where TValue : class, IDisposable
    {
        private readonly Dictionary<TKey, LinkedListNode<TValue>> dict;
        private readonly Dictionary<LinkedListNode<TValue>, TKey> revdict;
        private readonly LinkedList<TValue> list;
        private readonly int entryLimit;
        private readonly int sizeLimit;
        private int currentSize;
        private readonly Func<TValue, int> slotSizeFunc;

        public LRUCache(int entryLimit) : this(entryLimit, 0, null)
        {
        }

        public LRUCache(int entryLimit, int sizeLimit, Func<TValue, int> slotSizer)
        {
            list = new LinkedList<TValue>();
            dict = new Dictionary<TKey, LinkedListNode<TValue>>();
            revdict = new Dictionary<LinkedListNode<TValue>, TKey>();

            if (sizeLimit != 0 && slotSizer == null)
                throw new ArgumentNullException("If sizeLimit is set, the slotSizer must be provided");

            this.entryLimit = entryLimit;
            this.sizeLimit = sizeLimit;
            this.slotSizeFunc = slotSizer;
        }

        private void Evict()
        {
            var last = list.Last;
            var key = revdict[last];

            if (sizeLimit > 0)
            {
                int size = slotSizeFunc(last.Value);
                currentSize -= size;
            }

            dict.Remove(key);
            revdict.Remove(last);
            list.RemoveLast();
            last.Value.Dispose();

            Console.WriteLine("Evicted, got: {0} bytes and {1} slots", currentSize, list.Count);
        }

        public void Purge()
        {
            foreach (var element in list)
                element.Dispose();

            dict.Clear();
            revdict.Clear();
            list.Clear();
            currentSize = 0;
        }

        public TValue this[TKey key]
        {
            get
            {
                LinkedListNode<TValue> node;

                if (dict.TryGetValue(key, out node))
                {
                    list.Remove(node);
                    list.AddFirst(node);

                    return node.Value;
                }
                return null;
            }

            set
            {
                LinkedListNode<TValue> node;
                int size = sizeLimit > 0 ? slotSizeFunc(value) : 0;

                if (dict.TryGetValue(key, out node))
                {
                    if (sizeLimit > 0 && node.Value != null)
                    {
                        int repSize = slotSizeFunc(node.Value);
                        currentSize -= repSize;
                        currentSize += size;
                    }

                    // If we already have a key, move it to the front
                    list.Remove(node);
                    list.AddFirst(node);

                    // Remove the old value
                    if (node.Value != null)
                        node.Value.Dispose();
                    node.Value = value;
                    while (sizeLimit > 0 && currentSize > sizeLimit && list.Count > 1)
                        Evict();
                    return;
                }
                if (sizeLimit > 0)
                {
                    while (sizeLimit > 0 && currentSize + size > sizeLimit && list.Count > 0)
                        Evict();
                }
                if (dict.Count >= entryLimit)
                    Evict();
                // Adding new node
                node = new LinkedListNode<TValue>(value);
                list.AddFirst(node);
                dict[key] = node;
                revdict[node] = key;
                currentSize += size;
                Console.WriteLine("new size: {0} with {1}", currentSize, list.Count);
            }
        }

        public override string ToString()
        {
            return "LRUCache dict={0} revdict={1} list={2}";
        }
    }
}