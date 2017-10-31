using System;
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
        public BindableObject BindableObj { get; private set; }
        public BindableProperty BindableProp { get; private set; }

        public abstract object ProvideValue(IServiceProvider serviceProvider);

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            Target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            BindableObj = Target.TargetObject as BindableObject;
            BindableProp = Target.TargetProperty as BindableProperty;

            return ProvideValue(serviceProvider);
        }
    }
}
