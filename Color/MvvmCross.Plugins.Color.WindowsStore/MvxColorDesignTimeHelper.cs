// MvxColorDesignTimeHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.UI;
using Cirrious.CrossCore.WindowsCommon.Platform;

namespace MvvmCross.Plugins.Color.WindowsStore
{
    public class MvxColorDesignTimeHelper
        : MvxDesignTimeHelper
    {
        public MvxColorDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (Mvx.CanResolve<IMvxNativeColor>())
                return;

            var forceLoaded = new Plugin();
            forceLoaded.Load();
        }
    }
}