// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Linq.Expressions;
using System.Reflection;

namespace MvvmCross.Base;

public static class MvxPropertyNameExtensions
{
    private const string WrongExpressionMessage =
        "Wrong expression\nshould be called with expression like\n() => PropertyName";

    private const string WrongUnaryExpressionMessage =
        "Wrong unary expression\nshould be called with expression like\n() => PropertyName";

    public static string GetPropertyNameFromExpression<T>(
        this object target,
        Expression<Func<T>> expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        var memberExpression = FindMemberExpression(expression);

        if (memberExpression == null)
        {
            throw new ArgumentException(WrongExpressionMessage, nameof(expression));
        }

        var member = memberExpression.Member as PropertyInfo;
        if (member == null)
        {
            throw new ArgumentException(WrongExpressionMessage, nameof(expression));
        }

        if (member.DeclaringType == null)
        {
            throw new ArgumentException(WrongExpressionMessage, nameof(expression));
        }

        if (target != null && !member.DeclaringType.IsInstanceOfType(target))
        {
            throw new ArgumentException(WrongExpressionMessage, nameof(expression));
        }

        if (member.GetGetMethod(true)?.IsStatic == true)
        {
            throw new ArgumentException(WrongExpressionMessage, nameof(expression));
        }

        return member.Name;
    }

    private static MemberExpression? FindMemberExpression<T>(Expression<Func<T>> expression)
    {
        if (expression.Body is UnaryExpression body)
        {
            if (body.Operand is not MemberExpression member)
                throw new ArgumentException(WrongUnaryExpressionMessage, nameof(expression));
            return member;
        }

        return expression.Body as MemberExpression;
    }
}
