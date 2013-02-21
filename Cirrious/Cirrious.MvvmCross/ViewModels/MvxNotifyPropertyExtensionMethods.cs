// MvxNotifyPropertyExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxNotifyPropertyExtensionMethods
    {
        private const string WrongExpressionMessage =
            "Wrong expression\nshould be called with expression like\n() => PropertyName";

        public static string GetPropertyNameFromExpression<T>(
            this INotifyPropertyChanged target,
            Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            var member = memberExpression.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (member.DeclaringType == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (!member.DeclaringType.IsAssignableFrom(target.GetType()))
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (member.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            return member.Name;
        }
    }
}