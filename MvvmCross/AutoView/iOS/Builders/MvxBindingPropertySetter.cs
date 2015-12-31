// MvxBindingPropertySetter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders
{
    using System;

    using CrossUI.Core.Builder;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;
    using MvvmCross.iOS.Views;

    public class MvxBindingPropertySetter : IPropertySetter

    {
        private readonly IMvxTouchView _touchView;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxTouchView touchView, object source)
        {
            this._touchView = touchView;
            this._source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = Mvx.Resolve<IMvxBinder>()
                                 .BindSingle(this._source, element, targetPropertyName, configuration);
                this._touchView.BindingContext.RegisterBinding(element, binding);
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