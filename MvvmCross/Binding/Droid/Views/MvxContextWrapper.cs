namespace MvvmCross.Binding.Droid.Views
{
    using System;

    using Android.Content;
    using Android.Views;

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

            this._bindingContextOwner = bindingContextOwner;
        }

        public override Java.Lang.Object GetSystemService(string name)
        {
            if (name.Equals(LayoutInflaterService))
            {
                return this._inflater ??
                       (this._inflater =
                           new MvxLayoutInflater(LayoutInflater.From(this.BaseContext), this, null, false));
            }

            return base.GetSystemService(name);
        }
    }
}