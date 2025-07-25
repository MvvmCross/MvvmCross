// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    public interface IMvxSourceBindingFactoryExtension
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        bool TryCreateBinding(
            object source,
            IMvxPropertyToken propertyToken,
            List<IMvxPropertyToken> remainingTokens,
            out IMvxSourceBinding result);
    }
}
