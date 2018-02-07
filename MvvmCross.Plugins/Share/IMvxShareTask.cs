// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Share
{
    public interface IMvxShareTask
    {
        void ShareShort(string message);

        void ShareLink(string title, string message, string link);
    }
}
