// MvxStringToTypeParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Platform
{
#warning Should this be an interface/service?
#warning This should be plugginable - crying out to be made OO 
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

        public static object ReadValue(string rawValue, Type targetType, string fieldOrParameterName)
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
                    MvxTrace.Error("Failed to parse boolean parameter {0} from string {1}",
                                   fieldOrParameterName, rawValue);
                }
                return boolValue;
            }

            if (targetType == typeof (int))
            {
                int intValue;
                if (!int.TryParse(rawValue, out intValue))
                {
                    MvxTrace.Error("Failed to parse int parameter {0} from string {1}",
                                   fieldOrParameterName,
                                   rawValue);
                }
                return intValue;
            }

            if (targetType == typeof (long))
            {
                long longValue;
                if (!long.TryParse(rawValue, out longValue))
                {
                    MvxTrace.Error("Failed to parse long parameter {0} from string {1}",
                                   fieldOrParameterName,
                                   rawValue);
                }
                return longValue;
            }

            if (targetType == typeof (double))
            {
                double doubleValue;
                if (!double.TryParse(rawValue, out doubleValue))
                {
                    MvxTrace.Error("Failed to parse double parameter {0} from string {1}",
                                   fieldOrParameterName, rawValue);
                }
                return doubleValue;
            }

            if (targetType == typeof (Guid))
            {
                Guid guidValue;
                if (!Guid.TryParse(rawValue, out guidValue))
                {
                    MvxTrace.Error("Failed to parse Guid parameter {0} from string {1}",
                                   fieldOrParameterName, rawValue);
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
                    MvxTrace.Error("Failed to parse enum parameter {0} from string {1}",
                                   fieldOrParameterName,
                                   rawValue);
                }
                if (enumValue == null)
                {
                    try
                    {
                        // we set enumValue to 0 here - just have to hope that's the default
                        enumValue = Enum.ToObject(targetType, 0);
                    }
                    catch (Exception)
                    {
                        MvxTrace.Error("Failed to create default enum value for {0} - will return null",
                                       fieldOrParameterName);
                    }
                }
                return enumValue;
            }

            MvxTrace.Error("Parameter {0} is invalid targetType {1}", fieldOrParameterName,
                           targetType.Name);
            return null;
        }
    }
}