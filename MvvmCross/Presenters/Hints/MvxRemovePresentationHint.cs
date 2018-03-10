// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
    public class MvxRemovePresentationHint
        : MvxPresentationHint
    {
        public MvxRemovePresentationHint(Type viewModelToRemove)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public Type ViewModelToRemove { get; private set; }
    }
}
