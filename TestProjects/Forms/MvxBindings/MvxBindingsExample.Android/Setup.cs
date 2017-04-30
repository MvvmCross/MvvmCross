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

        protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<System.Reflection.Assembly>(base.ValueConverterAssemblies);
                toReturn.Add(typeof(MvxLanguageConverter).Assembly);
                return toReturn;
            }
        }

        protected override void InitializeBindingBuilder()
        {
            MvxBindingBuilder bindingBuilder = CreateBindingBuilder();

            this.RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        protected new MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }
    }
}