using Android.Content;
using Cirrious.MvvmCross.AutoView.Droid;
using Cirrious.MvvmCross.Dialog.Droid;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.Views;
using CustomerManagement.AutoViews.Core;

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

        protected override IMvxShowViewModelRequestSerializer CreateShowViewModelRequestSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            var json = this.GetService<IMvxJsonConverter>();
            return new MvxShowViewModelRequestSerializer(json);
        }

        protected override void InitializeLastChance()
        {
            var autoViewSetup = new MvxAutoViewSetup();
            autoViewSetup.Initialize(typeof(Resource.Layout));
            base.InitializeLastChance();
        }
    }
}