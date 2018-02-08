// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Base.Core;
using MvvmCross.Base.IoC;
using MvvmCross.Binding;
using MvvmCross.Binding.Parse.Binding;
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

            // if Application.Current == null Forms is in design mode
            if (Application.Current != null)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                Mvx.RegisterSingleton(iocProvider);
            }

            if (!Mvx.CanResolve<IMvxBindingParser>())
            {
                //We might want to look into returning the platform specific Forms binder
                var builder = new MvxBindingBuilder();
                builder.DoRegistration();
            }
        }
    }
}
