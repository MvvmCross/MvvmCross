// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Platforms.Tvos.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    internal static class MvxSegueExtensions
    {
        internal static Type GetViewModelType(this IMvxView view)
        {
            var viewType = view.GetType();
            var props = viewType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = props.Where(p => p.Name == "ViewModel").FirstOrDefault();
            return prop?.PropertyType;
        }

        internal static void ViewModelRequestForSegue(this IMvxEventSourceViewController self, UIStoryboardSegue segue, NSObject sender)
        {
            var view = self as IMvxTvosViewSegue;
            var parameterValues = view == null ? null : view.PrepareViewModelParametersForSegue(segue, sender);

            if (parameterValues is IMvxBundle)
                self.ViewModelRequestForSegueImpl(segue, (IMvxBundle)parameterValues);
            else if (parameterValues is IDictionary<string, string>)
                self.ViewModelRequestForSegueImpl(segue, (IDictionary<string, string>)parameterValues);
            else
                self.ViewModelRequestForSegueImpl(segue, parameterValues);
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, UIStoryboardSegue segue, object parameterValuesObject)
        {
            self.ViewModelRequestForSegueImpl(segue, parameterValuesObject.ToSimplePropertyDictionary());
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, UIStoryboardSegue segue, IDictionary<string, string> parameterValues)
        {
            self.ViewModelRequestForSegueImpl(segue, new MvxBundle(parameterValues));
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController _, UIStoryboardSegue segue, IMvxBundle parameterBundle = null)
        {
            var view = segue.DestinationViewController as IMvxTvosView;
            if (view != null && view.Request == null)
            {
                var type = view.GetViewModelType();
                if (type != null)
                {
                    view.Request = new MvxViewModelRequest(type, parameterBundle, null);
                }
            }
        }
    }
}
