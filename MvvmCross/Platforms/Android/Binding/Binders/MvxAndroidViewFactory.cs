// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.Content;
using Android.Util;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
#nullable enable
    public class MvxAndroidViewFactory
        : IMvxAndroidViewFactory
    {
        private IMvxViewTypeResolver? _viewTypeResolver;

        protected IMvxViewTypeResolver? ViewTypeResolver => _viewTypeResolver ??= Mvx.IoCProvider?.Resolve<IMvxViewTypeResolver>();

        public virtual View? CreateView(View? parent, string name, Context context, IAttributeSet attrs)
        {
            // resolve the tag name to a type
            var viewType = ViewTypeResolver?.Resolve(name);

            if (viewType == null)
            {
                //MvxBindingLog.Error( "View type not found - {0}", name);
                return null;
            }

            try
            {
                var view = Activator.CreateInstance(viewType, context, attrs) as View;
                if (view == null)
                {
                    MvxBindingLog.Error("Unable to load view {0} from type {1}", name,
                                          viewType.FullName ?? string.Empty);
                }
                return view;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxBindingLog.Error(
                                      "Exception during creation of {0} from type {1} - exception {2}", name,
                                      viewType.FullName ?? string.Empty, exception.ToLongString());
                return null;
            }
        }
    }
#nullable restore
}
