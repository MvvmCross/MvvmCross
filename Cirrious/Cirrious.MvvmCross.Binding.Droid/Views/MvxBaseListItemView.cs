// MvxBaseListItemView.cs
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
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public abstract class MvxBaseListItemView
        : FrameLayout
          , IMvxBindingContextOwner
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        protected MvxBaseListItemView(Context context, IMvxLayoutInflater layoutInflater, object dataContext)
            : base(context)
        {
            _bindingContext = new MvxAndroidBindingContext(context, layoutInflater, dataContext);
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