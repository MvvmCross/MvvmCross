// MvxDesignTimeHelper.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;

namespace MvvmCross.Platform.Wpf.Platform
{
    using System.ComponentModel;

    using Core;
    using IoC;

    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTime)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxSimpleIoCContainer.Initialize();
                Mvx.RegisterSingleton(iocProvider);
            }
        }

        private static bool? _isInDesignTime;

        protected static bool IsInDesignTime
        {
            get
            {
                if (!_isInDesignTime.HasValue)
                {
                    _isInDesignTime =
                        (bool)
                        DesignerProperties.IsInDesignModeProperty
                                          .GetMetadata(typeof(DependencyObject))
                                          .DefaultValue;
                }

                return _isInDesignTime.Value;
            }
        }
    }
}