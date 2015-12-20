// IMvxWindowsAssemblyCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.BindingEx.WindowsPhone
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMvxWindowsAssemblyCache
    {
        IList<Assembly> Assemblies { get; }
    }
}