using System.Collections;

namespace MvvmCross.Droid.Support.V7.RecyclerView.Grouping
{
    public class MvxGroupedData
    {
        public object Key { get; set; }

        public IEnumerable Items { get; set; }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as MvxGroupedData;
            return other != null && Key.Equals(other.Key);
        }
    }
}