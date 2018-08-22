// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace Playground.Core.ViewModels
{
    public class SharedElementSecondChildViewModel : BaseViewModel
    {
        public SharedElementSecondChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }
    }
}
