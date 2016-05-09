// MvxPropertyExpressionParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;

namespace MvvmCross.Binding.ExpressionParse
{
    using System;
    using System.Linq.Expressions;

    using MvvmCross.Platform;

    // This class was inspired and influenced by the excellent binding work
    // by https://github.com/reactiveui/ReactiveUI/
    // Inspiration used under Microsoft Public License Ms-PL
    public class MvxPropertyExpressionParser : IMvxPropertyExpressionParser
    {
        public IMvxParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath)
        {
            return this.Parse((LambdaExpression)propertyPath);
        }

        public IMvxParsedExpression Parse(LambdaExpression propertyPath)
        {
            var toReturn = new MvxParsedExpression();

            var current = propertyPath.Body;
            while (current != null
                   && current.NodeType != ExpressionType.Parameter)
            {
                current = ParseTo(current, toReturn);
            }

            return toReturn;
        }

        private static Expression ParseTo(Expression current, MvxParsedExpression toReturn)
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
                return ParseMethodCall(current, toReturn);
            }

            throw new ArgumentException(
                "Property expression must be of the form 'x => x.SomeProperty.SomeOtherProperty'");
        }

        private static Expression ParseMethodCall(Expression current, MvxParsedExpression toReturn)
        {
            var me = (MethodCallExpression)current;
            if (me.Method.Name != "get_Item"
                || me.Arguments.Count != 1)
            {
                throw new ArgumentException(
                    "Property expression must be of the form 'x => x.SomeProperty.SomeOtherProperty' or 'x => x.SomeCollection[0].Property'");
            }
            var argument = me.Arguments[0];
            argument = ConvertMemberAccessToConstant(argument);
            toReturn.PrependIndexed(argument.ToString());
            current = me.Object;
            return current;
        }

        private static Expression ConvertMemberAccessToConstant(Expression argument)
        {
            var memberExpr = argument as MemberExpression;
            if (memberExpr == null)
                return argument;

            try
            {
                var constExpr = ConvertMemberAccessToConstant(memberExpr.Expression) as ConstantExpression;
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
                Mvx.Trace("Failed to evaluate member expression.");
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