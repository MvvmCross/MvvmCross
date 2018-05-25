// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;

namespace Playground.Core.ViewModels
{
    public class DictionaryBindingViewModel : BaseViewModel
    {
        int _value = 0;
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        IMvxAsyncCommand _closeCommand;
        public IMvxAsyncCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this)));


        IMvxCommand _incrementCommand;

        public IMvxCommand IncrementCommand =>
            _incrementCommand ?? (_incrementCommand = new MvxCommand(Increment));

        private void Increment()
        {
            if(Value < 3)
            {
                Value++;
            }
            else
            {
                Value = 0;
            }
        }
    }
}
