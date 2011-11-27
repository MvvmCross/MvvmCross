#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXCrossThreadDispatcher.cs" company="Cirrious">
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

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXCrossThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}