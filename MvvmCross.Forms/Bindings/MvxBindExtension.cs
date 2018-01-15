using System;
using System.Text;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    [ContentProperty("Path")]
    public class MvxBindExtension : MvxBaseBindExtension
    {
        public string Path { get; set; } = ".";

        public string StringFormat { get; set; }

        public string CommandParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (BindableObj is BindableObject obj && !string.IsNullOrEmpty(PropertyName))
            {
                StringBuilder bindingBuilder = new StringBuilder($"{PropertyName} {Path}, Mode={Mode}");

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
            else if (BindableObj is IMarkupExtension ext)
            {
                return ext.ProvideValue(serviceProvider);
            }
            else
            {
                Mvx.Trace(MvxTraceLevel.Diagnostic, "Can only use MvxBind on a bindable property");
            }

            return null;
        }
    }
}
