// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.Views;

namespace MvvmCross.Platform.Uap.Attributes
{
    public sealed class MvxRegionPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxRegionPresentationAttribute(string regionName = null)
        {
            Name = regionName;
        }

        public string Name { get; private set; }
    }
}
