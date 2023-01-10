// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Views
{
#nullable enable
    public interface IMvxViewFinder
    {
        Type? GetViewType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType);
    }
#nullable restore
}
