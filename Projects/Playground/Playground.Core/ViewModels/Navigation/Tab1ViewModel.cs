// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab1ViewModel : MvxViewModel<string>
    {
        public Tab1ViewModel()
        {
            OpenChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));
        }

        public override async Task Initialize()
        {
            await Task.Delay(3000);
        }

        string para;
        public override void Prepare(string parameter)
        {
            para = parameter;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        public IMvxAsyncCommand OpenChildCommand { get; private set; }

        public IMvxAsyncCommand OpenModalCommand { get; private set; }

        public IMvxAsyncCommand OpenNavModalCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
