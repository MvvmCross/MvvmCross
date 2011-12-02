//
// A simple LRU cache used for tracking the images
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010 Miguel de Icaza
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using System.Collections.Generic;

namespace MonoTouch.Dialog.Utilities {
	
public class LRUCache<TKey, TValue> where TValue : class, IDisposable  {
	Dictionary<TKey, LinkedListNode <TValue>> dict;
	Dictionary<LinkedListNode<TValue>, TKey> revdict;
	LinkedList<TValue> list;
	int limit;
	
	public LRUCache (int limit)
	{
		list = new LinkedList<TValue> ();
		dict = new Dictionary<TKey, LinkedListNode<TValue>> ();
		revdict = new Dictionary<LinkedListNode<TValue>, TKey> ();
		
		this.limit = limit;
	}

	void Evict ()
	{
		var last = list.Last;
		var key = revdict [last];
		
		dict.Remove (key);
		revdict.Remove (last);
		list.RemoveLast ();
		last.Value.Dispose ();
	}

	public void Purge ()
	{
		foreach (var element in list)
			element.Dispose ();
		
		dict.Clear ();
		revdict.Clear ();
		list.Clear ();
	}

	public TValue this [TKey key] {
		get {
			LinkedListNode<TValue> node;
			
			if (dict.TryGetValue (key, out node)){
				list.Remove (node);
				list.AddFirst (node);

				return node.Value;
			}
			return null;
		}

		set {
			LinkedListNode<TValue> node;
	
			if (dict.TryGetValue (key, out node)){
				// If we already have a key, move it to the front
				list.Remove (node);
				list.AddFirst (node);
	
				// Remove the old value
				if (node.Value != null)
					node.Value.Dispose ();
				node.Value = value;
				return;
			}
			if (dict.Count >= limit)
				Evict ();
			// Adding new node
			node = new LinkedListNode<TValue> (value);
			list.AddFirst (node);
			dict [key] = node;
			revdict [node] = key;
		}
	}

	public override string ToString ()
	{
		return "LRUCache dict={0} revdict={1} list={2}";
	}		
}
}