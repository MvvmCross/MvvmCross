﻿// Licensed to the .NET Foundation under one or more agreements.
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
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Uap.Core
{
    public abstract class MvxFormsWindowsSetup : MvxWindowsSetup
    {
        private Application _formsApplication;
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
