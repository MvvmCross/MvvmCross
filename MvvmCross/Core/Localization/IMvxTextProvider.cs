﻿// IMvxTextProvider.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Localization
{
    public interface IMvxTextProvider
    {
        string GetText(string namespaceKey, string typeKey, string name);

        string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs);
    }
}