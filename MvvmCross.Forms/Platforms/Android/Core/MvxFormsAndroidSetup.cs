// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platforms.Android.Bindings;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Android;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.Platforms.Android.Presenters;
using Xamarin.Forms;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Core;

namespace MvvmCross.Forms.Platforms.Android.Core
{
    public abstract class MvxFormsAndroidSetup : MvxAppCompatSetup, IMvxFormsSetup
    {
        private List<Assembly> _viewAssemblies;
        private Application _formsApplication;

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
            {
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies());
            }

            return _viewAssemblies;
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            Mvx.IoCProvider.RegisterSingleton<IMvxFormsSetup>(this);
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
                {
                    var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity ?? ApplicationContext;
                    var asmb = activity.GetType().Assembly;
                    Xamarin.Forms.Forms.Init(activity, null, ExecutableAssembly ?? asmb);
                }
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

        protected virtual IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPagePresenter = new MvxFormsPagePresenter(viewPresenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxFormsPagePresenter>(formsPagePresenter);
            return formsPagePresenter;
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsAndroidViewPresenter(GetViewAssemblies(), FormsApplication);
            Mvx.IoCProvider.RegisterSingleton<IMvxFormsViewPresenter>(presenter);
            presenter.FormsPagePresenter = CreateFormsPagePresenter(presenter);
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

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Activity", "Fragment", "Page");
        }
    }

    public class MvxFormsAndroidSetup<TApplication, TFormsApplication> : MvxFormsAndroidSetup
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
