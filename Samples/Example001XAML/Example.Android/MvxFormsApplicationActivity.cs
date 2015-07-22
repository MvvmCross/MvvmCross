
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
using Android.Content.PM;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Cirrious.MvvmCross.Forms.Presenter.Core;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Forms.Presenter.Droid;
using Cirrious.MvvmCross.ViewModels;

namespace Example.Droid
{
    [Activity(Label = "MvxFormsApplicationActivity", ScreenOrientation=ScreenOrientation.Portrait)]
    public class MvxFormsApplicationActivity
        : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            var mvxFormsApp = new MvxFormsApp();
            LoadApplication(mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}

