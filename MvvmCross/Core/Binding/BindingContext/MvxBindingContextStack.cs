// MvxBindingContextStack.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System.Collections.Generic;

    public class MvxBindingContextStack<TContext>
        : Stack<TContext>
          , IMvxBindingContextStack<TContext>
    {
        public TContext Current
        {
            get
            {
                if (this.Count == 0)
                    return default(TContext);
                return this.Peek();
            }
        }
    }
}