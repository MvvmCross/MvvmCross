// IMvxPropertyInjectorOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.IoC
{
    public interface IMvxPropertyInjectorOptions
    {
        MvxPropertyInjection InjectIntoProperties { get; }
        bool ThrowIfPropertyInjectionFails { get; }
    }
}