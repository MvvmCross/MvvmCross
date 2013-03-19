// MvxBaseListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public abstract class MvxBaseListItemView
        : FrameLayout
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        protected MvxBaseListItemView(Context context, IMvxLayoutInflater layoutInflater, object source)
            : base(context)
        {
            _bindingContext = new MvxAndroidBindingContext(context, layoutInflater, source);
        }

        public void ClearBindings()
        {
            _bindingContext.ClearBindings(this);
        }

        protected IMvxAndroidBindingContext BindingContext
        {
            get { return _bindingContext; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearBindings();
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