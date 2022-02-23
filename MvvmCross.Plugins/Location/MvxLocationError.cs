// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Location
{
    [Preserve(AllMembers = true)]
    public class MvxLocationError
    {
        public MvxLocationError(MvxLocationErrorCode code)
        {
            Code = code;
        }

        public MvxLocationErrorCode Code { get; private set; }
    }
}
