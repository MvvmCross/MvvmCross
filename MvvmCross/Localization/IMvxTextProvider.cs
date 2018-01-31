// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Localization
{
    public interface IMvxTextProvider
    {
        string GetText(string namespaceKey, string typeKey, string name);

        string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs);

        bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name);

        bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name, params object[] formatArgs);
    }
}