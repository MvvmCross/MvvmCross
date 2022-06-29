// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public abstract class MvxBaseTableViewSource : UITableViewSource
    {
        public event EventHandler SelectedItemChanged;
        private object _selectedItem;
        private readonly WeakReference<UITableView> _tableView;

        protected MvxBaseTableViewSource(UITableView tableView)
        {
            _tableView = new WeakReference<UITableView>(tableView);
        }

        protected MvxBaseTableViewSource(IntPtr handle)
            : base(handle)
        {
            MvxLogHost.GetLog<MvxBaseTableViewSource>()?.Log(LogLevel.Warning,
                "MvxBaseTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        protected UITableView TableView
        {
            get
            {
                if (_tableView.TryGetTarget(out var tableView))
                    return tableView;

                // This is not a array Sonar. You are drunk...
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }
        }

        public bool DeselectAutomatically { get; set; }

        public bool DeselectChangedEnabled { get; set; }

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
                TableView.ReloadData();
            }
            catch (Exception exception)
            {
                MvxLogHost.GetLog<MvxBaseTableViewSource>()?.Log(LogLevel.Warning, exception,
                    "Exception masked during TableView ReloadData");
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (DeselectAutomatically)
            {
                tableView.DeselectRow(indexPath, true);
            }

            var item = GetItemAt(indexPath);

            var command = SelectionChangedCommand;
            if (command?.CanExecute(item) == true)
                command.Execute(item);

            SelectedItem = item;
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            if (DeselectChangedEnabled && !DeselectAutomatically)
            {
                var item = GetItemAt(indexPath);

                var command = SelectionChangedCommand;
                if (command != null && command.CanExecute(item))
                    command.Execute(item);

                SelectedItem = null;
            }
        }

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
                SelectedItemChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            BindCell(tableView, cell, item);

            return cell;
        }

        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            // Don't bind to NULL to speed up cells in lists when fast scrolling
            // There should be almost no scenario in which this is required
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item);

        protected abstract object GetItemAt(NSIndexPath indexPath);

        private static void BindCell(UITableView tableView, UITableViewCell cell, object item)
        {
            if (cell is IMvxBindable bindable)
            {
                var bindingContext = bindable.BindingContext as MvxTaskBasedBindingContext;

                var isTaskBasedBindingContextAndHasAutomaticDimension =
                    bindingContext != null && tableView.RowHeight == UITableView.AutomaticDimension;

                // RunSynchronously must be called before DataContext is set
                if (isTaskBasedBindingContextAndHasAutomaticDimension)
                    bindingContext.RunSynchronously = true;

                bindable.DataContext = item;

                // If AutomaticDimension is used, xib based cells need to re-layout everything after bindings are applied
                // otherwise the cell height will be wrong
                if (isTaskBasedBindingContextAndHasAutomaticDimension)
                    cell.LayoutIfNeeded();
            }
        }
    }
}
