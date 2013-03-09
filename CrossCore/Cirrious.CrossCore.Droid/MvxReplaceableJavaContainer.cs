using Java.Lang;

namespace Cirrious.CrossCore.Droid
{
    public class MvxReplaceableJavaContainer : Object
    {
        public object Object { get; set; }

        public override string ToString()
        {
            return Object == null ? string.Empty : Object.ToString();
        }
    }
}