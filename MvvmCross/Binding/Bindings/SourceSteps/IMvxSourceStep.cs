// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    public interface IMvxSourceStep : IMvxBinding
    {
        Type TargetType { get; set; }
        Type SourceType { get; }

        void SetValue(object value);

        event EventHandler Changed;

        object GetValue();

        object DataContext
        {
            get;
            [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
            set;
        }
    }
}
