#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXControllerConvention.cs" company="Cirrious">
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

#endregion

namespace Cirrious.MonoCross.Extensions.Interfaces.Conventions
{
    public interface IMXControllerConvention
    {
        string ConventionName { get; }
        string DefaultActionName { get; }
        IEnumerable<IMXActionConvention> ActionConventions { get; }
    }
}