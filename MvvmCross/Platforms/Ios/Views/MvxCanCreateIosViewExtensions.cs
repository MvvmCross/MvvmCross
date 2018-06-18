// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Ios.Views
{
    public static class MvxCanCreateIosViewExtensions
    {
        public static IMvxIosView CreateViewControllerFor<TTargetViewModel>(this IMvxCanCreateIosView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject?.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?

        public static IMvxIosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateIosView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxIosView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateIosView view,
            MvxViewModelRequest request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateView(request);
        }

        public static IMvxIosView CreateViewControllerFor(
            this IMvxCanCreateIosView view,
            MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateView(request);
        }

        public static IMvxIosView CreateViewControllerFor(
            this IMvxCanCreateIosView view, Type viewType,
            MvxViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateViewOfType(viewType, request);
        }

        public static IMvxIosView CreateViewControllerFor(
            this IMvxCanCreateIosView view,
            IMvxViewModel viewModel)
        {
            return Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateView(viewModel);
        }
    }
}
