#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXConventionBasedApplication.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Application.RouteHelpers;
using Cirrious.MonoCross.Extensions.Conventions;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.Application
{
    public class MXConventionBasedApplication : MXApplication
    {
#warning Need to move IMXRouteDescriptionGenerator into "Services"
        protected IMXRouteDescriptionGenerator PatternGenerator { get; set; }

        public MXConventionBasedApplication()
        {
            // default implementation of IMXConventionBasedPatternGenerator
            PatternGenerator = new MXRouteDescriptionGenerator();
        }

        public void AddEmptyRoute(IMXController defaultController)
        {
            NavigationMap.Add(string.Empty, defaultController);
        }

        public void AddEmptyRoute(IMXController defaultController, string actionName)
        {
            NavigationMap.Add(string.Empty, defaultController,
                              new Dictionary<string, string>()
                                  {{MXConventionConstants.ActionParameterKeyName, actionName}});
        }

        public void AddRoutesbyConvention(IEnumerable<IMXConventionBasedController> controllers)
        {
            foreach (var controller in controllers)
            {
                AddRoutesbyConvention(controller);
            }
        }

        public void AddRoutesbyConvention(IMXConventionBasedController controller)
        {
            var controllerConvention = controller.Convention;

            // add default controller route
            if (controllerConvention.DefaultActionName != null)
            {
                var routeDescription = PatternGenerator.Generate(controllerConvention);
                NavigationMap.Add(routeDescription.Pattern, controller, routeDescription.DefaultParameters);
            }

            // add controller routes for each action
            foreach (var actionConvention in controllerConvention.ActionConventions)
            {
                // within each action, there can be multiple routes - e.g. when default parameter values are available.
                // we have to be very careful with default parameter values because:
                // - some people have experienced crashes in some MonoDroid builds near to default parameters
                // - Windows Phone 7 reflection does not supply access to default values (it supplies null instead)
                foreach (
                    var routeDescription in
                        new MXRouteDescriptionSet(PatternGenerator, controllerConvention, actionConvention))
                {
                    NavigationMap.Add(routeDescription.Pattern, controller, routeDescription.DefaultParameters);
                }
            }
        }
    }
}