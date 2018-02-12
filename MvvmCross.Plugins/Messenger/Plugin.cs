// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Messenger
{
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        private bool _loaded;

        public void Load()
        {
            if (_loaded) return;

            Mvx.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
            _loaded = true;
        }
    }
}
