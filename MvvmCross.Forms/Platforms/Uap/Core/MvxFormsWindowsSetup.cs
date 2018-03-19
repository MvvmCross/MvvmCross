﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Presenters;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platforms.Uap.Bindings;
using MvvmCross.Forms.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using XamlControls = Windows.UI.Xaml.Controls;
using MvvmCross.Platforms.Uap.Presenters;
using System.Linq;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Uap.Core
{
    public abstract class MvxFormsWindowsSetup : MvxWindowsSetup
    {
        private List<Assembly> _viewAssemblies;
        private Application _formsApplication;

        /// <summary>
        /// Override to provide list of assemblies to search for views. 
        /// Additionally for UWP .NET Native compilation include the assemblies containing custom controls and renderers to be passed to <see cref="Xamarin.Forms.Forms.Init" /> method. 
        /// </summary>
        /// <returns>Custom view and renderer assemblies</returns>
        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return _viewAssemblies ?? (_viewAssemblies = new List<Assembly>(base.GetViewAssemblies()));
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
                    Xamarin.Forms.Forms.Init(ActivationArguments, GetViewAssemblies());

                if (_formsApplication == null)
                {
                    _formsApplication = CreateFormsApplication();
                }
                if(Application.Current != _formsApplication)
                {
                    Application.Current = _formsApplication;
                }
                return _formsApplication;
            }
        }

        protected abstract Application CreateFormsApplication();

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

    public class MvxFormsWindowsSetup<TApplication, TFormsApplication> : MvxFormsWindowsSetup
        where TApplication : IMvxApplication, new()
        where TFormsApplication : Application, new()        
    {
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
