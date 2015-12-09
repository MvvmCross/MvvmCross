// MvxAndroidViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    using System;
    using System.Threading;

    using Android.Content;
    using Android.Util;
    using Android.Views;

    using MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxAndroidViewFactory
        : IMvxAndroidViewFactory
    {
        private IMvxViewTypeResolver _viewTypeResolver;

        protected IMvxViewTypeResolver ViewTypeResolver => this._viewTypeResolver ?? (this._viewTypeResolver = Mvx.Resolve<IMvxViewTypeResolver>());

        public virtual View CreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            // resolve the tag name to a type
            var viewType = this.ViewTypeResolver.Resolve(name);

            if (viewType == null)
            {
                //MvxBindingTrace.Trace(MvxTraceLevel.Error, "View type not found - {0}", name);
                return null;
            }

            try
            {
                var view = Activator.CreateInstance(viewType, context, attrs) as View;
                if (view == null)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Error, "Unable to load view {0} from type {1}", name,
                                          viewType.FullName);
                }
                return view;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Exception during creation of {0} from type {1} - exception {2}", name,
                                      viewType.FullName, exception.ToLongString());
                return null;
            }
        }
    }
}