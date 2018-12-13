// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MvvmCross.Base;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;

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
            if (!IsInDesignTime)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }

            MvxSetup.RegisterSetupType<MvxWpfSetup<App>>(System.Reflection.Assembly.GetExecutingAssembly());
            var instance = MvxWpfSetupSingleton.EnsureSingletonAvailable(Application.Current.Dispatcher, new Content());
            instance.InitializeAndMonitor(null);
        }

        class App : ViewModels.MvxApplication
        {
        }

        class Content : ContentControl
        {
        }
    }
}
