// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Core
{
    public interface IMvxSettings
    {
        bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        bool ShouldRaisePropertyChanging { get; set; }

        bool ShouldLogInpc { get; set; }
    }
}
