// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public interface IMvxPluginTargetBindingFactory : IMvxTargetBindingFactory
    {
        IEnumerable<MvxTypeAndNamePair> SupportedTypes { get; }
    }
}
