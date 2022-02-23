// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Java.Lang;

namespace MvvmCross.Platforms.Android
{
    public class MvxReplaceableJavaContainer : Object
    {
        public object Object { get; set; }

        public override string ToString()
        {
            return Object?.ToString() ?? string.Empty;
        }
    }
}
