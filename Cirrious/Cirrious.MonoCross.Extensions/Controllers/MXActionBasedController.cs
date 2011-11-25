using System;
using System.Collections.Generic;
using MonoCross.Navigation;
using MonoCross.Navigation.ActionResults;
using MonoCross.Navigation.Exceptions;

namespace Cirrious.MonoCross.Extensions.Controllers
{
    public abstract class MXActionBasedController<T> : MXController<T>
    {
        public const string ActionParameterName = "INDEX";
        public const string DefaultAction = "INDEX";
        public const string ActionParameterKey = "Action";

        private readonly string _defaultAction;

        protected MXActionBasedController()
            : this(DefaultAction)
        {
        }

        protected MXActionBasedController(string defaultAction)
        {
            _defaultAction = defaultAction;
        }

        public override IMXActionResult Load(Dictionary<string, string> parameters)
        {
            var actionName = ReadActionName(parameters);
            return DoAction(actionName, parameters);
        }

        protected abstract IMXActionResult DoAction(string actionName, Dictionary<string, string> parameters);

        private string ReadActionName(Dictionary<string, string> parameters)
		{
			return ReadActionName(parameters, _defaultAction);
		}
		
        private static string ReadActionName(Dictionary<string, string> parameters, string defaultAction)
        {
            string action;
            if (!parameters.TryGetValue(ActionParameterKey, out action))
            {
                // set default action if none specified
                action = defaultAction;
            }
            return action;
        }

        protected IMXActionResult ShowView(string perspective)
        {
            return new MXShowPerspectiveAction(perspective);
        }

        protected IMXActionResult ShowError(string messageFormat, params object[] formatArguments)
        {
            var exception = new MonoCrossException(messageFormat, formatArguments);
            return ShowError(exception);
        }

        protected IMXActionResult ShowError(Exception exception)
        {
            return new MXShowErrorAction(exception);
        }

        protected IMXActionResult RedirectToUrl(string url)
        {
            return new MXRedirectToUrlActionResult(url);
        }
    }

}