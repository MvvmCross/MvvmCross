// MvxBindingLayoutInflatorFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Threading;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxBindingLayoutInflatorFactory
        : Java.Lang.Object
          , LayoutInflater.IFactory
    {
        private readonly object _source;

        private readonly List<IMvxUpdateableBinding> _viewBindings
            = new List<IMvxUpdateableBinding>();

        private IMvxViewTypeResolver _viewTypeResolver;
        private IMvxBinder _binder;

        public MvxBindingLayoutInflatorFactory(
            object source)
        {
            _source = source;
        }

        private IMvxViewTypeResolver ViewTypeResolver
        {
            get
            {
                if (_viewTypeResolver == null)
                    _viewTypeResolver = Mvx.Resolve<IMvxViewTypeResolver>();
                return _viewTypeResolver;
            }
        }

        private IMvxBinder Binder
        {
            get
            {
                if (_binder == null)
                    _binder = Mvx.Resolve<IMvxBinder>();
                return _binder;
            }
        }

        public List<IMvxUpdateableBinding> CreatedBindings
        {
            get { return _viewBindings; }
        }

        #region IFactory Members

        public View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            View view = CreateView(name, context, attrs);
            if (view != null)
            {
                BindView(view, context, attrs);
            }

            return view;
        }

        #endregion

        private void BindView(View view, Context context, IAttributeSet attrs)
        {
            using (
                var typedArray = context.ObtainStyledAttributes(attrs,
                                                                MvxAndroidBindingResource.Instance
                                                                                         .BindingStylableGroupId))
            {
                int numStyles = typedArray.IndexCount;
                for (var i = 0; i < numStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxAndroidBindingResource.Instance.BindingBindId)
                    {
                        ApplyBindingsFromAttribute(view, typedArray, attributeId);
                    }
                    else if (attributeId == MvxAndroidBindingResource.Instance.BindingLangId)
                    {
                        ApplyLanguageBindingsFromAttribute(view, typedArray, attributeId);
                    }
                }
                typedArray.Recycle();
            }
        }

        private void ApplyBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
        {
            try
            {
                var bindingText = typedArray.GetString(attributeId);
                var newBindings = Binder.Bind(_source, view, bindingText);
                if (newBindings != null)
                {
                    _viewBindings.AddRange(newBindings);
                }
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view binding {0}",
                                      exception.ToLongString());
            }
        }

        private void ApplyLanguageBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
        {
            try
            {
                var bindingText = typedArray.GetString(attributeId);
                var newBindings = Binder.LanguageBind(_source, view, bindingText);
                if (newBindings != null)
                {
                    _viewBindings.AddRange(newBindings);
                }
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view language binding {0}",
                                      exception.ToLongString());
                throw;
            }
        }

        private View CreateView(string name, Context context, IAttributeSet attrs)
        {
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
    }
}