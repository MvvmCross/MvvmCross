// MvxBindingPropertySetter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Touch.Views;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders
{
    public class MvxBindingPropertySetter : IPropertySetter

    {
        private readonly IMvxTouchView _touchView;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxTouchView touchView, object source)
        {
            _touchView = touchView;
            _source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = Mvx.Resolve<IMvxBinder>()
                                 .BindSingle(_source, element, targetPropertyName, configuration);
                _touchView.BindingContext.RegisterBinding(element, binding);
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