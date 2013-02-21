using Android.Content;
using Android.Support.V4.App;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxFragmentBindingOwnerHelper : MvxBindingOwnerHelper
    {
        public MvxFragmentBindingOwnerHelper(Context context, FragmentActivity fragmentActivity, IMvxDataConsumer dataConsumer)
            : base(context, ToLayoutProvider(fragmentActivity), dataConsumer)
        {
        }

        private static IMvxLayoutInflaterProvider ToLayoutProvider(FragmentActivity fragmentActivity)
        {
            var provider = fragmentActivity as IMvxLayoutInflaterProvider;
            if (provider == null)
                throw new MvxException("Binding requires that incoming FragmentActivity must support IMvxLayoutInflaterProvider - incoming type is {0}", fragmentActivity.GetType().Name);

            return provider;
        }
    }
}