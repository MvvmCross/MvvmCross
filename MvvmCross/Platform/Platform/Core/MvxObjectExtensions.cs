// MvxObjectExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Platform.Core
{
    public static class MvxObjectExtensions
    {
        public static void DisposeIfDisposable(this object thing)
        {
            var disposable = thing as IDisposable;
            disposable?.Dispose();
        }
    }
}