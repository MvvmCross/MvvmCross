#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXViewModelPhoneContainer.cs" company="Cirrious">
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
using Microsoft.Phone.Controls;
using MonoCross.Navigation;
using MonoCross.WindowsPhone;

#endregion

namespace Cirrious.MonoCross.Extensions.WindowsPhone
{
    public class MXViewModelPhoneContainer : MXPhoneContainer
    {
        private readonly MXViewModelLifeCycleHelper _viewModelLifeCycleHelper;

        public new static void Initialize(MXApplication theApp, PhoneApplicationFrame rootFrame)
        {
            InitializeContainer(new MXViewModelPhoneContainer(theApp, rootFrame));
        }

        public MXViewModelPhoneContainer(MXApplication theApp, PhoneApplicationFrame rootFrame)
            : base(theApp, rootFrame)
        {
            var dispatcher = rootFrame.Dispatcher;
            _viewModelLifeCycleHelper = new MXViewModelLifeCycleHelper(() => new MXPhoneViewDispatcher(dispatcher));
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller,
                                                         MXShowViewRequest showViewRequest)
        {
            _viewModelLifeCycleHelper.OnControllerLoadComplete(controller, showViewRequest);
            base.OnControllerLoadComplete(fromView, controller, showViewRequest);
        }
    }
}