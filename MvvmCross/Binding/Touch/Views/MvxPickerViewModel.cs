// MvxPickerViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Windows.Input;

    using MvvmCross.Platform;
    using MvvmCross.Platform.WeakSubscription;

    using UIKit;

    public class MvxPickerViewModel
        : UIPickerViewModel
    {
        private readonly UIPickerView _pickerView;
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        private object _selectedItem;

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public MvxPickerViewModel(UIPickerView pickerView)
        {
            this._pickerView = pickerView;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }
            }

            base.Dispose(disposing);
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return this._itemsSource; }
            set
            {
                if (Object.ReferenceEquals(this._itemsSource, value)
                    && !this.ReloadOnAllItemsSourceSets)
                    return;

                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }

                this._itemsSource = value;

                var collectionChanged = this._itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    this._subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }

                this.Reload();
                this.ShowSelectedItem();
            }
        }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Mvx.Trace(
                "CollectionChanged called inside MvxPickerViewModel - beware that this isn't fully tested - picker might not fully support changes while the picker is visible");
            this.Reload();
        }

        protected virtual void Reload()
        {
            this._pickerView.ReloadComponent(0);
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return this._itemsSource?.Count() ?? 0;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return this._itemsSource == null ? "-" : this.RowTitle(row, this._itemsSource.ElementAt((int)row));
        }

        protected virtual string RowTitle(nint row, object item)
        {
            return item.ToString();
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            if (this._itemsSource.Count() == 0)
                return;

            this._selectedItem = this._itemsSource.ElementAt((int)row);

            var handler = this.SelectedItemChanged;
            handler?.Invoke(this, EventArgs.Empty);

            var command = this.SelectedChangedCommand;
            if (command != null)
                if (command.CanExecute(this._selectedItem))
                    command.Execute(this._selectedItem);
        }

        public object SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                this._selectedItem = value;
                this.ShowSelectedItem();
            }
        }

        public event EventHandler SelectedItemChanged;

        public ICommand SelectedChangedCommand { get; set; }

        protected virtual void ShowSelectedItem()
        {
            if (this._itemsSource == null)
                return;

            var position = this._itemsSource.GetPosition(this._selectedItem);
            if (position < 0)
                return;

            var animated = !this._pickerView.Hidden;
            this._pickerView.Select(position, 0, animated);
        }
    }
}