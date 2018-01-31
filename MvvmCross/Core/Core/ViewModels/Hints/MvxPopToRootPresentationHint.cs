// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Core.ViewModels.Hints
{
    public class MvxPopToRootPresentationHint
        : MvxPresentationHint
    {
        public MvxPopToRootPresentationHint(bool animated = true)
        {
            Animated = animated;
        }

        public bool Animated { get; set; }
    }
}
