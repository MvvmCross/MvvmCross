// MvxListItemView.cs

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
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;

    [Register("mvvmcross.binding.droid.views.MvxListItemView")]
    public class MvxListItemView 
        : Java.Lang.Object
        , IMvxListItemView
        , IMvxBindingContextOwner
        , View.IOnAttachStateChangeListener
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        public MvxListItemView(Context context,
                               IMvxLayoutInflaterHolder layoutInflaterHolder,
                               object dataContext,
                               ViewGroup parent,
                               int templateId)
        {
            this._bindingContext = new MvxAndroidBindingContext(context, layoutInflaterHolder, dataContext);
            TemplateId = templateId;
            Content = _bindingContext.BindingInflate(templateId, parent, false);
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

        public IMvxBindingContext BindingContext
        {
            get { return this._bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private View _content;
        public View Content
        {
            get { return _content; }
            set
            {
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

        public int TemplateId { get; }
    }
}