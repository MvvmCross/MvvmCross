// MvxDesignTimeChecker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.ComponentModel;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Binding.Parse.Binding;

// ReSharper disable CheckNamespace
namespace Cirrious.MvvmCross.BindingEx.WindowsShared
// ReSharper restore CheckNamespace
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;
#if WINDOWS_PHONE
            if (!DesignerProperties.IsInDesignTool)
                return;
#endif
#if WINDOWS_WPF
            if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
                return;
#endif
#if NETFX_CORE
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