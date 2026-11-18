// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Hosting;
using Windows.ApplicationModel;

namespace MvvmCross.Platforms.WinUi.Binding
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;

            if (!DesignMode.DesignModeEnabled)
                return;

            if (MvxHost.Current == null)
                return;

            if (MvxHost.Current?.Services.GetService<IMvxBindingParser>() == null)
            {
                // Design-time minimal initialization is now a no-op since the host builder handles service registration.
            }
        }
    }
}
