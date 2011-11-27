#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXControllerAnalyser.cs" company="Cirrious">
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
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXControllerAnalyser
    {
        string GenerateUrlFor(Type type, string actionName, IDictionary<string, string> inputParameterValues);
        Dictionary<string, MethodInfo> GenerateActionMap(Type type);
    }
}