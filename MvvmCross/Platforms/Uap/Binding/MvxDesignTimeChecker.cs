// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Hosting;
using Windows.ApplicationModel;

namespace MvvmCross.Platforms.Uap.Binding
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

            if (MvxHost.Current?.Services.GetService<IMvxBindingParser>() == null)
            {
                // Design-time: binding setup is a no-op with the host builder pattern.
                // Configure the host builder at startup; this path is design-time only.
            }
        }
    }
}
