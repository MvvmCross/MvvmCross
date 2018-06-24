// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Tvos.Views
{
    public static class MvxCanCreateTvosViewExtensions
    {
        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(this IMvxCanCreateTvosView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxTvosView CreateViewControllerFor<TTargetViewModel>(
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
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateView(request);
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            Type viewtype,
            MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateViewOfType(viewtype, request);    
        }

        public static IMvxTvosView CreateViewControllerFor(
            this IMvxCanCreateTvosView view,
            IMvxViewModel viewModel)
        {
            return Mvx.IoCProvider.Resolve<IMvxTvosViewCreator>().CreateView(viewModel);
        }
    }
}
