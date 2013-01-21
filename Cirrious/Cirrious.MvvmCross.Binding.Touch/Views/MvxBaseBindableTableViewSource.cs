// MvxBaseBindableTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.Commands;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxBaseBindableTableViewSource : UITableViewSource
    {
        private readonly UITableView _tableView;

        protected MvxBaseBindableTableViewSource(UITableView tableView)
        {
            _tableView = tableView;
        }

        protected UITableView TableView
        {
            get { return _tableView; }
        }

        public event EventHandler<MvxSimpleSelectionChangedEventArgs> SelectionChanged;

        public ICommand SelectionChangedCommand { get; set; }

        public virtual void ReloadTableData()
        {
            _tableView.ReloadData();
        }

        protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var selectionChangedArgs = MvxSimpleSelectionChangedEventArgs.JustAddOneItem(item);

            var handler = SelectionChanged;
            if (handler != null)
                handler(this, selectionChangedArgs);

            var command = SelectionChangedCommand;
            if (command != null)
                command.Execute(item);
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