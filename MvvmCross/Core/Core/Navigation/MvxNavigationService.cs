using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation.EventArguments;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Core.Navigation
{
    public class MvxNavigationService : IMvxNavigationService
    {
        private IMvxViewDispatcher _viewDispatcher;

        public IMvxViewDispatcher ViewDispatcher
        {
            get { return _viewDispatcher ?? (IMvxViewDispatcher) MvxMainThreadDispatcher.Instance; }
            set { _viewDispatcher = value; }
        }

        private static readonly Dictionary<Regex, Type> Routes = new Dictionary<Regex, Type>();

        private IMvxNavigationCache navigationCache;

        protected virtual IMvxNavigationCache NavigationCache
        {
            get
            {
                if (navigationCache == null)
                {
                    navigationCache = new MvxNavigationCache();
                    Mvx.RegisterSingleton<IMvxNavigationCache>(navigationCache);
                }

                return navigationCache;
            }
        }

        public event BeforeNavigateEventHandler BeforeNavigate;
        public event AfterNavigateEventHandler AfterNavigate;
        public event BeforeCloseEventHandler BeforeClose;
        public event AfterCloseEventHandler AfterClose;

        public MvxNavigationService()
        {
        }

        public static void LoadRoutes(IEnumerable<Assembly> assemblies)
        {
            Routes.Clear();
            foreach (var routeAttr in
                assemblies.SelectMany(a => a.GetCustomAttributes<MvxNavigationAttribute>()))
            {
                Routes.Add(
                    new Regex(routeAttr.UriRegex,
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

        protected async Task<MvxViewModelInstanceRequest> NavigationRouteRequest(string path,
            IMvxBundle presentationBundle = null)
        {
            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(path, out entry)) return null;

            var regex = entry.Key;
            var match = regex.Match(path);
            var paramDict = BuildParamDictionary(regex, match);

            var viewModelType = entry.Value;
            IMvxViewModel viewModel;
            if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
            {
                var facade = (IMvxNavigationFacade) Mvx.IocConstruct(viewModelType);

                try
                {
                    var facadeRequest = await facade.BuildViewModelRequest(path, paramDict);
                    viewModel = (IMvxViewModel) Mvx.IocConstruct(facadeRequest.ViewModelType);

                    if (facadeRequest == null)
                    {
                        Mvx.TaggedWarning("MvxNavigationService", "Facade did not return a valid MvxViewModelRequest.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Mvx.TaggedError("MvxNavigationService",
                        "Exception thrown while processing URL: {0} with RoutingFacade: {1}, {2}",
                        path, viewModelType, ex);
                    return null;
                }
            }
            else
            {
                viewModel = (IMvxViewModel) Mvx.IocConstruct(viewModelType);
            }
            var parameterValues = new MvxBundle(paramDict);
            RunViewModelLifecycle(viewModel, parameterValues);

            return new MvxViewModelInstanceRequest(viewModel)
            {
                ParameterValues = parameterValues.SafeGetData(),
                PresentationValues = presentationBundle?.SafeGetData()
            };
        }

        public virtual IMvxViewModel LoadViewModel<TViewModel>(IMvxBundle parameterValues = null,
            IMvxBundle savedState = null)
        {
            IMvxViewModel viewModel;
            try
            {
                viewModel = (IMvxViewModel) Mvx.IocConstruct<TViewModel>();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", typeof(TViewModel).Name);
            }

            RunViewModelLifecycle(viewModel, parameterValues, savedState);

            return viewModel;
        }

        protected virtual void CallCustomInitMethods(IMvxViewModel viewModel, IMvxBundle parameterValues)
        {
            viewModel.CallBundleMethods("Init", parameterValues);
        }

        protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle savedState)
        {
            viewModel.CallBundleMethods("ReloadState", savedState);
        }

        protected void RunViewModelLifecycle(IMvxViewModel viewModel, IMvxBundle parameterValues = null,
            IMvxBundle savedState = null)
        {
            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if (savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }

        public virtual Task<bool> CanNavigate(string path)
        {
            KeyValuePair<Regex, Type> entry;

            return Task.FromResult(TryGetRoute(path, out entry));
        }

        public virtual Task<bool> Close(IMvxViewModel viewModel)
        {
            var args = new NavigateEventArgs();
            OnBeforeClose(this, args);
            var close = ViewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel));
            OnAfterClose(this, args);

            return Task.FromResult(close);
        }

        public virtual async Task Navigate(string path, IMvxBundle presentationBundle = null)
        {
            var request = await NavigationRouteRequest(path, presentationBundle);
            var viewModel = request.ViewModelInstance;

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize();

            OnAfterNavigate(this, args);
        }

        public virtual async Task Navigate<TParameter>(string path, TParameter param,
            IMvxBundle presentationBundle = null) where TParameter : class
        {
            var request = await NavigationRouteRequest(path, presentationBundle);
            var viewModel = (IMvxViewModel<TParameter>) request.ViewModelInstance;

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);
        }

        public virtual async Task<TResult> Navigate<TResult>(string path, IMvxBundle presentationBundle = null,
            CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            var request = await NavigationRouteRequest(path, presentationBundle);
            var viewModel = (IMvxViewModelResult<TResult>) request.ViewModelInstance;

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs, cancellationToken);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize();

            OnAfterNavigate(this, args);

            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default(TResult);
            }
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param,
            IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TParameter : class where TResult : class
        {
            var request = await NavigationRouteRequest(path, presentationBundle);
            var viewModel = (IMvxViewModel<TParameter, TResult>) request.ViewModelInstance;

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs, cancellationToken);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);

            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default(TResult);
            }
        }

        public virtual async Task Navigate<TViewModel>(IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel
        {
            var viewModel = LoadViewModel<TViewModel>();
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize();

            OnAfterNavigate(this, args);
        }

        public virtual async Task Navigate<TViewModel, TParameter>(TParameter param,
            IMvxBundle presentationBundle = null)
            where TViewModel : IMvxViewModel<TParameter>
            where TParameter : class
        {
            var viewModel = (IMvxViewModel<TParameter>) LoadViewModel<TViewModel>();
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);
        }

        public virtual async Task<TResult> Navigate<TViewModel, TResult>(IMvxBundle presentationBundle = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModelResult<TResult>
            where TResult : class
        {
            var viewModel = (IMvxViewModelResult<TResult>) LoadViewModel<TViewModel>();
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs, cancellationToken);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize();

            OnAfterNavigate(this, args);

            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default(TResult);
            }
        }

        public virtual async Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param,
            IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IMvxViewModel<TParameter, TResult>
            where TParameter : class
            where TResult : class
        {
            var viewModel = (IMvxViewModel<TParameter, TResult>) LoadViewModel<TViewModel>();
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs, cancellationToken);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);

            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default(TResult);
            }
        }

        public virtual async Task Navigate(IMvxViewModel viewModel, IMvxBundle presentationBundle = null)
        {
            RunViewModelLifecycle(viewModel);
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize();

            OnAfterNavigate(this, args);
        }

        public virtual async Task Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param,
            IMvxBundle presentationBundle = null) where TParameter : class
        {
            RunViewModelLifecycle(viewModel);
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);
        }

        public virtual async Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel,
            IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TResult : class
        {
            RunViewModelLifecycle(viewModel);
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs, cancellationToken);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize();

            OnAfterNavigate(this, args);

            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default(TResult);
            }
        }

        public virtual async Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel,
            TParameter param, IMvxBundle presentationBundle = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where TParameter : class
            where TResult : class
        {
            RunViewModelLifecycle(viewModel);
            var request =
                new MvxViewModelInstanceRequest(viewModel) {PresentationValues = presentationBundle?.SafeGetData()};

            var args = new NavigateEventArgs(viewModel);
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs, cancellationToken);

            ViewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);

            try
            {
                return await tcs.Task;
            }
            catch
            {
                return default(TResult);
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