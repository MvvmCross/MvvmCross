// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.IoC
{
    public class MvxPropertyInjectorOptions : IMvxPropertyInjectorOptions
    {
        public MvxPropertyInjectorOptions()
        {
            InjectIntoProperties = MvxPropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
        }

        public MvxPropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }

        private static IMvxPropertyInjectorOptions _mvxInjectProperties;

        public static IMvxPropertyInjectorOptions MvxInject
        {
            get
            {
                _mvxInjectProperties = _mvxInjectProperties ?? new MvxPropertyInjectorOptions()
                {
                    InjectIntoProperties = MvxPropertyInjection.MvxInjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _mvxInjectProperties;
            }
        }

        private static IMvxPropertyInjectorOptions _allProperties;

        public static IMvxPropertyInjectorOptions All
        {
            get
            {
                _allProperties = _allProperties ?? new MvxPropertyInjectorOptions()
                {
                    InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _allProperties;
            }
        }
    }
}
