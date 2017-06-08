// IMvxWindowsAssemblyCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Reflection;

#if WINDOWS_COMMON
namespace MvvmCross.BindingEx.WindowsCommon
#endif

#if WINDOWS_WPF
namespace MvvmCross.BindingEx.Wpf
#endif
{
    public interface IMvxWindowsAssemblyCache
    {
        IList<Assembly> Assemblies { get; }
    }
}