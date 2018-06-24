// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows;
using MvvmCross.Base;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Wpf
{
    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTime)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }
        }

        private static bool? _isInDesignTime;

        protected static bool IsInDesignTime
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
    }
}
