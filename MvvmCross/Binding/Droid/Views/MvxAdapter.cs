// MvxAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;

    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.WeakSubscription;

    using Object = Java.Lang.Object;

    public class MvxAdapter
        : BaseAdapter
        , IMvxAdapter
    {
        private readonly IMvxAndroidBindingContext _bindingContext;
        private readonly Context _context;
        private int _itemTemplateId;
        private int _dropDownItemTemplateId;
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        // Note - _currentSimpleId is a bit of a hack
        //      - it's essentially a state flag identifying wheter we are currently inflating a dropdown or a normal view
        //      - ideally we would have passed the current simple id as a parameter through the GetView chain instead
        //      - but that was too big a breaking change for this release (7th Oct 2013)
        private int _currentSimpleId;

        // Note2 - _currentParent is similarly a bit of a hack
        //       - it is just here to avoid a breaking api change for now
        //       - will seek to remove both the _currentSimpleId and _currentParent private fields in a major release soon
        private ViewGroup _currentParent;

        public MvxAdapter(Context context)
            : this(context, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public MvxAdapter(Context context, IMvxAndroidBindingContext bindingContext)
        {
            this._context = context;
            this._bindingContext = bindingContext;
            if (this._bindingContext == null)
            {
                throw new MvxException(
                    "bindingContext is null during MvxAdapter creation - Adapter's should only be created when a specific binding context has been placed on the stack");
            }
            this.SimpleViewLayoutId = Android.Resource.Layout.SimpleListItem1;
            this.SimpleDropDownViewLayoutId = Android.Resource.Layout.SimpleSpinnerDropDownItem;
        }

        protected MvxAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected Context Context => this._context;

        protected IMvxAndroidBindingContext BindingContext => this._bindingContext;

        public int SimpleViewLayoutId { get; set; }

        public int SimpleDropDownViewLayoutId { get; set; }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return this._itemsSource; }
            set { this.SetItemsSource(value); }
        }

        public virtual int ItemTemplateId
        {
            get { return this._itemTemplateId; }
            set
            {
                if (this._itemTemplateId == value)
                    return;
                this._itemTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (this._itemsSource != null)
                    this.NotifyDataSetChanged();
            }
        }

        public virtual int DropDownItemTemplateId
        {
            get { return this._dropDownItemTemplateId; }
            set
            {
                if (this._dropDownItemTemplateId == value)
                    return;
                this._dropDownItemTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (this._itemsSource != null)
                    this.NotifyDataSetChanged();
            }
        }

        public override int Count => this._itemsSource.Count();

        protected virtual void SetItemsSource(IEnumerable value)
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

            if (this._itemsSource != null && !(this._itemsSource is IList))
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "You are currently binding to IEnumerable - this can be inefficient, especially for large collections. Binding to IList is more efficient.");

            var newObservable = this._itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
            {
                this._subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
            }
            this.NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.NotifyDataSetChanged(e);
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RealNotifyDataSetChanged();
        }

        public override void NotifyDataSetChanged()
        {
            this.RealNotifyDataSetChanged();
        }

        protected virtual void RealNotifyDataSetChanged()
        {
            try
            {
                base.NotifyDataSetChanged();
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during Adapter RealNotifyDataSetChanged {0}", exception.ToLongString());
            }
        }

        public virtual int GetPosition(object item)
        {
            return this._itemsSource.GetPosition(item);
        }

        public virtual System.Object GetRawItem(int position)
        {
            return this._itemsSource.ElementAt(position);
        }

        public override Object GetItem(int position)
        {
            // we return null to Java here
            // we do **not**: return new MvxJavaContainer<object>(GetRawItem(position));
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            this._currentSimpleId = this.SimpleDropDownViewLayoutId;
            this._currentParent = parent;
            var toReturn = this.GetView(position, convertView, parent, this.DropDownItemTemplateId);
            this._currentParent = null;
            return toReturn;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            this._currentSimpleId = this.SimpleViewLayoutId;
            this._currentParent = parent;
            var toReturn = this.GetView(position, convertView, parent, this.ItemTemplateId);
            this._currentParent = null;
            return toReturn;
        }

        protected virtual View GetView(int position, View convertView, ViewGroup parent, int templateId)
        {
            if (this._itemsSource == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "GetView called when ItemsSource is null");
                return null;
            }

            var source = this.GetRawItem(position);

            return this.GetBindableView(convertView, source, templateId);
        }

        protected virtual View GetSimpleView(View convertView, object dataContext)
        {
            if (convertView == null)
            {
                convertView = this.CreateSimpleView(dataContext);
            }
            else
            {
                this.BindSimpleView(convertView, dataContext);
            }

            return convertView;
        }

        protected virtual void BindSimpleView(View convertView, object dataContext)
        {
            var textView = convertView as TextView;
            if (textView != null)
            {
                textView.Text = (dataContext ?? string.Empty).ToString();
            }
        }

        protected virtual View CreateSimpleView(object dataContext)
        {
            // note - this could technically be a non-binding inflate - but the overhead is minimal
            // note - it's important to use `false` for the attachToRoot argument here
            //    see discussion in https://github.com/MvvmCross/MvvmCross/issues/507
            var view = this._bindingContext.BindingInflate(this._currentSimpleId, this._currentParent, false);
            this.BindSimpleView(view, dataContext);
            return view;
        }

        protected virtual View GetBindableView(View convertView, object dataContext)
        {
            return this.GetBindableView(convertView, dataContext, this.ItemTemplateId);
        }

        protected virtual View GetBindableView(View convertView, object dataContext, int templateId)
        {
            if (templateId == 0)
            {
                // no template seen - so use a standard string view from Android and use ToString()
                return this.GetSimpleView(convertView, dataContext);
            }

            // we have a templateid so lets use bind and inflate on it :)
            var viewToUse = convertView as IMvxListItemView;
            if (viewToUse != null)
            {
                if (viewToUse.TemplateId != templateId)
                {
                    viewToUse = null;
                }
            }

            if (viewToUse == null)
            {
                viewToUse = this.CreateBindableView(dataContext, templateId);
            }
            else
            {
                this.BindBindableView(dataContext, viewToUse);
            }

            return viewToUse as View;
        }

        protected virtual void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            viewToUse.DataContext = source;
        }

        protected virtual IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            return new MvxListItemView(this._context, this._bindingContext.LayoutInflaterHolder, dataContext, templateId);
        }
    }
}