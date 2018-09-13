// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
    public class MvxPagePresentationHint
        : MvxPresentationHint
    {
        public MvxPagePresentationHint(Type viewModel)
        {
            ViewModel = viewModel;
        }

        public Type ViewModel { get; private set; }
    }
}
