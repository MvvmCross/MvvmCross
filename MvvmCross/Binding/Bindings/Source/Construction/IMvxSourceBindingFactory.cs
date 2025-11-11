// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    public interface IMvxSourceBindingFactory
    {
        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        IMvxSourceBinding CreateBinding(object source, string combinedPropertyName);

        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        IMvxSourceBinding CreateBinding(object source, IList<IMvxPropertyToken> tokens);
    }
}
