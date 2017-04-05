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
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// The ViewModel will be dispatched with MvxRequestedBy.Bookmark
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>A task to await upon</returns>
        Task Navigate(string path);

        Task Navigate<TViewModel>() where TViewModel : IMvxViewModel;

        //TODO: Find out if we can use IMvxViewModel instead of IMvxViewModelInitializer<TParameter>
        //Task Navigate<TViewModel, TParameter>(TParameter param) where TViewModel : IMvxViewModel;
        Task Navigate<TViewModel, TParameter>(TParameter param) where TViewModel : IMvxViewModelInitializer<TParameter>;

        //Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param) where TViewModel : IMvxViewModel;
        //Task<TResult> Navigate<TViewModel, TResult>() where TViewModel : IMvxViewModel;
        //Task Navigate<TParameter>(string path, TParameter param);
        //Task<TResult> Navigate<TResult>(string path);
        //Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param);

        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        Task<bool> CanNavigate(string path);

        //Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel;
    }
}