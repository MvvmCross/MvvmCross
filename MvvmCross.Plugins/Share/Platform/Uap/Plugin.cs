﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base.Plugins;

namespace MvvmCross.Plugin.Share.Platform.Uap
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
