// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using MvvmCross.Plugins.Network.Rest;

namespace MvvmCross.Plugins.Network.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
#warning TODO - WINDOWS PHONE SHOULD ADD IMvxReachability!
#warning TODO - WINDOWS PHONE MUST ADD GZIP COMPRESSION!
            //Mvx.RegisterType<IMvxReachability, MvxReachability>();
            Mvx.RegisterType<IMvxRestClient, MvxJsonRestClient>();
            Mvx.RegisterType<IMvxJsonRestClient, MvxJsonRestClient>();
        }
    }
}