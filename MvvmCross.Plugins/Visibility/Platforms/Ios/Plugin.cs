// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.Ios
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();
            Mvx.IoCProvider.RegisterSingleton<IMvxNativeVisibility>(new MvxIosVisibility());
        }
    }
}
