namespace MvvmCross.iOS.Platform
{
    using System;

    [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
    public interface IMvxIosPlatformProperties
    {
        MvxIosFormFactor FormFactor { get; }
        MvxIosDisplayDensity DisplayDensity { get; }
    }
}
