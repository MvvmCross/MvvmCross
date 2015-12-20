// MvxDesignTimeHelper.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WindowsStore.Platform
{
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.IoC;

    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxSimpleIoCContainer.Initialize();
                Mvx.RegisterSingleton(iocProvider);
            }
        }

        private static bool? _isInDesignTime;

        protected static bool IsInDesignTool
        {
            get
            {
                if (!_isInDesignTime.HasValue)
                    _isInDesignTime = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
                return _isInDesignTime.Value;
            }
        }
    }
}