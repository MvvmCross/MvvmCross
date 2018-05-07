// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Droid.Platform;
using MvvmCross.Forms.Platform;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Zero.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterSingleton<Core.Services.ILocalizeService>(() => new Services.LocalizeService());
            Mvx.RegisterSingleton<ISettings>(() => CrossSettings.Current);
        }

        protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.None;

        protected override MvxFormsApplication CreateFormsApplication()
        {
            return new Core.FormsApp();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.MvxApp();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new Core.DebugTrace();
        }
    }
}