// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Tvos.Binding;
using MvvmCross.Platforms.Tvos.Presenters;
using MvvmCross.Platforms.Tvos.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Tvos.Core
{
#nullable enable
    public abstract class MvxTvosSetup
        : MvxSetup, IMvxTvosSetup
    {
        private IMvxApplicationDelegate? _applicationDelegate;
        private UIWindow? _window;

        private IMvxTvosViewPresenter? _presenter;

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter)
        {
            _presenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected UIWindow? Window => _window;

        protected IMvxApplicationDelegate? ApplicationDelegate => _applicationDelegate;

        protected sealed override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        {
            var container = CreateTvosViewsContainer();
            RegisterTvosViewCreator(iocProvider, container);
            return container;
        }

        protected virtual IMvxTvosViewsContainer CreateTvosViewsContainer()
        {
            return new MvxTvosViewsContainer();
        }

        protected virtual void RegisterTvosViewCreator(IMvxIoCProvider iocProvider, IMvxTvosViewsContainer container)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<IMvxTvosViewCreator>(container);
            iocProvider.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxTvosViewDispatcher(Presenter);
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            RegisterPlatformProperties(iocProvider);
            RegisterPresenter(iocProvider);
            RegisterLifetime(iocProvider);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void RegisterPlatformProperties(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<IMvxTvosSystem>(CreateTvosSystemProperties());
        }

        protected virtual MvxTvosSystem CreateTvosSystemProperties()
        {
            return new MvxTvosSystem();
        }

        protected virtual void RegisterLifetime(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            if (_applicationDelegate == null)
                throw new InvalidOperationException("Cannot register lifetime with null ApplicationDelegate");

            iocProvider.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxTvosViewPresenter Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxTvosViewPresenter CreateViewPresenter()
        {
            return new MvxTvosViewPresenter(_applicationDelegate, _window);
        }

        protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var presenter = Presenter;
            iocProvider.RegisterSingleton(presenter);
            iocProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxTvosBindingBuilder(FillTargetFactories, FillValueConverters, FillBindingNames,
                FillValueCombiners);
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
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

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }
    }

    public abstract class MvxTvosSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxTvosSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp(IMvxIoCProvider iocProvider) =>
            iocProvider.IoCConstruct<TApplication>();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
#nullable restore
}
