// IMvxBindingNameRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public interface IMvxBindingNameRegistry
    {
        void AddOrOverwrite(Type type, string name);

        void AddOrOverwrite<T>(Expression<Func<T, object>> nameExpression);
    }
}