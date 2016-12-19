// MvxVisibilityDesignTimeHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.UI;
using MvvmCross.Platform.Uwp.Platform;

namespace MvvmCross.Plugins.Visibility.WindowsCommon
{
    public class MvxVisibilityDesignTimeHelper
        : MvxDesignTimeHelper
    {
        public MvxVisibilityDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (Mvx.CanResolve<IMvxNativeVisibility>())
                return;

            var forceVisibilityLoaded = new Plugin();
            forceVisibilityLoaded.Load();
        }
    }
}