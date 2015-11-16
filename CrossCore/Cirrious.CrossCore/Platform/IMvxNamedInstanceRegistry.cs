// IMvxNamedInstanceRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;

namespace Cirrious.CrossCore.Platform
{
    public interface IMvxNamedInstanceRegistry<in T>
    {
        void AddOrOverwrite(string name, T instance);
        void AddOrOverwriteFrom(Assembly assembly);
    }
}