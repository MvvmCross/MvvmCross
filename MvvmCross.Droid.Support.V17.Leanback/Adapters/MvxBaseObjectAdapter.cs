using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Runtime;
using Android.Support.V17.Leanback.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using MvvmCross.Binding.Droid.BindingContext;

namespace MvvmCross.Droid.Support.V17.Leanback.Adapters
{
    public abstract class MvxBaseObjectAdapter : Android.Support.V17.Leanback.Widget.ObjectAdapter, IMvxObjectAdapter
    {
        public event EventHandler DataSetChanged;

        private IDisposable _subscription;

        protected IMvxAndroidBindingContext BindingContext { get; }

        private IEnumerable _itemsSource;

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (ReferenceEquals(_itemsSource, value))
                {
                    return;
                }

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }

                _itemsSource = value;

                if (_itemsSource != null)
                {
                    if (!(value is ICollection))
                    {
                        MvxTrace.Warning("Using a enumerable is not recommended due to performance issues. Consider using an ICollection (e.g. List) as ItemsSource.");
                    }
                }

                var newObservable = _itemsSource as INotifyCollectionChanged;
                if (newObservable != null)
                {
                    _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
                }

                NotifyAndRaiseDataSetChanged();
            }
        }

        protected MvxBaseObjectAdapter() : this(MvxAndroidBindingContextHelpers.Current())
        {
        }

        protected MvxBaseObjectAdapter(IMvxAndroidBindingContext bindingContext)
        {
            BindingContext = bindingContext;
        }

        protected MvxBaseObjectAdapter(Presenter presenter) : this(presenter, MvxAndroidBindingContextHelpers.Current())
        {
        }

        protected MvxBaseObjectAdapter(Presenter presenter, IMvxAndroidBindingContext bindingContext) : base(presenter)
        {
            BindingContext = bindingContext;
        }

        protected MvxBaseObjectAdapter(PresenterSelector presenterSelector) : this(presenterSelector, MvxAndroidBindingContextHelpers.Current())
        {
        }

        protected MvxBaseObjectAdapter(PresenterSelector presenterSelector, IMvxAndroidBindingContext bindingContext) : base(presenterSelector)
        {
            BindingContext = bindingContext;
        }

        protected MvxBaseObjectAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            NotifyChanged(eventArgs);
        }

        public virtual void NotifyChanged(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                        RaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Move:
                        NotifyAndRaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                        RaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                        RaiseDataSetChanged();
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        NotifyAndRaiseDataSetChanged();
                        break;
                }
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during Adapter NotifyChanged {0}", exception.ToLongString());
            }
        }

        private void RaiseDataSetChanged()
        {
            var handler = DataSetChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private void NotifyAndRaiseDataSetChanged()
        {
            RaiseDataSetChanged();
            NotifyChanged();
        }
    }
}