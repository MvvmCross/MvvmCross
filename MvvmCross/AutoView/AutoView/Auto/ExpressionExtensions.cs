// ExpressionExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public static class ExpressionExtensions
    {
        public static string CreateBindingText<T>(this Expression<Func<T, object>> bindingExpression, string converter,
                                                  string converterParameter)
        {
            var path = bindingExpression.GetPropertyText();
            return CreateBindingText(path, converter, converterParameter);
        }

        public static string CreateBindingText(this Expression<Func<object>> bindingExpression, string converter,
                                               string converterParameter)
        {
            return bindingExpression.GetPropertyText().CreateBindingText(converter, converterParameter);
        }

        public static string CreateBindingText(this string path, string converter, string converterParameter)
        {
#warning Could do with adding this to a helper object rather than embedding it here in an ext method
            var bindingText = new StringBuilder();

            if (!string.IsNullOrEmpty(path))
            {
                bindingText.Append(path);
            }
            if (!string.IsNullOrEmpty(converter))
            {
                if (bindingText.Length > 0)
                    bindingText.Append(",");
                bindingText.AppendFormat("Converter={0}", converter);
            }
#warning This needs finishing! Converter Parameter needs escaping!
            if (!string.IsNullOrEmpty(converterParameter))
            {
                if (bindingText.Length > 0)
                    bindingText.Append(",");
                bindingText.AppendFormat("ConverterParameter={0}", converterParameter);
            }
            return bindingText.ToString();
            //	var binding = new MvxSerializableBindingDescription
            //	{
            //	Path = path,
            //	Converter = converter,
            //	ConverterParameter = converterParameter
            //};
            //var json = MvxServiceProviderExtensions.Resolve<IMvxJsonConverter>().SerializeObject(binding);
            //var bindingText = json;
            //return bindingText;
        }

        public static string GetPropertyText<T, R>(this Expression<Func<T, R>> expression)
        {
            var memberExpression = (expression.Body is UnaryExpression) ? ((UnaryExpression)expression.Body).Operand as MemberExpression : expression.Body as MemberExpression;
            return GetPropertyText(memberExpression, ".");
        }

        public static string GetPropertyText<T>(this Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentException("WrongExpressionMessage (memberExpression is null)", nameof(expression));
            var memberExpression =
                (expression.Body is UnaryExpression) ? ((UnaryExpression)expression.Body).Operand as MemberExpression : expression.Body as MemberExpression;
            return GetPropertyText(memberExpression, ").");
        }

        private static string GetPropertyText(MemberExpression memberExpression, string endOfOwnerDelimeter)
        {
            if (memberExpression == null)
            {
                throw new ArgumentException("WrongExpressionMessage (memberExpression is null)", nameof(memberExpression));
            }

            var member = memberExpression.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException(
                    $"WrongExpressionMessage (memberExpression.Member is not PropertyInfo but {memberExpression.Member})", nameof(memberExpression));
            }

            var text = memberExpression.ToString();
            var endOfOwnerPosition = text.IndexOf(endOfOwnerDelimeter);
            if (endOfOwnerPosition < 0)
            {
                MvxTrace.Error("Failed to convert text - cannot find expected text in the Expression: {0}", text);
                throw new MvxException("Failed to convert text - cannot find expected text in the Expression: {0}", text);
            }

            text = text.Substring(endOfOwnerPosition + endOfOwnerDelimeter.Length);
            return text;
        }
    }
}