// CountingMockMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.Dispatchers
{
    using System;

    using MvvmCross.Platform.Core;

    public class CountingMockMainThreadDispatcher
        : MvxMainThreadDispatcher
          , IMvxMainThreadDispatcher
    {
        public int Count { get; set; }

        public bool RequestMainThreadAction(Action action)
        {
            this.Count++;
            return true;
        }
    }
}