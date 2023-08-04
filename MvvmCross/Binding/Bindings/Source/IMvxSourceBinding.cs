// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Binding.Bindings.Source
{
    public interface IMvxSourceBinding : IMvxBinding
    {
        Type SourceType { get; }

        [RequiresUnreferencedCode("Type is accessed dynamically and public constructors are invoked")]
        void SetValue(object value);

        event EventHandler Changed;

        object GetValue();
    }
}
