// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Views;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.Uap.Bindings;
using MvvmCross.Forms.Platform.Uap.Views;
using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views;
using MvvmCross.Plugins;
using MvvmCross.ViewModels;
using XamlControls = Windows.UI.Xaml.Controls;

namespace MvvmCross.Forms.Platform.Uap.Core
{
    public abstract class MvxFormsWindowsSetup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;
        private List<Assembly> _viewAssemblies;
        private MvxFormsApplication _formsApplication;

        protected MvxFormsWindowsSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e)
            : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
            {
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies());
            }

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
                if (_formsApplication == null)
                {
                    Xamarin.Forms.Forms.Init(_launchActivatedEventArgs);
                    _formsApplication = _formsApplication ?? CreateFormsApplication();
                }
                return _formsApplication;
            }
        }

        protected abstract MvxFormsApplication CreateFormsApplication();

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
