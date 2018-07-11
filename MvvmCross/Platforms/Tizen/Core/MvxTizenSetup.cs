// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Tizen.Binding;
using MvvmCross.Platforms.Tizen.Presenters;
using MvvmCross.Platforms.Tizen.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
{
    public abstract class MvxTizenSetup
        : MvxSetup, IMvxTizenSetup
    {
        protected CoreApplication CoreApplication { get; private set; }

        public virtual void PlatformInitialize(CoreApplication coreApplication)
        {
            CoreApplication = coreApplication;
        }

        protected IMvxTizenViewPresenter Presenter
        {
            get
            {
                return base.ViewPresenter as IMvxTizenViewPresenter;
            }
        }

        protected override void RegisterViewPresenter()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxTizenViewPresenter>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter => Mvx.IoCProvider.RegisterSingleton((IMvxTizenViewPresenter)presenter));
        }

        protected override void RegisterViewsContainer()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewsContainer, MvxTizenViewsContainer>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewsContainer>(container => Mvx.IoCProvider.RegisterSingleton((IMvxTizenViewsContainer)container));
        }

        protected override void RegisterViewDispatcher()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxTizenViewDispatcher>();
        }

        protected override void InitializeLastChance()
        {
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxTizenBindingBuilder();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View");
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }

    public class MvxTizenSetup<TApplication> : MvxTizenSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterApp()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxApplication, TApplication>();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
