// MvxBaseListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public abstract class MvxBaseListItemView
        : FrameLayout
        , IMvxBindingContextOwner
        , ICheckable
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        protected MvxBaseListItemView(Context context, IMvxLayoutInflater layoutInflater, object dataContext)
            : base(context)
        {
            _bindingContext = new MvxAndroidBindingContext(context, layoutInflater, dataContext);
        }

		protected MvxBaseListItemView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }

        /*
        protected override ViewGroup.LayoutParams GenerateDefaultLayoutParams()
        {
            return new FrameLayout.LayoutParams(FrameLayout.LayoutParams.FillParent, FrameLayout.LayoutParams.WrapContent);
        }
        */

        protected IMvxAndroidBindingContext AndroidBindingContext
        {
            get { return _bindingContext; }
        }

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private object _cachedDataContext;
        private bool _isAttachedToWindow;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                _cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        protected View Content
        {
            get { return FirstChild; }
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

        protected virtual View FirstChild
        {
            get
            {
                if (ChildCount == 0)
                    return null;
                var firstChild = this.GetChildAt(0);
                return firstChild;
            }
        }

        protected virtual ICheckable ContentCheckable
        {
            get
            {
                var firstChild = FirstChild;
                return firstChild as ICheckable;
            }
        }

        public virtual void Toggle()
        {
            var contentCheckable = ContentCheckable;
            if (contentCheckable == null)
            {
                _checked = !_checked;
                return;
            }

            contentCheckable.Toggle();
        }

        private bool _checked;
        public virtual bool Checked
        {
            get
            {
                var contentCheckable = ContentCheckable;
                if (contentCheckable != null)
                {
                    return contentCheckable.Checked;
                }

                return _checked;
            }
            set
            {
                _checked = value;

                var contentCheckable = ContentCheckable;
                if (contentCheckable != null)
                {
                    contentCheckable.Checked = value;
                    return;
                }

                // since we don't have genuinely checked content, then use FirstChild activation instead
                // see https://github.com/MvvmCross/MvvmCross/issues/481
                var firstChild = FirstChild;
                if (firstChild == null)
                    return;

                if (Context.ApplicationInfo.TargetSdkVersion
                    >= Android.OS.BuildVersionCodes.Honeycomb &&
                    Android.OS.Build.VERSION.SdkInt
                    >= Android.OS.BuildVersionCodes.Honeycomb)
                {
                    firstChild.Activated = value;
                }
            }
        }
    }
}