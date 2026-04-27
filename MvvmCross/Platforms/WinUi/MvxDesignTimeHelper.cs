// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Hosting;
using Windows.ApplicationModel;

namespace MvvmCross.Platforms.WinUi
{
    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (MvxHost.Current == null)
            {
                // Design-time host initialization is handled by the host builder.
            }
        }

        private static bool? _isInDesignTime;

        protected static bool IsInDesignTool
        {
            get
            {
                if (!_isInDesignTime.HasValue)
                    _isInDesignTime = DesignMode.DesignModeEnabled;
                return _isInDesignTime.Value;
            }
        }
    }
}
