// IMvxNamedInstanceRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Platform
{
    using System.Reflection;

    public interface IMvxNamedInstanceRegistry<in T>
    {
        void AddOrOverwrite(string name, T instance);

        void AddOrOverwriteFrom(Assembly assembly);
    }
}