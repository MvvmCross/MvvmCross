using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Droid.Presenters;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Forms.Droid
{
    public abstract class MvxFormsAndroidSetup : MvxAndroidSetup
    {
        public MvxFormsAndroidSetup(Context applicationContext) : base(applicationContext)
        {
        }

        private List<Assembly> viewAssemblies;
        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (viewAssemblies == null)
                viewAssemblies = new List<Assembly>(base.GetViewAssemblies());

            return viewAssemblies;
        }

        protected override void InitializeApp(Platform.Plugins.IMvxPluginManager pluginManager)
        {
            base.InitializeApp(pluginManager);
            viewAssemblies.AddRange(GetViewModelAssemblies());
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                    _formsApplication = CreateFormsApplication();
                return _formsApplication;
            }
        }

        protected virtual MvxFormsApplication CreateFormsApplication()
        {
            return new MvxFormsApplication();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            global::Xamarin.Forms.Forms.Init(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, null);
            return new MvxFormsDroidPagePresenter(FormsApplication);
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>(base.ValueConverterAssemblies)
                {
                    typeof(MvxLanguageConverter).Assembly
                };
                return toReturn;
            }
        }

        protected override void InitializeBindingBuilder()
        {
            MvxBindingBuilder bindingBuilder = CreateBindingBuilder();

            RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsBindingBuilder();
    }
}
