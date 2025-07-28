// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Ios.Core;

public abstract class MvxIosSetup
    : MvxSetup, IMvxIosSetup
{
    protected IMvxLifetime? LifetimeInstance { get; private set; }
    protected UIWindow? Window { get; private set; }

    private IMvxIosViewPresenter? _presenter;

    public virtual void PlatformInitialize(IMvxLifetime lifetimeInstance, UIWindow window)
    {
        Window = window;
        LifetimeInstance = lifetimeInstance;
    }

    public virtual void PlatformInitialize(IMvxLifetime lifetimeInstance, IMvxIosViewPresenter presenter)
    {
        LifetimeInstance = lifetimeInstance;
        _presenter = presenter;
    }

    protected sealed override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
    {
        var container = CreateIosViewsContainer();
        RegisterIosViewCreator(iocProvider, container);
        return container;
    }

    protected virtual IMvxIosViewsContainer CreateIosViewsContainer()
    {
        return new MvxIosViewsContainer();
    }

    protected virtual void RegisterIosViewCreator(IMvxIoCProvider iocProvider, IMvxIosViewsContainer container)
    {
        ValidateArguments(iocProvider);

        iocProvider.RegisterSingleton<IMvxIosViewCreator>(container);
        iocProvider.RegisterSingleton<IMvxCurrentRequest>(container);
    }

    protected override IMvxViewDispatcher CreateViewDispatcher()
    {
        return new MvxIosViewDispatcher(Presenter);
    }

    protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
    {
        RegisterPlatformProperties(iocProvider);
        RegisterPresenter(iocProvider);
        RegisterPopoverPresentationSourceProvider(iocProvider);
        RegisterLifetime(iocProvider);
        base.InitializeFirstChance(iocProvider);
    }

    protected virtual void RegisterPlatformProperties(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        iocProvider.RegisterSingleton<IMvxIosSystem>(CreateIosSystemProperties());
    }

    protected virtual MvxIosSystem CreateIosSystemProperties()
    {
        return new MvxIosSystem();
    }

    protected virtual void RegisterLifetime(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        if (LifetimeInstance == null)
        {
            SetupLog?.LogError(
                "ApplicationDelegate is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
                nameof(RegisterLifetime), nameof(PlatformInitialize));
            return;
        }

        iocProvider.RegisterSingleton<IMvxLifetime>(LifetimeInstance);
    }

    protected IMvxIosViewPresenter? Presenter
    {
        get
        {
            _presenter ??= CreateViewPresenter();
            return _presenter;
        }
    }

    protected virtual IMvxIosViewPresenter? CreateViewPresenter()
    {
        if (Window == null)
        {
            SetupLog?.LogError(
                "Window is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
                nameof(CreateViewPresenter), nameof(PlatformInitialize));
            return null;
        }

        return new MvxIosViewPresenter(Window);
    }

    protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        if (Presenter == null)
        {
            SetupLog?.LogError("Presenter is null in {MethodName}. Make sure to call {CreatePresenterMethodName}",
                nameof(RegisterPresenter), nameof(CreateViewPresenter));

            return;
        }

        var presenter = Presenter;
        iocProvider.RegisterSingleton(presenter);
        iocProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
    }

    protected virtual void RegisterPopoverPresentationSourceProvider(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        iocProvider.RegisterSingleton(CreatePopoverPresentationSourceProvider());
    }

    protected virtual IMvxPopoverPresentationSourceProvider CreatePopoverPresentationSourceProvider()
    {
        return new MvxPopoverPresentationSourceProvider();
    }

    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
    {
        var bindingBuilder = CreateBindingBuilder();
        bindingBuilder.DoRegistration(iocProvider);
    }

    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    protected virtual MvxBindingBuilder CreateBindingBuilder()
    {
        return new MvxIosBindingBuilder(FillTargetFactories, FillValueConverters, FillValueCombiners,
            FillBindingNames);
    }

    protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
    {
        // this base class does nothing
    }

    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
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
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
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

public abstract class MvxIosSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxIosSetup
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
