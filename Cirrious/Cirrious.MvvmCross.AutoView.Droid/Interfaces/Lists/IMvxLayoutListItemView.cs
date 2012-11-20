namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists
{
    public interface IMvxLayoutListItemView
    {
        string UniqueName { get; }
        void BindTo(object source);
    }
}