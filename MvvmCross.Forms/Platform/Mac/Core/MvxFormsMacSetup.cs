// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Localization;
using System.Collections.Generic;
using System.Reflection;
using AppKit;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.Mac.Bindings;
using MvvmCross.Platform.Mac.Core;
using MvvmCross.Platform.Mac.Presenters;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using MvvmCross.Forms.Platform.Mac.Presenters;
using MvvmCross.Forms.Presenters;
using Xamarin.Forms;
using System.Linq;

namespace MvvmCross.Forms.Platform.Mac.Core
{
    public abstract class MvxFormsMacSetup : MvxMacSetup
    {
        private List<Assembly> _viewAssemblies;
        private Application _formsApplication;

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

        public Application FormsApplication
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

        protected abstract Application CreateFormsApplication();

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

    public class MvxFormsMacSetup<TApplication, TFormsApplication> : MvxFormsMacSetup
        where TApplication : IMvxApplication, new()
        where TFormsApplication : Application, new()
    {
        protected MvxFormsMacSetup(IMvxApplicationDelegate applicationDelegate, NSWindow window) : base(applicationDelegate, window)
        {
        }

        protected MvxFormsMacSetup(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter) : base(applicationDelegate, presenter)
        {
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(TFormsApplication).GetTypeInfo().Assembly }));
        }

        protected override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }

        protected override Application CreateFormsApplication() => new TFormsApplication();

        protected override IMvxApplication CreateApp() => Mvx.IocConstruct<TApplication>();
    }
}
