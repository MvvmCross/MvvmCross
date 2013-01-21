// MvxBindingLayoutInflatorFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.Content;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxBindingLayoutInflatorFactory
        : Object
          , LayoutInflater.IFactory
          , IMvxServiceConsumer
    {
        private readonly LayoutInflater _layoutInflater;
        private readonly object _source;

        private readonly Dictionary<View, IList<IMvxUpdateableBinding>> _viewBindings
            = new Dictionary<View, IList<IMvxUpdateableBinding>>();

        private IMvxViewTypeResolver _viewTypeResolver;

        public MvxBindingLayoutInflatorFactory(
            object source,
            LayoutInflater layoutInflater)
        {
            _source = source;
            _layoutInflater = layoutInflater;
        }

        private IMvxViewTypeResolver ViewTypeResolver
        {
            get
            {
                if (_viewTypeResolver == null)
                    _viewTypeResolver = this.GetService<IMvxViewTypeResolver>();
                return _viewTypeResolver;
            }
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
                        try
                        {
                            var bindingText = typedArray.GetString(attributeId);
                            var newBindings = this.GetService<IMvxBinder>().Bind(_source, view, bindingText);
                            if (newBindings != null)
                            {
                                var asList = newBindings.ToList();
                                _viewBindings[view] = asList;
                            }
                        }
                        catch (Exception exception)
                        {
                            MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view binding {0}",
                                                  exception.ToLongString());
                            throw;
                        }
                    }
                }
                typedArray.Recycle();
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

        public void StoreBindings(View view)
        {
            view.StoreBindings(_viewBindings);
        }
    }
}