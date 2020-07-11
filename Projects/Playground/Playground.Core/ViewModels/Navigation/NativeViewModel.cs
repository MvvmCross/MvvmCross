// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class NativeViewModel:MvxViewModel
    {
        private static int _counter = 0;

        public NativeViewModel(IMvxNavigationService navigationService)
        {
            ForwardCommand = new MvxAsyncCommand(
                async () => await navigationService.Navigate<NativeViewModel>().ConfigureAwait(false));
            CloseCommand = new MvxAsyncCommand(
                async () => await navigationService.Close(this).ConfigureAwait(false));

            Description = $"View number {_counter++}";
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public IMvxAsyncCommand ForwardCommand { get; }
        public IMvxAsyncCommand CloseCommand { get; }

    }
}
