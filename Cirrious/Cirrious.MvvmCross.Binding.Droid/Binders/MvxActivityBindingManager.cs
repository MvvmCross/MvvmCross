using System;
using Android.App;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    /// <summary>
    /// Activity binding manager handles bindings for an activity. This is only useful for you if you're implementing
    /// binding support for custom activity base class.
    /// </summary>
    /// 
    /// <example>
    /// You should only have one instance of it per activity, ie:
    /// 
    /// <code>
    /// public class MvxMyAndroidBindingActivity : extends MyAndroidActivity {
    ///     // ...
    /// 
    ///     private readonly IMvxViewBindingManager _bindings;
    ///     
    ///     public MvxMyAndroidBindingActivity() : base() {
    ///         _bindings = new MvxActivityBindingManager(this);
    ///     }
    /// 
    ///     public IMvxViewBindingManager BindingManager {
    ///         get { return _bindings; }
    ///     }
    /// 
    ///     // ...
    /// }
    /// </code>
    /// </example>
    /// 
    public class MvxActivityBindingManager
        : IMvxViewBindingManager, IMvxServiceConsumer
    {
        private static void ClearBoundTags(IList<WeakReference> tags) {
            var viewTags = from weak in tags
                where weak != null
                select weak.Target as MvxViewBindingTag;
            foreach (var tag in viewTags) {
                if (tag != null) {
                    foreach (var binding in tag.Bindings) {
                        binding.Dispose();
                    }
                    tag.Bindings.Clear();
                }
            }
            tags.Clear ();
        }

        private readonly WeakReference _activity;
        private readonly List<IMvxBinding> _manualBindings = new List<IMvxBinding> ();
        private readonly List<WeakReference> _viewTags = new List<WeakReference> ();

        public MvxActivityBindingManager (Activity activity)
        {
            // We're using weak references just in case
            _activity = new WeakReference (activity);
        }

        /// <summary>
        /// Recursively go through the parents of the view trying to find one that specifies
        /// a data source.
        /// </summary>
        /// <returns>The parent data source, <c>null</c> if no source found.</returns>
        private object FindParentDataSource(IViewParent viewParent) {
            var view = viewParent as View;
            if (view == null)
                return null;

            var tag = view.GetBindingTag ();
            if (tag != null) {
                if (tag.OverrideDataSource) {
                    return tag.DataSource;
                }
            }

            return FindParentDataSource (view.Parent);
        }

        /// <summary>
        /// Traverses the view hierarchy and updates the bindings.
        /// </summary>
        protected void BindViewTree(object dataSource, View view, IList<WeakReference> reusableBindings = null) {
            var tag = view.GetBindingTag ();

            if (tag != null) {
                if (!tag.BindingEnabled)
                    return;

                if (tag.OverrideDataSource)
                    dataSource = tag.DataSource;

                if (tag.BindingDescriptions != null) {
                    if (dataSource == null) {
                        MvxBindingTrace.Trace(
                            MvxTraceLevel.Warning,
                            "Binding view {0} with an empty data source.",
                            view.GetType().Name);
                    }

                    if (tag.Bindings.Count > 0 || dataSource != null) {
                        // Try to reuse weak reference for the tag:
                        WeakReference weakTag = null;
                        if (reusableBindings != null) {
                            for (var i = 0; i < reusableBindings.Count; i++) {
                                if (reusableBindings[i] == null)
                                    continue;
                                if (reusableBindings[i].Target == tag) {
                                    weakTag = reusableBindings[i];
                                    reusableBindings[i] = null;
                                }
                            }
                        }
                        if (weakTag == null)
                            weakTag = new WeakReference(tag);
                        _viewTags.Add(weakTag);

                        if (tag.Bindings.Count > 0) {
                            // Update old bindings only:
                            foreach (var binding in tag.Bindings) {
                                binding.DataContext = dataSource;
                            }
                        } else {
                            // Create bindings:
                            var bindings = this.GetService<IMvxBinder>()
                                .Bind(dataSource, view, tag.BindingDescriptions);
                            if (bindings != null) {
                                foreach (var binding in bindings) {
                                    tag.Bindings.Add(binding);
                                }
                            }
                        }
                    }
                }
            }

            var viewGroup = view as ViewGroup;
            if (viewGroup != null) {
                var count = viewGroup.ChildCount;
                for (var i = 0; i < count; i++) {
                    BindViewTree(dataSource, viewGroup.GetChildAt(i), reusableBindings);
                }
            }
        }

        #region IMvxViewBindingManager implementation

        public View RootView {
            get {
                var activity = _activity.Target as Activity;
                if (activity == null)
                    return null;
                return activity.Window.FindViewById (Android.Resource.Id.Content);
            }
        }
        
        public void BindView (View view, object dataSource = null)
        {
            if (view == null)
                return;
            var isRoot = view == RootView;
            IList<WeakReference> lingeringTags = null;

            if (dataSource != null) {
                view.UpdateDataSource(dataSource);
            }

            if (isRoot) {
                lingeringTags = new List<WeakReference>(_viewTags);
                _viewTags.Clear();
            }

            if (dataSource == null) {
                dataSource = FindParentDataSource(view.Parent);
            }

            BindViewTree (dataSource, view, lingeringTags);

            if (isRoot) {
                ClearBoundTags(lingeringTags);
            }
        }

        public void UnbindView (View view)
        {
            if (view == null)
                return;

            var tag = view.GetBindingTag ();
            if (tag != null) {
                if (tag.Bindings != null) {
                    foreach (var binding in tag.Bindings) {
                        binding.Dispose();
                    }
                    tag.Bindings.Clear();
                }

                _viewTags.RemoveAll(weak => weak.Target == tag || weak.Target == null);
            }

            var viewGroup = view as ViewGroup;
            if (viewGroup != null) {
                var count = viewGroup.ChildCount;
                for (var i = 0; i < count; i++) {
                    UnbindView(viewGroup.GetChildAt(i));
                }
            }
        }

        public void AddBinding (IMvxBinding binding)
        {
            _manualBindings.Add (binding);
        }

        public void RemoveBinding (IMvxBinding binding)
        {
            _manualBindings.Remove (binding);
        }

        public void UnbindAll ()
        {
            _manualBindings.ForEach (binding => binding.Dispose ());
            _manualBindings.Clear ();
            ClearBoundTags (_viewTags);
        }

        #endregion
    }
}

