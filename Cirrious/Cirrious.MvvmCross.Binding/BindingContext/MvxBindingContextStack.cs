// MvxBindingContextStack.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.BindingContext
{
    public class MvxBindingContextStack<TContext>
        : Stack<TContext>
          , IMvxBindingContextStack<TContext>
    {
        public TContext Current
        {
            get { return Peek(); }
        }
    }
}