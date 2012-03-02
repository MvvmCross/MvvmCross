#region Copyright
// <copyright file="MvxJavaContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Android
{
    public class MvxJavaContainer : Object
    {
        public MvxJavaContainer(object theObject)
        {
            Object = theObject;
        }

        public object Object { get; private set; }
    }
    public class MvxJavaContainer<T> : MvxJavaContainer
    {
        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }

        public new T Object { get { return (T)base.Object; } }
    }
}