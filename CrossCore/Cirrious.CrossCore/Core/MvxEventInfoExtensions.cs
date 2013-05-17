// MvxEventInfoExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Cirrious.CrossCore.Core
{
    public static class MvxEventInfoExtensions
    {
        private const string WrongExpressionMessage =
            "Wrong expression\nshould be called with expression like\n() => PropertyName";

        public static EventInfo GetEventInfo<TSource>(this Expression<Func<TSource>> expression)
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

            var member = memberExpression.Member as EventInfo;
            if (member == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (member.DeclaringType == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (!member.DeclaringType.IsAssignableFrom(typeof (TSource)))
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            if (member.GetAddMethod(true).IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "expression");
            }

            return member;
        }
    }
}