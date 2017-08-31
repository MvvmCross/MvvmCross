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
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        public static Task<bool> CanNavigate(this IMvxNavigationService navigationService, Uri path)
        {
            return navigationService.CanNavigate(path.ToString());
        }

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
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

        public static Task Navigate<TViewModel>(this IMvxNavigationService navigationService, IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel
        {
            return navigationService.Navigate(typeof(TViewModel), presentationBundle);
        }

        public static Task Navigate<TViewModel, TParameter>(this IMvxNavigationService navigationService, TParameter param, IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel<TParameter>
        {
            return navigationService.Navigate<TParameter>(typeof(TViewModel), param, presentationBundle);
        }

        public static Task<TResult> Navigate<TViewModel, TResult>(this IMvxNavigationService navigationService, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModelResult<TResult>
        {
            return navigationService.Navigate<TResult>(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public static Task<TResult> Navigate<TViewModel, TParameter, TResult>(this IMvxNavigationService navigationService, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModel<TParameter, TResult>
        {
            return navigationService.Navigate<TParameter, TResult>(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }
    }
}
