// MvxStringToTypeParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Platform
{
#warning Should this be an interface/service?
    public static class MvxStringToTypeParser
    {
        public static bool TypeSupported(Type targetType)
        {
            return targetType == typeof (string)
                    || targetType == typeof (int)
                    || targetType == typeof (long)
                    || targetType == typeof (double)
                    || targetType == typeof (bool)
                    || targetType == typeof (Guid)
                    || targetType.IsEnum;
        }

        public static object ReadValue(string rawValue, Type targetType, string hint)
        {
            if (targetType == typeof (string))
            {
                return rawValue;
            }

            if (targetType == typeof (bool))
            {
                bool boolValue;
                if (!bool.TryParse(rawValue, out boolValue))
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse boolean parameter {0} from string {1}",
                                   hint, rawValue);
                }
                return boolValue;
            }

            if (targetType == typeof (int))
            {
                int intValue;
                if (!int.TryParse(rawValue, out intValue))
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse int parameter {0} from string {1}",
                                   hint,
                                   rawValue);
                }
                return intValue;
            }

            if (targetType == typeof (long))
            {
                long longValue;
                if (!long.TryParse(rawValue, out longValue))
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse long parameter {0} from string {1}",
                                   hint,
                                   rawValue);
                }
                return longValue;
            }

            if (targetType == typeof (double))
            {
                double doubleValue;
                if (!double.TryParse(rawValue, out doubleValue))
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse double parameter {0} from string {1}",
                                   hint, rawValue);
                }
                return doubleValue;
            }

            if (targetType == typeof (Guid))
            {
                Guid guidValue;
                if (!Guid.TryParse(rawValue, out guidValue))
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse Guid parameter {0} from string {1}",
                                   hint, rawValue);
                }
                return guidValue;
            }

            if (targetType.IsEnum)
            {
                object enumValue = null;
                try
                {
                    enumValue = Enum.Parse(targetType, rawValue, true);
                }
                catch (Exception exception)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse enum parameter {0} from string {1}",
                                   hint,
                                   rawValue);
                }
                return enumValue;
            }

            MvxTrace.Trace(MvxTraceLevel.Error, "Parameter {0} is invalid targetType {1}", hint,
                           targetType.Name);
            return null;
        }
    }
}