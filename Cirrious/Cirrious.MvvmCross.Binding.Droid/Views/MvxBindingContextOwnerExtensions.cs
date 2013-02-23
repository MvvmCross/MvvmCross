using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public interface IMvxBindingContextOwner
    {
        IMvxBindingContext BindingContext { get; set; }
    }

    public static class MvxBindingContextOwnerExtensions
    {
        public static void RegisterBindingsFor(this IMvxBindingContextOwner owner, View view)
        {
            owner.BindingContext.RegisterBindingsFor(view);
        }

        public static void RegisterBinding(this IMvxBindingContextOwner owner, IMvxBinding binding)
        {
            owner.BindingContext.RegisterBinding(binding);
        }

        public static void ClearBindings(this IMvxBindingContextOwner owner, View view)
        {
            owner.BindingContext.ClearBindings(view);
        }

        public static void ClearAllBindings(this IMvxBindingContextOwner owner)
        {
            owner.BindingContext.ClearAllBindings();
        }

        public static View BindingInflate(this IMvxBindingContextOwner owner, int resourceId, ViewGroup viewGroup)
        {
            return owner.BindingContext.BindingInflate(resourceId, viewGroup);
        }

        public static View BindingInflate(this IMvxBindingContextOwner owner, object source, int resourceId, ViewGroup viewGroup)
        {
            return owner.BindingContext.BindingInflate(source, resourceId, viewGroup);
        }
    }
}