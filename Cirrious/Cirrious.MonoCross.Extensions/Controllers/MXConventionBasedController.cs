#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXConventionBasedController.cs" company="Cirrious">
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
using System.Reflection;
using System.Threading;
using Cirrious.MonoCross.Extensions.Controllers.ActionResults;
using Cirrious.MonoCross.Extensions.Conventions.Default;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;
using MonoCross.Navigation.ActionResults;
using MonoCross.Navigation.Exceptions;

#endregion

namespace Cirrious.MonoCross.Extensions.Controllers
{
#warning Need to move IMXControllerAnalyser into "Services"

    public abstract class MXConventionBasedController : MXActionBasedController, IMXConventionBasedController
    {
        public static IMXControllerAnalyser ControllerAnalyser { get; set; }

        static MXConventionBasedController()
        {
            ControllerAnalyser = new MXDefaultControllerAnalyser();
        }

        private readonly Dictionary<string, MethodInfo> _actionMap;
        private string _currentAction;

        protected MXConventionBasedController()
            : base()
        {
            _actionMap = MXConventionBasedController.ControllerAnalyser.GenerateActionMap(GetType());
        }

        protected MXConventionBasedController(string defaultActionName)
            : base(defaultActionName)
        {
            _actionMap = MXConventionBasedController.ControllerAnalyser.GenerateActionMap(GetType());
        }

        #region IMXConventionBasedController

        public virtual IMXControllerConvention Convention
        {
            get { return new MXDefaultControllerConvention(this.GetType().Name, _defaultActionName, _actionMap.Values); }
        }

        #endregion

        #region Protected interface

        protected virtual IMXActionResult OnUnimplementedAction(string actionName, IDictionary<string, string> args)
        {
            // default behaviour is to show an error!
            return ShowError("Action name not found for {0} - action name {1}", this.GetType().FullName, actionName);
        }

        protected IMXActionResult ShowView<TViewModel>(TViewModel viewModel)
        {
            return ShowView(_currentAction, viewModel);
        }

        protected IMXActionResult RedirectTo<TTarget>() where TTarget : IMXConventionBasedController
        {
            return new MXRedirectToActionResult<TTarget>();    
        }

        protected IMXActionResult RedirectTo<TTarget>(string actionName) where TTarget : IMXConventionBasedController
        {
            return new MXRedirectToActionResult<TTarget>(actionName);
        }

        protected IMXActionResult RedirectTo<TTarget>(string actionName, object parameterObject) where TTarget : IMXConventionBasedController
        {
            return new MXRedirectToActionResult<TTarget>(actionName, parameterObject);
        }

        protected IMXActionResult RedirectTo<TTarget>(string actionName, IDictionary<string, string> parameterDictionary) where TTarget : IMXConventionBasedController
        {
            return new MXRedirectToActionResult<TTarget>(actionName, parameterDictionary);
        }

        #endregion //Protected interface

        #region Action!

        protected override IMXActionResult DoAction(string actionName, Dictionary<string, string> parameters)
        {
#if DEBUG
            if (_currentAction != null)
                throw new MonoCrossException(
                    "_currentAction must be null when we call DoAction - if it isn't then this suggests that multiple Actions have been invoked concurrently");
#endif
            try
            {
                _currentAction = actionName;

                MethodInfo actionMethodInfo;
                if (!_actionMap.TryGetValue(actionName, out actionMethodInfo))
                    return OnUnimplementedAction(actionName, parameters);

                var argumentList = new List<object>();
                foreach (var parameter in actionMethodInfo.GetParameters())
                {
                    string parameterValue;
                    if (!parameters.TryGetValue(parameter.Name, out parameterValue))
                        return ShowError("Missing parameter in call to {0} action {1} - missing parameter {2}",
                                         this.GetType().FullName, actionName, parameter.Name);
                    argumentList.Add(parameterValue);
                }

                return InvokeAction(actionMethodInfo, argumentList);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                return ShowError(exception);
            }
            finally
            {
                _currentAction = null;
            }
        }

        private IMXActionResult InvokeAction(MethodInfo actionMethodInfo, List<object> argumentList)
        {
            try
            {
                return (IMXActionResult) actionMethodInfo.Invoke(this, argumentList.ToArray());
            }
            catch (ArgumentException exception)
            {
                // this should be impossible - as we've checked the types in the GenerateActionMap method
                return ShowError("parameter type mismatch seen " + exception.Message);
            }
            catch (TargetParameterCountException exception)
            {
                // this should be impossible as we've created the parameter array above to match the method signature
                return ShowError("parameter count mismatch seen " + exception.Message);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException != null)
                    return ShowError(exception.InnerException);
                else
                    return ShowError("unknown action exception seen " + exception.Message);
            }
        }

        #endregion // Action!
    }
}