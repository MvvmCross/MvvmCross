// MvxJavaContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid
{
    using Java.Lang;

    public class MvxJavaContainer : Object
    {
        protected MvxJavaContainer(object theObject)
        {
            this.Object = theObject;
        }

        public object Object { get; private set; }
    }

    public class MvxJavaContainer<T> : MvxJavaContainer
    {
        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }

        public new T Object => (T)base.Object;
    }
}