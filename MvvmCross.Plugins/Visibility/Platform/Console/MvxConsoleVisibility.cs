// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugin.Visibility.Platform.Console
{
    [Preserve(AllMembers = true)]
	public class MvxConsoleVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible;
        }

        #endregion Implementation of IMvxNativeVisibility
    }
}
