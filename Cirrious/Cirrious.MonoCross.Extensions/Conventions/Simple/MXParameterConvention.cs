#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXParameterConvention.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Conventions
{
    public class MXParameterConvention : IMXParameterConvention
    {
        public string ConventionName { get; set; }
        public Type Type { get; set; }
        public bool IsOptional { get; set; }
        public object DefaultValue { get; set; }
    }
}