using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoutingExample.Core.RoutingFacades;
using RoutingExample.Core.ViewModels;

[assembly: MvxNavigation(typeof(RandomRoutingFacade), @"mvx://random")]

namespace RoutingExample.Core.RoutingFacades
{
    public class RandomRoutingFacade
        : IMvxNavigationFacade
    {
        public Task<MvxViewModelRequest> BuildViewModelRequest(string url, IDictionary<string, string> currentParameters)
        {
            // you can also use the values captured by the regex in currentPrameters

            var viewModelType = (new Random().Next() % 2 == 0) ? typeof(TestAViewModel) : typeof(TestBViewModel);

            return Task.FromResult(new MvxViewModelRequest(viewModelType, null, null));
        }
    }
}
