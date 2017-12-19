﻿// MvxDesignTimeChecker.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com\

#if WINDOWS_COMMON
namespace MvvmCross.BindingEx.WindowsCommon
#endif

#if WINDOWS_WPF
using System.ComponentModel;
using System.Windows;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Binding.Wpf
#endif
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;

#if WINDOWS_WPF
            if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
                return;
#endif
#if WINDOWS_COMMON
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
#endif

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxSimpleIoCContainer.Initialize();
                Mvx.RegisterSingleton(iocProvider);
            }

            if (!Mvx.CanResolve<IMvxBindingParser>())
            {
                var builder = new MvxWindowsBindingBuilder(MvxWindowsBindingBuilder.BindingType.MvvmCross);
                builder.DoRegistration();
            }
        }
    }
}