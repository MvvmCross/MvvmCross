using System;
using System.Threading.Tasks;

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
        /// The ViewModel will be dispatched with MvxRequestedBy.Bookmark
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>A task to await upon</returns>
        public static Task Navigate(this IMvxNavigationService navigationService, Uri path)
        {
            return navigationService.Navigate(path.ToString());
        }

        //Task Navigate<TParameter>(Uri path, TParameter param);
        //Task<TResult> Navigate<TResult>(Uri path);
        //Task<TResult> Navigate<TParameter, TResult>(Uri path, TParameter param);
    }
}
