using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.iOS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UIKit;

namespace MvvmCross.iOS.Views {
    internal static class MvxSegueExtensionMethods {

        internal static Type GetViewModelType(this IMvxView view)
        {
            var viewType = view.GetType();
            var props = viewType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = props.Where(p => p.Name == "ViewModel").FirstOrDefault();
            return prop?.PropertyType;
        }

        internal static void InstantiateViewModelForSegue(this IMvxEventSourceViewController _, UIStoryboardSegue segue)
        {
            var view = segue.DestinationViewController as IMvxIosView;
            if (view != null && view.Request == null)
            {
                var type = view.GetViewModelType();
                if (type != null)
                {
                    var by = new MvxRequestedBy(MvxRequestedByType.Other, $"Segue: {segue.Identifier}");
                    view.Request = new MvxViewModelRequest(type, null, null, by);
                }
            }
        }
    }
}
