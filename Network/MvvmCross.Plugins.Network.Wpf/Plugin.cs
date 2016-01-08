// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;
using MvvmCross.Plugins.Network.Wpf.Reachability;

namespace MvvmCross.Plugins.Network.Wpf
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxReachability, MvxWpfReachability>();
            Mvx.RegisterType<IMvxRestClient, MvxJsonRestClient>();
            Mvx.RegisterType<IMvxJsonRestClient, MvxJsonRestClient>();
        }
    }
}