
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Xamarin.Forms;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Droid;
using MvvmCross.Forms.Droid.Presenters;

namespace Example.Droid
{
    [Activity(Label = "ExampleApplicationActivity", ScreenOrientation=ScreenOrientation.Portrait)]
    public class ExampleApplicationActivity : MvxFormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            var mvxFormsApp = new MvxFormsApplication();
            LoadApplication(mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}

