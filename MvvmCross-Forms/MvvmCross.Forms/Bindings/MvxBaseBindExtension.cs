using System;
using System.Reflection;
using MvvmCross.Binding;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    public abstract class MvxBaseBindExtension : IMarkupExtension
    {
        public MvxBindingMode Mode { get; set; } = MvxBindingMode.Default;
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }
        public string FallbackValue { get; set; }

        public IProvideValueTarget Target { get; private set; }
        public object BindableObj { get; private set; }
        public object BindableProp { get; private set; }
        public string PropertyName { get; private set; } = string.Empty;

        public abstract object ProvideValue(IServiceProvider serviceProvider);

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            Target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            BindableObj = Target.TargetObject;
            BindableProp = Target.TargetProperty;

            if (Target.TargetProperty is BindableProperty bp)
                PropertyName = bp.PropertyName;
            else if (Target.TargetProperty is PropertyInfo pi)
                PropertyName = pi.Name;

            return ProvideValue(serviceProvider);
        }
    }
}
