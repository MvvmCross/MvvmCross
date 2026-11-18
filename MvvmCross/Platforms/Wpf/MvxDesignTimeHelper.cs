// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows;
using MvvmCross.Hosting;

namespace MvvmCross.Platforms.Wpf
{
    internal static class MvxDesignTimeHelper
    {
        private static bool? _isInDesignTime;

        public static bool IsInDesignTime
        {
            get
            {
                if (!_isInDesignTime.HasValue)
                {
                    _isInDesignTime =
                        (bool)
                        DesignerProperties.IsInDesignModeProperty
                            .GetMetadata(typeof(DependencyObject))
                            .DefaultValue;
                }

                return _isInDesignTime.Value;
            }
        }

        public static void Initialize()
        {
            // Design-time initialization is no longer needed with the host builder pattern.
            // If you need design-time data, create a view model directly in your XAML designer.
        }
    }
}
