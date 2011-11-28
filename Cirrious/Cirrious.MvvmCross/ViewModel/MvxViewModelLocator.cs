#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MvxConventionBasedController.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cirrious.MvvmCross.Conventions;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Interfaces.ViewModel;

#endregion

namespace Cirrious.MvvmCross.ViewModel
{
    public abstract class MvxViewModelLocator<TViewModel> 
        : IMvxViewModelLocator<TViewModel>
        , IMvxServiceConsumer<IMvxViewModelLocatorAnalyser>
        where TViewModel : IMvxViewModel
    {
        private readonly Dictionary<string, MethodInfo> _actionMap;
        private readonly string _defaultActionName;

        protected MvxViewModelLocator()
            : this(MvxConventionConstants.DefaultActionName)
        {
        }

        protected MvxViewModelLocator(string defaultActionName)
        {
            _actionMap = this.GetService().GenerateLocatorMap(GetType(), ViewModelType);
            _defaultActionName = defaultActionName;
        }

        public string DefaultAction { get { return _defaultActionName; } }

        public Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }

        public bool TryLoad(string actionName, IDictionary<string, string> parameters, out IMvxViewModel model)
        {
            if (string.IsNullOrEmpty(actionName))
                actionName = _defaultActionName;
            model = DoLoad(actionName, parameters);
            return true;
        }

        public bool TryLoad(string actionName, IDictionary<string, string> parameters, out TViewModel model)
        {
            if (string.IsNullOrEmpty(actionName))
                actionName = _defaultActionName;
            model = DoLoad(actionName, parameters);
            return true;
        }

        #region Protected interface

#warning Dead Code
        protected virtual TViewModel LoadUnimplementedPerspective(string actionName, IDictionary<string, string> args)
        {
            // default behaviour is to throw an error!
            throw new MvxException("Action name not found for {0} - action name {1}", typeof(TViewModel).FullName, actionName);
        }

        #endregion //Protected interface

        #region Action!

        private TViewModel DoLoad(string actionName, IDictionary<string, string> parameters)
        {
            try
            {
                MethodInfo perspectiveMethodInfo;
                if (!_actionMap.TryGetValue(actionName, out perspectiveMethodInfo))
                    return LoadUnimplementedPerspective(actionName, parameters);

                var argumentList = new List<object>();
                foreach (var parameter in perspectiveMethodInfo.GetParameters())
                {
                    string parameterValue;
                    if (!parameters.TryGetValue(parameter.Name, out parameterValue))
                        throw new MvxException("Missing parameter in call to {0} action {1} - missing parameter {2}",
                                         typeof(TViewModel).FullName, actionName, parameter.Name);
                    argumentList.Add(parameterValue);
                }

                return InvokePerspective(perspectiveMethodInfo, argumentList);
            }
            catch (ThreadAbortException)
            {
                throw;
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

        private TViewModel InvokePerspective(MethodInfo actionMethodInfo, IEnumerable<object> argumentList)
        {
            try
            {
                return (TViewModel)actionMethodInfo.Invoke(this, argumentList.ToArray());
            }
            catch (ArgumentException exception)
            {
                // this should be impossible - as we've checked the types in the GenerateLocatorMap method
                throw exception.MvxWrap("parameter type mismatch seen");
            }
            catch (TargetParameterCountException exception)
            {
                // this should be impossible as we've created the parameter array above to match the method signature
                throw exception.MvxWrap("parameter count mismatch seen");
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException != null)
                    throw exception.InnerException.MvxWrap();
                else
                    throw exception.MvxWrap("unknown action exception seen " + exception.Message);
            }
        }

        #endregion // Action!
    }
}