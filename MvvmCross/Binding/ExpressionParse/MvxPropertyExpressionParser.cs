// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MvvmCross.Binding.ExpressionParse
{
    // This class was inspired and influenced by the excellent binding work
    // by https://github.com/reactiveui/ReactiveUI/
    // Inspiration used under Microsoft Public License Ms-PL
    public class MvxPropertyExpressionParser : IMvxPropertyExpressionParser
    {
        private readonly ILogger<MvxPropertyExpressionParser> _log;

        public MvxPropertyExpressionParser(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<MvxPropertyExpressionParser>();
        }

        public IMvxParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath)
        {
            if (propertyPath.Body is MethodCallExpression
                && (propertyPath.Body as MethodCallExpression).Method.Name.Contains("Bind"))
            {
                return ParseBindExtensionMethod(propertyPath as LambdaExpression, default(TObj));
            }

            return Parse((LambdaExpression)propertyPath);
        }

        public IMvxParsedExpression Parse(LambdaExpression propertyPath)
        {
            var toReturn = new MvxParsedExpression();

            var current = propertyPath.Body;
            while (current != null
                   && current.NodeType != ExpressionType.Parameter)
            {
                current = ParseTo(current, toReturn, _log);
            }

            return toReturn;
        }

        private static Expression ParseTo(Expression current, MvxParsedExpression toReturn, ILogger log)
        {
            // This happens when a value type gets boxed
            if (current.NodeType == ExpressionType.Convert || current.NodeType == ExpressionType.ConvertChecked)
            {
                return Unbox(current);
            }

            if (current.NodeType == ExpressionType.MemberAccess)
            {
                return ParseProperty(current, toReturn);
            }

            if (current is MethodCallExpression)
            {
                return ParseMethodCall(current, toReturn, log);
            }

            throw new ArgumentException(
                "Property expression must be of the form 'x => x.SomeProperty.SomeOtherProperty'");
        }

        private static Expression ParseMethodCall(Expression current, MvxParsedExpression toReturn, ILogger log)
        {
            var me = (MethodCallExpression)current;
            if (me.Method.Name != "get_Item"
                || me.Arguments.Count != 1)
            {
                throw new ArgumentException(
                    "Property expression must be of the form 'x => x.SomeProperty.SomeOtherProperty' or 'x => x.SomeCollection[0].Property'");
            }
            var argument = me.Arguments[0];
            argument = ConvertMemberAccessToConstant(argument, log);
            toReturn.PrependIndexed(argument.ToString());
            current = me.Object;
            return current;
        }

        private static IMvxParsedExpression ParseBindExtensionMethod(LambdaExpression propertyPath, object controlType)
        {
            var compiled = propertyPath.Compile();
            var virtualPropertyName = compiled.DynamicInvoke(controlType) as string;

            var toReturn = new MvxParsedExpression();
            toReturn.PrependProperty(virtualPropertyName);
            return toReturn;
        }

        private static Expression ConvertMemberAccessToConstant(Expression argument, ILogger log)
        {
            var memberExpr = argument as MemberExpression;
            if (memberExpr == null)
                return argument;

            try
            {
                var constExpr = ConvertMemberAccessToConstant(memberExpr.Expression, log) as ConstantExpression;
                var value = constExpr?.Value;

                var property = memberExpr.Member as PropertyInfo;
                if (property != null)
                {
                    var constant = property.GetValue(value);
                    return Expression.Constant(constant);
                }

                var field = memberExpr.Member as FieldInfo;
                if (field != null)
                {
                    var constant = field.GetValue(value);
                    return Expression.Constant(constant);
                }
            }
            catch
            {
                log.LogTrace("Failed to evaluate member expression.");
            }

            return argument;
        }

        private static Expression ParseProperty(Expression current, MvxParsedExpression toReturn)
        {
            var me = (MemberExpression)current;
            toReturn.PrependProperty(me.Member.Name);
            current = me.Expression;
            return current;
        }

        private static Expression Unbox(Expression current)
        {
            var ue = (UnaryExpression)current;
            current = ue.Operand;
            return current;
        }
    }
}
