// MvxFrameControl.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;

    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Views;
    using Android.Widget;

    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Platform;

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
            this._templateId = templateId;

            if (!(context is IMvxLayoutInflaterHolder))
            {
                throw Mvx.Exception("The owning Context for a MvxFrameControl must implement LayoutInflater");
            }

            this._bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
            this.DelayBind(() =>
                {
                    if (this.Content == null && this._templateId != 0)
                    {
                        Mvx.Trace("DataContext is {0}", this.DataContext?.ToString() ?? "Null");
                        this.Content = this._bindingContext.BindingInflate(this._templateId, this);
                    }
                });
        }

        protected MvxFrameControl(IntPtr javaReference, JniHandleOwnership transfer)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                this._cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            this._isAttachedToWindow = true;
            if (this._cachedDataContext != null
                && this.DataContext == null)
            {
                this.DataContext = this._cachedDataContext;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            this._cachedDataContext = this.DataContext;
            this.DataContext = null;
            base.OnDetachedFromWindow();
            this._isAttachedToWindow = false;
        }

        protected View Content { get; set; }

        [MvxSetToNullAfterBinding]
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