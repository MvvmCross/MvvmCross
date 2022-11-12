// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Core
{
#nullable enable
    public enum MvxSetupState
    {
        Uninitialized,
        InitializingPrimary,
        InitializedPrimary,
        InitializingSecondary,
        Initialized
    }
#nullable restore
}
