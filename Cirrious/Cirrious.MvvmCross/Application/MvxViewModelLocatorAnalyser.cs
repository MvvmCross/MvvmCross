// MvxViewModelLocatorAnalyser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Application
{
    public class MvxViewModelLocatorAnalyser : IMvxViewModelLocatorAnalyser
    {
        #region IMvxViewModelLocatorAnalyser Members

        public IEnumerable<MethodInfo> GenerateLocatorMethods(Type locatorType)
        {
#if NETFX_CORE
            var locators = from methodInfo in locatorType.GetTypeInfo().DeclaredMethods
#else
            var locators = from methodInfo in locatorType.GetMethods()
#endif
                           where IsLocatorCandidate(methodInfo)
                           select methodInfo;

#if DEBUG
            // this to a list (to stop R# complaining about multiple enumeration operations on the Linq)
            locators = locators.ToList();
            CheckLocatorsHaveUniqueName(locators);
#endif

            return locators;
        }

        #endregion

        private static void CheckLocatorsHaveUniqueName(IEnumerable<MethodInfo> methods)
        {
            var locatorsWithMoreThanOneMethod = from method in methods
                                                group method by method.ReturnType.FullName
                                                into grouped
                                                where grouped.Count() > 1
                                                select grouped.Key;

            var locatorsWithMoreThanOneMethodList = locatorsWithMoreThanOneMethod.ToArray();
            if (locatorsWithMoreThanOneMethodList.Length > 0)
                throw new MvxException(
                    "You've built a view model locator with multiple public locator methods and/or get accessors returning the same type - conflicting methods are " +
                    string.Join(",", locatorsWithMoreThanOneMethodList));
        }

        protected static bool IsLocatorParameterCandidate(ParameterInfo parameterInfo)
        {
            return !parameterInfo.IsOut
                   &&
                   (parameterInfo.ParameterType == typeof (string)
                    || parameterInfo.ParameterType == typeof (double)
                    || parameterInfo.ParameterType == typeof (int)
                    || parameterInfo.ParameterType == typeof (long)
                    || parameterInfo.ParameterType.IsEnum)
                   && !parameterInfo.IsOptional;
        }

        protected static bool IsLocatorCandidate(MethodInfo methodInfo)
        {
            return methodInfo.IsPublic
                   && !methodInfo.IsStatic
                   && !methodInfo.IsGenericMethod
#if NETFX_CORE
                   && typeof (IMvxViewModel).GetTypeInfo().IsAssignableFrom(methodInfo.ReturnType.GetTypeInfo())
#else
                   && typeof (IMvxViewModel).IsAssignableFrom(methodInfo.ReturnType)
#endif
                   && methodInfo.GetParameters().All(IsLocatorParameterCandidate);
        }
    }
}