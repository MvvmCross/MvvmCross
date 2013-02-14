using System;
using System.Collections.Generic;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    /// <summary>
    /// Java object used to tag <see cref="View"/> instances with binding data.
    /// </summary>
    /// 
    /// <seealso cref="M:MvxViewBindingExtensions.SetBindingTag"/>
    public sealed class MvxViewBindingTag : Java.Lang.Object
    {
        private readonly IList<MvxBindingDescription> _descriptions;
        private readonly IList<IMvxUpdateableBinding> _bindings;
        private object _source;
        private bool _useSource;

        public MvxViewBindingTag (IEnumerable<MvxBindingDescription> bindingDescriptions = null)
        {
            if (bindingDescriptions != null) {
                _descriptions = new List<MvxBindingDescription>(bindingDescriptions).AsReadOnly();
                _bindings = new List<IMvxUpdateableBinding>(_descriptions.Count);
            } else {
                _descriptions = null;
                _bindings = null;
            }
            BindingEnabled = true;
        }

        /// <summary>
        /// Gets the binding descriptions used to create bindings.
        /// </summary>
        /// <value>The binding descriptions, null if the view shouldn't be bound.</value>
        public IList<MvxBindingDescription> BindingDescriptions {
            get { return _descriptions; }
        }
        
        /// <summary>
        /// Gets the currently active bindings.
        /// </summary>
        /// <value>The bindings list containing currently active bindings.</value>
        public IList<IMvxUpdateableBinding> Bindings {
            get { return _bindings; }
        }

        /// <summary>
        /// Should we use the <see cref="DataSource"/> instead of the inherited data source for binding
        /// this view and it's decendants.
        /// </summary>
        /// <value><c>true</c> to override the inherited data source; otherwise, <c>false</c>.</value>
        /// <seealso cref="M:MvxViewBindingExtensions.RemoveDataSource"/>
        public bool OverrideDataSource {
            get {
                return _useSource;
            }
            set {
                if (_useSource != value) {
                    _useSource = value;
                    if (!_useSource)
                        DataSource = null;
                }
            }
        }

        /// <summary>
        /// Data source to use instead of the inherited source when binding views.
        /// </summary>
        /// <value>The data source.</value>
        /// <seealso cref="M:MvxViewBindingExtensions.UpdateDataSource"/>
        public object DataSource {
            get {
                return _source;
            }
            set {
                if (_source != value) {
                    _source = value;
                    OverrideDataSource = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets if the view and it's desecendants should be bound. This should only be used to optimize
        /// the binding process by excluding parts of the hierarchy from being bound.
        /// </summary>
        /// <value>The bindings enabled.</value>
        public bool BindingEnabled { get; set; }
    }
}

