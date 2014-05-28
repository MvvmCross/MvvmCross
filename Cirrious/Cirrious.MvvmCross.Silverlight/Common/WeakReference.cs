using System;

namespace Cirrious.MvvmCross.Silverlight.Common
{
    public class WeakReference<T> where T : class
    {
        private readonly WeakReference _inner;

        public WeakReference(T target)
            : this(target, false)
        { }

        public WeakReference(T target, bool trackResurrection)
        {
            if (target == null) throw new ArgumentNullException("target");
            _inner = new WeakReference(target, trackResurrection);
        }

        public T Target
        {
            get
            {
                return (T)_inner.Target;
            }
            set
            {
                _inner.Target = value;
            }
        }

        public bool IsAlive
        {
            get
            {
                return _inner.IsAlive;
            }
        }

        //TODo Mvx - Verify WeakReference TryGetTarget
        public bool TryGetTarget(out T target)
        {
            if (_inner.Target != null)
            {
                target = Target;
                return true;
            }
            target = null;
            return false;
        }
    }
}