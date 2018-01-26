// MvxDesignTimeChecker.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com\

using System.ComponentModel;
using System.Windows;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Binding.Wpf
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
