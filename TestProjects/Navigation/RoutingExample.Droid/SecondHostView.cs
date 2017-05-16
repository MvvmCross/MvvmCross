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
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Content.PM;

namespace RoutingExample.Droid
{
    [Activity(
        Label = "Example", 
        MainLauncher = true, 
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Icon = "@mipmap/icon")]
    [IntentFilter(new[] { Intent.ActionView }, DataScheme = "mvx",
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable })]
    [IntentFilter(new[] { Intent.ActionView }, DataScheme = "https", DataHost = "mvvmcross.com",
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable })]
    public class SecondHostView : MvxCachingFragmentCompatActivity<SecondHostViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.secondhost);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if(!Mvx.CanResolve<IMvxNavigationService>()) return;

            var url = WebUtility.UrlDecode(intent.DataString);

            Mvx.Resolve<IMvxNavigationService>().Navigate(url);
        }
    }
}

