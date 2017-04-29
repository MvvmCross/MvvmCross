// MvxPropertyInjectorOptions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    public class MvxPropertyInjectorOptions : IMvxPropertyInjectorOptions
    {
        private static IMvxPropertyInjectorOptions _mvxInjectProperties;

        private static IMvxPropertyInjectorOptions _allProperties;

        public MvxPropertyInjectorOptions()
        {
            InjectIntoProperties = MvxPropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
        }

        public static IMvxPropertyInjectorOptions MvxInject
        {
            get
            {
                _mvxInjectProperties = _mvxInjectProperties ?? new MvxPropertyInjectorOptions
                {
                    InjectIntoProperties = MvxPropertyInjection.MvxInjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _mvxInjectProperties;
            }
        }

        public static IMvxPropertyInjectorOptions All
        {
            get
            {
                _allProperties = _allProperties ?? new MvxPropertyInjectorOptions
                {
                    InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _allProperties;
            }
        }

        public MvxPropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }
    }
}