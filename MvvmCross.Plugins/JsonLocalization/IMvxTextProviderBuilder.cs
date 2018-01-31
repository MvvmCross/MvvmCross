// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Localization;

namespace MvvmCross.Plugins.JsonLocalization
{
    public interface IMvxTextProviderBuilder
    {
        IMvxTextProvider TextProvider { get; }

        void LoadResources(string whichLocalizationFolder);
    }
}