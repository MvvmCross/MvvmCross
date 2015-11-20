// BindingAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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