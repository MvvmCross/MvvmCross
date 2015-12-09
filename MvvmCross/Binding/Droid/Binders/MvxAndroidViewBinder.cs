// MvxAndroidViewBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxAndroidViewBinder : IMvxAndroidViewBinder
    {
        private readonly List<KeyValuePair<object, IMvxUpdateableBinding>> _viewBindings
            = new List<KeyValuePair<object, IMvxUpdateableBinding>>();

        private readonly object _source;

        public MvxAndroidViewBinder(object source)
        {
            _source = source;
        }

        private IMvxBinder _binder;

        protected IMvxBinder Binder => _binder ?? (_binder = Mvx.Resolve<IMvxBinder>());

        public IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings => _viewBindings;

        public virtual void BindView(View view, Context context, IAttributeSet attrs)
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
                StoreBindings(view, newBindings);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view binding {0}",
                                      exception.ToLongString());
            }
        }

        private void StoreBindings(View view, IEnumerable<IMvxUpdateableBinding> newBindings)
        {
            if (newBindings != null)
            {
                _viewBindings.AddRange(newBindings.Select(b => new KeyValuePair<object, IMvxUpdateableBinding>(view, b)));
            }
        }

        private void ApplyLanguageBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
        {
            try
            {
                var bindingText = typedArray.GetString(attributeId);
                var newBindings = Binder.LanguageBind(_source, view, bindingText);
                StoreBindings(view, newBindings);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view language binding {0}",
                                      exception.ToLongString());
                throw;
            }
        }
    }
}