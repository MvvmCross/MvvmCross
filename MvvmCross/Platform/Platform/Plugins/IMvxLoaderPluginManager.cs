// IMvxLoaderPluginManager.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Plugins
{
    using System;
    using System.Collections.Generic;

    public interface IMvxLoaderPluginManager
        : IMvxPluginManager
    {
        IDictionary<string, Func<IMvxPlugin>> Finders { get; }
    }
}