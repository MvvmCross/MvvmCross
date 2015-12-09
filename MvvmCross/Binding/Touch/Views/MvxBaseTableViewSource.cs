// MvxBaseTableViewSource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Foundation;
using System;
using System.Windows.Input;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public abstract class MvxBaseTableViewSource : UITableViewSource
    {
        private readonly UITableView _tableView;

        protected MvxBaseTableViewSource(UITableView tableView)
        {
            _tableView = tableView;
        }

        protected MvxBaseTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxBaseTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        protected UITableView TableView => _tableView;

        public bool DeselectAutomatically { get; set; }

        public ICommand SelectionChangedCommand { get; set; }

        public ICommand AccessoryTappedCommand { get; set; }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            var command = AccessoryTappedCommand;
            if (command == null)
                return;

            var item = GetItemAt(indexPath);
            if (command.CanExecute(item))
                command.Execute(item);
        }

        public virtual void ReloadTableData()
        {
            try
            {
                _tableView.ReloadData();
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during TableView ReloadData {0}", exception.ToLongString());
            }
        }

        protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (DeselectAutomatically)
            {
                tableView.DeselectRow(indexPath, true);
            }

            var item = GetItemAt(indexPath);

            var command = SelectionChangedCommand;
            if (command != null && command.CanExecute(item))
                command.Execute(item);

            SelectedItem = item;
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                _selectedItem = value;
                var handler = SelectedItemChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedItemChanged;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;

            return cell;
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = null;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}