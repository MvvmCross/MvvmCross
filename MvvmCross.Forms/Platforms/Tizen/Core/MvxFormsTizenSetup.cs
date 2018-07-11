// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Presenters;
using MvvmCross.Localization;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platforms.Tizen.Bindings;
using MvvmCross.Platforms.Tizen.Core;
using MvvmCross.Platforms.Tizen.Presenters;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using MvvmCross.Forms.Platforms.Tizen.Presenters;
using Xamarin.Forms;
using System.Linq;
using MvvmCross.Presenters;
using MvvmCross.IoC;

namespace MvvmCross.Forms.Platforms.Tizen.Core
{
    public abstract class MvxFormsTizenSetup : MvxTizenSetup, IMvxFormsSetup
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
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxFormsTizenViewPresenter>();
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
                if (!Xamarin.Forms.Platform.Tizen.Forms.IsInitialized)
                    Xamarin.Forms.Platform.Tizen.Forms.Init(CoreApplication);

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

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsTizenBindingBuilder();

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }

    public class MvxFormsTizenSetup<TApplication, TFormsApplication> : MvxFormsTizenSetup
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
