#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXServiceFactorySingleton.cs" company="Cirrious">
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

using Cirrious.MonoCross.Extensions.Interfaces;

#endregion

namespace Cirrious.MonoCross.Extensions.Platform
{
    public class MXServiceFactorySingleton
    {
        public static readonly MXServiceFactorySingleton Instance = new MXServiceFactorySingleton();

        private IMXServiceFactory _serviceFactory;

        public IMXServiceFactory ServiceFactory
        {
            get { return _serviceFactory; }
            set
            {
                if (_serviceFactory != null)
                {
                    // TODO - commented out for now, but we do need to understand the lifecycle here!
                    //throw new InvalidOperationException("ServiceFactory already set");
                }
                _serviceFactory = value;
            }
        }
    }
}