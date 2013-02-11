using System;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Android.Views;
using Android.Util;
using Android.Content;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using System.Threading;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    /// <summary>
    /// Parses binding information from Android XML and attaches it to views.
    /// </summary>
    /// <seealso cref="IMvxBindingInflater"/>
    public class MvxBindingInflater : IMvxBindingInflater, IMvxServiceConsumer
    {
        private IMvxViewTypeResolver _viewTypeResolver;

        protected IMvxViewTypeResolver ViewTypeResolver
        {
            get
            {
                if (_viewTypeResolver == null)
                    _viewTypeResolver = this.GetService<IMvxViewTypeResolver>();
                return _viewTypeResolver;
            }
        }

        /// <summary>
        /// Parses the binding attributes into MvxViewBindingTag.
        /// </summary>
        /// <returns>The tag with parsed values, null if no binding attributes found.</returns>
        protected MvxViewBindingTag ParseAttributes (Context context, IAttributeSet attrs)
        {
            var res = MvxAndroidBindingResource.Instance;
            IEnumerable<MvxBindingDescription> descriptions = null;

            using (var typedArray = context.ObtainStyledAttributes(
                    attrs, res.BindingStylableGroupId)) {

                int numStyles = typedArray.IndexCount;
                for (var i = 0; i < numStyles; ++i) {
                    var attributeId = typedArray.GetIndex (i);

                    if (attributeId == res.BindingBindId) {
                        try {
                            var bindingText = typedArray.GetString (attributeId);
                            descriptions = this.GetService<IMvxBindingDescriptionParser> ().Parse (bindingText);
                        } catch (Exception exception) {
                            MvxBindingTrace.Trace (
                                MvxTraceLevel.Error, "Exception thrown during the parsing the bindings {0}",
                                exception.ToLongString ());
                            throw;
                        }
                    }
                }
                typedArray.Recycle ();
            }

            if (descriptions != null) {
                return new MvxViewBindingTag (descriptions);
            }
            return null;
        }

        /// <summary>
        /// Attempts to create the View from the XML tag. Can miss some Android side classes.
        /// </summary>
        /// <returns>The newly created view, null on error.</returns>
        protected View CreateView(string name, Context context, IAttributeSet attrs) {
            // resolve the tag name to a type
            var viewType = ViewTypeResolver.Resolve(name);
            
            if (viewType == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "View type not found - {0}", name);
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

        #region IMvxBindingInflater implementation
        public void Attach (View view, IAttributeSet attrs)
        {
            var tag = ParseAttributes (view.Context, attrs);
            if (tag == null)
                return;

            if (view.GetBindingTag () != null) {
                MvxBindingTrace.Trace (MvxTraceLevel.Warning,
                    "Trying to attach binding information to already configured view. Using binding elements on <fragment> tag?");
                return;
            }

            view.SetBindingTag (tag);
        }

        public View Inflate (string name, Context context, IAttributeSet attrs)
        {
            View view = null;
            var tag = ParseAttributes (context, attrs);

            if (tag == null) {
                // No binding information present, no need to inflate the view ourselves.
                return null;
            }

            view = CreateView (name, context, attrs);
            if (view == null) {
                return null;
            }

            view.SetBindingTag (tag);
            return view;
        }
        #endregion
    }
}