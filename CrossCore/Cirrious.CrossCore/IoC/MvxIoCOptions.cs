// MvxIoCOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.IoC
{
    public class MvxIoCOptions
    {
        public MvxIoCOptions()
        {
            ExceptionOnSingletonCircularReferences = true;
            ExceptionOnDynamicCircularReferences = false;
            InjectIntoProperties = PropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
            CheckDisposeIfPropertyInjectionFails = true;
        }
        public enum PropertyInjection
        {
            None,
            MvxInjectInterfaceProperties,
            AllInterfacesProperties
        }
        public bool ExceptionOnSingletonCircularReferences { get; set; }
        public bool ExceptionOnDynamicCircularReferences { get; set; }
        public PropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }
        public bool CheckDisposeIfPropertyInjectionFails { get; set; }
    }
}