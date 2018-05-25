// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SheetViewModel : MvxViewModel
    {
        public SheetViewModel()
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
