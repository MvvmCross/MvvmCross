// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Hints;

namespace Playground.Core.ViewModels
{
    public class NestedChildViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public NestedChildViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
            PopToChildCommand = new MvxCommand(() => _navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(ChildViewModel))));
            PopToRootCommand = new MvxCommand(() => _navigationService.ChangePresentation(new MvxPopToRootPresentationHint()));
            RemoveCommand = new MvxCommand(() => _navigationService.ChangePresentation(new MvxRemovePresentationHint(typeof(SecondChildViewModel))));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxCommand PopToChildCommand { get; private set; }

        public IMvxCommand PopToRootCommand { get; private set; }

        public IMvxCommand RemoveCommand { get; private set; }
    }
}
