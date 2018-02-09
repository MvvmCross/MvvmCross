// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation
{
    public interface IMvxNavigationFacade
    {
        Task<MvxViewModelRequest> BuildViewModelRequest(string url, IDictionary<string, string> currentParameters);
    }
}
