using Android.Content.Res;
using Android.Views;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Android.Views;

namespace Cirrious.Conference.UI.Droid.Views
{
    public abstract class BaseMapView<TViewModel>
        : MvxBindingMapActivityView<TViewModel>
        , IBaseView<TViewModel>
    where TViewModel : BaseViewModel
    {
        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            //RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            var drawable = Resources.GetDrawable(Resource.Drawable.background);
            drawable.SetDither(true);
            Window.SetBackgroundDrawable(drawable);
        }
    }
}