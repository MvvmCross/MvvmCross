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
    using Binding.BindingContext;
    using BindingContext;

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
            _bindingContext = new MvxAndroidBindingContext(context, layoutInflaterHolder, dataContext);
            TemplateId = templateId;
            Content = _bindingContext.BindingInflate(templateId, parent, false);
        }

        private object _cachedDataContext;
        private bool _isAttachedToWindow;

        public void OnViewAttachedToWindow(View attachedView)
        {
            _isAttachedToWindow = true;
            if (_cachedDataContext != null
                && DataContext == null)
            {
                DataContext = _cachedDataContext;
            }
        }

        public void OnViewDetachedFromWindow(View detachedView)
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            _isAttachedToWindow = false;
        }

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
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
            get { return _bindingContext.DataContext; }
            set
            {
                if (_isAttachedToWindow)
                {
                    _bindingContext.DataContext = value;
                }
                else
                {
                    _cachedDataContext = value;
                    if (_bindingContext.DataContext != null)
                    {
                        _bindingContext.DataContext = null;
                    }
                }
            }
        }

        public int TemplateId { get; }
    }
}