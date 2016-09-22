using System;
using Android.Runtime;

namespace MvvmCross.Droid.Support.V17.Leanback
{
    [Register("mvvmcross.droid.support.v17.leanback.MvxObjectWrapper")]
    public class MvxObjectWrapper : Java.Lang.Object
    {
        /// <summary>
        /// Strong Reference for special cases. Beware of memory leaks by reference cycles!
        /// </summary>
        private object _strongReference;

        /// <summary>
        /// Weak reference. Should be fine for most cases, since data usually will be referenced somewhere else too, e.g. by a List in a MvxViewModel or similar.
        /// </summary>
        private readonly WeakReference<object> _weakReference;

        public object Instance
        {
            get
            {
                if (_strongReference != null)
                {
                    return _strongReference;
                }

                object target = null;
                return _weakReference?.TryGetTarget(out target) == true
                    ? target
                    : null;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instance">The C# instance to be wrapped in a Java-Object</param>
        /// <param name="useStrongReference">Set to true, if instance isn't referenced anywhere else (e.g. in your MvxViewModel)</param>
        public MvxObjectWrapper(object instance, bool useStrongReference = false)
        {
            if (useStrongReference)
            {
                _strongReference = instance;
            }
            else
            {
                _weakReference = new WeakReference<object>(instance);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _strongReference = null;
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Generic class for convience. Use this if you hate casting.
    /// </summary>
    /// <typeparam name="T">Type you want to wrap.</typeparam>
    public class MvxObjectWrapper<T> : MvxObjectWrapper
    {
        public new T Instance => (T)base.Instance;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instance">The C# instance to be wrapped in a Java-Object</param>
        /// <param name="useStrongReference">Set to true, if instance isn't referenced anywhere else (e.g. in your MvxViewModel)</param>
        public MvxObjectWrapper(T instance, bool useStrongReference = false) : base(instance, useStrongReference)
        {
        }
    }
}