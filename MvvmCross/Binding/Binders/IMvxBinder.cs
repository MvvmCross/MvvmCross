// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.Binders
{
#nullable enable
    public interface IMvxBinder
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                IEnumerable<MvxBindingDescription> bindingDescriptions);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IEnumerable<IMvxUpdateableBinding> LanguageBind(object source, object target, string bindingText);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IMvxUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                         string partialBindingDescription);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest);
    }
#nullable restore
}
