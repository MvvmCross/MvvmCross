// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Bindings
{
    public static class MvxBindingModeExtensions
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
                    MvxBindingLog.Warning("Mode of default seen for binding - assuming TwoWay");
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
                    MvxBindingLog.Warning("Mode of default seen for binding - assuming TwoWay");
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
                    MvxBindingLog.Warning("Mode of default seen for binding - assuming TwoWay");
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
