// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;

namespace MvvmCross.Plugin.Email.Platform.Wpf
{
    [MvxPlugin]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            // note that Wpf does not support IMvxComposeEmailTaskEx
        }
    }
}
