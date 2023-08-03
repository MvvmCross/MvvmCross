// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace MvvmCross.Binding.BindingContext
{
    public interface IMvxBindingNameRegistry
    {
        void AddOrOverwrite([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type,
            string name);

        void AddOrOverwrite<T>(Expression<Func<T, object>> nameExpression);
    }
}
