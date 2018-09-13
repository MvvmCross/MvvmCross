// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using MvvmCross.Localization;
using MvvmCross.Logging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Bindings
{
    [ContentProperty("Source")]
    public class MvxLangExtension : MvxBaseBindExtension
    {
        public string Source { get; set; }
        public string NameSpaceKey { get; set; } = "";
        public string TypeKey { get; set; } = "";
        public object[] Arguments { get; set; }
        public string Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrEmpty(PropertyName))
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

                if (Bindable != null)
                {
                    Bindable.SetValue(La.ngProperty, bindingBuilder.ToString());
                }
                else
                {
                    MvxFormsLog.Instance.Trace("Can only use MvxLang on a bindable type");
                }
            }
            else if (!string.IsNullOrEmpty(Source) && Mvx.IoCProvider.CanResolve<IMvxTextProvider>())
            {
                return GetStringFromLanguageBinder();
            }
            else if (BindableObjectRaw is IMarkupExtension ext)
            {
                return ext.ProvideValue(serviceProvider);
            }
            else
            {
                MvxFormsLog.Instance.Trace("Can only use MvxLang on a bindable property");
            }

            return null;
        }

        private string GetStringFromLanguageBinder()
        {
            if (Arguments == null)
                return new MvxLanguageBinder(NameSpaceKey, TypeKey).GetText(Source);
            else
                return new MvxLanguageBinder(NameSpaceKey, TypeKey).GetText(Source, Arguments);
        }
    }
}
