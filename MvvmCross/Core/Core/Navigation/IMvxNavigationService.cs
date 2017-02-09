using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation
{
    /// <summary>
    /// Allows for URI based navigation in MvvmCross
    /// </summary>
    public interface IMvxNavigationService
    {
        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="uri">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        bool CanRoute(string uri);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// The ViewModel will be dispatched with MvxRequestedBy.Bookmark
        /// </summary>
        /// <param name="uri">URI to route</param>
        /// <returns>A task to await upon</returns>
        Task RouteAsync(string uri);


        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// The ViewModel will be dispatched with MvxRequestedBy.Bookmark
        /// </summary>
        /// <param name="uri">URI to route</param>
        /// <returns>A task to await upon</returns>
        Task RouteAsync(Uri uri);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="uri">URI to route</param>
        /// <param name="requestedBy">Specify how the route was requested. This can be useful if you want to clear your stack, etc.</param>
        /// <returns>A task to await upon</returns>
        Task RouteAsync(string uri, MvxRequestedBy requestedBy);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="uri">URI to route</param>
        /// <param name="requestedBy">Specify how the route was requested. This can be useful if you want to clear your stack, etc.</param>
        /// <returns>A task to await upon</returns>
        Task RouteAsync(Uri uri, MvxRequestedBy requestedBy);
    }
}
