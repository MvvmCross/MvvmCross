// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModelByNameLookup
    {
        bool TryLookupByName(string name, out Type viewModelType);

        bool TryLookupByFullName(string name, out Type viewModelType);
    }
}