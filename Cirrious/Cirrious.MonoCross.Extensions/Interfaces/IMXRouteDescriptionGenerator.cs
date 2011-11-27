#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXRouteDescriptionGenerator.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXRouteDescriptionGenerator
    {
        MXRouteDescription Generate(IMXControllerConvention controllerConvention);
        MXRouteDescription Generate(IMXControllerConvention controllerConvention, IMXActionConvention actionConvention);

        MXRouteDescription Generate(IMXControllerConvention controllerConvention, IMXActionConvention actionConvention,
                                    IEnumerable<IMXParameterConvention> activeParameterList);

        MXRouteDescription Generate(IMXControllerConvention controllerConvention, IMXActionConvention actionConvention,
                                    IEnumerable<IMXParameterConvention> activeParameterList,
                                    IEnumerable<IMXParameterConvention> defaultParameterList);
    }
}