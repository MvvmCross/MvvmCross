// IMvxPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Interfaces.Plugins
{
    public interface IMvxPluginManager
    {
        bool IsPluginLoaded<T>() where T : IMvxPluginLoader;
        void EnsureLoaded<T>() where T : IMvxPluginLoader;
    }

    public interface IMvxPluginLoader
    {
        void EnsureLoaded();
    }
}