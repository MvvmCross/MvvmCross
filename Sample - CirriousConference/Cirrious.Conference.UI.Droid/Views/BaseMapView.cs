using Android.Content.Res;
using Android.Views;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Maps;

namespace Cirrious.Conference.UI.Droid.Views
{
    public abstract class BaseMapView<TViewModel>
        : MvxMapActivity
        , IBaseView<TViewModel>
    where TViewModel : BaseViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            //RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);

            var drawable = Resources.GetDrawable(Resource.Drawable.background);
            drawable.SetDither(true);
            Window.SetBackgroundDrawable(drawable);
        }

        protected override bool IsRouteDisplayed
        {
            get { return false; }
        }
    }
}