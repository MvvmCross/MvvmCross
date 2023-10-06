// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Bindings
{
    public class FluentBindingViewModel : BaseViewModel
    {
        bool _bindingsEnabled = true;

        public FluentBindingViewModel(ILoggerFactory loggerFactory, IMvxNavigationService navigationService)
            : base(loggerFactory, navigationService)
        {
            ClearBindingsCommand = new MvxCommand(ClearBindings);
        }

        public IMvxCommand ClearBindingsCommand { get; }

        public MvxInteraction<bool> ClearBindingInteraction { get; } = new MvxInteraction<bool>();

        string _textValue;
        public string TextValue
        {
            get => _textValue;
            set => SetProperty(ref _textValue, value);
        }

        void ClearBindings()
        {
            _bindingsEnabled = !_bindingsEnabled;
            ClearBindingInteraction.Raise(_bindingsEnabled);
        }
    }
}
