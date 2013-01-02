// MvxViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

#endregion

namespace Cirrious.MvvmCross.Application
{
    public abstract class MvxViewModelLocator
        : MvxBaseViewModelLocator
          , IMvxServiceConsumer
    {
        private readonly Dictionary<Type, MethodInfo> _locatorMap;

        protected MvxViewModelLocator()
        {
            _locatorMap = this
                .GetService<IMvxViewModelLocatorAnalyser>()
                .GenerateLocatorMethods(GetType())
                .ToDictionary(x => x.ReturnType, x => x);
        }

        #region IMvxViewModelLocator Members

        public override bool TryLoad(Type viewModelType, IDictionary<string, string> parameterValueLookup,
                                     out IMvxViewModel model)
        {
            model = DoLoad(viewModelType, parameterValueLookup);
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

                var argumentList = CreateArgumentList(viewModelType, parameters, methodInfo.GetParameters());

                return InvokeAction(methodInfo, argumentList);
            }
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