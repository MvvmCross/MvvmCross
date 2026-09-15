// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.ViewModels;

namespace MvvmCross.DependencyInjection;

/// <summary>
/// Options for configuring the MvvmCross framework via <see cref="MvxServiceCollectionExtensions.AddMvxCore"/>.
/// </summary>
public sealed class MvxOptions
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type? _appStartType;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type? _appStartViewModelType;

    /// <summary>
    /// Gets the assemblies that contain views and view-models to register.
    /// Add entries via <see cref="AddViewAssembly"/> or <see cref="StartWith{TViewModel}"/>.
    /// </summary>
    public List<System.Reflection.Assembly> ViewAssemblies { get; } = [];

    /// <summary>
    /// The concrete <see cref="IMvxAppStart"/> type to register, if set via <see cref="UseAppStart{TAppStart}"/>.
    /// </summary>
    internal Type? AppStartType => _appStartType;

    /// <summary>
    /// When set via <see cref="StartWith{TViewModel}"/>, the framework will use the default
    /// <see cref="MvxAppStart{TViewModel}"/> implementation targeting this ViewModel type.
    /// </summary>
    internal Type? AppStartViewModelType => _appStartViewModelType;

    /// <summary>
    /// Registers a custom <see cref="IMvxAppStart"/> implementation as the application entry point.
    /// </summary>
    public MvxOptions UseAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TAppStart>()
        where TAppStart : class, IMvxAppStart
    {
        _appStartType = typeof(TAppStart);
        _appStartViewModelType = null;
        return this;
    }

    /// <summary>
    /// Uses the default <see cref="MvxAppStart{TViewModel}"/> targeting <typeparamref name="TViewModel"/>
    /// as the application entry point.
    /// </summary>
    public MvxOptions StartWith<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>()
        where TViewModel : IMvxViewModel
    {
        _appStartViewModelType = typeof(TViewModel);
        _appStartType = null;
        return this;
    }

    /// <summary>
    /// Adds an assembly to scan for view-to-viewmodel mappings.
    /// </summary>
    public MvxOptions AddViewAssembly(System.Reflection.Assembly assembly)
    {
        ViewAssemblies.Add(assembly);
        return this;
    }
}
