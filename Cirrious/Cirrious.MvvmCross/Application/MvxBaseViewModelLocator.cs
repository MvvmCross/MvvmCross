// MvxBaseViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Application
{
    public abstract class MvxBaseViewModelLocator
        : IMvxViewModelLocator
          , IMvxServiceConsumer
    {
        #region IMvxViewModelLocator Members

        public abstract bool TryLoad(Type viewModelType, IDictionary<string, string> parameterValueLookup,
                                     out IMvxViewModel model);

        #endregion

        protected static bool IsConvertibleParameter (ParameterInfo parameterInfo)
		{
			if (parameterInfo.IsOut)
				return false;

			if (parameterInfo.IsOptional) {
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
                                                                IDictionary<string, string> parameterValueLookup,
                                                                IEnumerable<ParameterInfo> requiredParameters)
        {
            var argumentList = new List<object>();
            foreach (var requiredParameter in requiredParameters)
            {
                string parameterValue;
                if (parameterValueLookup == null ||
                    !parameterValueLookup.TryGetValue(requiredParameter.Name, out parameterValue))
                {
                    MvxTrace.Trace("Missing parameter in call to {0} - missing parameter {1} - asssuming null - this may fail for value types!",
                                   viewModelType,
                                   requiredParameter.Name);
                    parameterValue = null;
                }

                if (requiredParameter.ParameterType == typeof (string))
                {
                    argumentList.Add(parameterValue);
                }
                else if (requiredParameter.ParameterType == typeof (bool))
                {
                    bool boolValue;
                    if (!bool.TryParse(parameterValue, out boolValue))
                    {
                        MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse boolean parameter {0} from string {1}",
                                       requiredParameter.Name, parameterValue);
                    }
                    argumentList.Add(boolValue);
                }
                else if (requiredParameter.ParameterType == typeof (int))
                {
                    int intValue;
                    if (!int.TryParse(parameterValue, out intValue))
                    {
                        MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse int parameter {0} from string {1}",
                                       requiredParameter.Name,
                                       parameterValue);
                    }
                    argumentList.Add(intValue);
                }
                else if (requiredParameter.ParameterType == typeof (long))
                {
                    long longValue;
                    if (!long.TryParse(parameterValue, out longValue))
                    {
                        MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse long parameter {0} from string {1}",
                                       requiredParameter.Name,
                                       parameterValue);
                    }
                    argumentList.Add(longValue);
                }
                else if (requiredParameter.ParameterType == typeof (double))
                {
                    double doubleValue;
                    if (!double.TryParse(parameterValue, out doubleValue))
                    {
                        MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse double parameter {0} from string {1}",
                                       requiredParameter.Name, parameterValue);
                    }
                    argumentList.Add(doubleValue);
                }
                else if (requiredParameter.ParameterType == typeof (Guid))
                {
                    Guid guidValue;
                    if (!Guid.TryParse(parameterValue, out guidValue))
                    {
                        MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse Guid parameter {0} from string {1}",
                                       requiredParameter.Name, parameterValue);
                    }
                    argumentList.Add(guidValue);
                }
                else if (requiredParameter.ParameterType.IsEnum)
                {
                    object enumValue = null;
                    try
                    {
                        enumValue = Enum.Parse(requiredParameter.ParameterType, parameterValue, true);
                    }
                    catch (Exception exception)
                    {
                        MvxTrace.Trace(MvxTraceLevel.Error, "Failed to parse enum parameter {0} from string {1}",
                                       requiredParameter.Name,
                                       parameterValue);
                    }
                    argumentList.Add(enumValue);
                }
                else
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Parameter {0} is invalid type {1}", requiredParameter.Name,
                                   requiredParameter.ParameterType.Name);
                }
            }
            return argumentList;
        }
    }
}