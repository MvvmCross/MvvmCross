// MvxReflectionEx.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;

// ReSharper disable CheckNamespace
namespace MvvmCross.Plugins.ReflectionEx.HackShare
// ReSharper restore CheckNamespace
{
    public class MvxReflectionEx
        : IMvxReflectionEx
    {
        public AssemblyName[] GetReflectedAssembliesEx(Assembly assembly)
        {
#if NETFX_CORE || WINDOWS_PHONE
            // sadly we can't do this in WinRT or WindowsPhone :/
            return new AssemblyName[0];
#else
            return assembly.GetReferencedAssemblies();
#endif
        }
    }
}