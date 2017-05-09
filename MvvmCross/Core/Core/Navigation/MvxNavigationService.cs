﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation.EventArguments;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.Navigation
{
    public class MvxNavigationService : IMvxNavigationService
    {
        //TODO: Should we get the dispatcher via MvxMainThreadDispatcher.Instance; ?
        private readonly IMvxViewDispatcher _viewDispatcher;

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

        public MvxNavigationService(IMvxViewDispatcher viewDispatcher)
        {
            _viewDispatcher = viewDispatcher;
        }

        public static void LoadRoutes(IEnumerable<Assembly> assemblies)
        {
            Routes.Clear();
            foreach (var routeAttr in
                assemblies.SelectMany(a => a.GetCustomAttributes<MvxNavigationAttribute>()))
            {
                Routes.Add(new Regex(routeAttr.UriRegex, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    routeAttr.ViewModelOrFacade);
            }
        }

        private static bool TryGetRoute(string url, out KeyValuePair<Regex, Type> entry)
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

        private static IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
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

        public async Task Navigate(string path)
        {
            var args = new NavigateEventArgs(path);
            OnBeforeNavigate(this, args);

            await NavigateRoute(path);

            OnAfterNavigate(this, args);
        }

        private async Task<IMvxViewModel> NavigateRoute(string path)
        {
            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(path, out entry)) return null;

            var regex = entry.Key;
            var match = regex.Match(path);
            var paramDict = BuildParamDictionary(regex, match);

            var viewModelType = entry.Value;
            MvxViewModelRequest request = null;
            IMvxViewModel viewModel;
            if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
            {
                var facade = (IMvxNavigationFacade)Mvx.IocConstruct(viewModelType);
                viewModel = (IMvxViewModel)facade;
                
                try
                {
                    request = await facade.BuildViewModelRequest(path, paramDict);
                }
                catch (Exception ex)
                {
                    Mvx.TaggedError("MvxNavigationService",
                        "Exception thrown while processing URL: {0} with RoutingFacade: {1}, {2}",
                        path, viewModelType, ex);
                }

                if (request == null)
                {
                    Mvx.TaggedWarning("MvxNavigationService", "Facade did not return a valid MvxViewModelRequest.");
                    return null;
                }
            }
            else
            {
                viewModel = (IMvxViewModel)Mvx.IocConstruct(viewModelType);
                request = new MvxViewModelInstanceRequest(viewModel) { ParameterValues = new MvxBundle(paramDict).SafeGetData() };
            }
            _viewDispatcher.ShowViewModel(request);

            return viewModel;
        }

        public async Task<bool> CanNavigate(string path)
        {
            KeyValuePair<Regex, Type> entry;

            return TryGetRoute(path, out entry);
        }

        public Task Navigate<TViewModel>() where TViewModel : IMvxViewModel
        {
            var args = new NavigateEventArgs(typeof(TViewModel));
            OnBeforeNavigate(this, args);
            _viewDispatcher.ShowViewModel(new MvxViewModelRequest<TViewModel>());
            OnAfterNavigate(this, args);

            return Task.FromResult(true);
        }

        public Task<bool> Close(IMvxViewModel viewModel)
        {
            var args = new NavigateEventArgs();
            OnBeforeClose(this, args);
            var close = _viewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel));
            OnAfterClose(this, args);

            return Task.FromResult(close);
        }

        public async Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param)
            where TViewModel : IMvxViewModel<TParameter, TResult>
            where TParameter : class
            where TResult : class
        {
            var args = new NavigateEventArgs(typeof(TViewModel));
            OnBeforeNavigate(this, args);

            var viewModel = (IMvxViewModel<TParameter, TResult>)Mvx.IocConstruct<TViewModel>();
            var request = new MvxViewModelInstanceRequest(viewModel);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs);

            _viewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);

            return await tcs.Task;
        }

        public async Task<TResult> Navigate<TViewModel, TResult>()
            where TViewModel : IMvxViewModelResult<TResult>
            where TResult : class
        {
            var args = new NavigateEventArgs(typeof(TViewModel));
            OnBeforeNavigate(this, args);

            var viewModel = (IMvxViewModelResult<TResult>)Mvx.IocConstruct<TViewModel>();
            var request = new MvxViewModelInstanceRequest(viewModel);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs);

            _viewDispatcher.ShowViewModel(request);

            OnAfterNavigate(this, args);

            return await tcs.Task;
        }

        public async Task Navigate<TParameter>(string path, TParameter param) where TParameter : class
        {
            var args = new NavigateEventArgs(path);
            OnBeforeNavigate(this, args);

            var viewModel = (IMvxViewModel<TParameter>)await NavigateRoute(path);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);
        }

        public async Task<TResult> Navigate<TResult>(string path) where TResult : class
        {
            var args = new NavigateEventArgs(path);
            OnBeforeNavigate(this, args);

            var viewModel = (IMvxViewModelResult<TResult>)await NavigateRoute(path);
            
            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs);

            OnAfterNavigate(this, args);

            return await tcs.Task;
        }

        public async Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param) where TParameter : class where TResult : class
        {
            var args = new NavigateEventArgs(path);
            OnBeforeNavigate(this, args);

            var viewModel = (IMvxViewModel<TParameter, TResult>)await NavigateRoute(path);

            var tcs = new TaskCompletionSource<TResult>();
            viewModel.SetClose(tcs);

            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);

            return await tcs.Task;
        }

        public async Task Navigate<TViewModel, TParameter>(TParameter param)
            where TViewModel : IMvxViewModel<TParameter>
            where TParameter : class
        {
            var args = new NavigateEventArgs(typeof(TViewModel));
            OnBeforeNavigate(this, args);

            var viewModel = (IMvxViewModel<TParameter>)Mvx.IocConstruct<TViewModel>();

            var request = new MvxViewModelInstanceRequest(viewModel);
            _viewDispatcher.ShowViewModel(request);
            await viewModel.Initialize(param);

            OnAfterNavigate(this, args);
        }

        private void OnBeforeNavigate(object sender, NavigateEventArgs e)
        {
            BeforeNavigate?.Invoke(sender, e);
        }

        private void OnAfterNavigate(object sender, NavigateEventArgs e)
        {
            AfterNavigate?.Invoke(sender, e);
        }

        private void OnBeforeClose(object sender, NavigateEventArgs e)
        {
            BeforeClose?.Invoke(sender, e);
        }

        private void OnAfterClose(object sender, NavigateEventArgs e)
        {
            AfterClose?.Invoke(sender, e);
        }
    }
}
