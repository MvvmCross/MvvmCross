// MvxFrameControl.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
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

            if (!(context is IMvxLayoutInflater))
            {
                throw Mvx.Exception("The owning Context for a MvxFrameControl must implement LayoutInflater");
            }

            _bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflater)context);
            this.DelayBind(() =>
                {
                    if (Content == null && _templateId != 0)
                    {
                        Mvx.Trace("DataContext is {0}", DataContext == null ? "Null" : DataContext.ToString());
                        Content = _bindingContext.BindingInflate(_templateId, this);
                    }
                });
        }


        protected IMvxAndroidBindingContext AndroidBindingContext
        {
            get { return _bindingContext; }
        }

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
            }

            base.Dispose(disposing);
        }

        protected View Content { get; set; }

        public object DataContext
        {
            get { return _bindingContext.DataContext; }
            set { _bindingContext.DataContext = value; }
        }
    }
}