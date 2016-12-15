// MvxBaseListItemView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;

    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;

    public abstract class MvxBaseListItemView
        : View
        , IMvxBindingContextOwner
        , View.IOnAttachStateChangeListener
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        protected MvxBaseListItemView(Context context, IMvxLayoutInflaterHolder layoutInflaterHolder, object dataContext, ViewGroup parent)
            : base(context)
        {
            this._bindingContext = new MvxAndroidBindingContext(context, layoutInflaterHolder, dataContext);
        }
        protected MvxBaseListItemView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected IMvxAndroidBindingContext AndroidBindingContext => this._bindingContext;

        public IMvxBindingContext BindingContext
        {
            get { return this._bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private object _cachedDataContext;
        private bool _isAttachedToWindow;

        public void OnViewAttachedToWindow(View attachedView)
        {
            this._isAttachedToWindow = true;
            if (this._cachedDataContext != null
                && this.DataContext == null)
            {
                this.DataContext = this._cachedDataContext;
            }
        }

        public void OnViewDetachedFromWindow(View detachedView)
        {
            this._cachedDataContext = this.DataContext;
            this.DataContext = null;
            this._isAttachedToWindow = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                this._cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        private View _content;

        public View Content { 
            get { return _content; } 
            set { 
                _content = value;
                _content.AddOnAttachStateChangeListener(this);
            }
        }

        public object DataContext
        {
            get { return this._bindingContext.DataContext; }
            set
            {
                if (this._isAttachedToWindow)
                {
                    this._bindingContext.DataContext = value;
                }
                else
                {
                    this._cachedDataContext = value;
                    if (this._bindingContext.DataContext != null)
                    {
                        this._bindingContext.DataContext = null;
                    }
                }
            }
        }
    }
}