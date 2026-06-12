// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Core;
using MvvmCross.Hosting;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Ios.Views;

public static class MvxCanCreateIosViewExtensions
{
    public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateIosView view,
            object parameterObject)
        where TTargetViewModel : class, IMvxViewModel =>
        view.CreateViewControllerFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());

    // TODO - could this move down to IMvxView level?
    public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
        this IMvxCanCreateIosView view,
        IDictionary<string, string>? parameterValues = null)
        where TTargetViewModel : class, IMvxViewModel
    {
        var parameterBundle = new MvxBundle(parameterValues);
        var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null);
        return view.CreateViewControllerFor(request);
    }

    public static IMvxIosView? CreateViewControllerFor(
        this IMvxCanCreateIosView view,
        MvxViewModelRequest request)
    {
        return MvxHost.Current?.Services.GetService<IMvxIosViewCreator>()?.CreateView(request);
    }

    public static IMvxIosView? CreateViewControllerFor(
        this IMvxCanCreateIosView view, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType)
    {
        return MvxHost.Current?.Services.GetService<IMvxIosViewCreator>()?.CreateViewOfType(viewType);
    }

    public static IMvxIosView? CreateViewControllerFor(
        this IMvxCanCreateIosView view,
        IMvxViewModel viewModel)
    {
        return MvxHost.Current?.Services.GetService<IMvxIosViewCreator>()?.CreateView(viewModel);
    }
}
