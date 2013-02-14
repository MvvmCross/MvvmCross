// MvxBindingPropertySetter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxBindingPropertySetter : IPropertySetter
                                            , IMvxServiceConsumer
    {
        private readonly IMvxViewBindingManager _bindingManager;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxViewBindingManager bindingManager, object source)
        {
            _bindingManager = bindingManager;
            _source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = this.GetService<IMvxBinder>()
                                  .BindSingle(_source, element, targetPropertyName, configuration);
                _bindingManager.AddBinding(binding);
            }
            catch (Exception exception)
            {
                MvxAutoViewTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view binding {0}",
                                      exception.ToLongString());
                throw;
            }
        }
    }
}