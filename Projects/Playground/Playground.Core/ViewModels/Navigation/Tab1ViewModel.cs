// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab1ViewModel : MvxNavigationViewModel<string>
    {
        public Tab1ViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            OpenChildCommand = new MvxAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new MvxAsyncCommand(() => NavigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new MvxAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new MvxAsyncCommand(() => NavigationService.Close(this));

            OpenTab2Command = new MvxAsyncCommand(() => NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab2ViewModel))));
        }

        public override async Task Initialize()
        {
            await Task.Delay(3000);
        }


        #region PassedParameter;
        private string _passedParameter;
        public string PassedParameter
        {
            get => _passedParameter;
            set => SetProperty(ref _passedParameter, value);
        }
        #endregion
        public override void Prepare(string parameter)
        {
            PassedParameter = parameter;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        public IMvxAsyncCommand OpenChildCommand { get; }

        public IMvxAsyncCommand OpenModalCommand { get; }

        public IMvxAsyncCommand OpenNavModalCommand { get; }

        public IMvxAsyncCommand OpenTab2Command { get; }

        public IMvxAsyncCommand CloseCommand { get; }
    }
}
