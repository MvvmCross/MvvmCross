#region Copyright
// <copyright file="MvxJavaBindingWrapper.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Android.Binders
{
    public class MvxJavaBindingWrapper : Object
    {
        public MvxJavaBindingWrapper(IEnumerable<IMvxUpdateableBinding> bindings)
        {
            Bindings = bindings.ToList();
        }

        public IList<IMvxUpdateableBinding> Bindings { get; private set; }
    }
}