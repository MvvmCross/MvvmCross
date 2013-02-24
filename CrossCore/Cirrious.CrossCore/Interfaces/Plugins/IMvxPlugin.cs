// IMvxPlugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;

namespace Cirrious.CrossCore.Interfaces.Plugins
{
    public interface IMvxPlugin
    {
        void Load();
    }
}