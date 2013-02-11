using System;
using System.Collections.Generic;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    /// <summary>
    /// Java object used to tag <see cref="View"/> instances with binding data.
    /// </summary>
    public sealed class MvxViewBindingTag : Java.Lang.Object
    {
        private readonly IEnumerable<MvxBindingDescription> _descriptions;
        private object _source;
        private bool _useSource;

        public MvxViewBindingTag (IEnumerable<MvxBindingDescription> bindingDescriptions = null)
        {
            _descriptions = bindingDescriptions;
            BindingEnabled = true;
        }

        /// <summary>
        /// Gets the binding descriptions used to create bindings.
        /// </summary>
        /// <value>The binding descriptions, null if the view shouldn't be bound.</value>
        public IEnumerable<MvxBindingDescription> BindingDescriptions {
            get { return _descriptions; }
        }

        /// <summary>
        /// Should we use the <see cref="DataSource"/> instead of the inherited data source for binding
        /// this view and it's decendants.
        /// </summary>
        /// <value><c>true</c> to override the inherited data source; otherwise, <c>false</c>.</value>
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

