// IMvxIocOptions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Platform.IoC
{
    public interface IMvxIocOptions
    {
        bool TryToDetectSingletonCircularReferences { get; }
        bool TryToDetectDynamicCircularReferences { get; }
        bool CheckDisposeIfPropertyInjectionFails { get; }
        Type PropertyInjectorType { get; }
        IMvxPropertyInjectorOptions PropertyInjectorOptions { get; }
    }
}