using System;
using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.Droid.RecyclerView
{
    public class MvxViewHolder : Android.Support.V7.Widget.RecyclerView.ViewHolder, IMvxBindingContextOwner
    {
        private readonly IMvxBindingContext _bindingContext;

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the view holder"); }
        }

        public object DataContext
        {
            get { return _bindingContext.DataContext; }
            set { _bindingContext.DataContext = value; }
        }

        public MvxViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView)
        {
            this._bindingContext = context;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bindingContext.ClearAllBindings();
            }

            base.Dispose(disposing);
        }
    }
}