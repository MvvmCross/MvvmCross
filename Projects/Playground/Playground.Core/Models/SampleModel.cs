// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace Playground.Core.Models
{
    public class SampleModel : MvxNotifyPropertyChanged
    {
        private string _message;
        private decimal _value;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public decimal Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
