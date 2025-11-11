// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Runtime type inspection is necessary for segue-based navigation")]
        [UnconditionalSuppressMessage("Trimming", "IL2073", Justification = "PropertyInfo.PropertyType doesn't preserve annotations")]
        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime property inspection is necessary for ViewModel discovery")]
        internal static Type GetViewModelType(this IMvxView view)
        {
            var viewType = view.GetType();
            var props = viewType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = Array.Find(props, p => p.Name == "ViewModel");
            return prop?.PropertyType;
        }

        internal static void ViewModelRequestForSegue(this IMvxEventSourceViewController self, NSStoryboardSegue segue, NSObject sender)
        {
            var parameterValues = self is not IMvxMacViewSegue view
                ? null
                : view.PrepareViewModelParametersForSegue(segue, sender);

            if (parameterValues is IMvxBundle bundle)
                self.ViewModelRequestForSegueImpl(segue, bundle);
            else if (parameterValues is IDictionary<string, string> values)
                self.ViewModelRequestForSegueImpl(segue, values);
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
            if (segue.DestinationController is IMvxMacView { Request: null } view)
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
