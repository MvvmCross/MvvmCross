using System;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxPropertyExpressionParser : IMvxPropertyExpressionParser
    {
        public IMvxParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath)
        {
            return Parse((LambdaExpression)propertyPath);
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

            throw new ArgumentException("Property expression must be of the form 'x => x.SomeProperty.SomeOtherProperty'");
        }

        private static Expression ParseMethodCall(Expression current, MvxParsedExpression toReturn)
        {
            var me = (MethodCallExpression) current;
            if (me.Method.Name != "get_Item"
                || me.Arguments.Count != 1)
            {
                throw new ArgumentException(
                    "Property expression must be of the form 'x => x.SomeProperty.SomeOtherProperty' or 'x => x.SomeCollection[0].Property'");
            }
            var argument = me.Arguments[0];
            toReturn.PrependIndexed(argument.ToString());
            current = me.Object;
            return current;
        }

        private static Expression ParseProperty(Expression current, MvxParsedExpression toReturn)
        {
            var me = (MemberExpression) current;
            toReturn.PrependProperty(me.Member.Name);
            current = me.Expression;
            return current;
        }

        private static Expression Unbox(Expression current)
        {
            var ue = (UnaryExpression) current;
            current = ue.Operand;
            return current;
        }
    }
}