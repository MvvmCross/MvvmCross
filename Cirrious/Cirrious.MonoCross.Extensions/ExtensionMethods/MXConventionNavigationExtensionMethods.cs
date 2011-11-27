#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXConventionNavigationExtensionMethods.cs" company="Cirrious">
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

using System.Collections.Generic;
using Cirrious.MonoCross.Extensions.Controllers;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.ExtensionMethods
{
    public static class MXConventionNavigationExtensionMethods
    {
        public static void Navigate<T>(this IMXView view) where T : IMXConventionBasedController
        {
            NavigateImpl<T>(null, null, null);
        }

        public static void Navigate<T>() where T : IMXConventionBasedController
        {
            NavigateImpl<T>(null, null, null);
        }

        public static void Navigate<T>(string actionName, IDictionary<string, string> parameterValues)
            where T : IMXConventionBasedController
        {
            NavigateImpl<T>(null, actionName, parameterValues);
        }

        public static void Navigate<T>(string actionName) where T : IMXConventionBasedController
        {
            NavigateImpl<T>(null, actionName, null);
        }

        public static void Navigate<T>(this IMXView view, string actionName) where T : IMXConventionBasedController
        {
            NavigateImpl<T>(view, actionName, null);
        }

        public static void Navigate<T>(string actionName, object parameterObject)
            where T : IMXConventionBasedController
        {
            NavigateImpl<T>(null, actionName, parameterObject.ToSimplePropertyDictionary());
        }

        public static void Navigate<T>(this IMXView view, string actionName, object parameterObject)
            where T : IMXConventionBasedController
        {
            NavigateImpl<T>(null, actionName, parameterObject.ToSimplePropertyDictionary());
        }

        public static void Navigate<T>(this IMXView view, string actionName, IDictionary<string, string> parameterValues)
            where T : IMXConventionBasedController
        {
            NavigateImpl<T>(view, actionName, parameterValues);
        }

        private static void NavigateImpl<T>(IMXView view, string actionName, IDictionary<string, string> parameterValues)
            where T : IMXConventionBasedController
        {
            var url = MXConventionBasedController.ControllerAnalyser.GenerateUrlFor(typeof(T), actionName,
                                                                                    parameterValues);
            MXContainer.Navigate(view, url);
        }
    }
}