using System;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public interface IMvxPropertyExpressionParser
    {
        IMvxParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath);
        IMvxParsedExpression Parse(LambdaExpression propertyPath);
    }
}