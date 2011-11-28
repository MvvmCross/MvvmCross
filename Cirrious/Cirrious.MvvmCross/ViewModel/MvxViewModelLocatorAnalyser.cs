using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Services;

namespace Cirrious.MvvmCross.ViewModel
{
    public class MvxViewModelLocatorAnalyser : IMvxViewModelLocatorAnalyser
    {
        public Dictionary<string, MethodInfo> GenerateLocatorMap(Type locatorType, Type viewModelType)
        {
            var locators = from methodInfo in locatorType.GetMethods()
                           where IsLocatorCandidate(methodInfo, viewModelType)
                          select methodInfo;

#if DEBUG
            // this to a list (to stop R# complaining about multiple enumeration operations on the Linq)
            locators = locators.ToList();
            CheckLocatorsHaveUniqueName(locators);
#endif

            var actionMap = locators.ToDictionary(x => x.Name, x => x);
            return actionMap;
        }

        private static void CheckLocatorsHaveUniqueName(IEnumerable<MethodInfo> actions)
        {
            var locatorsWithMoreThanOneMethod = from action in actions
                                               group action by action.Name
                                               into grouped
                                               where grouped.Count() > 1
                                               select new {name = grouped.Key};

            var locatorsWithMoreThanOneMethodList = locatorsWithMoreThanOneMethod.ToList();
            if (locatorsWithMoreThanOneMethodList.Count > 0)
                throw new MvxException(
                    "You muppet - you have built a view model locator with multiple locators with the same name: " +
                    string.Join(",", locatorsWithMoreThanOneMethodList));
        }

        protected static bool IsLocatorParameterCandidate(ParameterInfo parameterInfo)
        {
            return !parameterInfo.IsOut
                   && parameterInfo.ParameterType == typeof(string)
                   && !parameterInfo.IsOptional;
        }

        protected static bool IsLocatorCandidate(MethodInfo methodInfo, Type viewModelType)
        {
            return methodInfo.IsPublic
                   && !methodInfo.IsStatic
                   && !methodInfo.IsGenericMethod
                   && methodInfo.ReturnType == viewModelType
                   && methodInfo.GetParameters().All(IsLocatorParameterCandidate);
        }
    }
}