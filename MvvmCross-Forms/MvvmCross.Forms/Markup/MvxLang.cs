using System;
using MvvmCross.Forms.Bindings;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCross.Forms.Markup
{
    [ContentProperty("Binding")]
    public class MvxLang : IMarkupExtension
    {
        public string Binding { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            BindableObject obj = target.TargetObject as BindableObject;
            BindableProperty bindableProperty = target.TargetProperty as BindableProperty;

            if (obj != null && bindableProperty != null)
            {
                obj.SetValue(La.ngProperty, $"{bindableProperty.PropertyName} {Binding}");
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
