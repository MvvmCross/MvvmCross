// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
            if (!MvxDesignTimeHelper.IsInDesignTime)
                return;

            MvxDesignTimeHelper.Initialize();

            if (!Mvx.IoCProvider.CanResolve<IMvxBindingParser>())
            {
                var builder = new MvxWindowsBindingBuilder(bindingType: MvxWindowsBindingBuilder.BindingType.MvvmCross);
                builder.DoRegistration(Mvx.IoCProvider);
            }
        }
    }
}
