// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
#nullable enable
    public class MvxPopRecursivePresentationHint
        : MvxPresentationHint
    {
        public MvxPopRecursivePresentationHint(int levelsDeep, bool animated = false)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public MvxPopRecursivePresentationHint(MvxBundle body, int levelsDeep, bool animated = true) : base(body)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public MvxPopRecursivePresentationHint(IDictionary<string, string> hints, int levelsDeep, bool animated = true)
            : this(new MvxBundle(hints), levelsDeep, animated)
        {
        }

        public int LevelsDeep { get; }

        public bool Animated { get; set; }
    }
#nullable restore
}
