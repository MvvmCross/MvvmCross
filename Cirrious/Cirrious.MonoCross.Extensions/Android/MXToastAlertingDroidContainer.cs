#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXToastAlertingDroidContainer.cs" company="Cirrious">
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
using Android.App;
using Android.Content;
using Android.Widget;
using MonoCross.Droid;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.Android
{
    public class MXToastAlertingDroidContainer : MXDroidContainer
    {
        public MXToastAlertingDroidContainer(MXApplication theApp, Context applicationContext)
            : base(theApp, applicationContext)
        {
        }

        public override void ShowError(IMXView fromView, IMXController controller, Exception exception)
        {
            if (fromView != null)
            {
                var activity = fromView as Activity;
                if (activity != null)
                    activity.RunOnUiThread(
                        () => Toast.MakeText(activity, "Error: " + exception.Message, ToastLength.Long).Show());
            }

            base.ShowError(fromView, controller, exception);
        }
    }
}