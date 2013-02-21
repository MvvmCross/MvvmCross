using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
   
namespace Cirrious.MvvmCross.Droid.Fragging
{
    public abstract class MvxFragment
        : Fragment
        , IMvxAndroidFragmentView
    {
        public IMvxBindingOwnerHelper BindingOwnerHelper { get; private set; }

        public object DataContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            BindingOwnerHelper = new MvxFragmentBindingOwnerHelper(Activity, Activity, this);
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        protected virtual void OnViewModelSet()
        {
        }

        public override void OnDestroyView()
        {
            BindingOwnerHelper.ClearAllBindings();
            base.OnDestroyView();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
