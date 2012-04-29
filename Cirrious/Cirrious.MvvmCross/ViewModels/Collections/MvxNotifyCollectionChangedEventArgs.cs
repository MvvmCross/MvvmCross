#region Copyright
// <copyright file="MvxBaseResourceLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

// External credit:
// This file relies heavily on the Mono project - used under MIT license:
// https://github.com/mono/mono

using System;
using System.Collections;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxNotifyCollectionChangedEventArgs : EventArgs
    {
        private MvxNotifyCollectionChangedAction action;
        private IList oldItems, newItems;
        private int oldIndex = -1, newIndex = -1;

        #region Constructors

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action)
        {
            this.action = action;

            if (action != MvxNotifyCollectionChangedAction.Reset)
                throw new ArgumentException("This constructor can only be used with the Reset action.", "action");
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, IList changedItems)
            : this(action, changedItems, -1)
        {
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, object changedItem)
            : this(action, changedItem, -1)
        {
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, IList newItems, IList oldItems)
            : this(action, newItems, oldItems, -1)
        {
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            this.action = action;

            if (action == MvxNotifyCollectionChangedAction.Add || action == MvxNotifyCollectionChangedAction.Remove)
            {
                if (changedItems == null)
                    throw new ArgumentNullException("changedItems");

                if (startingIndex < -1)
                    throw new ArgumentException("The value of startingIndex must be -1 or greater.", "startingIndex");

                if (action == MvxNotifyCollectionChangedAction.Add)
                    InitializeAdd(changedItems, startingIndex);
                else
                    InitializeRemove(changedItems, startingIndex);
            }
            else if (action == MvxNotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                    throw new ArgumentException("This constructor can only be used with the Reset action if changedItems is null", "changedItems");

                if (startingIndex != -1)
                    throw new ArgumentException("This constructor can only be used with the Reset action if startingIndex is -1", "startingIndex");
            }
            else
            {
                throw new ArgumentException("This constructor can only be used with the Reset, Add, or Remove actions.", "action");
            }
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, object changedItem, int index)
        {
            IList changedItems = new object[] { changedItem };
            this.action = action;

            if (action == MvxNotifyCollectionChangedAction.Add)
                InitializeAdd(changedItems, index);
            else if (action == MvxNotifyCollectionChangedAction.Remove)
                InitializeRemove(changedItems, index);
            else if (action == MvxNotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                    throw new ArgumentException("This constructor can only be used with the Reset action if changedItem is null", "changedItem");

                if (index != -1)
                    throw new ArgumentException("This constructor can only be used with the Reset action if index is -1", "index");
            }
            else
            {
                throw new ArgumentException("This constructor can only be used with the Reset, Add, or Remove actions.", "action");
            }
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, object newItem, object oldItem)
            : this(action, newItem, oldItem, -1)
        {
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, IList newItems, IList oldItems, int index)
        {
            this.action = action;

            if (action != MvxNotifyCollectionChangedAction.Replace)
                throw new ArgumentException("This constructor can only be used with the Replace action.", "action");

            if (newItems == null)
                throw new ArgumentNullException("newItems");

            if (oldItems == null)
                throw new ArgumentNullException("oldItems");

            this.oldItems = oldItems;
            this.newItems = newItems;

            oldIndex = index;
            newIndex = index;
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            this.action = action;

            if (action != MvxNotifyCollectionChangedAction.Move)
                throw new ArgumentException("This constructor can only be used with the Move action.", "action");

            if (index < -1)
                throw new ArgumentException("The value of index must be -1 or greater.", "index");

            InitializeMove(changedItems, index, oldIndex);
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
            : this(action, new object[] { changedItem }, index, oldIndex)
        {
        }

        public MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            this.action = action;

            if (action != MvxNotifyCollectionChangedAction.Replace)
                throw new ArgumentException("This constructor can only be used with the Replace action.", "action");

            InitializeReplace(new object[] { newItem }, new object[] { oldItem }, index);
        }

        #endregion

        #region Accessor Properties

        public MvxNotifyCollectionChangedAction Action
        {
            get { return action; }
        }

        public IList NewItems
        {
            get { return newItems; }
        }

        public int NewStartingIndex
        {
            get { return newIndex; }
        }

        public IList OldItems
        {
            get { return oldItems; }
        }

        public int OldStartingIndex
        {
            get { return oldIndex; }
        }

        #endregion

        #region Initialize Methods

        private static IList CopyList(IList input)
        {
            var list = new List<object>(input.Count);
            foreach (var o in list)
            {
                list.Add(o);
            }
            return list;
        }

        private void InitializeAdd(IList items, int index)
        {
            this.newItems = CopyList(items);
            this.newIndex = index;
        }

        private void InitializeRemove(IList items, int index)
        {
            this.oldItems = CopyList(items);
            this.oldIndex = index;
        }

        private void InitializeMove(IList changedItems, int newItemIndex, int oldItemIndex)
        {
            InitializeAdd(changedItems, newItemIndex);
            InitializeRemove(changedItems, oldItemIndex);
        }

        private void InitializeReplace(IList addedItems, IList removedItems, int index)
        {
            InitializeAdd(addedItems, index);
            InitializeRemove(removedItems, index);
        }

        #endregion
    }
}