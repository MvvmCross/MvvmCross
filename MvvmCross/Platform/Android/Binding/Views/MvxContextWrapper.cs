using System;
using Android.Content;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace MvvmCross.Binding.Droid.Views
{
    public class MvxContextWrapper : ContextWrapper
    {
        private LayoutInflater _inflater;
        private readonly IMvxBindingContextOwner _bindingContextOwner;

        public static ContextWrapper Wrap(Context @base, IMvxBindingContextOwner bindingContextOwner)
        {
            return new MvxContextWrapper(@base, bindingContextOwner);
        }

        protected MvxContextWrapper(Context context, IMvxBindingContextOwner bindingContextOwner)
            : base(context)
        {
            if (bindingContextOwner == null)
                throw new InvalidOperationException("Wrapper can only be set on IMvxBindingContextOwner");

            _bindingContextOwner = bindingContextOwner;
        }

        public override Object GetSystemService(string name)
        {
            if (name.Equals(LayoutInflaterService))
            {
                return _inflater ??
                       (_inflater =
                           new MvxLayoutInflater(LayoutInflater.From(BaseContext), this, null, false));
            }

            return base.GetSystemService(name);
        }
    }
}