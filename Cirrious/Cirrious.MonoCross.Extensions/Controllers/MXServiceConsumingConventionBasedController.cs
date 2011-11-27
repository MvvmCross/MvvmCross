#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXServiceConsumingConventionBasedController.cs" company="Cirrious">
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

using Cirrious.MonoCross.Extensions.Platform;

#endregion

namespace Cirrious.MonoCross.Extensions.Controllers
{
    public abstract class MXServiceConsumingConventionBasedController : MXConventionBasedController,
                                                                        IMXServiceConsumer
    {
    }
}