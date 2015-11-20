// IMvxLoaderPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Plugins
{
    public interface IMvxLoaderPluginManager
        : IMvxPluginManager
    {
        IDictionary<string, Func<IMvxPlugin>> Finders { get; }
    }
}