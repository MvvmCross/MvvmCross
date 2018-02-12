// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Views;
using MvvmCross.Localization;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.Android.Bindings;
using MvvmCross.Forms.Platform.Android.Views;
using MvvmCross.Platform.Android.Core;
using MvvmCross.Platform.Android.Presenters;
using MvvmCross.Plugins;
using MvvmCross.ViewModels;
using MvvmCross.Platform.Android;

namespace MvvmCross.Forms.Platform.Android.Core
{
    public abstract class MvxFormsAndroidSetup : MvxAndroidSetup
    {
        private List<Assembly> _viewAssemblies;
        private MvxFormsApplication _formsApplication;

        protected MvxFormsAndroidSetup(Context applicationContext) : base(applicationContext)
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
                {
                    var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity ?? ApplicationContext;
                    Xamarin.Forms.Forms.Init(activity, null, activity.GetType().Assembly);
                }

                if (_formsApplication == null)
                {
                    _formsApplication = _formsApplication ?? CreateFormsApplication();
                }
                return _formsApplication;
            }
        }

        protected abstract MvxFormsApplication CreateFormsApplication();

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

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Activity", "Fragment", "Page");
        }
    }    
}
