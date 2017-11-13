// InlineMockMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Test.Mocks.Dispatchers
{
    public class InlineMockMainThreadDispatcher
        : MvxMainThreadDispatcher, IMvxMainThreadDispatcher
    {
        public virtual bool RequestMainThreadAction(Action action, 
            bool maskExceptions = true)
        {
            action();
            return true;
        }
    }
}