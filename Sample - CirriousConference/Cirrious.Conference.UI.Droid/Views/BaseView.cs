using Android.Views;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Android.Views;

namespace Cirrious.Conference.UI.Droid.Views
{
    public abstract class BaseView<TViewModel> 
        : MvxBindingActivityView<TViewModel>
        , IBaseView<TViewModel>
        where TViewModel : BaseViewModel
    {
        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            this.SetBackground();
        }
    }
}