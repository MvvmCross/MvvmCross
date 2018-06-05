﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

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
            _closeCommand ?? (_closeCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this)));


        IMvxCommand _incrementCommand;
        private IMvxNavigationService _navigationService;

        public IMvxCommand IncrementCommand =>
            _incrementCommand ?? (_incrementCommand = new MvxCommand(Increment));

        public DictionaryBindingViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

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
