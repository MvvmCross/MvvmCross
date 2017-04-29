// MvxDesignTimeHelper.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.ComponentModel;
using System.Windows;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Platform.Wpf.Platform
{
    public abstract class MvxDesignTimeHelper
    {
        private static bool? _isInDesignTime;

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

        protected static bool IsInDesignTime
        {
            get
            {
                if (!_isInDesignTime.HasValue)
                    _isInDesignTime =
                        (bool)
                        DesignerProperties.IsInDesignModeProperty
                            .GetMetadata(typeof(DependencyObject))
                            .DefaultValue;

                return _isInDesignTime.Value;
            }
        }
    }
}