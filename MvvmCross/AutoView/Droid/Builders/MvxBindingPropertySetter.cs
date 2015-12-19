// MvxBindingPropertySetter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Builders
{
    using System;

    using CrossUI.Core.Builder;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxBindingPropertySetter : IPropertySetter

    {
        private readonly IMvxAndroidBindingContext _androidBindingContext;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxAndroidBindingContext androidBindingContext, object source)
        {
            this._androidBindingContext = androidBindingContext;
            this._source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = Mvx.Resolve<IMvxBinder>()
                                 .BindSingle(this._source, element, targetPropertyName, configuration);
                this._androidBindingContext.RegisterBinding(element, binding);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view binding {0}",
                                      exception.ToLongString());
                throw;
            }
        }
    }
}