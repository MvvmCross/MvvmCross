#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXViewModelDroidContainer.cs" company="Cirrious">
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

using Android.Content;
using Cirrious.MonoCross.Extensions.Platform;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.Android
{
    public class MXViewModelDroidContainer : MXToastAlertingDroidContainer
    {
        private MXViewModelLifeCycleHelper _viewModelLifeCycleHelper;

        public new static void Initialize(MXApplication theApp, Context applicationContext)
        {
            InitializeContainer(new MXViewModelDroidContainer(theApp, applicationContext));
        }

        public MXViewModelDroidContainer(MXApplication theApp, Context applicationContext)
            : base(theApp, applicationContext)
        {
            _viewModelLifeCycleHelper = new MXViewModelLifeCycleHelper(() => new MXSimpleDispatcher());
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller,
                                                         MXViewPerspective viewPerspective)
        {
            _viewModelLifeCycleHelper.OnControllerLoadComplete(controller);
            base.OnControllerLoadComplete(fromView, controller, viewPerspective);
        }
    }
}