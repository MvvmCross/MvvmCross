// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.Share.Wpf
{
    public class Plugin
        : IMvxPlugin          
    {
        public void Load()
        {
            // nothing to do - WinRT does not currently do share this way...
        }
    }
}