// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxViewModel<SampleModel>
    {
        private SampleModel _parameter;

        public ChildViewModel()
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SecondChildViewModel>());

            ShowRootCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>());
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        public override void Prepare(SampleModel parameter)
        {
            _parameter = parameter;
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);
        }

        public async override System.Threading.Tasks.Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(8500);
        }

        public void Init()
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }

        public IMvxAsyncCommand ShowRootCommand { get; private set; }
    }
}
