// MvxFrameControl.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
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
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Register("cirrious.mvvmcross.binding.droid.views.MvxFrameControl")]
    public class MvxFrameControl
        : FrameLayout
          , IMvxBindingContextOwner
    {
        private readonly int _templateId;
        private readonly IMvxAndroidBindingContext _bindingContext;

        public MvxFrameControl(Context context, IAttributeSet attrs)
            : this(MvxAttributeHelpers.ReadTemplateId(context, attrs), context, attrs)
        {
        }

        public MvxFrameControl(int templateId, Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _templateId = templateId;

            if (!(context is IMvxLayoutInflaterHolder))
            {
                throw Mvx.Exception("The owning Context for a MvxFrameControl must implement LayoutInflater");
            }

            _bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
            this.DelayBind(() =>
                {
                    if (Content == null && _templateId != 0)
                    {
                        Mvx.Trace("DataContext is {0}", DataContext?.ToString() ?? "Null");
                        Content = _bindingContext.BindingInflate(_templateId, this);
                    }
                });
        }

	    protected MvxFrameControl(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }

        protected IMvxAndroidBindingContext AndroidBindingContext => _bindingContext;

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private object _cachedDataContext;
        private bool _isAttachedToWindow;

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
            {
                DataContext = _cachedDataContext;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            base.OnDetachedFromWindow();
            _isAttachedToWindow = false;
        }


        protected View Content { get; set; }

        [MvxSetToNullAfterBinding]
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
    }
}