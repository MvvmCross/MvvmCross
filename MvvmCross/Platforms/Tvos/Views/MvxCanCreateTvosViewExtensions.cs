// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Core;
using MvvmCross.Hosting;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Tvos.Views
{
    public static class MvxCanCreateTvosViewExtensions
    {
        public static IMvxTvosView CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            object parameterObject)
                where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxTvosView CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            IDictionary<string, string> parameterValues = null)
                where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateTvosView view,
            MvxViewModelRequest request)
                where TTargetViewModel : class, IMvxViewModel
        {
            return MvxHost.Current!.Services.GetRequiredService<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            MvxViewModelRequest request)
        {
            return MvxHost.Current!.Services.GetRequiredService<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewtype,
            MvxViewModelRequest request)
        {
            return MvxHost.Current!.Services.GetRequiredService<IMvxTvosViewCreator>().CreateViewOfType(viewtype, request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            IMvxViewModel viewModel)
        {
            return MvxHost.Current!.Services.GetRequiredService<IMvxTvosViewCreator>().CreateView(viewModel);
        }
    }
}
