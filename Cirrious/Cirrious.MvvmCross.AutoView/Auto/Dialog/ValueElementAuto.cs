using System;
using System.Linq.Expressions;
using System.Windows.Input;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Foobar.Dialog.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class ValueElementAuto : ElementAuto, IMvxServiceConsumer
    {
        public string Value { get; set; }
        public Expression<Func<object>> BindingExpression { get; set; }
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }

        public ValueElementAuto(string key = null, Expression<Func<object>> bindingExpression = null, string converter = null, string converterParameter = null, string value = null, string caption = null, string onlyFor = null, string notFor = null, Expression<Func<ICommand>> selectedCommand = null, string layoutName = null)
            : base(key ?? "String", caption, onlyFor, notFor, selectedCommand, layoutName)
        {
            Value = value;
            BindingExpression = bindingExpression;
            Converter = converter;
            ConverterParameter = converterParameter;
        }

        public override ElementDescription ToElementDescription()
        {
            var toReturn = base.ToElementDescription();
            
            if (Value != null)
            {
                toReturn.Properties["Value"] = Value;
            }

            if (BindingExpression != null)
            {
                var bindingText = BindingExpression.CreateBindingText(Converter, ConverterParameter);
                toReturn.Properties["Value"] = "@MvxBind:" + bindingText;
            }

            return toReturn;
        }
    }
}