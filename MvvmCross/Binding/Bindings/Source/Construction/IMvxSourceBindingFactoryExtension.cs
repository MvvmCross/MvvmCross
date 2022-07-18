// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    public interface IMvxSourceBindingFactoryExtension
    {
        bool TryCreateBinding(object source, MvxPropertyToken propertyToken, List<MvxPropertyToken> remainingTokens, out IMvxSourceBinding result);
    }
}
