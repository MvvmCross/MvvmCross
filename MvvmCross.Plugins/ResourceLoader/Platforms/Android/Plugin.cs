// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.IoC;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.Android
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load(IMvxIoCProvider provider)
        {
            provider.RegisterType<IMvxResourceLoader, MvxAndroidResourceLoader>();
        }
    }
}
