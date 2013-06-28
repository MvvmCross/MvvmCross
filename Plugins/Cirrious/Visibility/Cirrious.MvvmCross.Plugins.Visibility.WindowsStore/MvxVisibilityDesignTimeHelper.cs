// MvxVisibilityDesignTimeHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.UI;
using Cirrious.CrossCore.WindowsStore.Platform;

namespace Cirrious.MvvmCross.Plugins.Visibility.WindowsStore
{
    public class MvxVisibilityDesignTimeHelper
        : MvxDesignTimeHelper
    {
        public MvxVisibilityDesignTimeHelper()
        {
            if (Mvx.CanResolve<IMvxNativeVisibility>())
                return;

            var forceVisibilityLoaded = new Plugin();
            forceVisibilityLoaded.Load();
        }
    }
}