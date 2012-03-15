#region Copyright
// <copyright file="MvxViewModelLocator.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;

#endregion

namespace Cirrious.MvvmCross.Application
{
    public abstract class MvxViewModelLocator
        : IMvxViewModelLocator
        , IMvxServiceConsumer<IMvxViewModelLocatorAnalyser>
    {
        private readonly Dictionary<Type, MethodInfo> _locatorMap;

        protected MvxViewModelLocator()
        {
            _locatorMap = this
                .GetService()
                .GenerateLocatorMethods(GetType())
                .ToDictionary(x => x.ReturnType, x => x);
        }

        #region IMvxViewModelLocator Members

        public bool TryLoad(Type viewModelType, IDictionary<string, string> parameters, out IMvxViewModel model)
        {
            model = DoLoad(viewModelType, parameters);
            return true;
        }

        #endregion

        #region Protected interface

        protected virtual IMvxViewModel LoadUnimplementedAction(Type viewModelType, IDictionary<string, string> args)
        {
            // default behaviour is to throw an error!
            throw new MvxException("ViewModel not found for {0}", viewModelType.FullName);
        }

        #endregion //Protected interface

        #region Action!

        private IMvxViewModel DoLoad(Type viewModelType, IDictionary<string, string> parameters)
        {
            try
            {
                MethodInfo methodInfo;
                if (!_locatorMap.TryGetValue(viewModelType, out methodInfo))
                    return LoadUnimplementedAction(viewModelType, parameters);

                var argumentList = new List<object>();
                foreach (var parameter in methodInfo.GetParameters())
                {
                    string parameterValue;
                    if (parameters == null ||
                        !parameters.TryGetValue(parameter.Name, out parameterValue))
                    {
                        MvxTrace.Trace("Missing parameter in call to {0} - missing parameter {1} - asssuming null", viewModelType,
                                       parameter.Name);
                        parameterValue = null;
                    }
                    argumentList.Add(parameterValue);
                }

                return InvokeAction(methodInfo, argumentList);
            }
#if !NETFX_CORE
            catch (ThreadAbortException)
            {
                throw;
            }
#endif
            catch (MvxException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap();
            }
        }

        private IMvxViewModel InvokeAction(MethodInfo methodInfo, IEnumerable<object> argumentList)
        {
            try
            {
                return (IMvxViewModel) methodInfo.Invoke(this, argumentList.ToArray());
            }
            catch (ArgumentException exception)
            {
                // this should be impossible - as we've checked the types in the GenerateLocatorMethods method
                throw exception.MvxWrap("parameter type mismatch seen");
            }
            catch (TargetParameterCountException exception)
            {
                // this should be impossible as we've created the parameter array above to match the method signature
                throw exception.MvxWrap("parameter count mismatch seen");
            }
            catch (TargetInvocationException exception)
            {
                MvxTrace.Trace("Exception seen during viewmodel create " + exception.Message);
                if (exception.InnerException != null)
                    throw exception.InnerException.MvxWrap();
                else
                    throw exception.MvxWrap("unknown action exception seen " + exception.Message);
            }
        }

        #endregion // Action!
    }
}