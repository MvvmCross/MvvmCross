// ExpressionExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.AutoView.Auto
{
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

        public static string GetPropertyText<T>(this Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return GetPropertyText(memberExpression, ".");
        }

        public static string GetPropertyText<T>(this Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return GetPropertyText(memberExpression, ").");
        }

        private static string GetPropertyText(MemberExpression memberExpression, string endOfOwnerDelimeter)
        {
            if (memberExpression == null)
            {
                throw new ArgumentException("WrongExpressionMessage", "expression");
            }

            var member = memberExpression.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException("WrongExpressionMessage", "expression");
            }

            var text = memberExpression.ToString();
            var endOfOwnerPosition = text.IndexOf(endOfOwnerDelimeter);
            if (endOfOwnerPosition < 0)
            {
                MvxTrace.Trace(MvxTraceLevel.Error,
                               "Failed to convert text - cannot find expected text in the Expression: {0}", text);
                throw new MvxException("Failed to convert text - cannot find expected text in the Expression: {0}", text);
            }

            text = text.Substring(endOfOwnerPosition + endOfOwnerDelimeter.Length);
            return text;
        }
    }
}