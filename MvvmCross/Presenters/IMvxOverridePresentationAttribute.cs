// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
#nullable enable
    public interface IMvxOverridePresentationAttribute
    {
        MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request);
    }
#nullable restore
}
