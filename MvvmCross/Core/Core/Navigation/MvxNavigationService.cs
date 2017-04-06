using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation.EventArguments;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.Navigation
{
    //TODO: Remove MvxNavigatingObject workaround when new navigation is fully integrated
    public class MvxNavigationService : MvxNavigatingObject, IMvxNavigationService
    {
        //TODO: Should we get the dispatcher via MvxMainThreadDispatcher.Instance; ?
        private readonly IMvxViewDispatcher _viewDispatcher;

        private static readonly Dictionary<Regex, Type> Routes = new Dictionary<Regex, Type>();

        protected virtual IMvxNavigationCache NavigationCache => new MvxNavigationCache();

        public event BeforeNavigateEventHandler BeforeNavigate;
        public event BeforeNavigateEventHandler AfterNavigate;
        public event BeforeNavigateEventHandler BeforeClose;
        public event BeforeNavigateEventHandler AfterClose;

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

            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(path, out entry)) return;

            var regex = entry.Key;
            var match = regex.Match(path);
            var paramDict = BuildParamDictionary(regex, match);

            var viewModelType = entry.Value;
            MvxViewModelRequest request = null;
            if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
            {
                var facade = (IMvxNavigationFacade)Mvx.IocConstruct(viewModelType);

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
                    return;
                }
            }
            else
            {
                request = new MvxViewModelRequest(
                    viewModelType,
                    new MvxBundle(paramDict),
                    null,
                    null);
            }

            _viewDispatcher.ShowViewModel(request);
            OnAfterNavigate(this, args);
        }


        public async Task<bool> CanNavigate(string path)
        {
            KeyValuePair<Regex, Type> entry;

            return TryGetRoute(path, out entry);
        }

        public async Task Navigate<TViewModel>() where TViewModel : IMvxViewModel
        {
            var args = new NavigateEventArgs(typeof(TViewModel));
            OnBeforeNavigate(this, args);
            ShowViewModel<TViewModel>();
            OnAfterNavigate(this, args);
        }

        /*public async Task Navigate<TViewModel, TParameter>(TParameter param) where TViewModel : IMvxViewModelInitializer<TParameter>
        {
            var args = new NavigateEventArgs(typeof(TViewModel));
            OnBeforeNavigate(this, args);
            ShowViewModel<TViewModel, TParameter>(param);
            OnAfterNavigate(this, args);
        }
*/
        public async Task<bool> Close(IMvxViewModel viewModel)
        {
            var args = new NavigateEventArgs();
            OnBeforeClose(this, args);
            var close = base.Close(viewModel);
            OnAfterClose(this, args);
            return close;
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

        public Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param)
            where TViewModel : IMvxViewModel<TParameter, TResult>
            where TParameter : class
            where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TViewModel, TResult>()
            where TViewModel : IMvxViewModelReturn<TResult>
            where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task Navigate<TParameter>(string path, TParameter param)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TResult>(string path)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param)
        {
            throw new NotImplementedException();
        }

        public async Task Navigate<TViewModel, TParameter>(TParameter param)
            where TViewModel : IMvxViewModel<TParameter>
            where TParameter : class
        {
            var cacheKey = Guid.NewGuid().ToString();

            var args = new NavigateEventArgs(typeof(TViewModel));

            OnBeforeNavigate(this, args);

            NavigationCache.AddValue<TParameter>(cacheKey, param);

            //TODO: Await showing viewmodel and return instance
            ShowViewModel<TViewModel>(cacheKey);
            //TODO: Call Init(TParameter param) here

            OnAfterNavigate(this, args);
        }
    }
}
