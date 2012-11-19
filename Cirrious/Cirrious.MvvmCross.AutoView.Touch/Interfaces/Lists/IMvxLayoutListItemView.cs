namespace Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists
{
    public interface IMvxLayoutListItemView
    {
        string UniqueName { get; }
        void BindTo(object source);
    }
}