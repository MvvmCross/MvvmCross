using System.Collections.Generic;
using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Localization;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Droid;
using MvvmCross.Forms.Droid.Presenters;

namespace MvxBindingsExample.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}