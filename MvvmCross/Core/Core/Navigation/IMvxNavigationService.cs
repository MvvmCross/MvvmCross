using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation.EventArguments;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation
{
    public delegate void BeforeNavigateEventHandler(object sender, NavigateEventArgs e);
    public delegate void AfterNavigateEventHandler(object sender, NavigateEventArgs e);
    public delegate void BeforeCloseEventHandler(object sender, NavigateEventArgs e);
    public delegate void AfterCloseEventHandler(object sender, NavigateEventArgs e);

    /// <summary>
    /// Allows for URI based navigation in MvvmCross
    /// </summary>
    public interface IMvxNavigationService
    {
        event BeforeNavigateEventHandler BeforeNavigate;
        event AfterNavigateEventHandler AfterNavigate;
        event BeforeCloseEventHandler BeforeClose;
        event AfterCloseEventHandler AfterClose;

        Task Navigate<TViewModel>(IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel;
        Task Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel<TParameter> where TParameter : class;
        Task<TResult> Navigate<TViewModel, TResult>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResult<TResult> where TResult : class;
        Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter, TResult> where TParameter : class where TResult : class;

        Task Navigate(IMvxViewModel viewModel, IMvxBundle presentationBundle = null);
        Task Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null) where TParameter : class;
        Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class;
        Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : class where TResult : class;

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>A task to await upon</returns>
        Task Navigate(string path, IMvxBundle presentationBundle = null);
        Task Navigate<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null) where TParameter : class;
        Task<TResult> Navigate<TResult>(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class;
        Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TParameter : class where TResult : class;

        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        Task<bool> CanNavigate(string path);

        Task<bool> Close(IMvxViewModel viewModel);
    }
}