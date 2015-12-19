// IMvxBindingNameRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;
    using System.Linq.Expressions;

    public interface IMvxBindingNameRegistry
    {
        void AddOrOverwrite(Type type, string name);

        void AddOrOverwrite<T>(Expression<Func<T, object>> nameExpression);
    }
}