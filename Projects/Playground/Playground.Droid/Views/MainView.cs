using Android.App;
using Android.Content.PM;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Views.InputMethods;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Droid.ViewModels;

namespace Playground.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "Star Wars",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "playground.droid.views.MainView"
        )]
    public class MainView : MvxAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Playground.Droid.Resource.Layout.MainView);

            if (bundle == null)
            {
                ViewModel.ShowFirstViewModelCommand.Execute(null);
            }
        }
    }
}

