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
using MvvmCross.Forms.Platforms.Mac.Bindings;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using MvvmCross.Forms.Platforms.Mac.Presenters;
using MvvmCross.Forms.Presenters;
using Xamarin.Forms;
using System.Linq;
using MvvmCross.Presenters;
using MvvmCross.IoC;

namespace MvvmCross.Forms.Platforms.Mac.Core
{
    public abstract class MvxFormsMacSetup : MvxMacSetup, IMvxFormsSetup
    {
        private List<Assembly> _viewAssemblies;
        private Application _formsApplication;
        private IMvxFormsSetupHelper _formsSetupHelper;

        public virtual IMvxFormsSetupHelper FormsSetupHelper
        {
            get
            {
                return _formsSetupHelper ?? (_formsSetupHelper = Mvx.IoCProvider.Resolve<IMvxFormsSetupHelper>());
            }
        }

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
            {
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies());
            }

            return _viewAssemblies;
        }

        protected override void RegisterImplementations()
        {
            base.RegisterImplementations();
            Mvx.IoCProvider.RegisterSingleton<IMvxFormsSetup>(this);

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxFormsSetupHelper, MvxFormsSetupHelper>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxFormsPagePresenter, MvxFormsPagePresenter>();
        }

        protected override void RegisterViewPresenter()
        {
            base.RegisterViewPresenter();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxFormsMacViewPresenter>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter =>
            {
                if (presenter is IMvxFormsViewPresenter formsPresenter)
                    FormsSetupHelper.InitializeFormsViewPresenter(formsPresenter, FormsApplication);
            });
        }

        protected abstract void RegisterFormsApp();

        protected override void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            base.InitializeApp(pluginManager, app);
            _viewAssemblies.AddRange(GetViewModelAssemblies());
        }

        public virtual Application FormsApplication
        {
            get
            {
                if (!Xamarin.Forms.Forms.IsInitialized)
                    Xamarin.Forms.Forms.Init();

                return FormsSetupHelper.FormsApplication;
            }
        }

        protected IMvxFormsViewPresenter FormsPresenter
        {
            get
            {
                return base.ViewPresenter as IMvxFormsViewPresenter;
            }
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
            FormsSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        protected override void FillBindingNames(Binding.BindingContext.IMvxBindingNameRegistry registry)
        {
            FormsSetupHelper.FillBindingNames(registry);
            base.FillBindingNames(registry);
        }

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsMacBindingBuilder();

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController", "Page");
        }
    }

    public class MvxFormsMacSetup<TApplication, TFormsApplication> : MvxFormsMacSetup
        where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()
    {
        protected override void RegisterApp()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxApplication, TApplication>();
        }

        protected override void RegisterFormsApp()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<Application, TFormsApplication>();
        }

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(TFormsApplication).GetTypeInfo().Assembly }));
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
