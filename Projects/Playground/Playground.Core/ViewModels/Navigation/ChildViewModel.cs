// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxNavigationViewModel<SampleModel, SampleModel>
    {
        public string BrokenTextValue { get => _brokenTextValue; set => SetProperty(ref _brokenTextValue, value); }
        public string AnotherBrokenTextValue { get => _anotherBrokenTextValue; set => SetProperty(ref _anotherBrokenTextValue, value); }

        private SampleModel _parameter;
        private string _brokenTextValue;
        private string _anotherBrokenTextValue;

        public ChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this, new SampleModel
            {
                Message = "This returned correctly",
                Value = 5.67m
            }).ConfigureAwait(true));

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(true));

            ShowRootCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(true));

            PropertyChanged += ChildViewModel_PropertyChanged;
        }

        private void ChildViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Demonstrates that exceptions can be raised on property changed but are swallowed by default to 
            // protect the app from crashing
            if (e.PropertyName == nameof(BrokenTextValue))
                throw new System.NotImplementedException();
        }

        public override ValueTask Prepare()
        {
            return base.Prepare();
        }

        public override ValueTask Prepare(SampleModel parameter)
        {
            _parameter = parameter;
            return new ValueTask();
        }

        protected override ValueTask SaveStateToBundle(IMvxBundle bundle)
        {
            return base.SaveStateToBundle(bundle);
        }

        protected override ValueTask ReloadFromBundle(IMvxBundle state)
        {
            return base.ReloadFromBundle(state);
        }

        public override async ValueTask Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            await Task.Delay(8500).ConfigureAwait(false);
        }

        public override ValueTask Start()
        {
            return base.Start();
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }

        public IMvxAsyncCommand ShowRootCommand { get; private set; }

        public override ValueTask ViewAppeared()
        {
            base.ViewAppeared();

            return new ValueTask(Task.Run(async () =>
            {
                await Task.Delay(1000).ConfigureAwait(true);
                BrokenTextValue = "This will throw exception in UI layer";
                AnotherBrokenTextValue = "This will throw exception in page";
            }));
        }
    }
}
