// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
    public enum MvxPopBackStack
    {
        /// <summary>
        /// All entries up to but not including that entry will be removed.
        /// </summary>
        None = 0,
        /// <summary>
        /// All matching entries will be consumed until one that doesn't match is found or the bottom of the stack is reached.
        /// </summary>
        Inclusive = 1
    }
}
