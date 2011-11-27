#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXRouteDescriptionGenerator.cs" company="Cirrious">
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
using System.Linq;
using Cirrious.MonoCross.Extensions.Conventions;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Application.RouteHelpers
{
    public class MXRouteDescriptionGenerator : IMXRouteDescriptionGenerator
    {
        public MXRouteDescription Generate(IMXControllerConvention controllerConvention)
        {
            return Generate(controllerConvention, null, null, null);
        }

        public MXRouteDescription Generate(IMXControllerConvention controllerConvention,
                                           IMXActionConvention actionConvention)
        {
            return Generate(controllerConvention, actionConvention, null, null);
        }

        public MXRouteDescription Generate(IMXControllerConvention controllerConvention,
                                           IMXActionConvention actionConvention,
                                           IEnumerable<IMXParameterConvention> activeParametersList)
        {
            return Generate(controllerConvention, actionConvention, activeParametersList, null);
        }

        public MXRouteDescription Generate(IMXControllerConvention controllerConvention,
                                           IMXActionConvention actionConvention,
                                           IEnumerable<IMXParameterConvention> activeParametersList,
                                           IEnumerable<IMXParameterConvention> defaultParametersList)
        {
            return new MXRouteDescription()
                       {
                           DefaultParameters =
                               GenerateFullDefaultParameters(controllerConvention, actionConvention,
                                                             defaultParametersList),
                           Pattern = GeneratePattern(controllerConvention, actionConvention, activeParametersList)
                       };
        }

        private string GeneratePattern(IMXControllerConvention controllerConvention,
                                       IMXActionConvention actionConvention,
                                       IEnumerable<IMXParameterConvention> activeParametersList)
        {
            var patternParts = new List<string>() {controllerConvention.ConventionName};
            if (actionConvention != null)
                patternParts.Add(actionConvention.ConventionName);
            if (activeParametersList != null)
                patternParts.AddRange(activeParametersList.Select(x => "{" + x.ConventionName + "}"));
            var pattern = string.Join(MXConventionConstants.UrlPathSeparator, patternParts);
            return pattern;
        }

        private Dictionary<string, string> GenerateFullDefaultParameters(IMXControllerConvention controllerConvention,
                                                                         IMXActionConvention actionConvention,
                                                                         IEnumerable<IMXParameterConvention>
                                                                             defaultParametersList)
        {
            var toReturn = new Dictionary<string, string>();
            if (controllerConvention != null)
                toReturn[MXConventionConstants.ControllerParameterKeyName] = controllerConvention.ConventionName;

            if (actionConvention != null)
                toReturn[MXConventionConstants.ActionParameterKeyName] = actionConvention.ConventionName;

            if (defaultParametersList != null)
                foreach (var p in defaultParametersList)
                    toReturn[p.ConventionName] = p.DefaultValue == null ? null : p.DefaultValue.ToString();

            return toReturn;
        }
    }
}