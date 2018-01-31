// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace MvvmCross.Core.Navigation
{
    public static class MvxNavigationExtensions
    {
        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        public static Task<bool> CanNavigate(this IMvxNavigationService navigationService, Uri path)
        {
            return navigationService.CanNavigate(path.ToString());
        }

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="path">URI to route</param>
        /// <param name="presentationBundle"></param>
        /// <returns>A task to await upon</returns>
        public static Task Navigate(this IMvxNavigationService navigationService, Uri path, IMvxBundle presentationBundle = null)
        {
            return navigationService.Navigate(path.ToString(), presentationBundle);
        }

        public static Task Navigate<TParameter>(this IMvxNavigationService navigationService, Uri path, TParameter param, IMvxBundle presentationBundle = null)
        {
            return navigationService.Navigate<TParameter>(path.ToString(), param, presentationBundle);
        }

        public static Task Navigate<TResult>(this IMvxNavigationService navigationService, Uri path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return navigationService.Navigate<TResult>(path.ToString(), presentationBundle, cancellationToken);
        }

        public static Task Navigate<TParameter, TResult>(this IMvxNavigationService navigationService, Uri path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return navigationService.Navigate<TParameter, TResult>(path.ToString(), param, presentationBundle, cancellationToken);
        }
    }
}
