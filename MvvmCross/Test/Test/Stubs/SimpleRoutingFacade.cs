using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Test.Mocks.TestViewModels;

namespace MvvmCross.Test.Stubs
{
    public class SimpleRoutingFacade
        : IMvxNavigationFacade
    {
        public Task<MvxViewModelRequest> BuildViewModelRequest(string url,
            IDictionary<string, string> currentParameters)
        {

            var viewModelType = currentParameters["vm"] == "a" ? typeof(ViewModelA) : typeof(ViewModelB);

            return Task.FromResult(new MvxViewModelRequest(viewModelType, null, null, null));
        }
    }
}
