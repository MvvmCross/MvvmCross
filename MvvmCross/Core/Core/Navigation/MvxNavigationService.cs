using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation.EventArguments;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.Navigation
{
    public class MvxNavigationService : IMvxNavigationService
    {
        private IMvxViewDispatcher _viewDispatcher;
        public IMvxViewDispatcher ViewDispatcher 
        {
            get => _viewDispatcher ?? (IMvxViewDispatcher)MvxMainThreadDispatcher.Instance;
            set => _viewDispatcher = value;
        }

        protected static readonly Dictionary<Regex, Type> Routes = new Dictionary<Regex, Type>();
        protected virtual IMvxNavigationCache NavigationCache { get; private set; }
        protected IMvxViewModelLoader ViewModelLoader { get; set; }
        protected ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object>> _tcsResults = new ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object>>();

        public event BeforeNavigateEventHandler BeforeNavigate;
        public event AfterNavigateEventHandler AfterNavigate;
        public event BeforeCloseEventHandler BeforeClose;
        public event AfterCloseEventHandler AfterClose;

        public MvxNavigationService(IMvxNavigationCache navigationCache, IMvxViewModelLoader viewModelLoader)
        {
            NavigationCache = navigationCache;
            ViewModelLoader = viewModelLoader;
        }

        public static void LoadRoutes(IEnumerable<Assembly> assemblies)
        {
            Routes.Clear();
            foreach (var routeAttr in
                assemblies.SelectMany(a => a.GetCustomAttributes<MvxNavigationAttribute>()))
            {
                Routes.Add(new Regex(routeAttr.UriRegex,
                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    routeAttr.ViewModelOrFacade);
            }
        }

        protected bool TryGetRoute(string url, out KeyValuePair<Regex, Type> entry)
        {
            try
            {
                var matches = Routes.Where(t => t.Key.IsMatch(url)).ToList();

                switch (matches.Count)
                {
                    case 0:
                        entry = default(KeyValuePair<Regex, Type>);
                        Mvx.TaggedTrace("MvxNavigationService", "Unable to find routing for {0}", url);
                        return false;
                    case 1:
                        entry = matches[0];
                        return true;
                }

                var directMatch = matches.Where(t => t.Key.Match(url).Groups.Count == 1).ToList();

                if (directMatch.Count == 1)
                {
                    entry = directMatch[0];
                    return true;
                }

                Mvx.TaggedWarning("MvxNavigationService",
                    "The following regular expressions match the provided url ({0}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {1}",
                    matches.Count - 1,
                    string.Join(", ", matches.Select(t => t.Key.ToString())));
                // there is more than one match
                return false;
            }
            catch (Exception ex)
            {
                Mvx.TaggedError("MvxNavigationService", "Unable to determine routability: {0}", ex);
                return false;
            }
        }

        protected IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
        {
            var paramDict = new Dictionary<string, string>();

            for (var i = 1 /* 0 == Match itself */; i < match.Groups.Count; i++)
            {
                var group = match.Groups[i];
                var name = regex.GroupNameFromNumber(i);
                var value = group.Value;
                paramDict.Add(name, value);
            }
            return paramDict;
        }

        protected async Task<MvxViewModelInstanceRequest> NavigationRouteRequest(string path, IMvxBundle presentationBundle = null)
        {
            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(path, out entry)) return null;

            var regex = entry.Key;
            var match = regex.Match(path);
            var paramDict = BuildParamDictionary(regex, match);
            var parameterValues = new MvxBundle(paramDict);

            var viewModelType = entry.Value;

            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData(),
                ParameterValues = parameterValues?.SafeGetData()
            };

            if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
            {
                var facade = (IMvxNavigationFacade)Mvx.IocConstruct(viewModelType);

                try
                {
                    var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                    if (facadeRequest == null)
                    {
                        Mvx.TaggedWarning(nameof(MvxNavigationService), $"Facade did not return a valid {nameof(MvxViewModelRequest)}.");
                        return null;
                    }

                    request.ViewModelType = facadeRequest.ViewModelType;

                    if (facadeRequest.ParameterValues != null)
                    {
                        request.ParameterValues = facadeRequest.ParameterValues;
                    }

                    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
                }
                catch (Exception ex)
                {
                    Mvx.TaggedError(nameof(MvxNavigationService),
                        "Exception thrown while processing URL: {0} with RoutingFacade: {1}, {2}",
                                    path, viewModelType, ex);
                    return null;
                }
            }
            else
            {
                request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            }

            return request;
        }

        public virtual Task<bool> CanNavigate(string path)
        {
            KeyValuePair<Regex, Type> entry;

            return Task.FromResult(TryGetRoute(path, out entry));
        }

        protected virtual async Task Navigate(MvxViewModelRequest request, IMvxViewModel viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            viewModel.Prepare();
            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize().ConfigureAwait(false);

            OnAfterNavigate(this, args);
        }

        protected virtual async Task Navigate<TParameter>(MvxViewModelRequest request, IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            viewModel.Prepare();
            viewModel.Prepare(param);
            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize().ConfigureAwait(false);

            OnAfterNavigate(this, args);
        }

        protected virtual async Task<TResult> Navigate<TResult>(MvxViewModelRequest request, IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            if(cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(async () => {
                    await Close(viewModel, default(TResult));
                });
            }

            var tcs = new TaskCompletionSource<object>();
            viewModel.CloseCompletionSource = tcs;
            _tcsResults.Add(viewModel, tcs);

            viewModel.Prepare();
            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize().ConfigureAwait(false);

            OnAfterNavigate(this, args);

            try
            {
                return (TResult)await tcs.Task;
            }
            catch (Exception e)
            {
                return default(TResult);
            }
        }

        protected virtual async Task<TResult> Navigate<TParameter, TResult> (MvxViewModelRequest request, IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(async () => {
                    await Close(viewModel, default(TResult));
                });
            }

            var tcs = new TaskCompletionSource<object>();
            viewModel.CloseCompletionSource = tcs;
            _tcsResults.Add(viewModel, tcs);

            viewModel.Prepare();
            viewModel.Prepare(param);
            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize().ConfigureAwait(false);
            
            OnAfterNavigate(this, args);

            try
            {
                return (TResult)await tcs.Task;
            }
            catch (Exception e)
            {
                return default(TResult);
            }
        }

        public virtual async Task Navigate(string path, IMvxBundle presentationBundle = null)
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            await Navigate(request, request.ViewModelInstance, presentationBundle).ConfigureAwait(false);
        }

        public virtual async Task Navigate<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null)
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            await Navigate<TParameter>(request, (IMvxViewModel<TParameter>)request.ViewModelInstance, param, presentationBundle).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TResult>(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            return await Navigate<TResult>(request, (IMvxViewModelResult<TResult>)request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            return await Navigate<TParameter, TResult>(request, (IMvxViewModel<TParameter, TResult>)request.ViewModelInstance, param, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task Navigate(Type viewModelType, IMvxBundle presentationBundle = null)
        {
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            await Navigate(request, request.ViewModelInstance, presentationBundle).ConfigureAwait(false);
        }

        public virtual async Task Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null)
        {
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = (IMvxViewModel<TParameter>)ViewModelLoader.LoadViewModel(request, null);
            await Navigate<TParameter>(request, (IMvxViewModel<TParameter>)request.ViewModelInstance, param, presentationBundle).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TResult>(Type viewModelType, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = (IMvxViewModelResult<TResult>)ViewModelLoader.LoadViewModel(request, null);
            return await Navigate<TResult>(request, (IMvxViewModelResult<TResult>)request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = (IMvxViewModel<TParameter, TResult>)ViewModelLoader.LoadViewModel(request, null);
            return await Navigate<TParameter, TResult>(request, (IMvxViewModel<TParameter, TResult>)request.ViewModelInstance, param, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual Task Navigate<TViewModel>(IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel
        {
            return Navigate(typeof(TViewModel), presentationBundle);
        }

        public virtual Task Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel<TParameter>
        {
            return Navigate<TParameter>(typeof(TViewModel), param, presentationBundle);
        }

        public virtual Task<TResult> Navigate<TViewModel, TResult>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResult<TResult>
        {
            return Navigate<TResult>(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public virtual Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter, TResult>
        {
            return Navigate<TParameter, TResult>(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }

        public virtual async Task Navigate(IMvxViewModel viewModel, IMvxBundle presentationBundle = null)
        {
            var request = new MvxViewModelInstanceRequest(viewModel){ PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            await Navigate(request, viewModel, presentationBundle).ConfigureAwait(false);
        }

        public virtual async Task Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null)
        {
            var request = new MvxViewModelInstanceRequest(viewModel){ PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            await Navigate<TParameter>(request, viewModel, param, presentationBundle).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModel){ PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return await Navigate<TResult>(request, viewModel, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModel){ PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return await Navigate<TParameter, TResult>(request, viewModel, param, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Trace("Requesting presentation change");
            return ViewDispatcher.ChangePresentation(hint);
        }

        public virtual Task<bool> Close(IMvxViewModel viewModel)
        {
            var args = new NavigateEventArgs(viewModel);
            OnBeforeClose(this, args);
            var close = ViewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel));
            OnAfterClose(this, args);

            return Task.FromResult(close);
        }

        public virtual async Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result)
        {
            _tcsResults.TryGetValue(viewModel, out TaskCompletionSource<object> _tcs);

            //Disable cancelation of the Task when closing ViewModel through the service
            viewModel.CloseCompletionSource = null;

            try
            {
                var closeResult = await Close(viewModel);
                if (closeResult)
                {
                    _tcs?.TrySetResult(result);
                    _tcsResults.Remove(viewModel);
                }
                else
                    viewModel.CloseCompletionSource = _tcs;
                return closeResult;
            }
            catch (Exception ex)
            {
                _tcs?.TrySetException(ex);
                return false;
            }
        }

        protected virtual void OnBeforeNavigate(object sender, NavigateEventArgs e)
        {
            BeforeNavigate?.Invoke(sender, e);
        }

        protected virtual void OnAfterNavigate(object sender, NavigateEventArgs e)
        {
            AfterNavigate?.Invoke(sender, e);
        }

        protected virtual void OnBeforeClose(object sender, NavigateEventArgs e)
        {
            BeforeClose?.Invoke(sender, e);
        }

        protected virtual void OnAfterClose(object sender, NavigateEventArgs e)
        {
            AfterClose?.Invoke(sender, e);
        }
    }
}
