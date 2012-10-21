using System;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Foobar.Dialog.Core.Builder;

namespace CustomerManagement.Droid.Views
{
    public class MvxBindingPropertySetter : IPropertySetter
                                            , IMvxServiceConsumer
    {
        private IMvxBindingDialogActivity _bindingActivity;
        private object _source;

        public MvxBindingPropertySetter(IMvxBindingDialogActivity bindingActivity, object source)
        {
            _bindingActivity = bindingActivity;
            _source = source;
        }

        public void Set(object element, string targetPropertyName, string configuration)
        {
            try
            {
                var binding = this.GetService<IMvxBinder>().BindSingle(_source, element, targetPropertyName, configuration);
                if (binding != null)
                {
                    _bindingActivity.RegisterDialogBinding(binding);
                }
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Exception thrown during the view binding {0}", exception.ToLongString());
                throw;
            }
        }
    }
}