// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Navigation
{
    public class MvxNavigationService : IMvxNavigationService
    {
        protected readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MvxNavigationService>();

        private IMvxViewDispatcher _viewDispatcher;
        public IMvxViewDispatcher ViewDispatcher
        {
            get => _viewDispatcher ?? (IMvxViewDispatcher)MvxMainThreadDispatcher.Instance;
            set => _viewDispatcher = value;
        }

        private IMvxViewsContainer _viewsContainer;
        protected IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
            set => _viewsContainer = value;
        }

        protected static readonly Dictionary<Regex, Type> Routes = new Dictionary<Regex, Type>();
        protected virtual IMvxNavigationCache NavigationCache { get; private set; }
        protected IMvxViewModelLoader ViewModelLoader { get; set; }
        protected ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object>> _tcsResults = new ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object>>();

        public event BeforeNavigateEventHandler BeforeNavigate;
        public event AfterNavigateEventHandler AfterNavigate;
        public event BeforeCloseEventHandler BeforeClose;
        public event AfterCloseEventHandler AfterClose;
        public event BeforeChangePresentationEventHandler BeforeChangePresentation;
        public event AfterChangePresentationEventHandler AfterChangePresentation;

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

        protected virtual bool TryGetRoute(string url, out KeyValuePair<Regex, Type> entry)
        {
            try
            {
                var matches = Routes.Where(t => t.Key.IsMatch(url)).ToList();

                switch (matches.Count)
                {
                    case 0:
                        entry = default(KeyValuePair<Regex, Type>);
                        Log.Trace("Unable to find routing for {0}", url);
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

                Log.Warn("The following regular expressions match the provided url ({0}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {1}",
                    matches.Count - 1,
                    string.Join(", ", matches.Select(t => t.Key.ToString())));
                // there is more than one match
                entry = default(KeyValuePair<Regex, Type>);
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("MvxNavigationService", "Unable to determine routability: {0}", ex);
                entry = default(KeyValuePair<Regex, Type>);
                return false;
            }
        }

        protected virtual IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
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

        protected virtual async Task<MvxViewModelInstanceRequest> NavigationRouteRequest(string path, IMvxBundle presentationBundle = null)
        {
            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(path, out entry))
            {
                throw new MvxException($"Navigation route request could not be obtained for path: {path}");
            }

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
                var facade = (IMvxNavigationFacade)Mvx.IoCProvider.IoCConstruct(viewModelType);

                try
                {
                    var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                    if (facadeRequest == null)
                    {
                        throw new MvxException($"{nameof(MvxNavigationService)}: Facade did not return a valid {nameof(MvxViewModelRequest)}.");
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
                    throw ex.MvxWrap($"{nameof(MvxNavigationService)}: Exception thrown while processing URL: {path} with RoutingFacade: {viewModelType}");
                }
            }
            else
            {
                request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            }

            return request;
        }

        protected async Task<MvxViewModelInstanceRequest> NavigationRouteRequest<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null)
        {
            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(path, out entry))
            {
                throw new MvxException($"Navigation route request could not be obtained for path: {path}");
            }

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
                var facade = (IMvxNavigationFacade)Mvx.IoCProvider.IoCConstruct(viewModelType);

                try
                {
                    var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                    if (facadeRequest == null)
                    {
                        throw new MvxException($"{nameof(MvxNavigationService)}: Facade did not return a valid {nameof(MvxViewModelRequest)}.");
                    }

                    request.ViewModelType = facadeRequest.ViewModelType;

                    if (facadeRequest.ParameterValues != null)
                    {
                        request.ParameterValues = facadeRequest.ParameterValues;
                    }

                    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
                }
                catch (Exception ex)
                {
                    ex.MvxWrap($"{nameof(MvxNavigationService)}: Exception thrown while processing URL: {path} with RoutingFacade: {viewModelType}");
                }
            }
            else
            {
                request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
            }

            return request;
        }

        public virtual Task<bool> CanNavigate(string path)
        {
            return Task.FromResult(TryGetRoute(path, out var entry));
        }

        public virtual Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel
        {
            return Task.FromResult(ViewsContainer.GetViewType(typeof(TViewModel)) != null);
        }

        public virtual Task<bool> CanNavigate(Type viewModelType)
        {
            return Task.FromResult(ViewsContainer.GetViewType(viewModelType) != null);
        }

        protected virtual async Task<bool> Navigate(MvxViewModelRequest request, IMvxViewModel viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var hasNavigated = false;

            var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            OnBeforeNavigate(this, args);

            if (args.Cancel)
                return false;

            hasNavigated = await ViewDispatcher.ShowViewModel(request);
            if (!hasNavigated)
                return false;

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnAfterNavigate(this, args);
            return true;
        }

        protected virtual async Task<TResult> Navigate<TResult>(MvxViewModelRequest request, IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var hasNavigated = false;
            var tcs = new TaskCompletionSource<object>();

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(async () =>
                {
                    if (hasNavigated && !tcs.Task.IsCompleted)
                        await Close(viewModel, default(TResult));
                });
            }

            var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            OnBeforeNavigate(this, args);

            viewModel.CloseCompletionSource = tcs;
            _tcsResults.Add(viewModel, tcs);

            if (cancellationToken.IsCancellationRequested)
                return default(TResult);

            hasNavigated = await ViewDispatcher.ShowViewModel(request);
            if (!hasNavigated)
                return default(TResult);

            // TODO: Should Initialize task be done on UI or non-UI thread? Now that ShowViewModel is async, we could ConfigureAwait it to return on non-UI thread
            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnAfterNavigate(this, args);

            try
            {
                return (TResult)await tcs.Task;
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }

        protected virtual async Task<TResult> Navigate<TParameter, TResult>(MvxViewModelRequest request, IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken), IMvxNavigateEventArgs args = null)
        {
            var hasNavigated = false;
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(async () =>
                {
                    if (hasNavigated)
                        await Close(viewModel, default(TResult));
                });
            }

            if (args == null)
            {
                args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            }
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<object>();
            viewModel.CloseCompletionSource = tcs;
            _tcsResults.Add(viewModel, tcs);

            if (cancellationToken.IsCancellationRequested)
                return default(TResult);

            hasNavigated = await ViewDispatcher.ShowViewModel(request);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnAfterNavigate(this, args);

            try
            {
                return (TResult)await tcs.Task;
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }

        public virtual async Task<bool> Navigate(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<bool> Navigate<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(false);
            return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TResult>(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            return await Navigate<TResult>(request, (IMvxViewModelResult<TResult>)request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(false);
            return await Navigate<TParameter, TResult>(request, (IMvxViewModel<TParameter, TResult>)request.ViewModelInstance, param, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<bool> Navigate(Type viewModelType, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
            return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
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
            var args = new MvxNavigateEventArgs(NavigationMode.Show, cancellationToken);
            var request = new MvxViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = (IMvxViewModel<TParameter, TResult>)ViewModelLoader.LoadViewModel(request, param, null);
            args.ViewModel = request.ViewModelInstance;
            return await Navigate<TParameter, TResult>(request, (IMvxViewModel<TParameter, TResult>)request.ViewModelInstance, param, presentationBundle, cancellationToken, args).ConfigureAwait(false);
        }

        public virtual Task<bool> Navigate<TViewModel>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel
        {
            return Navigate(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter>
        {
            return Navigate(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }

        public virtual Task<TResult> Navigate<TViewModel, TResult>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResult<TResult>
        {
            return Navigate<TResult>(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public virtual Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter, TResult>
        {
            return Navigate<TParameter, TResult>(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }

        public virtual async Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return await Navigate(request, viewModel, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
            return await Navigate(request, viewModel, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new MvxViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return await Navigate<TResult>(request, viewModel, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            var request = new MvxViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
            return await Navigate<TParameter, TResult>(request, viewModel, param, presentationBundle, cancellationToken, args).ConfigureAwait(false);
        }

        public virtual async Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default)
        {
            MvxLog.Instance.Trace("Requesting presentation change");
            var args = new ChangePresentationEventArgs(hint, cancellationToken);
            OnBeforeChangePresentation(this, args);

            if (args.Cancel)
                return false;

            var result = await ViewDispatcher.ChangePresentation(hint);

            args.Result = result;
            OnAfterChangePresentation(this, args);

            return result;
        }

        public virtual async Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Close, cancellationToken);
            OnBeforeClose(this, args);

            if (args.Cancel)
                return false;

            var close = await ViewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel));
            OnAfterClose(this, args);

            return close;
        }

        public virtual async Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default(CancellationToken))
        {
            _tcsResults.TryGetValue(viewModel, out var _tcs);

            //Disable cancelation of the Task when closing ViewModel through the service
            viewModel.CloseCompletionSource = null;

            try
            {
                var closeResult = await Close(viewModel, cancellationToken);
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

        protected virtual void OnBeforeNavigate(object sender, IMvxNavigateEventArgs e)
        {
            BeforeNavigate?.Invoke(sender, e);
        }

        protected virtual void OnAfterNavigate(object sender, IMvxNavigateEventArgs e)
        {
            AfterNavigate?.Invoke(sender, e);
        }

        protected virtual void OnBeforeClose(object sender, IMvxNavigateEventArgs e)
        {
            BeforeClose?.Invoke(sender, e);
        }

        protected virtual void OnAfterClose(object sender, IMvxNavigateEventArgs e)
        {
            AfterClose?.Invoke(sender, e);
        }

        protected virtual void OnBeforeChangePresentation(object sender, ChangePresentationEventArgs e)
        {
            BeforeChangePresentation?.Invoke(sender, e);
        }

        protected virtual void OnAfterChangePresentation(object sender, ChangePresentationEventArgs e)
        {
            AfterChangePresentation?.Invoke(sender, e);
        }
    }
}
