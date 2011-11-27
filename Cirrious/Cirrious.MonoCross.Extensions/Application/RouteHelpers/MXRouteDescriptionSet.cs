#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXRouteDescriptionSet.cs" company="Cirrious">
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

using System.Collections;
using System.Collections.Generic;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Application.RouteHelpers
{
    public class MXRouteDescriptionSet : IEnumerable<MXRouteDescription>
    {
        private readonly IMXRouteDescriptionGenerator _patternGenerator;
        private readonly IMXControllerConvention _controllerConvention;
        private readonly IMXActionConvention _actionConvention;

        public MXRouteDescriptionSet(IMXRouteDescriptionGenerator patternGenerator,
                                     IMXControllerConvention controllerConvention, IMXActionConvention actionConvention)
        {
            _patternGenerator = patternGenerator;
            _controllerConvention = controllerConvention;
            _actionConvention = actionConvention;
        }

        public IEnumerator<MXRouteDescription> GetEnumerator()
        {
            return new MXRouteDescriptionEnumerator(_patternGenerator, _controllerConvention, _actionConvention);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}