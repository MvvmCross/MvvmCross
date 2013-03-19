// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.Network.Touch
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxReachability, MvxReachability>();
        }
    }

	#warning Move this to a plugin (on Mac)
}