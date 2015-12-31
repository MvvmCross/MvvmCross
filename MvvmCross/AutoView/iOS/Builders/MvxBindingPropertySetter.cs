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
        private readonly IMvxIosView _iosView;
        private readonly object _source;

        public MvxBindingPropertySetter(IMvxIosView iosView, object source)
        {
            this._iosView = iosView;
            this._source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = Mvx.Resolve<IMvxBinder>()
                                 .BindSingle(this._source, element, targetPropertyName, configuration);
                this._iosView.BindingContext.RegisterBinding(element, binding);
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