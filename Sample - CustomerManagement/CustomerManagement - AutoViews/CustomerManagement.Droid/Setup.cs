using Android.Content;
using Cirrious.MvvmCross.AutoView.Droid;
using Cirrious.MvvmCross.Dialog.Droid;
using Cirrious.MvvmCross.Application;
using CustomerManagement.AutoViews.Core;

namespace CustomerManagement.AutoViews.Droid
{
    public class Setup 
        : MvxBaseAndroidDialogBindingSetup
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

        protected override void InitializeLastChance()
        {
            var autoViewSetup = new MvxAutoViewSetup();
            autoViewSetup.Initialize(typeof(Resource.Layout));
            base.InitializeLastChance();
        }
    }
}