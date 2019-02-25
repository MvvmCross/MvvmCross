// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using MvvmCross.Logging;
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
            if (!string.IsNullOrEmpty(PropertyName))
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

                if (Bindable != null)
                {
                    Bindable.SetValue(Bi.ndProperty, bindingBuilder.ToString());
                }
                else
                {
                    MvxFormsLog.Instance.Trace("Can only use MvxBind on a bindable view");
                }
            }
            else if (BindableObjectRaw is IMarkupExtension ext)
            {
                return ext.ProvideValue(serviceProvider);
            }
            else
            {
                MvxFormsLog.Instance.Trace("Can only use MvxBind on a bindable property");
            }

            return null;
        }
    }
}
