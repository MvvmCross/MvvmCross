// MvxObjectExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Core
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