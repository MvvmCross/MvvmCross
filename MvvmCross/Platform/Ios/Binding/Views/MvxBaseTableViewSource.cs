// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Foundation;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MvvmCross.Platform.Ios.Binding.Views
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
            MvxLog.Instance.Warn("MvxBaseTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
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
                MvxLog.Instance.Warn("Exception masked during TableView ReloadData {0}", exception.ToLongString());
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
            get
            {
                return _selectedItem;
            }
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

            if (cell is IMvxBindable bindable)
            {
                var bindingContext = bindable.BindingContext as MvxTaskBasedBindingContext;

                var isTaskBasedBindingContextAndHasAutomaticDimension = bindingContext != null && TableView.RowHeight == UITableView.AutomaticDimension;

                // RunSynchronously must be called before DataContext is set
                if (isTaskBasedBindingContextAndHasAutomaticDimension)
                    bindingContext.RunSynchronously = true;

                bindable.DataContext = item;

                // If AutomaticDimension is used, xib based cells need to re-layout everything after bindings are applied
                // otherwise the cell height will be wrong
                if (isTaskBasedBindingContextAndHasAutomaticDimension)
                    cell.LayoutIfNeeded();
            }

            return cell;
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            //Don't bind to NULL to speed up cells in lists when fast scrolling
            //There should be almost no scenario in which this is required
            //If it is required, do this in your own subclass using this code:

            //var bindable = cell as IMvxDataConsumer;
            //if (bindable != null)
            //    bindable.DataContext = null;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}
