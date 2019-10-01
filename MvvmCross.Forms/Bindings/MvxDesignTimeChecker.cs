// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.IoC;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    public static class MvxDesignTimeChecker
    {
        private static bool _checked;

        public static void Check()
        {
            if (_checked)
                return;

            _checked = true;

            if (!IsDesignTime)
                return;

            try
            {
                if (MvxSingleton<IMvxIoCProvider>.Instance == null)
                {
                    var iocProvider = MvxIoCProvider.Initialize();
                    Mvx.IoCProvider.RegisterSingleton(iocProvider);
                }

                if (!Mvx.IoCProvider.CanResolve<IMvxBindingParser>())
                {
                    //We might want to look into returning the platform specific Forms binder
                    var builder = new MvxBindingBuilder();
                    builder.DoRegistration();
                }
            }
            catch
            {
                // ignore
            }
        }

        public static bool IsDesignTime => DesignMode.IsDesignModeEnabled;
    }
}
