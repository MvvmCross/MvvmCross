using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;

namespace MvvmCross.Core.Navigation
{
    public class MvxNavigationService
        : IMvxNavigationService
    {
        private readonly IMvxViewDispatcher _viewDispatcher;
        private static readonly Dictionary<Regex, Type> Routes = new Dictionary<Regex, Type>();

        public MvxNavigationService(IMvxViewDispatcher viewDispatcher)
        {
            _viewDispatcher = viewDispatcher;
        }

        public static void LoadRoutes(Assembly[] assemblies)
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
                        Mvx.TaggedTrace("MvxRoutingService", "Unable to find routing for {0}", url);
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

                Mvx.TaggedWarning("MvxRoutingService",
                    "The following regular expressions match the provided url ({0}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {1}",
                    matches.Count - 1,
                    string.Join(", ", matches.Select(t => t.Key.ToString())));
                // there is more than one match
                return false;
            }
            catch (Exception ex)
            {
                Mvx.TaggedError("MvxRoutingService", "Unable to determine routability: {0}", ex);
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

        public bool CanRoute(string url)
        {
            KeyValuePair<Regex, Type> entry;

            return TryGetRoute(url, out entry);
        }

        public Task RouteAsync(string url)
        {
            return RouteAsync(url, MvxRequestedBy.UserAction);
        }

        public Task RouteAsync(Uri url)
        {
            return RouteAsync(url, MvxRequestedBy.UserAction);
        }

        public Task RouteAsync(Uri url, MvxRequestedBy requestedBy)
        {
            return RouteAsync(url.ToString(), requestedBy);
        }
        
        public async Task RouteAsync(string url, MvxRequestedBy requestedBy)
        {
            KeyValuePair<Regex, Type> entry;

            if (!TryGetRoute(url, out entry)) return;

            var regex = entry.Key;
            var match = regex.Match(url);
            var paramDict = BuildParamDictionary(regex, match);

            var viewModelType = entry.Value;
            MvxViewModelRequest request = null;
            if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
            {
                var facade = (IMvxNavigationFacade)Mvx.IocConstruct(viewModelType);
                
                try
                {
                    request = await facade.BuildViewModelRequest(url, paramDict, requestedBy);
                }
                catch (Exception ex)
                {
                    Mvx.TaggedError("MvxRoutingService",
                        "Exception thrown while processing URL: {0} with RoutingFacade: {1}, {2}",
                        url, viewModelType, ex);
                }

                if (request == null)
                {
                    Mvx.TaggedWarning("MvxRoutingService", "Facade did not return a valid MvxViewModelRequest.");
                    return;
                }
            }
            else
            {
                request = new MvxViewModelRequest(
                    viewModelType,
                    new MvxBundle(paramDict),
                    null,
                    requestedBy);
            }
            
            _viewDispatcher.ShowViewModel(request);
        }
    }
}
