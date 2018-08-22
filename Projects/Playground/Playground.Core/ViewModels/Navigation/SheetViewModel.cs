// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SheetViewModel : MvxNavigationViewModel
    {
        public SheetViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(CloseSheet);
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        private async Task CloseSheet()
        {
            await NavigationService.Close(this);
        }
    }
}
