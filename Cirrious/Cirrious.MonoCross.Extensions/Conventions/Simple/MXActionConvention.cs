#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXActionConvention.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Conventions
{
    public class MXActionConvention : IMXActionConvention
    {
        public string ConventionName { get; set; }
        public IEnumerable<IMXParameterConvention> Parameters { get; set; }
    }
}