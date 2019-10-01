// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class CodeBehindViewModel : MvxViewModel
    {
        private string _bindableText = "I'm bound!";
        public string BindableText
        {
            get
            {
                return _bindableText;
            }
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
