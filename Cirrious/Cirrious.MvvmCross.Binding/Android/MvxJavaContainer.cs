namespace Cirrious.MvvmCross.Binding.Android
{
    public class MvxJavaContainer : Java.Lang.Object
    {
        public object Object { get; private set; }

        public MvxJavaContainer(object theObject)
        {
            Object = theObject;
        }
    }
    public class MvxJavaContainer<T> : MvxJavaContainer
    {
        public new T Object { get { return (T)base.Object; } }

        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }
    }
}