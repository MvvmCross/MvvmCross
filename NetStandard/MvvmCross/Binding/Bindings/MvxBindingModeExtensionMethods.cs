﻿// MvxBindingModeExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Bindings
{
    public static class MvxBindingModeExtensionMethods
    {
        public static MvxBindingMode IfDefault(this MvxBindingMode bindingMode, MvxBindingMode modeIfDefault)
        {
            if (bindingMode == MvxBindingMode.Default)
                return modeIfDefault;
            return bindingMode;
        }

        public static bool RequireSourceObservation(this MvxBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case MvxBindingMode.Default:
                    MvxLog.InternalLogInstance.Warn("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case MvxBindingMode.OneWay:
                case MvxBindingMode.TwoWay:
                    return true;

                case MvxBindingMode.OneTime:
                case MvxBindingMode.OneWayToSource:
                    return false;

                default:
                    throw new MvxException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequiresTargetObservation(this MvxBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case MvxBindingMode.Default:
                    MvxLog.InternalLogInstance.Warn("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case MvxBindingMode.OneWay:
                case MvxBindingMode.OneTime:
                    return false;

                case MvxBindingMode.TwoWay:
                case MvxBindingMode.OneWayToSource:
                    return true;

                default:
                    throw new MvxException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequireTargetUpdateOnFirstBind(this MvxBindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case MvxBindingMode.Default:
                    MvxLog.InternalLogInstance.Warn("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case MvxBindingMode.OneWay:
                case MvxBindingMode.OneTime:
                case MvxBindingMode.TwoWay:
                    return true;

                case MvxBindingMode.OneWayToSource:
                    return false;

                default:
                    throw new MvxException("Unexpected ActualBindingMode");
            }
        }
    }
}
