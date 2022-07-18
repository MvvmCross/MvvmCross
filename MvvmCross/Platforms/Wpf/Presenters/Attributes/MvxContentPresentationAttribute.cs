// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Wpf.Presenters.Attributes
{
    public class MvxContentPresentationAttribute : MvxBasePresentationAttribute
    {
        public string WindowIdentifier { get; set; }
        public bool StackNavigation { get; set; } = true;
    }
}
