#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXViewModelLifeCycleHelper.cs" company="Cirrious">
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
using Cirrious.MonoCross.Extensions.Interfaces;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.Platform
{
    public class MXViewModelLifeCycleHelper
    {
        private readonly Func<IMXViewDispatcher> _dispatcherFactory;
        private IMXViewModel _lastSeenViewModel;

        public MXViewModelLifeCycleHelper(Func<IMXViewDispatcher> dispatcherFactory)
        {
            _dispatcherFactory = dispatcherFactory;
        }

        public void OnControllerLoadComplete(IMXController controller, MXShowViewRequest showViewRequest)
        {
            if (_lastSeenViewModel != null)
            {
                // TODO - this requestStop is possibly called too late... but not sure we have any 
                _lastSeenViewModel.RequestStop();
                _lastSeenViewModel = null;
            }

            var model = showViewRequest.ViewModel;
            _lastSeenViewModel = model as IMXViewModel;
            if (_lastSeenViewModel != null)
            {
                _lastSeenViewModel.ViewDispatcher = _dispatcherFactory();
            }
        }
    }
}