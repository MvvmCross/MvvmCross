#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXConventionConstants.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Conventions;
using MonoCross.Navigation;
using MonoCross.Navigation.ActionResults;
using MonoCross.Navigation.Exceptions;

#endregion

namespace Cirrious.MonoCross.Extensions.Controllers
{
    public abstract class MXActionBasedController : MXController
    {
        protected readonly string _defaultActionName;

        protected MXActionBasedController()
            : this(MXConventionConstants.DefaultAction)
        {
        }

        protected MXActionBasedController(string defaultActionName)
        {
            _defaultActionName = defaultActionName;
        }

        public override IMXActionResult Load(Dictionary<string, string> parameters)
        {
            var actionName = ReadActionName(parameters, _defaultActionName);
            return DoAction(actionName, parameters);
        }

        protected abstract IMXActionResult DoAction(string actionName, Dictionary<string, string> parameters);

        protected IMXActionResult ShowView<TViewModel>(string perspective, TViewModel viewModel)
        {
            return new MXShowPerspectiveAction<TViewModel>(viewModel, perspective);
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

        private static string ReadActionName(Dictionary<string, string> parameters, string defaultAction)
        {
            string action;
            if (!parameters.TryGetValue(MXConventionConstants.ActionParameterKeyName, out action))
            {
                // set default action if none specified
                action = defaultAction;
            }
            return action;
        }
    }
}