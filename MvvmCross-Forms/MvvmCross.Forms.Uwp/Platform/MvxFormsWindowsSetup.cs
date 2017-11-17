using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Uwp.Bindings;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Forms.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using XamlControls = Windows.UI.Xaml.Controls;

namespace MvvmCross.Forms.Uwp
{
    public abstract class MvxFormsWindowsSetup<TForms> : MvxWindowsSetup
        where TForms : MvxFormsApplication, new()
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;
        private List<Assembly> _viewAssemblies;
        private TForms _formsApplication;

        public MvxFormsWindowsSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e)
            : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
            {
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(TForms).GetTypeInfo().Assembly }));
            }

            return _viewAssemblies;
        }

        protected override void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            base.InitializeApp(pluginManager, app);
            _viewAssemblies.AddRange(GetViewModelAssemblies());
        }

        public TForms FormsApplication
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

        protected virtual TForms CreateFormsApplication() => new TForms();

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            var presenter = new MvxFormsUwpViewPresenter(rootFrame, FormsApplication);
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

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsWindowsBindingBuilder();
    }
}
