// IMvxConsumer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Platform.IoC
{
    // just a marker interface
    [Obsolete("Prefer to use Mvx.Resolve<T> static methods now")]
    public interface IMvxConsumer
    {
    }
}