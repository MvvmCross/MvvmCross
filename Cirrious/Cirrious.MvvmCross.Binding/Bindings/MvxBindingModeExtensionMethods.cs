// MvxBindingModeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Bindings
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
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                          "Mode of default seen for binding - assuming OneWay");
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
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                          "Mode of default seen for binding - assuming OneWay");
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
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                          "Mode of default seen for binding - assuming OneWay");
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