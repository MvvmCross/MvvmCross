using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Droid.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvxBindingsExample.Droid
{
    public class Setup : MvxAndroidSetup
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

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsDroidPagePresenter();
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }

        protected override void InitializeBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();

            RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        protected new MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }
    }
}