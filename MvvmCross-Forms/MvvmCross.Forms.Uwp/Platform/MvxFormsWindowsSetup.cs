using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using MvvmCross.Binding;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Platform;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Plugins;
using XamlControls = Windows.UI.Xaml.Controls;
using MvvmCross.Binding.Bindings.Target.Construction;

namespace MvvmCross.Forms.Uwp
{
    public abstract class MvxFormsWindowsSetup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;
        private List<Assembly> _viewAssemblies;

        public MvxFormsWindowsSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e)
            : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
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

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                {
                    Xamarin.Forms.Forms.Init(_launchActivatedEventArgs);
                    _formsApplication = _formsApplication ?? CreateFormsApplication();
                }
                return _formsApplication;
            }
        }

        protected virtual MvxFormsApplication CreateFormsApplication() => new MvxFormsApplication();

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            var presenter = new MvxFormsUwpPagePresenter(rootFrame, FormsApplication);
            Mvx.RegisterSingleton<IMvxFormsViewPresenter>(presenter);
            return presenter;
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

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsBindingBuilder();
    }
}
