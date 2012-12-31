#region Copyright

// <copyright file="ValueElementAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Linq.Expressions;
using System.Windows.Input;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CrossUI.Core.Descriptions.Dialog;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class ValueElementAuto : ElementAuto, IMvxServiceConsumer
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

            string bindingText = null;
            if (BindingExpressionTextOverride != null)
            {
                bindingText = BindingExpressionTextOverride.CreateBindingText(Converter, ConverterParameter);
            }
            else if (BindingExpression != null)
            {
                bindingText = BindingExpression.CreateBindingText(Converter, ConverterParameter);
            }
            if (bindingText != null)
            {
                toReturn.Properties["Value"] = "@MvxBind:" + bindingText;
            }

            return toReturn;
        }
    }
}