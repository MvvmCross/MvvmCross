// MvxBindableTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindableTableViewSource : MvxBaseBindableTableViewSource
    {
        private IEnumerable _itemsSource;

        protected MvxBindableTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public MvxBindableTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier,
                                          string bindingText,
                                          UITableViewCellAccessory tableViewCellAccessory =
                                              UITableViewCellAccessory.None)
            : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
        {
        }

        public MvxBindableTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier,
                                          IEnumerable<MvxBindingDescription> descriptions,
                                          UITableViewCellAccessory tableViewCellAccessory =
                                              UITableViewCellAccessory.None)
            : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource == value)
                    return;

                var collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                    collectionChanged.CollectionChanged -= CollectionChangedOnCollectionChanged;
                _itemsSource = value;
                collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                    collectionChanged.CollectionChanged += CollectionChangedOnCollectionChanged;
                ReloadTableData();
            }
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (ItemsSource == null)
                return null;

            return ItemsSource.ElementAt(indexPath.Row);
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender,
                                                                    NotifyCollectionChangedEventArgs
                                                                        notifyCollectionChangedEventArgs)
        {
            ReloadTableData();
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (ItemsSource == null)
                return 0;

            return ItemsSource.Count();
        }
    }
}