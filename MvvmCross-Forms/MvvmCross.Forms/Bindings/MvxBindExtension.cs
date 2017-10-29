using System;
using System.Text;
using MvvmCross.Binding;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    [ContentProperty("Path")]
    public class MvxBindExtension : IMarkupExtension
    {
        public MvxBindExtension()
        {
            Mode = MvxBindingMode.Default;
            Path = ".";
        }

        public string Path { get; set; }

        public MvxBindingMode Mode { get; set; }

        public string Converter { get; set; }

        public string ConverterParameter { get; set; }

        public string StringFormat { get; set; }

        public string FallbackValue { get; set; }

        public string CommandParameter { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            BindableObject obj = target.TargetObject as BindableObject;
            BindableProperty bindableProperty = target.TargetProperty as BindableProperty;

            IRootObjectProvider rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;

            if (obj != null && bindableProperty != null)
            {
                StringBuilder bindingBuilder = new StringBuilder($"{bindableProperty.PropertyName} {Path}, Mode={Mode}");

                if (!string.IsNullOrEmpty(Converter))
                {
                    bindingBuilder.Append($", Converter={Converter}");
                }

                if (!string.IsNullOrEmpty(ConverterParameter))
                {
                    bindingBuilder.Append($", ConverterParameter={ConverterParameter}");
                }

                if (!string.IsNullOrEmpty(FallbackValue))
                {
                    bindingBuilder.Append($", FallbackValue={FallbackValue}");
                }

                if (!string.IsNullOrEmpty(CommandParameter))
                {
                    bindingBuilder.Append($", CommandParameter={CommandParameter}");
                }

                obj.SetValue(Bi.ndProperty, bindingBuilder.ToString());
            }
            else
            {
                Mvx.Trace(MvxTraceLevel.Diagnostic, "Cannot only use MvxBind on a bindable property");
            }

            return null;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}
