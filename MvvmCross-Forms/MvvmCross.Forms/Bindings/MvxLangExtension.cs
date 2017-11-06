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
    public class MvxLangExtension : MvxBaseBindExtension
    {
        public string Source { get; set; }

        public string Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (BindableObj != null && !string.IsNullOrEmpty(PropertyName))
            {
                StringBuilder bindingBuilder = new StringBuilder($"{PropertyName} {Source}");

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

                BindableObj.SetValue(La.ngProperty, bindingBuilder.ToString());
            }
            else
            {
                Mvx.Trace(MvxTraceLevel.Diagnostic, "Cannot only use MvxLang on a bindable property");
            }

            return null;
        }
    }
}
