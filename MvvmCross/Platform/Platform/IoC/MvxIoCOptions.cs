// MvxIocOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.IoC
{
    public class MvxIocOptions : IMvxIocOptions
    {
        private IMvxPropertyInjector _injector;

        public MvxIocOptions()
        {
            TryToDetectSingletonCircularReferences = true;
            TryToDetectDynamicCircularReferences = true;
            CheckDisposeIfPropertyInjectionFails = true;
            PropertyInjectorType = typeof(MvxPropertyInjector);
            PropertyInjectorOptions = new MvxPropertyInjectorOptions();
        }

        public bool TryToDetectSingletonCircularReferences { get; set; }
        public bool TryToDetectDynamicCircularReferences { get; set; }
        public bool CheckDisposeIfPropertyInjectionFails { get; set; }
        public Type PropertyInjectorType { get; set; }
        public IMvxPropertyInjectorOptions PropertyInjectorOptions { get; set; }
    }
}