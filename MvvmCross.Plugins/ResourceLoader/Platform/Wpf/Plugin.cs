// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Plugins;

namespace MvvmCross.Plugin.ResourceLoader.Platform.Wpf
{
    [MvxPlugin]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxResourceLoader, MvxWpfResourceLoader>();
        }
    }
}
