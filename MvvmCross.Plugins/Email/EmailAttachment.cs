// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace MvvmCross.Plugin.Email
{
    [Preserve(AllMembers = true)]
    public class EmailAttachment
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public Stream Content { get; set; }
    }
}
