// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.BindingContext
{
    public interface IMvxBindingContext
        : IMvxDataConsumer
    {
        event EventHandler DataContextChanged;

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IMvxBindingContext Init(object dataContext, object firstBindingKey, IEnumerable<MvxBindingDescription> firstBindingValue);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IMvxBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue);

        void RegisterBinding(object target, IMvxUpdateableBinding binding);

        void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings);

        void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding);

        void ClearBindings(object clearKey);

        void ClearAllBindings();

        void DelayBind(Action action);
    }
}
