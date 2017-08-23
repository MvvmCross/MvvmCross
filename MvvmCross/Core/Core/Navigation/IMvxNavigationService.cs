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

        Task NavigateAsync<TViewModel>(IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel;
        Task NavigateAsync<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel<TParameter>;
        Task<TResult> NavigateAsync<TViewModel, TResult>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResult<TResult>;
        Task<TResult> NavigateAsync<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter, TResult>;

        Task NavigateAsync(IMvxViewModel viewModel, IMvxBundle presentationBundle = null);
        Task NavigateAsync<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null);
        Task<TResult> NavigateAsync<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<TResult> NavigateAsync<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>A task to await upon</returns>
        Task NavigateAsync(string path, IMvxBundle presentationBundle = null);
        Task NavigateAsync<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null);
        Task<TResult> NavigateAsync<TResult>(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<TResult> NavigateAsync<TParameter, TResult>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        Task<bool> CanNavigate(string path);

        Task<bool> Close(IMvxViewModel viewModel);
        Task<bool> CloseAsync<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result);

        bool ChangePresentation(MvxPresentationHint hint);
    }
}