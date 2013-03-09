// MvxBaseViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform;

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

            if (!MvxStringToTypeParser.TypeSupported(parameterInfo.ParameterType))
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

                var value = MvxStringToTypeParser.ReadValue(parameterValue, requiredParameter.ParameterType,
                                                          requiredParameter.Name);
                argumentList.Add(value);
            }
            return argumentList;
        }
    }
}