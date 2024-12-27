// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxNavigationViewModel<SampleModel>
    {
        public string BrokenTextValue { get => _brokenTextValue; set => SetProperty(ref _brokenTextValue, value); }
        public string AnotherBrokenTextValue { get => _anotherBrokenTextValue; set => SetProperty(ref _anotherBrokenTextValue, value); }

        private string _brokenTextValue;
        private string _anotherBrokenTextValue;

        public ChildViewModel(
            ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(DoCloseCommand);

            ShowSecondChildCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SecondChildViewModel>());

            ShowRootCommand = new MvxAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

            PropertyChanged += ChildViewModel_PropertyChanged;
        }

        private Task DoCloseCommand()
        {
            return NavigationService.Close(this);
        }

        private void ChildViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Demonstrates that exceptions can be raised on property changed but are swallowed by default to 
            // protect the app from crashing
            if (e.PropertyName == nameof(BrokenTextValue))
                throw new System.NotImplementedException();
        }

        public override async System.Threading.Tasks.Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(8500);
        }

        public void Init()
        {
            // Method intentionally left empty.
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }

        public IMvxAsyncCommand ShowRootCommand { get; private set; }

        public override void Prepare(SampleModel parameter)
        {
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                BrokenTextValue = "This will throw exception in UI layer";
                AnotherBrokenTextValue = "This will throw exception in page";
            });
        }
    }
}
