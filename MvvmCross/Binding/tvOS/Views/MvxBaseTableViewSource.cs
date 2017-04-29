// MvxBaseTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using UIKit;

namespace MvvmCross.Binding.tvOS.Views
{
    public abstract class MvxBaseTableViewSource : UITableViewSource
    {
        private object _selectedItem;

        protected MvxBaseTableViewSource(UITableView tableView)
        {
            TableView = tableView;
        }

        protected MvxBaseTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning(
                "MvxBaseTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        protected UITableView TableView { get; }

        public bool DeselectAutomatically { get; set; }

        public ICommand SelectionChangedCommand { get; set; }

        public ICommand AccessoryTappedCommand { get; set; }

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                _selectedItem = value;
                var handler = SelectedItemChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

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
                Mvx.Warning("Exception masked during TableView ReloadData {0}", exception.ToLongString());
            }
        }

        protected abstract UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath,
            object item);

        protected abstract object GetItemAt(NSIndexPath indexPath);

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (DeselectAutomatically)
                tableView.DeselectRow(indexPath, true);

            var item = GetItemAt(indexPath);

            var command = SelectionChangedCommand;
            if (command != null && command.CanExecute(item))
                command.Execute(item);

            SelectedItem = item;
        }

        public event EventHandler SelectedItemChanged;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            var bindable = cell as MvxTableViewCell;

            if (bindable != null)
            {
                var bindingContext = bindable.BindingContext as MvxTaskBasedBindingContext;
                if (bindingContext != null && TableView.RowHeight == UITableView.AutomaticDimension)
                    bindingContext.RunSynchronously = true;
                bindable.DataContext = item;
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