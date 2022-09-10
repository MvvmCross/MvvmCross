// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppKit;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Platforms.Mac.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Views
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

        internal static void ViewModelRequestForSegue(this IMvxEventSourceViewController self, NSStoryboardSegue segue, NSObject sender)
        {
            var view = self as IMvxMacViewSegue;
            var parameterValues = view == null ? null : view.PrepareViewModelParametersForSegue(segue, sender);

            if (parameterValues is IMvxBundle)
                self.ViewModelRequestForSegueImpl(segue, (IMvxBundle)parameterValues);
            else if (parameterValues is IDictionary<string, string>)
                self.ViewModelRequestForSegueImpl(segue, (IDictionary<string, string>)parameterValues);
            else
                self.ViewModelRequestForSegueImpl(segue, parameterValues);
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, NSStoryboardSegue segue, object parameterValuesObject)
        {
            self.ViewModelRequestForSegueImpl(segue, parameterValuesObject.ToSimplePropertyDictionary());
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, NSStoryboardSegue segue, IDictionary<string, string> parameterValues)
        {
            self.ViewModelRequestForSegueImpl(segue, new MvxBundle(parameterValues));
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController _, NSStoryboardSegue segue, IMvxBundle parameterBundle = null)
        {
            var view = segue.DestinationController as IMvxMacView;
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
