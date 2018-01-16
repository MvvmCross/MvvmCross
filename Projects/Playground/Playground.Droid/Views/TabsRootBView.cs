using System;
using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme")]
    public class TabsRootBView : MvxAppCompatActivity<TabsRootBViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TabsRootBView);

            if (bundle == null)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute();
            }
        }
    }
}
