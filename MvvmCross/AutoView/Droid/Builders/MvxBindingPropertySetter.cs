// MvxBindingPropertySetter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using CrossUI.Core.Builder;
using System;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxBindingPropertySetter : IPropertySetter

    {
        private readonly IMvxAndroidBindingContext _androidBindingContext;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxAndroidBindingContext androidBindingContext, object source)
        {
            _androidBindingContext = androidBindingContext;
            _source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = Mvx.Resolve<IMvxBinder>()
                                 .BindSingle(_source, element, targetPropertyName, configuration);
                _androidBindingContext.RegisterBinding(element, binding);
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