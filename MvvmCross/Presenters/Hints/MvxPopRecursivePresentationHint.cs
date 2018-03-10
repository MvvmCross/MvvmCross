// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
    public class MvxPopRecursivePresentationHint
        : MvxPresentationHint
    {
        public MvxPopRecursivePresentationHint(int levelsDeep, bool animated = false)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public int LevelsDeep { get; private set; }

        public bool Animated { get; set; }
    }
}
