// IMvxTvosPlatformProperties.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.tvOS.Platform
{
    [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
    public interface IMvxTvosPlatformProperties
    {
        MvxTvosFormFactor FormFactor { get; }
        MvxTvosDisplayDensity DisplayDensity { get; }
    }
}