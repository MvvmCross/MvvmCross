#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="IMXViewDispatcher.cs" company="Cirrious">
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

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXViewDispatcher : IMXCrossThreadDispatcher
    {
        bool RequestNavigate(string url, Dictionary<string, string> parameters);
        // TODO - maybe other things (timers? services for GPS, etc?)
    }
}