﻿#region Copyright

// <copyright file="MvxBaseViewModelLocator.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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

        protected static bool IsConvertibleParameter(ParameterInfo parameterInfo)
        {
            if (parameterInfo.IsOut)
                return false;

            if (parameterInfo.IsOptional)
                return false;

            if (parameterInfo.ParameterType != typeof (string)
                && parameterInfo.ParameterType != typeof (int)
                && parameterInfo.ParameterType != typeof (long)
                && parameterInfo.ParameterType != typeof (double)
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
                    MvxTrace.Trace("Missing parameter in call to {0} - missing parameter {1} - asssuming null",
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