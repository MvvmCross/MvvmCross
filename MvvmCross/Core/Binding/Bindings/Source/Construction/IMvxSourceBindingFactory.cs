// IMvxSourceBindingFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    using System.Collections.Generic;

    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    public interface IMvxSourceBindingFactory
    {
        IMvxSourceBinding CreateBinding(object source, string combinedPropertyName);

        IMvxSourceBinding CreateBinding(object source, IList<MvxPropertyToken> tokens);
    }
}