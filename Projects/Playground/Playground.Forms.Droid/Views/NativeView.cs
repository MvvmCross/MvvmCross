using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme",
    WindowSoftInputMode = SoftInput.AdjustPan)]
    public class NativeView : MvxAppCompatActivity<NativeViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.NativeView);
        }
    }
}
