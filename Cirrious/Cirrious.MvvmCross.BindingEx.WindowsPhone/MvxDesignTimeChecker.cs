// MvxDesignTimeChecker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;

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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
#endif
#if NETFX_CORE
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;

            // TODO - no easy way to get the assemblies at present
            var assemblies = new Assembly[0];
#endif

            var iocProvider = MvxSimpleIoCContainer.Initialise();
            Mvx.RegisterSingleton(iocProvider);


            var builder = new MvxWindowsBindingBuilder(MvxWindowsBindingBuilder.BindingType.MvvmCross, assemblies);
            builder.DoRegistration();
        }
    }
}