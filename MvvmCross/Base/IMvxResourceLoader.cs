// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    public interface IMvxResourceLoader
    {
        bool ResourceExists(string resourcePath);

        ValueTask<string> GetTextResource(string resourcePath);

        ValueTask GetResourceStream(string resourcePath, Action<Stream> streamAction);
    }
}
