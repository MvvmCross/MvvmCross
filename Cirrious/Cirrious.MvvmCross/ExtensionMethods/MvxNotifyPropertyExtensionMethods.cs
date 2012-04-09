#region Copyright
// <copyright file="MvxNotifyPropertyExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxNotifyPropertyExtensionMethods
    {
        private const string WrongExpressionMessage = "Wrong expression\nshould be called with expression like\n() => PropertyName";

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

#if NETFX_CORE
            if (!member.DeclaringType.GetTypeInfo().IsAssignableFrom(target.GetType().GetTypeInfo()))
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            if (member.GetMethod.IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#else
            if (!member.DeclaringType.IsAssignableFrom(target.GetType()))
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (member.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }
#endif
            return member.Name;
        }
    }
}