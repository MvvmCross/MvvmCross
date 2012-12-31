#region Copyright

// <copyright file="BindingAuto.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.AutoView.Auto
{
    public abstract class BindingAuto
    {
        public string Target { get; set; }

        public abstract string GetValueText();

        protected BindingAuto(string target = null)
        {
            Target = target;
        }
    }

    public class BindingAuto<T> : BindingAuto
    {
        public Expression<Func<T, object>> BindingExpression { get; set; }
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }

        public BindingAuto(string target = null, Expression<Func<T, object>> bindingExpression = null,
                           string converter = null, string converterParameter = null)
            : base(target)
        {
            BindingExpression = bindingExpression;
            Converter = converter;
            ConverterParameter = converterParameter;
        }

        public override string GetValueText()
        {
            return BindingExpression.CreateBindingText(Converter, ConverterParameter);
        }
    }
}