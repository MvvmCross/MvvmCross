#region Copyright
// <copyright file="MvxViewBindingExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.App;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Android.ExtensionMethods
{
    public static class MvxViewBindingExtensions
    {
        private static IMvxBinder Binder
        {
            get { return MvxServiceProviderExtensions.GetService<IMvxBinder>(); }            
        }

        public static IMvxBinding BindSubViewClickToCommand(this View view, int subViewId, object source, string propertyPath)
        {
            var subView = view.FindViewById(subViewId);
            if (subView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,"Problem finding clickable view id " + subViewId);
                return null;
            }
            return subView.BindClickToCommand(source, propertyPath);
        }

        public static IMvxBinding BindClickToCommand(this View view, object source, string propertyPath)
        {
            var bindingParameters = new MvxBindingRequest()
                                                      {
                                                          Source = source,
                                                          Target = view,
                                                          Description = new MvxBindingDescription()
                                                                            {
                                                                                SourcePropertyPath = propertyPath,
                                                                                TargetName = "Click"
                                                                            }
                                                      };
            return Binder.BindSingle(bindingParameters);
        }

        public static void BindView<TViewType>
            (this Activity activity, int viewId, object source, MvxBindingDescription bindingDescription)
             where TViewType : View
        {
            var view = activity.FindViewById<TViewType>(viewId);
            if (view == null)
            {
                MvxBindingTrace.Trace(
                                        MvxTraceLevel.Warning,
"Unable to bind: did not find view {0} of type {1}", viewId, typeof(TViewType));
                return;
            }

            view.Bind(source, bindingDescription);
        }

        public static IMvxBinding BindSubView(this View view, int targetViewId, object source, MvxBindingDescription bindingDescription)
        {
            var targetView = view.FindViewById(targetViewId);
            if (targetView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
"Unable to bind: did not find view {0}", targetViewId);
                return null;
            }

            return targetView.Bind(source, bindingDescription);
        }

        public static IMvxBinding Bind(
            this View targetView,
            object source,
            MvxBindingDescription bindingDescription)
        {
            return Binder.BindSingle(new MvxBindingRequest(source, targetView, bindingDescription));
        }
    }
}