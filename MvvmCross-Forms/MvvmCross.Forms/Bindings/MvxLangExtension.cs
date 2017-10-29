using System;
using System.Text;
using MvvmCross.Forms.Bindings;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    [ContentProperty("Source")]
    public class MvxLangExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public string Converter { get; set; }

        public string ConverterParameter { get; set; }

        public string Key { get; set; }

        public string FallbackValue { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            BindableObject obj = target.TargetObject as BindableObject;
            BindableProperty bindableProperty = target.TargetProperty as BindableProperty;

            if (obj != null && bindableProperty != null)
            {
                StringBuilder bindingBuilder = new StringBuilder($"{bindableProperty.PropertyName} {Source}");

                if (!string.IsNullOrEmpty(Converter))
                {
                    bindingBuilder.Append($", Converter={Converter}");
                }

                if (!string.IsNullOrEmpty(ConverterParameter))
                {
                    bindingBuilder.Append($", ConverterParameter={ConverterParameter}");
                }

                if (!string.IsNullOrEmpty(Key))
                {
                    bindingBuilder.Append($", Key={Key}");
                }

                if (!string.IsNullOrEmpty(FallbackValue))
                {
                    bindingBuilder.Append($", FallbackValue={FallbackValue}");
                }

                obj.SetValue(La.ngProperty, bindingBuilder.ToString());
            }
            else
            {
                Mvx.Trace(MvxTraceLevel.Diagnostic, "Cannot only use MvxLang on a bindable property");
            }

            return null;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}
