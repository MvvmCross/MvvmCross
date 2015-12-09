// IMvxTouchPlatformProperties.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Platform
{
    using System;

    [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
    public interface IMvxTouchPlatformProperties
    {
        MvxTouchFormFactor FormFactor { get; }
        MvxTouchDisplayDensity DisplayDensity { get; }
    }
}