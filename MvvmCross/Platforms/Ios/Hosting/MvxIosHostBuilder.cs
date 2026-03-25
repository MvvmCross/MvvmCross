// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platforms.Ios.Hosting;

/// <summary>
/// MvvmCross host builder for iOS/tvOS/MacCatalyst.
/// </summary>
public class MvxIosHostBuilder : MvxHostBuilder
{
    private Action<IMvxTargetBindingFactoryRegistry> _fillTargetFactories = _ => { };
    private Action<IMvxValueConverterRegistry> _fillValueConverters = _ => { };
    private Action<IMvxValueCombinerRegistry> _fillValueCombiners = _ => { };
    private Action<IMvxBindingNameRegistry> _fillBindingNames = _ => { };

    /// <summary>
    /// Creates an <see cref="MvxIosHostBuilder"/> with the given <see cref="UIWindow"/>.
    /// The window is registered as a singleton so the view presenter can resolve it.
    /// </summary>
    public static MvxIosHostBuilder CreateBuilder(UIWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);
        return new MvxIosHostBuilder(window);
    }

    /// <summary>
    /// Registers custom target binding factories (equivalent to overriding
    /// <c>FillTargetFactories</c> in the old <c>MvxIosSetup</c>).
    /// </summary>
    public MvxIosHostBuilder ConfigureTargetBindings(Action<IMvxTargetBindingFactoryRegistry> configure)
    {
        var previous = _fillTargetFactories;
        _fillTargetFactories = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Registers additional value converters.
    /// </summary>
    public MvxIosHostBuilder ConfigureValueConverters(Action<IMvxValueConverterRegistry> configure)
    {
        var previous = _fillValueConverters;
        _fillValueConverters = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Registers additional value combiners.
    /// </summary>
    public MvxIosHostBuilder ConfigureValueCombiners(Action<IMvxValueCombinerRegistry> configure)
    {
        var previous = _fillValueCombiners;
        _fillValueCombiners = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Registers additional default binding names.
    /// </summary>
    public MvxIosHostBuilder ConfigureBindingNames(Action<IMvxBindingNameRegistry> configure)
    {
        var previous = _fillBindingNames;
        _fillBindingNames = registry => { previous(registry); configure(registry); };
        return this;
    }

    private MvxIosHostBuilder(UIWindow window)
    {
        Services.TryAddSingleton(window);

        // View presenter (wraps UIWindow for navigation)
        Services.TryAddSingleton<IMvxIosViewPresenter>(new MvxIosViewPresenter(window));
        Services.TryAddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxIosViewPresenter>());

        // View dispatcher — also provides IMvxMainThreadDispatcher / IMvxMainThreadAsyncDispatcher
        // via MvxIosUIThreadDispatcher → MvxMainThreadAsyncDispatcher → MvxMainThreadDispatcher
        Services.TryAddSingleton<MvxIosViewDispatcher>();
        Services.TryAddSingleton<IMvxViewDispatcher>(
            sp => sp.GetRequiredService<MvxIosViewDispatcher>());
        Services.TryAddSingleton<IMvxMainThreadAsyncDispatcher>(
            sp => sp.GetRequiredService<MvxIosViewDispatcher>());
        Services.TryAddSingleton<IMvxMainThreadDispatcher>(
            sp => sp.GetRequiredService<MvxIosViewDispatcher>());

        // Views container — populated lazily from all IMvxIosViewRegistration entries
        Services.TryAddSingleton<MvxIosViewsContainer>(sp =>
        {
            var container = new MvxIosViewsContainer();
            var typeFinder = sp.GetService<IMvxViewModelTypeFinder>();
            foreach (var reg in sp.GetServices<IMvxIosViewRegistration>())
                reg.Apply(container, typeFinder);
            return container;
        });
        Services.TryAddSingleton<IMvxIosViewsContainer>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
        Services.TryAddSingleton<IMvxViewsContainer>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
        Services.TryAddSingleton<IMvxIosViewCreator>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
        Services.TryAddSingleton<IMvxCurrentRequest>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
    }

    /// <inheritdoc/>
    public override MvxHost Build()
    {
        // Binding registrations are deferred to Build() so that Configure* calls are included.
        var bindingBuilder = new MvxIosBindingBuilder(
            fillRegistryAction: _fillTargetFactories,
            fillValueConvertersAction: _fillValueConverters,
            fillValueCombinersAction: _fillValueCombiners,
            fillBindingNamesAction: _fillBindingNames);
#pragma warning disable IL2026
        bindingBuilder.DoRegistration(Services);
#pragma warning restore IL2026
        return base.Build();
    }
}

