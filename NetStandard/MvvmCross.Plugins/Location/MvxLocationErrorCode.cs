// MvxLocationErrorCode.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.Location
{
    public enum MvxLocationErrorCode
    {
        ServiceUnavailable,
        PermissionDenied,
        PositionUnavailable,
        Timeout,
        Network,
        Canceled
    }
}