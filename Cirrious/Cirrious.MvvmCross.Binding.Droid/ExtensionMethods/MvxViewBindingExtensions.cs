// MvxViewBindingExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.App;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.ExtensionMethods
{
    public static class MvxViewBindingExtensions
    {
        private static IMvxBinder Binder
        {
            get { return MvxServiceProviderExtensions.GetService<IMvxBinder>(); }
        }

        [System.Obsolete("Is this code required?", false)]
        public static IMvxBinding BindSubViewClickToCommand(this View view, int subViewId, object source,
                                                            string propertyPath)
        {
            var subView = view.FindViewById(subViewId);
            if (subView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Problem finding clickable view id " + subViewId);
                return null;
            }
            return subView.BindClickToCommand(source, propertyPath);
        }

        [System.Obsolete("Is this code required?", false)]
        public static IMvxBinding BindClickToCommand(this View view, object source, string propertyPath)
        {
            var bindingParameters = new MvxBindingRequest
                {
                    Source = source,
                    Target = view,
                    Description = new MvxBindingDescription
                        {
                            SourcePropertyPath = propertyPath,
                            TargetName = "Click"
                        }
                };
            return Binder.BindSingle(bindingParameters);
        }

        [System.Obsolete("Is this code required?", false)]
        public static void BindView<TViewType>
            (this Activity activity, int viewId, object source, MvxBindingDescription bindingDescription)
            where TViewType : View
        {
            var view = activity.FindViewById<TViewType>(viewId);
            if (view == null)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,
                    "Unable to bind: did not find view {0} of type {1}", viewId, typeof (TViewType));
                return;
            }

            view.Bind(source, bindingDescription);
        }

        [System.Obsolete("Is this code required?", false)]
        public static IMvxBinding BindSubView(this View view, int targetViewId, object source,
                                              MvxBindingDescription bindingDescription)
        {
            var targetView = view.FindViewById(targetViewId);
            if (targetView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Unable to bind: did not find view {0}", targetViewId);
                return null;
            }

            return targetView.Bind(source, bindingDescription);
        }

        [System.Obsolete("Is this code required?", false)]
        public static IMvxBinding Bind(
            this View targetView,
            object source,
            MvxBindingDescription bindingDescription)
        {
            return Binder.BindSingle(new MvxBindingRequest(source, targetView, bindingDescription));
        }

        [System.Obsolete("Is this code required?", false)]
        public static void StoreBindings(this View view, IList<IMvxUpdateableBinding> viewBindings)
        {
            var dict = new Dictionary<View, IList<IMvxUpdateableBinding>>
                {
                    {view, viewBindings}
                };

            view.StoreBindings(dict);
        }

        [System.Obsolete("Is this code required?", false)]
        public static void StoreBindings(this View view, IDictionary<View, IList<IMvxUpdateableBinding>> viewBindings)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic, "Storing bindings on {0} views", viewBindings.Count);
            IDictionary<View, IList<IMvxUpdateableBinding>> existingDictionary;
            if (view.TryGetStoredBindings(out existingDictionary))
            {
                MergeIntoDictionary(viewBindings, existingDictionary);
            }
            else
            {
                view.SetTag(MvxAndroidBindingResource.Instance.BindingTagUnique,
                            new MvxJavaContainer<IDictionary<View, IList<IMvxUpdateableBinding>>>(viewBindings));
            }
        }

        [System.Obsolete("Is this code required?", false)]
        private static void MergeIntoDictionary(IDictionary<View, IList<IMvxUpdateableBinding>> mergeThis,
                                                IDictionary<View, IList<IMvxUpdateableBinding>> intoThis)
        {
            foreach (var viewBinding in mergeThis)
            {
                IList<IMvxUpdateableBinding> existingList;
                if (intoThis.TryGetValue(viewBinding.Key, out existingList))
                {
                    foreach (var mvxUpdateableBinding in viewBinding.Value)
                    {
                        existingList.Add(mvxUpdateableBinding);
                    }
                }
                else
                {
                    intoThis[viewBinding.Key] = viewBinding.Value;
                }
            }
        }

        [System.Obsolete("Is this code required?", false)]
        public static bool TryGetStoredBindings(this View view,
                                                out IDictionary<View, IList<IMvxUpdateableBinding>> result)
        {
            result = null;

            if (view == null)
            {
                return false;
            }

            var tag = view.GetTag(MvxAndroidBindingResource.Instance.BindingTagUnique);
            if (tag == null)
            {
                return false;
            }

            var wrappedResult = tag as MvxJavaContainer<IDictionary<View, IList<IMvxUpdateableBinding>>>;
            if (wrappedResult == null)
            {
                return false;
            }

            result = wrappedResult.Object;
            return true;
        }

        /// <summary>
        /// Sets the binding tag for a <see cref="View"/>. 
        /// </summary>
        /// 
        /// <seealso cref="M:GetBindingTag"/>
        public static void SetBindingTag(this View view, MvxViewBindingTag tag)
        {
            view.SetTag (MvxAndroidBindingResource.Instance.BindingTagUnique, tag);
        }

        /// <summary>
        /// Gets the binding tag for a <see cref="View"/>. 
        /// </summary>
        /// <returns>The binding tag, or <c>null</c> if not set.</returns>
        /// 
        /// <seealso cref="M:SetBindingTag"/>
        public static MvxViewBindingTag GetBindingTag(this View view)
        {
            return view.GetTag (MvxAndroidBindingResource.Instance.BindingTagUnique) as MvxViewBindingTag;
        }

        /// <summary>
        /// Updates the <see cref="T:MvxViewBindingTag"/> data source.
        /// You need to call <see cref="M:IMvxViewBindingManager.BindView"/> to rebind items.
        /// </summary>
        public static void UpdateDataSource(this View view, object dataSource) {
            var tag = view.GetBindingTag();
            if (tag == null) {
                tag = new MvxViewBindingTag() {
                    DataSource = dataSource,
                };
                view.SetBindingTag(tag);
            } else {
                tag.DataSource = dataSource;
            }
        }

        /// <summary>
        /// Removes the <see cref="T:MvxViewBindingTag"/> data source, by disabling data source overriding.
        /// </summary>
        public static void RemoveDataSource(this View view) {
            var tag = view.GetBindingTag();
            if (tag != null) {
                tag.OverrideDataSource = false;
            }
        }
    }
}