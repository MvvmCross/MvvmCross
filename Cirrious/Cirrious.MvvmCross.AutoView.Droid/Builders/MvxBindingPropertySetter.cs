// MvxBindingPropertySetter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxBindingPropertySetter : IPropertySetter
                                            , IMvxServiceConsumer
    {
        private readonly IMvxDroidBindingContext _droidBindingContext;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxDroidBindingContext droidBindingContext, object source)
        {
            _droidBindingContext = droidBindingContext;
            _source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = this.GetService<IMvxBinder>()
                                  .BindSingle(_source, element, targetPropertyName, configuration);
                _droidBindingContext.RegisterBinding(binding);
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