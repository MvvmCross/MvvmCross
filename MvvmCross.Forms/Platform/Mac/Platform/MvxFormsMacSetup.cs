// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Mac.Bindings;
using MvvmCross.Forms.Mac.Presenters;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.Mac.Platform;
using MvvmCross.Mac.Views.Presenters;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using System.Collections.Generic;
using System.Reflection;
using AppKit;

namespace MvvmCross.Forms.iOS
{
    public abstract class MvxFormsMacSetup : MvxMacSetup
    {
        private List<Assembly> _viewAssemblies;
        private MvxFormsApplication _formsApplication;

        protected MvxFormsMacSetup(IMvxApplicationDelegate applicationDelegate, NSWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected MvxFormsMacSetup(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
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
                if (!Xamarin.Forms.Forms.IsInitialized)
                    Xamarin.Forms.Forms.Init();
                if (_formsApplication == null)
                {
                    _formsApplication = _formsApplication ?? CreateFormsApplication();
                }
                return _formsApplication;
            }
        }

        protected abstract MvxFormsApplication CreateFormsApplication();

        protected override IMvxMacViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsMacViewPresenter(ApplicationDelegate, FormsApplication);
            Mvx.RegisterSingleton<IMvxFormsViewPresenter>(presenter);
            return presenter;
        }

        protected override List<Assembly> ValueConverterAssemblies
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

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsMacBindingBuilder();

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController", "Page");
        }
    }
}
