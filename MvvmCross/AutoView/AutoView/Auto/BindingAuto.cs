// BindingAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto
{
    using System;
    using System.Linq.Expressions;

    public abstract class BindingAuto
    {
        public string Target { get; set; }

        public abstract string GetValueText();

        protected BindingAuto(string target = null)
        {
            this.Target = target;
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
            this.BindingExpression = bindingExpression;
            this.Converter = converter;
            this.ConverterParameter = converterParameter;
        }

        public override string GetValueText()
        {
            return this.BindingExpression.CreateBindingText(this.Converter, this.ConverterParameter);
        }
    }
}