// MvxDesignTimeHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using System.ComponentModel;

namespace Cirrious.CrossCore.Wpf.Platform
{
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
                                          .GetMetadata(typeof(System.Windows.DependencyObject))
                                          .DefaultValue;
                }

                return _isInDesignTime.Value;
            }
        }
    }
}