// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.Binders
{
    public interface IMvxBinder
    {
        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText);

        IEnumerable<IMvxUpdateableBinding> LanguageBind(object source, object target, string bindingText);

        IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                IEnumerable<MvxBindingDescription> bindingDescriptions);

        IMvxUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                         string partialBindingDescription);

        IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest);
    }
}