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
        : FrameLayout
        , IMvxBindingContextOwner
        , ICheckable
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        protected MvxBaseListItemView(Context context, IMvxLayoutInflaterHolder layoutInflaterHolder, object dataContext)
            : base(context)
        {
            this._bindingContext = new MvxAndroidBindingContext(context, layoutInflaterHolder, dataContext);
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

        protected IMvxAndroidBindingContext AndroidBindingContext => this._bindingContext;

        public IMvxBindingContext BindingContext
        {
            get { return this._bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private object _cachedDataContext;

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (this._cachedDataContext != null
                && this.DataContext == null)
            {
                this.DataContext = this._cachedDataContext;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            this._cachedDataContext = this.DataContext;

            base.OnDetachedFromWindow();
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

        protected View Content => this.FirstChild;

        public object DataContext
        {
            get { return this._bindingContext.DataContext; }
			set { this._bindingContext.DataContext = value; }
        }

        protected virtual View FirstChild
        {
            get
            {
                if (this.ChildCount == 0)
                    return null;
                var firstChild = this.GetChildAt(0);
                return firstChild;
            }
        }

        protected virtual ICheckable ContentCheckable
        {
            get
            {
                var firstChild = this.FirstChild;
                return firstChild as ICheckable;
            }
        }

        public virtual void Toggle()
        {
            var contentCheckable = this.ContentCheckable;
            if (contentCheckable == null)
            {
                this._checked = !this._checked;
                return;
            }

            contentCheckable.Toggle();
        }

        private bool _checked;

        public virtual bool Checked
        {
            get
            {
                var contentCheckable = this.ContentCheckable;
                if (contentCheckable != null)
                {
                    return contentCheckable.Checked;
                }

                return this._checked;
            }
            set
            {
                this._checked = value;

                var contentCheckable = this.ContentCheckable;
                if (contentCheckable != null)
                {
                    contentCheckable.Checked = value;
                    return;
                }

                // since we don't have genuinely checked content, then use FirstChild activation instead
                // see https://github.com/MvvmCross/MvvmCross/issues/481
                var firstChild = this.FirstChild;
                if (firstChild == null)
                    return;

                if (this.Context.ApplicationInfo.TargetSdkVersion
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