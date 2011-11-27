#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXParameterConvention.cs" company="Cirrious">
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

#endregion

namespace Cirrious.MonoCross.Extensions.Interfaces.Conventions
{
    public interface IMXParameterConvention
    {
        string ConventionName { get; }
        Type Type { get; }
        bool IsOptional { get; }
        object DefaultValue { get; }
    }
}