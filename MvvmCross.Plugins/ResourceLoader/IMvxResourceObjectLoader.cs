// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugins.ResourceLoader
{
    public interface IMvxResourceObjectLoader<out T>
        where T : IMvxResourceObject
    {
        T Load(string namespaceKey, string typeKey, string entryKey);
    }
}