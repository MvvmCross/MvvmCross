// IMvxPluginTargetBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    using System.Collections.Generic;

    public interface IMvxPluginTargetBindingFactory : IMvxTargetBindingFactory
    {
        IEnumerable<MvxTypeAndNamePair> SupportedTypes { get; }
    }
}