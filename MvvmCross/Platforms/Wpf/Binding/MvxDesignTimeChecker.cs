// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Binding.Parse.Binding;

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

            if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }

            if (!Mvx.IoCProvider.CanResolve<IMvxBindingParser>())
            {
                var builder = new MvxWindowsBindingBuilder(MvxWindowsBindingBuilder.BindingType.MvvmCross);
                builder.DoRegistration();
            }
        }
    }
}
