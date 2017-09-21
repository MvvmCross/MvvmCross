using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Droid.Views;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Plugins;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings.Target;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Droid.Bindings;

namespace MvvmCross.Forms.Droid.Platform
{
    public abstract class MvxFormsAndroidSetup : MvxAndroidSetup
    {
        private List<Assembly> _viewAssemblies;
        private MvxFormsApplication _formsApplication;

        public MvxFormsAndroidSetup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies());

            return _viewAssemblies;
        }

        protected override void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            base.InitializeApp(pluginManager, app);
            _viewAssemblies.AddRange(GetViewModelAssemblies());
        }

        public MvxFormsApplication FormsApplication
        {
            get
            {
                if (!Xamarin.Forms.Forms.IsInitialized)
                    Xamarin.Forms.Forms.Init(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, null);
                if (_formsApplication == null)
                    _formsApplication = CreateFormsApplication();
                return _formsApplication;
            }
        }

        protected virtual MvxFormsApplication CreateFormsApplication() => new MvxFormsApplication();

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsAndroidViewPresenter(GetViewAssemblies(), FormsApplication);
            Mvx.RegisterSingleton<IMvxFormsViewPresenter>(presenter);
            return presenter;
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

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxFormsSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        protected override void FillBindingNames(Binding.BindingContext.IMvxBindingNameRegistry registry)
        {
            MvxFormsSetupHelper.FillBindingNames(registry);
            base.FillBindingNames(registry);
        }

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsAndroidBindingBuilder();
    }
}
