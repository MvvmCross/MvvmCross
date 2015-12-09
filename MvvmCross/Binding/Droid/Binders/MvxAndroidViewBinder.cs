// MvxAndroidViewBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Android.Content;
    using Android.Content.Res;
    using Android.Util;
    using Android.Views;

    using MvvmCross.Binding.Droid.ResourceHelpers;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxAndroidViewBinder : IMvxAndroidViewBinder
    {
        private readonly List<KeyValuePair<object, IMvxUpdateableBinding>> _viewBindings
            = new List<KeyValuePair<object, IMvxUpdateableBinding>>();

        private readonly object _source;

        public MvxAndroidViewBinder(object source)
        {
            this._source = source;
        }

        private IMvxBinder _binder;

        protected IMvxBinder Binder => this._binder ?? (this._binder = Mvx.Resolve<IMvxBinder>());

        public IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings => this._viewBindings;

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
                        this.ApplyBindingsFromAttribute(view, typedArray, attributeId);
                    }
                    else if (attributeId == MvxAndroidBindingResource.Instance.BindingLangId)
                    {
                        this.ApplyLanguageBindingsFromAttribute(view, typedArray, attributeId);
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
                var newBindings = this.Binder.Bind(this._source, view, bindingText);
                this.StoreBindings(view, newBindings);
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
                this._viewBindings.AddRange(newBindings.Select(b => new KeyValuePair<object, IMvxUpdateableBinding>(view, b)));
            }
        }

        private void ApplyLanguageBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
        {
            try
            {
                var bindingText = typedArray.GetString(attributeId);
                var newBindings = this.Binder.LanguageBind(this._source, view, bindingText);
                this.StoreBindings(view, newBindings);
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