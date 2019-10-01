using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoutingExample.Core.RoutingFacades;
using RoutingExample.Core.ViewModels;

[assembly: MvxNavigation(typeof(PrePopulateRoutingFacade), @"mvx://prepop")]

namespace RoutingExample.Core.RoutingFacades
{
    public class PrePopulateRoutingFacade
        : IMvxNavigationFacade
    {
        public Task<MvxViewModelRequest> BuildViewModelRequest(string url, IDictionary<string, string> currentParameters)
        {
            var viewModelType = typeof(TestCViewModel);

            var testKey = "viewmodelcparameter";
            if (currentParameters.ContainsKey(testKey))
            {
                currentParameters.Remove(testKey);
            }
            currentParameters.Add(testKey, "this is set from the navigation facade");

            return Task.FromResult(new MvxViewModelRequest(viewModelType, new MvxBundle(currentParameters), null));
        }
    }
}
