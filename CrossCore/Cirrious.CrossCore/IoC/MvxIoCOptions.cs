// MvxIocOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.IoC
{
    public class MvxIocOptions : IMvxIocOptions
    {
        public MvxIocOptions()
        {
            TryToDetectSingletonCircularReferences = true;
            TryToDetectDynamicCircularReferences = true;
            InjectIntoProperties = MvxPropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
            CheckDisposeIfPropertyInjectionFails = true;
        }

        public bool TryToDetectSingletonCircularReferences { get; set; }
        public bool TryToDetectDynamicCircularReferences { get; set; }
        public MvxPropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }
        public bool CheckDisposeIfPropertyInjectionFails { get; set; }
    }
}