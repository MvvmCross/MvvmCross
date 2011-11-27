#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXDefaultControllerAnalyser.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Interfaces;
using MonoCross.Navigation.ActionResults;
using MonoCross.Navigation.Exceptions;

#endregion

namespace Cirrious.MonoCross.Extensions.Conventions.Default
{
    public class MXDefaultControllerAnalyser : IMXControllerAnalyser
    {
        public string GenerateUrlFor(Type type, string actionName, IDictionary<string, string> inputParameterValues)
        {
            try
            {
                var urlParts = GenerateUrlPartsFor(type, actionName, inputParameterValues);
                return string.Join(MXConventionConstants.UrlPathSeparator, urlParts);
            }
            catch (AmbiguousMatchException ambiguousMatchException)
            {
                throw ambiguousMatchException.MXWrap(
                    "action {1} is ambigious on controller {0} - sorry MXCross does not supoprt overloads at this time",
                    type.Name, actionName);
            }
        }

        private static IEnumerable<string> GenerateUrlPartsFor(Type type, string actionName,
                                                               IDictionary<string, string> inputParameterValues)
        {
            var urlParts = new List<string>()
                               {
                                   MXDefaultControllerConvention.CreateControllerConventionName(type.Name),
                               };

            if (string.IsNullOrEmpty(actionName))
                return urlParts;

            urlParts.Add(actionName);

            var methodInfo = type.GetMethod(actionName);
            if (methodInfo == null)
            {
                // this is actually allowed... but not normal behaviour
#warning TODO - trace a warning here please, especially if inputParameterValues are being ignored
                return urlParts;
            }

            urlParts.AddRange(GetUrlPartsForParameterValues(methodInfo, inputParameterValues));
            return urlParts;
        }

        private static IEnumerable<string> GetUrlPartsForParameterValues(MethodInfo methodInfo,
                                                                         IDictionary<string, string>
                                                                             inputParameterValues)
        {
            var urlParts = new List<string>();

            var parameters = methodInfo.GetParameters();
            foreach (var parameter in parameters)
            {
                string parameterValue;
                if (inputParameterValues == null ||
                    !inputParameterValues.TryGetValue(parameter.Name, out parameterValue))
                {
                    if (parameter.IsOptional)
                        break;
                    else
                        throw new KeyNotFoundException("Missing required parameter " + parameter.Name).MXWrap();
                }

#warning Should really make parameterValue url safe here :/
                urlParts.Add(parameterValue);
            }

            var inputCount = inputParameterValues == null ? 0 : inputParameterValues.Count;
            if (urlParts.Count != inputCount)
            {
                // TODO: this is really just a warning situation - it just means that the dictionary object has some unused "stuff"
            }

            return urlParts;
        }

        private static bool IsActionParameterCandidate(ParameterInfo parameterInfo)
        {
            return !parameterInfo.IsOut
                   && parameterInfo.ParameterType == typeof (string)
                   && !parameterInfo.IsOptional;
        }

        private static bool IsActionCandidate(MethodInfo methodInfo)
        {
            return methodInfo.IsPublic
                   && !methodInfo.IsStatic
                   && !methodInfo.IsGenericMethod
                   && methodInfo.ReturnType == typeof (IMXActionResult)
                   && methodInfo.GetParameters().All(p => IsActionParameterCandidate(p));
        }

        public Dictionary<string, MethodInfo> GenerateActionMap(Type type)
        {
            var actions = from methodInfo in type.GetMethods()
                          where IsActionCandidate(methodInfo)
                          select methodInfo;

#if DEBUG
            // this to a list (to stop R# complaining about multiple enumeration operations on the Linq)
            actions = actions.ToList();
            CheckActionsHaveUniqueName(actions);
#endif

            var actionMap = actions.ToDictionary(x => x.Name, x => x);
            return actionMap;
        }

        private static void CheckActionsHaveUniqueName(IEnumerable<MethodInfo> actions)
        {
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
        }
    }
}