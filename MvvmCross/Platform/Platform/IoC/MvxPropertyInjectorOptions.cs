// MvxPropertyInjectorOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.IoC
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