#region Copyright
// <copyright file="MvxPropertyUtils.cs" company="Cirrious">
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
using System.Reflection;

namespace Cirrious.MvvmCross.Property
{
    public static class MvxPropertyUtils
    {
        private const string WrongExpressionMessage = "Wrong expression\nshould be called as\nPropertyName(() => Property);";

        public static string PropertyName<T>(Expression<Func<T>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            var member = memberExpression.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            if (member.DeclaringType == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

#if NETFX_CORE
            if (member.GetMethod.IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#else
            if (member.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#endif

            return member.Name;
        }
    }
}
