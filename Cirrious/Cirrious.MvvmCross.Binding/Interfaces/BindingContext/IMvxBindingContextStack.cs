// IMvxBindingContextStack.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.Interfaces.BindingContext
{
    public interface IMvxBindingContextStack<TContext>
    {
        TContext Current { get; }

        void Push(TContext context);
        TContext Pop();
    }
}