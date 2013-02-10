using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
#warning Move to better namespace?
    public static class MvxBindingOwnerExtensions
    {
        public static void RegisterBindingsFor(this IMvxBindingOwner owner, View view)
        {
            owner.BindingOwnerHelper.RegisterBindingsFor(view);
        }

        public static void RegisterBinding(this IMvxBindingOwner owner, IMvxBinding binding)
        {
            owner.BindingOwnerHelper.RegisterBinding(binding);
        }

        public static void ClearBindings(this IMvxBindingOwner owner, View view)
        {
            owner.BindingOwnerHelper.ClearBindings(view);
        }

        public static void ClearAllBindings(this IMvxBindingOwner owner)
        {
            owner.BindingOwnerHelper.ClearAllBindings();
        }

        public static View BindingInflate(this IMvxBindingOwner owner, int resourceId, ViewGroup viewGroup)
        {
            return owner.BindingOwnerHelper.BindingInflate(resourceId, viewGroup);
        }

        public static View BindingInflate(this IMvxBindingOwner owner, object source, int resourceId, ViewGroup viewGroup)
        {
            return owner.BindingOwnerHelper.BindingInflate(source, resourceId, viewGroup);
        }
    }
}