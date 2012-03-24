#region Copyright
// <copyright file="MvxBaseBindableTableViewSource.cs" company="Cirrious">
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
using System.Collections.Specialized;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxBaseBindableTableViewSource : UITableViewSource
    {
        private static readonly NSString DefaultCellIdentifier = new NSString("BindableTableViewCell");
        private static readonly MvxBindingDescription[] DefaultBindingDescription = new MvxBindingDescription[]
                                                                                        {
                                                                                            new MvxBindingDescription()
                                                                                                {
                                                                                                    TargetName = "TitleText",
                                                                                                    SourcePropertyPath = string.Empty
                                                                                                }, 
                                                                                        };

        private readonly IEnumerable<MvxBindingDescription> _bindingDescriptions;
        private readonly NSString _cellIdentifier;
        private readonly UITableViewCellStyle _cellStyle;
        private readonly UITableView _tableView;
        private readonly UITableViewCellAccessory _tableViewCellAccessory = UITableViewCellAccessory.None;

        protected MvxBaseBindableTableViewSource(UITableView tableView)
            : this(tableView, UITableViewCellStyle.Default, DefaultCellIdentifier, DefaultBindingDescription)
        {
        }

        protected MvxBaseBindableTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, string bindingText, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : this(tableView, style, cellIdentifier, ParseBindingText(bindingText), tableViewCellAccessory)
        {
        }

        protected MvxBaseBindableTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, IEnumerable<MvxBindingDescription> descriptions, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
        {
            _tableView = tableView;
            _cellStyle = style;
            _cellIdentifier = cellIdentifier;
            _bindingDescriptions = descriptions;
            _tableViewCellAccessory = tableViewCellAccessory;
        }

        protected IEnumerable<MvxBindingDescription> BindingDescriptions { get { return _bindingDescriptions; } }
        protected UITableView TableView { get { return _tableView; } }

        private static IEnumerable<MvxBindingDescription> ParseBindingText(string bindingText)
        {
            if (string.IsNullOrEmpty(bindingText))
                return DefaultBindingDescription;

            return MvxServiceProviderExtensions.GetService<IMvxBindingDescriptionParser>().Parse(bindingText);
        }

        public event EventHandler<MvxSimpleSelectionChangedEventArgs> SelectionChanged;

        public virtual void ReloadTableData()
        {
            _tableView.ReloadData();
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ReloadTableData();
        }

        protected virtual UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuse = tableView.DequeueReusableCell(_cellIdentifier);
            if (reuse != null)
                return reuse;

            return CreateDefaultBindableCell(tableView, indexPath, item);
        }

        protected virtual MvxBindableTableViewCell CreateDefaultBindableCell(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return new MvxBindableTableViewCell(_bindingDescriptions, _cellStyle, _cellIdentifier,
                                                _tableViewCellAccessory);
        }

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var selectionChangedArgs = MvxSimpleSelectionChangedEventArgs.JustAddOneItem(item);

            var handler = SelectionChanged;
            if (handler != null)
                handler(this, selectionChangedArgs);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            var bindable = cell as IMvxBindableView;
            if (bindable != null)
                bindable.BindTo(item);

            return cell;
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}