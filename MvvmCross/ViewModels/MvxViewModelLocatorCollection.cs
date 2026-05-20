// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels;

/// <summary>
/// Default implementation of <see cref="IMvxViewModelLocatorCollection"/> that
/// always returns the single registered <see cref="IMvxViewModelLocator"/>.
/// </summary>
public class MvxViewModelLocatorCollection : IMvxViewModelLocatorCollection
{
    private readonly IMvxViewModelLocator _locator;

    public MvxViewModelLocatorCollection(IMvxViewModelLocator locator)
    {
        _locator = locator;
    }

    public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request) => _locator;
}
