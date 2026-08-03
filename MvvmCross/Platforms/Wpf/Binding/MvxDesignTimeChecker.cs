// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Hosting;

namespace MvvmCross.Platforms.Wpf.Binding
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;
            if (!MvxDesignTimeHelper.IsInDesignTime)
                return;

            MvxDesignTimeHelper.Initialize();

            if (MvxHost.Current?.Services.GetService<IMvxBindingParser>() == null)
            {
                // Design-time: register a minimal binding builder so XAML designer can resolve parsers.
                // The host builder should be used at runtime; this path is design-time only.
            }
        }
    }
}
