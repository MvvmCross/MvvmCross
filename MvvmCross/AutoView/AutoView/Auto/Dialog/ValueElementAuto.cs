// ValueElementAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.Dialog
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Input;

    using CrossUI.Core.Descriptions.Dialog;

    public class ValueElementAuto : ElementAuto
    {
        public string Value { get; set; }
        public Expression<Func<object>> BindingExpression { get; set; }
        public string BindingExpressionTextOverride { get; set; }
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }

        public ValueElementAuto(string key = null, Expression<Func<object>> bindingExpression = null,
                                string converter = null, string converterParameter = null, string value = null,
                                string caption = null, string onlyFor = null, string notFor = null,
                                Expression<Func<ICommand>> selectedCommand = null, string layoutName = null)
            : base(key ?? "String", caption, onlyFor, notFor, selectedCommand, layoutName)
        {
            this.Value = value;
            this.BindingExpression = bindingExpression;
            this.Converter = converter;
            this.ConverterParameter = converterParameter;
        }

        public override ElementDescription ToElementDescription()
        {
            var toReturn = base.ToElementDescription();

            if (this.Value != null)
            {
                toReturn.Properties["Value"] = this.Value;
            }

            string bindingText = null;
            if (this.BindingExpressionTextOverride != null)
            {
                bindingText = this.BindingExpressionTextOverride.CreateBindingText(this.Converter, this.ConverterParameter);
            }
            else if (this.BindingExpression != null)
            {
                bindingText = this.BindingExpression.CreateBindingText(this.Converter, this.ConverterParameter);
            }
            if (bindingText != null)
            {
                toReturn.Properties["Value"] = "@MvxBind:" + bindingText;
            }

            return toReturn;
        }
    }
}