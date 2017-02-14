using System;
using System.Net;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using RoutingExample.Core.ViewModels;
using MvvmCross.Core.Navigation;

namespace RoutingExample.Droid
{
    [Activity(Label = "Example", MainLauncher = true, Icon = "@drawable/icon")]
    [IntentFilter(new[] { Intent.ActionView }, DataScheme = "mvx",
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable })]
    [IntentFilter(new[] { Intent.ActionView }, DataScheme = "https", DataHost = "mvvmcross.com",
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable })]
    public class MainView : MvxActivity<MainViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if(!Mvx.CanResolve<IMvxNavigationService>()) return;

            var url = WebUtility.UrlDecode(intent.DataString);

            Mvx.Resolve<IMvxNavigationService>().RouteAsync(url);
        }
    }
}

