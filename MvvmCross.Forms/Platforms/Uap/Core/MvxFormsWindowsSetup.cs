// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platforms.Uap.Bindings;
using MvvmCross.Forms.Platforms.Uap.Presenters;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Plugin;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Uap.Core
{
    public abstract class MvxFormsWindowsSetup : MvxWindowsSetup, IMvxFormsSetup
    {
        private List<Assembly> _viewAssemblies;
        private Application _formsApplication;
        private IMvxFormsSetupHelper _formsSetupHelper;

        public virtual IMvxFormsSetupHelper FormsSetupHelper
        {
            get
            {
                return _formsSetupHelper ?? (_formsSetupHelper = Mvx.Resolve<IMvxFormsSetupHelper>());
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
            Mvx.RegisterSingleton<IMvxFormsSetup>(this);

            Mvx.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxFormsUwpViewPresenter>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsSetupHelper, MvxFormsSetupHelper>();
            Mvx.Resolve<IMvxFormsSetupHelper>().InitializeIoC();
            Mvx.LazyConstructAndRegisterSingleton(() => FormsPresenter);
        }

        protected virtual void RegisterSetupHelper()
        {
            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsSetupHelper, MvxFormsSetupHelper>();
        }

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
                    Xamarin.Forms.Forms.Init(ActivationArguments, GetViewAssemblies());

                if (_formsApplication == null)
                {
                    _formsApplication = CreateFormsApplication();
                }
                if (Application.Current != _formsApplication)
                {
                    Application.Current = _formsApplication;
                }
                return _formsApplication;
            }
        }


        protected abstract Application CreateFormsApplication();

        protected IMvxFormsViewPresenter FormsPresenter
        {
            get
            {
                return base.ViewPresenter as IMvxFormsViewPresenter;
            }
        }

        protected override IMvxViewPresenter CreateViewPresenter()
        {
            return FormsSetupHelper.SetupFormsViewPresenter(base.CreateViewPresenter() as IMvxFormsViewPresenter, FormsApplication);
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

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsWindowsBindingBuilder();
    }

    public class MvxFormsWindowsSetup<TApplication, TFormsApplication> : MvxFormsWindowsSetup
        where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()        
    {
        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(TFormsApplication).GetTypeInfo().Assembly }));
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }

        protected override Application CreateFormsApplication() => new TFormsApplication();

        protected override IMvxApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();
    }
}
