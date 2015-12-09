// CountingMockMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;

namespace Cirrious.MvvmCross.Test.Mocks.Dispatchers
{
    public class CountingMockMainThreadDispatcher
        : MvxMainThreadDispatcher
          , IMvxMainThreadDispatcher
    {
        public int Count { get; set; }

        public bool RequestMainThreadAction(Action action)
        {
            Count++;
            return true;
        }
    }
}