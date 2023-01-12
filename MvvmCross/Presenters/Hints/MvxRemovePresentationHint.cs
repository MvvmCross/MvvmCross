// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
#nullable enable
    public class MvxRemovePresentationHint
        : MvxPresentationHint
    {
        public MvxRemovePresentationHint(Type viewModelToRemove)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public MvxRemovePresentationHint(Type viewModelToRemove, MvxBundle body) : base(body)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public MvxRemovePresentationHint(Type viewModelToRemove, IDictionary<string, string> hints)
            : this(viewModelToRemove, new MvxBundle(hints))
        {
        }

        public Type ViewModelToRemove { get; }
    }
#nullable restore
}
