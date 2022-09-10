// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Platforms.Ios.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
#nullable enable
    internal static class MvxSegueExtensions
    {
        internal static Type? GetViewModelType(this IMvxView? view)
        {
            if (view == null)
                return null;

            var viewType = view.GetType();
            var props = viewType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = Array.Find(props, p => p.Name == "ViewModel");
            return prop?.PropertyType;
        }

        internal static void ViewModelRequestForSegue(this IMvxEventSourceViewController self, UIStoryboardSegue segue, NSObject sender)
        {
            object? parameterValues = null;
            if (self is IMvxIosViewSegue segueView)
            {
                parameterValues = segueView.PrepareViewModelParametersForSegue(segue, sender);
            }

            if (parameterValues is IMvxBundle bundleValues)
                self.ViewModelRequestForSegueImpl(segue, bundleValues);
            else if (parameterValues is IDictionary<string, string> dictValues)
                self.ViewModelRequestForSegueImpl(segue, dictValues);
            else
                self.ViewModelRequestForSegueImpl(segue, parameterValues);
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, UIStoryboardSegue segue, object? parameterValuesObject)
        {
            self.ViewModelRequestForSegueImpl(segue, parameterValuesObject.ToSimplePropertyDictionary());
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, UIStoryboardSegue segue, IDictionary<string, string> parameterValues)
        {
            self.ViewModelRequestForSegueImpl(segue, new MvxBundle(parameterValues));
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController _, UIStoryboardSegue segue, IMvxBundle? parameterBundle = null)
        {
            if (segue.DestinationViewController is IMvxIosView view && view.Request == null)
            {
                var type = view.GetViewModelType();
                if (type != null)
                {
                    view.Request = new MvxViewModelRequest(type, parameterBundle, null);
                }
            }
        }
    }
#nullable restore
}
