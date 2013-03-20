using Android.Content;
using Cirrious.CrossCore.IoC;
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

        protected override MvxApplication CreateApp()
        {
            var app =  new App();
            return app;
        }

        protected override IMvxNavigationRequestSerializer CreateNavigationRequestSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            var json = Mvx.Resolve<IMvxJsonConverter>();
            return new MvxNavigationRequestSerializer(json);
        }

        protected override void InitializeLastChance()
        {
            var autoViewSetup = new MvxAutoViewSetup();
            autoViewSetup.Initialize(typeof(Resource.Layout));

            var closer = new SimpleDroidViewModelCloser();
            Mvx.RegisterSingleton<IViewModelCloser>(closer);

            base.InitializeLastChance();
        }
    }
}