// MvxFrameControl.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Platform;

namespace MvvmCross.Binding.Droid.Views
{
    [Register("mvvmcross.binding.droid.views.MvxFrameControl")]
    public class MvxFrameControl
        : FrameLayout
            , IMvxBindingContextOwner
    {
        private readonly int _templateId;

        private object _cachedDataContext;

        private View _content;
        private bool _isAttachedToWindow;

        public MvxFrameControl(Context context, IAttributeSet attrs)
            : this(MvxAttributeHelpers.ReadTemplateId(context, attrs), context, attrs)
        {
        }

        public MvxFrameControl(int templateId, Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _templateId = templateId;

            if (!(context is IMvxLayoutInflaterHolder))
                throw Mvx.Exception("The owning Context for a MvxFrameControl must implement LayoutInflater");

            AndroidBindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder) context);
            this.DelayBind(() =>
            {
                if (Content == null && _templateId != 0)
                {
                    Mvx.Trace("DataContext is {0}", DataContext?.ToString() ?? "Null");
                    Content = AndroidBindingContext.BindingInflate(_templateId, this);
                }
            });
        }

        protected MvxFrameControl(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected IMvxAndroidBindingContext AndroidBindingContext { get; }

        protected View Content
        {
            get => _content;
            set
            {
                _content = value;
                OnContentSet();
            }
        }

        [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get { return AndroidBindingContext.DataContext; }
            set
            {
                if (_isAttachedToWindow)
                {
                    AndroidBindingContext.DataContext = value;
                }
                else
                {
                    _cachedDataContext = value;
                    if (AndroidBindingContext.DataContext != null)
                        AndroidBindingContext.DataContext = null;
                }
            }
        }

        public IMvxBindingContext BindingContext
        {
            get => AndroidBindingContext;
            set => throw new NotImplementedException("BindingContext is readonly in the list item");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                _cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _isAttachedToWindow = true;
            if (_cachedDataContext != null
                && DataContext == null)
                DataContext = _cachedDataContext;
        }

        protected override void OnDetachedFromWindow()
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            base.OnDetachedFromWindow();
            _isAttachedToWindow = false;
        }

        protected virtual void OnContentSet()
        {
        }
    }
}