// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.IoC
{
    public class MvxIocOptions : IMvxIocOptions
    {
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
