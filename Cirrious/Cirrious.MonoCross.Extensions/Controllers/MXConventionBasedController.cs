using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoCross.Navigation.ActionResults;
using MonoCross.Navigation.Exceptions;

namespace Cirrious.MonoCross.Extensions.Controllers
{
    public abstract class MXConventionBasedController<T> : MXActionBasedController<T>
    {
        private readonly Dictionary<string, MethodInfo> _actionMap;
        private string _currentAction;

        protected MXConventionBasedController()
            : base()
        {
            _actionMap = GenerateActionMap(GetType());
        }

        protected MXConventionBasedController(string defaultAction)
            : base(defaultAction)
        {
            _actionMap = GenerateActionMap(GetType());
        }

        private static Dictionary<string, MethodInfo> GenerateActionMap(Type type)
        {
            var actions = from methodInfo in type.GetMethods()
                          where methodInfo.IsPublic
                                 && !methodInfo.IsStatic 
                                 && !methodInfo.IsGenericMethod 
                          where methodInfo.ReturnType == typeof (IMXActionResult)
                          let parameters = methodInfo.GetParameters()
                          where parameters.All(x => !x.IsOut 
                                                        && x.ParameterType == typeof(string) 
                                                        && !x.IsOptional)
                          select methodInfo;

#if DEBUG
            // just convert this to a list (to stop R# complaining about multiple enumerations on the Linq)
            actions = actions.ToList();
            var actionsWithMoreThanOneMethod = from action in actions
                                               group action by action.Name
                                               into grouped
                                               where grouped.Count() > 1
                                               select new {name = grouped.Key};

            var actionsWithMoreThanOneMethodList = actionsWithMoreThanOneMethod.ToList();
            if (actionsWithMoreThanOneMethodList.Count > 0)
                throw new MonoCrossException(
                    "You muppet - you have built a controller with multiple actions of the same name: " +
                    string.Join(",", actionsWithMoreThanOneMethodList));
#endif
            var actionMap = actions.ToDictionary(x => x.Name, x => x);
            return actionMap;
        }

        protected IMXActionResult ShowView()
        {
            return ShowView(_currentAction);
        }

        protected override IMXActionResult DoAction(string actionName, Dictionary<string, string> parameters)
        {
#if DEBUG
            if (_currentAction != null)
                throw new MonoCrossException("_currentAction must be null when we call DoAction - if it isn't then this suggests that multiple Actions have been invoked concurrently");
#endif
            try
            {
                _currentAction = actionName;

                MethodInfo actionMethodInfo;
                if (!_actionMap.TryGetValue(actionName, out actionMethodInfo))
                    return ShowError("Action name not found for {0} - action name {1}", this.GetType().FullName, actionName);

                var argumentList = new List<object>();
                foreach (var parameter in actionMethodInfo.GetParameters())
                {
                    string parameterValue;
                    if (!parameters.TryGetValue(parameter.Name, out parameterValue))
                        return ShowError("Missing parameter in call to {0} action {1} - missing parameter {2}", this.GetType().FullName, actionName, parameter.Name);
                    argumentList.Add(parameterValue);
                }

                return InvokeAction(actionMethodInfo, argumentList);
            }
            catch (System.Threading.ThreadAbortException)
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
                return (IMXActionResult)actionMethodInfo.Invoke(this, argumentList.ToArray());
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
    }
}