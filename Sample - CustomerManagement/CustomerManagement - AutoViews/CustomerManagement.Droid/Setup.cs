using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.AutoView.Droid;
using Cirrious.MvvmCross.Dialog.Droid;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using CustomerManagement.AutoViews.Core;
using CustomerManagement.AutoViews.Core.Interfaces;
using CustomerManagement.Droid;

namespace CustomerManagement.AutoViews.Droid
{
    public class Setup 
        : MvxAndroidDialogSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            var app =  new App();
            return app;
        }

        protected override void InitializeLastChance()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();

            var autoViewSetup = new MvxAutoViewSetup();
            autoViewSetup.Initialize(typeof(Resource.Layout));

            var closer = new SimpleDroidViewModelCloser();
            Mvx.RegisterSingleton<IViewModelCloser>(closer);

            base.InitializeLastChance();
        }
    }
}