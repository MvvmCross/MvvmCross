// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
#nullable enable
    public class MvxPagePresentationHint
        : MvxPresentationHint
    {
        public MvxPagePresentationHint(Type viewModel)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(Type viewModel, MvxBundle body) : base(body)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(Type viewModel, IDictionary<string, string> hints) : this(viewModel, new MvxBundle(hints))
        {
        }

        public Type ViewModel { get; }
    }
#nullable restore
}
