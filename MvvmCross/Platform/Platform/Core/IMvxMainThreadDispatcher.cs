﻿// IMvxMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;

    public interface IMvxMainThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}