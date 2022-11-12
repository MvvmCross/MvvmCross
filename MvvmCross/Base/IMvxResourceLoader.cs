// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace MvvmCross.Base
{
#nullable enable
    public interface IMvxResourceLoader
    {
        bool ResourceExists(string resourcePath);

        string? GetTextResource(string resourcePath);

        void GetResourceStream(string resourcePath, Action<Stream> streamAction);
    }
#nullable restore
}
