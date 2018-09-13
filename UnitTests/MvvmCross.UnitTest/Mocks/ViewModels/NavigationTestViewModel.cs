// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.ViewModels
{
    public class NavigationTestViewModel : MvxNavigationViewModel
    {
        public NavigationTestViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        public IMvxNavigationService NavService => base.NavigationService;

        public IMvxLogProvider LoggingProvider => base.LogProvider;

        public IMvxLog ViewModelLog => base.Log;
    }
}
