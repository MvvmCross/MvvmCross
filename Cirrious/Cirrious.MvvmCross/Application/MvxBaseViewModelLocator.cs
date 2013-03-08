// MvxBaseViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Application
{
    public abstract class MvxBaseViewModelLocator
        : IMvxViewModelLocator
    {
        #region IMvxViewModelLocator Members

        public abstract bool TryLoad(Type viewModelType,
                                     IMvxBundle parameterValueLookup,
                                     IMvxBundle savedState,
                                     out IMvxViewModel model);

        #endregion

        protected static bool IsConvertibleParameter(ParameterInfo parameterInfo)
        {
            if (parameterInfo.IsOut)
                return false;

            if (parameterInfo.IsOptional)
            {
                MvxTrace.Trace("Warning - optional constructor parameters can behave oddly on different platforms");
            }

            if (parameterInfo.ParameterType != typeof (string)
                && parameterInfo.ParameterType != typeof (int)
                && parameterInfo.ParameterType != typeof (long)
                && parameterInfo.ParameterType != typeof (double)
                && parameterInfo.ParameterType != typeof (bool)
                && parameterInfo.ParameterType != typeof (Guid)
                && !parameterInfo.ParameterType.IsEnum)
            {
                return false;
            }

            return true;
        }

        protected static IEnumerable<object> CreateArgumentList(Type viewModelType,
                                                                IMvxBundle bundle,
                                                                IEnumerable<ParameterInfo> requiredParameters)
        {
            var argumentList = new List<object>();
            var parameterValueLookup = bundle.Data;
            foreach (var requiredParameter in requiredParameters)
            {
                string parameterValue;
                if (parameterValueLookup == null ||
                    !parameterValueLookup.TryGetValue(requiredParameter.Name, out parameterValue))
                {
                    MvxTrace.Trace(
                        "Missing parameter in call to {0} - missing parameter {1} - asssuming null - this may fail for value types!",
                        viewModelType,
                        requiredParameter.Name);
                    parameterValue = null;
                }

                var value = MvxHackRenameNeeded.ReadValue(parameterValue, requiredParameter.ParameterType, requiredParameter.Name);
                argumentList.Add(value);
            }
            return argumentList;
        }
    }

    public static class MvxHackRenameNeeded
    {
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