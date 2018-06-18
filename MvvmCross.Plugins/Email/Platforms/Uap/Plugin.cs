// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Email.Platforms.Uap
{
    [MvxPlugin]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.IoCProvider.RegisterType<IMvxComposeEmailTaskEx, MvxComposeEmailTask>();
        }
    }
}
