// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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
            return Task.FromResult(new MvxViewModelRequest(viewModelType, null, null));
        }
    }
}
