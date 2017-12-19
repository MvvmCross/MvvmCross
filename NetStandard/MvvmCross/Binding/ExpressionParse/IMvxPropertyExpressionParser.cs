// IMvxPropertyExpressionParser.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;

namespace MvvmCross.Binding.ExpressionParse
{
    public interface IMvxPropertyExpressionParser
    {
        IMvxParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath);

        IMvxParsedExpression Parse(LambdaExpression propertyPath);
    }
}