﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Base.UI;

namespace MvvmCross.Plugin.Visibility.Platform.iOS
{
    [Preserve(AllMembers = true)]
	public class MvxIosVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility;
        }

        #endregion Implementation of IMvxNativeVisibility
    }
}
