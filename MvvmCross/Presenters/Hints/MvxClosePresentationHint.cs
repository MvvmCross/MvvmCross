// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
#nullable enable
    public class MvxClosePresentationHint
        : MvxPresentationHint
    {
        public MvxClosePresentationHint(IMvxViewModel viewModelToClose)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(IMvxViewModel viewModelToClose, MvxBundle body) : base(body)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(IMvxViewModel viewModelToClose, IDictionary<string, string> hints) : this(viewModelToClose, new MvxBundle(hints))
        {
        }

        public IMvxViewModel ViewModelToClose { get; }
    }
#nullable restore
}
