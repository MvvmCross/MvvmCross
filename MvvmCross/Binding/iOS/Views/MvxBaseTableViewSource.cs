// MvxBaseTableViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;
    using System.Windows.Input;

    using Foundation;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

    using UIKit;

    public abstract class MvxBaseTableViewSource : UITableViewSource
    {
        private readonly UITableView _tableView;

        protected MvxBaseTableViewSource(UITableView tableView)
        {
            this._tableView = tableView;
        }

        protected MvxBaseTableViewSource(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxBaseTableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        protected UITableView TableView => this._tableView;

        public bool DeselectAutomatically { get; set; }

        public ICommand SelectionChangedCommand { get; set; }

        public ICommand AccessoryTappedCommand { get; set; }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            var command = this.AccessoryTappedCommand;
            if (command == null)
                return;

            var item = this.GetItemAt(indexPath);
            if (command.CanExecute(item))
                command.Execute(item);
        }

        public virtual void ReloadTableData()
        {
            try
            {
                this._tableView.ReloadData();
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
            if (this.DeselectAutomatically)
            {
                tableView.DeselectRow(indexPath, true);
            }

            var item = this.GetItemAt(indexPath);

            var command = this.SelectionChangedCommand;
            if (command != null && command.CanExecute(item))
                command.Execute(item);

            this.SelectedItem = item;
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                this._selectedItem = value;
                var handler = this.SelectedItemChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedItemChanged;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = this.GetItemAt(indexPath);
            var cell = this.GetOrCreateCellFor(tableView, indexPath, item);

            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;

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